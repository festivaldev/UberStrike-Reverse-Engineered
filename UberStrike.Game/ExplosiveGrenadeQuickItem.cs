using System;
using UberStrike.Core.Types;
using UnityEngine;

public class ExplosiveGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile {
	[SerializeField]
	private ExplosiveGrenadeConfiguration _config;

	[SerializeField]
	private ParticleEmitter _deployedEffect;

	[SerializeField]
	private GameObject _explosionSfx;

	[SerializeField]
	private AudioClip _explosionSound;

	private bool _isDestroyed;

	[SerializeField]
	private Renderer _renderer;

	[SerializeField]
	private ParticleEmitter _smoke;

	private StateMachine machine = new StateMachine();

	public ParticleEmitter Smoke {
		get { return _smoke; }
	}

	public ParticleEmitter DeployedEffect {
		get { return _deployedEffect; }
	}

	public Renderer Renderer {
		get { return _renderer; }
	}

	public override QuickItemConfiguration Configuration {
		get { return _config; }
		set { _config = (ExplosiveGrenadeConfiguration)value; }
	}

	public Vector3 Position {
		get { return (!transform) ? Vector3.zero : transform.position; }
		private set {
			if (transform) {
				transform.position = value;
			}
		}
	}

	public Vector3 Velocity {
		get { return (!rigidbody) ? Vector3.zero : rigidbody.velocity; }
		private set {
			if (rigidbody) {
				rigidbody.velocity = value;
			}
		}
	}

	public event Action<IGrenadeProjectile> OnProjectileExploded;
	public event Action<IGrenadeProjectile> OnProjectileEmitted;

	public IGrenadeProjectile Throw(Vector3 position, Vector3 velocity) {
		var explosiveGrenadeQuickItem = Instantiate(this) as ExplosiveGrenadeQuickItem;

		if (explosiveGrenadeQuickItem) {
			explosiveGrenadeQuickItem.gameObject.SetActive(true);

			for (var i = 0; i < explosiveGrenadeQuickItem.transform.childCount; i++) {
				explosiveGrenadeQuickItem.transform.GetChild(i).gameObject.SetActive(true);
			}

			explosiveGrenadeQuickItem.Position = position;
			explosiveGrenadeQuickItem.Velocity = velocity;
			explosiveGrenadeQuickItem.collider.material.bounciness = _config.Bounciness;
			explosiveGrenadeQuickItem.machine.RegisterState(1, new FlyingState(explosiveGrenadeQuickItem));
			explosiveGrenadeQuickItem.machine.RegisterState(2, new DeployedState(explosiveGrenadeQuickItem));
			explosiveGrenadeQuickItem.machine.PushState(1);
		}

		if (OnProjectileEmitted != null) {
			OnProjectileEmitted(explosiveGrenadeQuickItem);
		}

		return explosiveGrenadeQuickItem;
	}

	public void SetLayer(UberstrikeLayer layer) {
		LayerUtil.SetLayerRecursively(transform, layer);
	}

	public int ID { get; set; }

	public Vector3 Explode() {
		var vector = Vector3.zero;

		try {
			if (_explosionSound != null) {
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(_explosionSound, transform.position);
			}

			if (_explosionSfx) {
				var gameObject = Instantiate(_explosionSfx) as GameObject;

				if (gameObject) {
					gameObject.transform.position = transform.position;
					var selfDestroy = gameObject.AddComponent<SelfDestroy>();

					if (selfDestroy) {
						selfDestroy.SetDelay(2f);
					}
				}
			} else {
				ParticleEffectController.ShowExplosionEffect(ParticleConfigurationType.LauncherDefault, SurfaceEffectType.None, transform.position, Vector3.up);
			}

			if (OnProjectileExploded != null) {
				OnProjectileExploded(this);
			}

			vector = transform.position;
			Destroy();
		} catch (Exception ex) {
			Debug.LogException(ex);
		}

		return vector;
	}

	public void Destroy() {
		if (!_isDestroyed) {
			_isDestroyed = true;
			gameObject.SetActive(false);
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private event Action<Collider> OnTriggerEnterEvent;
	private event Action<Collision> OnCollisionEnterEvent;

	protected override void OnActivated() {
		var vector = GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition;
		var vector2 = vector + GameState.Current.PlayerData.ShootingDirection * 2f;
		var vector3 = GameState.Current.PlayerData.ShootingDirection * _config.Speed;
		var num = 2f;
		RaycastHit raycastHit;

		if (Physics.Raycast(vector, GameState.Current.PlayerData.ShootingDirection * 2f, out raycastHit, num, UberstrikeLayerMasks.LocalRocketMask)) {
			var explosiveGrenadeQuickItem = Throw(raycastHit.point, Vector3.zero) as ExplosiveGrenadeQuickItem;
			explosiveGrenadeQuickItem.machine.PopAllStates();
			var explosiveGrenadeQuickItem2 = explosiveGrenadeQuickItem;
			explosiveGrenadeQuickItem2.OnProjectileExploded = (Action<IGrenadeProjectile>)Delegate.Combine(explosiveGrenadeQuickItem2.OnProjectileExploded, new Action<IGrenadeProjectile>(delegate(IGrenadeProjectile p) { ProjectileDetonator.Explode(p.Position, p.ID, _config.Damage, Vector3.up, _config.SplashRadius, 5, 5, Configuration.ID, UberstrikeItemClass.WeaponLauncher); }));
			Singleton<ProjectileManager>.Instance.RemoveProjectile(explosiveGrenadeQuickItem.ID);
			GameState.Current.Actions.RemoveProjectile(explosiveGrenadeQuickItem.ID, true);
		} else {
			var grenadeProjectile = Throw(vector2, vector3);
			grenadeProjectile.OnProjectileExploded += delegate(IGrenadeProjectile p) { ProjectileDetonator.Explode(p.Position, p.ID, _config.Damage, Vector3.up, _config.SplashRadius, 5, 5, Configuration.ID, UberstrikeItemClass.WeaponLauncher); };
		}
	}

	private void Update() {
		machine.Update();
	}

	private void OnGUI() {
		if (Behaviour.IsCoolingDown && Behaviour.FocusTimeRemaining > 0f) {
			var num = Mathf.Clamp(Screen.height * 0.03f, 10f, 40f);
			var num2 = num * 10f;
			GUI.Label(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 20, num2, num), "Charging Grenade", BlueStonez.label_interparkbold_16pt);
			GUITools.DrawWarmupBar(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 50, num2, num), Behaviour.FocusTimeTotal - Behaviour.FocusTimeRemaining, Behaviour.FocusTimeTotal);
		}
	}

	private void OnTriggerEnter(Collider c) {
		if (OnTriggerEnterEvent != null) {
			OnTriggerEnterEvent(c);
		}
	}

	private void OnCollisionEnter(Collision c) {
		if (OnCollisionEnterEvent != null) {
			OnCollisionEnterEvent(c);
		}
	}

	private class FlyingState : IState {
		private float _timeOut;
		private ExplosiveGrenadeQuickItem behaviour;

		public FlyingState(ExplosiveGrenadeQuickItem behaviour) {
			this.behaviour = behaviour;
		}

		public void OnEnter() {
			_timeOut = Time.time + behaviour._config.LifeTime;
			var explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Combine(explosiveGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));

			if (!behaviour._config.IsSticky) {
				var explosiveGrenadeQuickItem2 = behaviour;
				explosiveGrenadeQuickItem2.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(explosiveGrenadeQuickItem2.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
			}

			var gameObject = behaviour.gameObject;

			if (gameObject && GameState.Current.Avatar.Decorator && gameObject.collider) {
				var collider = gameObject.collider;

				foreach (var characterHitArea in GameState.Current.Avatar.Decorator.HitAreas) {
					if (gameObject.activeSelf && characterHitArea.gameObject.activeSelf) {
						Physics.IgnoreCollision(collider, characterHitArea.collider);
					}
				}
			}
		}

		public void OnExit() {
			var explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Remove(explosiveGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));

			if (!behaviour._config.IsSticky) {
				var explosiveGrenadeQuickItem2 = behaviour;
				explosiveGrenadeQuickItem2.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(explosiveGrenadeQuickItem2.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
			}
		}

		public void OnUpdate() {
			if (_timeOut < Time.time) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}

		public void OnResume() { }

		private void OnCollisionEnterEvent(Collision c) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, true);
			} else if (behaviour._config.IsSticky) {
				if (c.contacts.Length > 0) {
					behaviour.transform.position = c.contacts[0].point + c.contacts[0].normal * behaviour.collider.bounds.extents.sqrMagnitude;
				}

				behaviour.machine.PopState();
				behaviour.machine.PushState(2);
			}

			PlayBounceSound(c.transform.position);
		}

		private void OnTriggerEnterEvent(Collider c) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, true);
			}
		}

		protected void PlayBounceSound(Vector3 position) {
			var audioClip = GameAudio.LauncherBounce1;
			var num = UnityEngine.Random.Range(0, 2);

			if (num > 0) {
				audioClip = GameAudio.LauncherBounce2;
			}

			AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(audioClip, position);
		}
	}

	private class DeployedState : IState {
		private float _timeOut;
		private ExplosiveGrenadeQuickItem behaviour;

		public DeployedState(ExplosiveGrenadeQuickItem behaviour) {
			this.behaviour = behaviour;
		}

		public void OnEnter() {
			_timeOut = Time.time + behaviour._config.LifeTime;
			var explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(explosiveGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));

			if (behaviour.rigidbody) {
				behaviour.rigidbody.isKinematic = true;
			}

			if (behaviour.collider) {
				Destroy(behaviour.collider);
			}

			behaviour.gameObject.layer = 2;

			if (behaviour.DeployedEffect) {
				behaviour.DeployedEffect.emit = true;
			}
		}

		public void OnResume() { }

		public void OnExit() {
			var explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(explosiveGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
		}

		public void OnUpdate() {
			if (_timeOut < Time.time) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}

		private void OnTriggerEnterEvent(Collider c) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, true);
			}
		}
	}
}
