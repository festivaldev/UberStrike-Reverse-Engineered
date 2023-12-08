using System;
using System.Collections;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ProjectileWeapon : BaseWeaponLogic {
	private ProjectileWeaponDecorator _decorator;
	private UberStrikeItemWeaponView _view;

	public override BaseWeaponDecorator Decorator {
		get { return _decorator; }
	}

	public int MaxConcurrentProjectiles { get; private set; }
	public int MinProjectileDistance { get; private set; }

	public override int AmmoCountPerShot {
		get { return _view.ProjectilesPerShot; }
	}

	public bool HasProjectileLimit {
		get { return MaxConcurrentProjectiles > 0; }
	}

	public ParticleConfigurationType ExplosionType { get; private set; }

	public ProjectileWeapon(WeaponItem item, ProjectileWeaponDecorator decorator, IWeaponController controller, UberStrikeItemWeaponView view) : base(item, controller) {
		_view = view;
		_decorator = decorator;
		MaxConcurrentProjectiles = item.Configuration.MaxConcurrentProjectiles;
		MinProjectileDistance = item.Configuration.MinProjectileDistance;
		ExplosionType = item.Configuration.ParticleEffect;
	}

	public event Action<ProjectileInfo> OnProjectileShoot;

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits) {
		hits = null;
		RaycastHit raycastHit;

		if (MinProjectileDistance > 0 && Physics.Raycast(ray.origin, ray.direction, out raycastHit, MinProjectileDistance, UberstrikeLayerMasks.LocalRocketMask)) {
			var num = Controller.NextProjectileId();
			hits = new CmunePairList<BaseGameProp, ShotPoint>(1);
			hits.Add(null, new ShotPoint(raycastHit.point, num));
			ShowExplosionEffect(raycastHit.point, raycastHit.normal, ray.direction, num);

			if (OnProjectileShoot != null) {
				OnProjectileShoot(new ProjectileInfo(num, new Ray(raycastHit.point, -ray.direction)));
			}
		} else {
			if (_decorator) {
				_decorator.ShowShootEffect(new RaycastHit[0]);
			}

			UnityRuntime.StartRoutine(EmitProjectile(ray));
		}
	}

	public void ShowExplosionEffect(Vector3 position, Vector3 normal, Vector3 direction, int projectileId) {
		if (_decorator) {
			_decorator.ShowExplosionEffect(position, normal, ExplosionType);
		}
	}

	private IEnumerator EmitProjectile(Ray ray) {
		if (AmmoCountPerShot > 1) {
			float angle = 360 / AmmoCountPerShot;

			for (var i = 0; i < AmmoCountPerShot; i++) {
				if (_decorator) {
					var shotCount = Controller.NextProjectileId();
					ray.origin = _decorator.MuzzlePosition + Quaternion.AngleAxis(angle * i, _decorator.transform.forward) * _decorator.transform.up * 0.2f;
					var p = EmitProjectile(ray, shotCount, Controller.Cmid);

					if (p && OnProjectileShoot != null) {
						OnProjectileShoot(new ProjectileInfo(shotCount, ray) {
							Projectile = p
						});
					}

					yield return new WaitForSeconds(0.2f);
				}
			}
		} else {
			var shotCount2 = Controller.NextProjectileId();
			var p2 = EmitProjectile(ray, shotCount2, Controller.Cmid);

			if (p2 && OnProjectileShoot != null) {
				OnProjectileShoot(new ProjectileInfo(shotCount2, ray) {
					Projectile = p2
				});
			}
		}
	}

	public Projectile EmitProjectile(Ray ray, int projectileID, int cmid) {
		if (_decorator && _decorator.Missle) {
			var muzzlePosition = _decorator.MuzzlePosition;
			var quaternion = Quaternion.LookRotation(ray.direction);
			var projectile = UnityEngine.Object.Instantiate(_decorator.Missle, muzzlePosition, quaternion) as Projectile;

			if (projectile) {
				if (projectile is GrenadeProjectile) {
					var grenadeProjectile = projectile as GrenadeProjectile;
					grenadeProjectile.Sticky = Config.Sticky;
				}

				projectile.transform.parent = ProjectileManager.Container.transform;
				projectile.gameObject.tag = "Prop";
				projectile.ExplosionEffect = ExplosionType;
				projectile.TimeOut = _decorator.MissileTimeOut;
				projectile.SetExplosionSound(_decorator.ExplosionSound);
				projectile.transform.position = ray.origin + MinProjectileDistance * ray.direction;

				if (Controller.IsLocal) {
					projectile.gameObject.layer = 26;
				} else {
					projectile.gameObject.layer = 24;
				}

				CharacterConfig characterConfig;

				if (GameState.Current != null && GameState.Current.TryGetPlayerAvatar(cmid, out characterConfig) && characterConfig.Avatar.Decorator && projectile.gameObject.activeSelf) {
					foreach (var characterHitArea in characterConfig.Avatar.Decorator.HitAreas) {
						if (characterHitArea.gameObject.activeInHierarchy) {
							Physics.IgnoreCollision(projectile.gameObject.collider, characterHitArea.collider);
						}
					}
				}

				projectile.MoveInDirection(ray.direction * WeaponConfigurationHelper.GetProjectileSpeed(_view));

				return projectile;
			}
		}

		return null;
	}
}
