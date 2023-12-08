using System.Collections;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MeleeWeapon : BaseWeaponLogic {
	private MeleeWeaponDecorator _decorator;

	public override BaseWeaponDecorator Decorator {
		get { return _decorator; }
	}

	public override float HitDelay {
		get { return 0.2f; }
	}

	public MeleeWeapon(WeaponItem item, MeleeWeaponDecorator decorator, IWeaponController controller) : base(item, controller) {
		_decorator = decorator;
	}

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits) {
		var origin = ray.origin;
		origin.y -= 0.1f;
		ray.origin = origin;
		hits = null;
		var num = 1f;
		var num2 = ((!Controller.IsLocal) ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask);
		var num3 = 1f;
		var array = Physics.SphereCastAll(ray, num, num3, num2);
		var num4 = Controller.NextProjectileId();

		if (array != null && array.Length > 0) {
			hits = new CmunePairList<BaseGameProp, ShotPoint>();
			var num5 = float.PositiveInfinity;
			var raycastHit = array[0];

			foreach (var raycastHit2 in array) {
				var vector = raycastHit2.point - ray.origin;

				if (Vector3.Dot(ray.direction, vector) > 0f && raycastHit2.distance < num5) {
					num5 = raycastHit2.distance;
					raycastHit = raycastHit2;
				}
			}

			if (raycastHit.collider) {
				var component = raycastHit.collider.GetComponent<BaseGameProp>();

				if (component != null) {
					hits.Add(component, new ShotPoint(raycastHit.point, num4));
				}

				if (_decorator) {
					_decorator.StartCoroutine(StartShowingEffect(raycastHit, ray.origin, HitDelay));
				}
			}
		} else if (_decorator) {
			_decorator.ShowShootEffect(new RaycastHit[0]);
		}

		EmitWaterImpactParticles(ray, num);
		OnHits(hits);
	}

	private IEnumerator StartShowingEffect(RaycastHit hit, Vector3 origin, float delay) {
		if (_decorator) {
			_decorator.ShowShootEffect(new[] {
				hit
			});
		}

		yield return new WaitForSeconds(delay);

		Decorator.PlayImpactSoundAt(new HitPoint(hit.point, TagUtil.GetTag(hit.collider)));
	}

	private void EmitWaterImpactParticles(Ray ray, float radius) {
		var origin = ray.origin;
		var vector = origin + ray.direction * radius;

		if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane && ((origin.y > GameState.Current.Map.WaterPlaneHeight && vector.y < GameState.Current.Map.WaterPlaneHeight) || (origin.y < GameState.Current.Map.WaterPlaneHeight && vector.y > GameState.Current.Map.WaterPlaneHeight))) {
			var vector2 = vector;
			vector2.y = GameState.Current.Map.WaterPlaneHeight;

			if (!Mathf.Approximately(ray.direction.y, 0f)) {
				vector2.x = (GameState.Current.Map.WaterPlaneHeight - vector.y) / ray.direction.y * ray.direction.x + vector.x;
				vector2.z = (GameState.Current.Map.WaterPlaneHeight - vector.y) / ray.direction.y * ray.direction.z + vector.z;
			}

			var trailRenderer = Decorator.TrailRenderer;
			ParticleEffectController.ShowHitEffect(ParticleConfigurationType.MeleeDefault, SurfaceEffectType.WaterEffect, Vector3.up, vector2, Vector3.up, origin, 1f, ref trailRenderer, Decorator.transform);
		}
	}
}
