using System;
using System.Collections;
using UnityEngine;

public class SpringGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile {
	[SerializeField]
	private SpringGrenadeConfiguration _config;

	[SerializeField]
	private ParticleEmitter _deployedEffect;

	private bool _isDestroyed;

	[SerializeField]
	private Renderer _renderer;

	[SerializeField]
	private ParticleEmitter _smoke;

	[SerializeField]
	private AudioClip _sound;

	private StateMachine<SpringGrenadeState> machine = new StateMachine<SpringGrenadeState>();

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
		set { _config = (SpringGrenadeConfiguration)value; }
	}

	public AudioClip ExplosionSound { get; set; }

	public AudioClip JumpSound {
		get { return _sound; }
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
		var springGrenadeQuickItem = Instantiate(this) as SpringGrenadeQuickItem;
		springGrenadeQuickItem.gameObject.SetActive(true);

		for (var i = 0; i < springGrenadeQuickItem.gameObject.transform.childCount; i++) {
			springGrenadeQuickItem.gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}

		if (springGrenadeQuickItem.rigidbody) {
			springGrenadeQuickItem.rigidbody.isKinematic = false;
		}

		springGrenadeQuickItem.Position = position;
		springGrenadeQuickItem.Velocity = velocity;
		springGrenadeQuickItem.machine.RegisterState(SpringGrenadeState.Flying, new FlyingState(springGrenadeQuickItem));
		springGrenadeQuickItem.machine.RegisterState(SpringGrenadeState.Deployed, new DeployedState(springGrenadeQuickItem));
		springGrenadeQuickItem.machine.PushState(SpringGrenadeState.Flying);

		if (OnProjectileEmitted != null) {
			OnProjectileEmitted(springGrenadeQuickItem);
		}

		return springGrenadeQuickItem;
	}

	public void SetLayer(UberstrikeLayer layer) {
		LayerUtil.SetLayerRecursively(transform, layer);
	}

	public int ID { get; set; }

	public Vector3 Explode() {
		var vector = Vector3.zero;

		try {
			if (OnExploded != null) {
				OnExploded(ID, transform.position);
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
	public event Action<int, Vector3> OnExploded;

	protected override void OnActivated() {
		var vector = GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition;
		var vector2 = vector + GameState.Current.PlayerData.ShootingDirection * 2f;
		var vector3 = GameState.Current.PlayerData.ShootingDirection * _config.Speed;
		var num = 2f;
		RaycastHit raycastHit;

		if (Physics.Raycast(vector, GameState.Current.PlayerData.ShootingDirection * 2f, out raycastHit, num, UberstrikeLayerMasks.LocalRocketMask)) {
			var springGrenadeQuickItem = Throw(raycastHit.point, Vector3.zero) as SpringGrenadeQuickItem;
			springGrenadeQuickItem.machine.PopAllStates();
			GameState.Current.Player.MoveController.ApplyForce(_config.JumpDirection.normalized * _config.Force, CharacterMoveController.ForceType.Additive);
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(JumpSound);
			StartCoroutine(DestroyDelayed(springGrenadeQuickItem.ID));
		} else {
			var grenadeProjectile = Throw(vector2, vector3);

			grenadeProjectile.OnProjectileExploded += delegate(IGrenadeProjectile p) {
				var array = Physics.OverlapSphere(p.Position, 2f, UberstrikeLayerMasks.ExplosionMask);

				foreach (var collider in array) {
					var component = collider.gameObject.GetComponent<CharacterHitArea>();

					if (component != null && component.RecieveProjectileDamage) {
						component.Shootable.ApplyForce(component.transform.position, _config.JumpDirection.normalized * _config.Force);
					}
				}
			};
		}
	}

	private IEnumerator DestroyDelayed(int projectileId) {
		yield return new WaitForSeconds(0.2f);

		Singleton<ProjectileManager>.Instance.RemoveProjectile(projectileId);
		GameState.Current.Actions.RemoveProjectile(projectileId, true);
	}

	private void Update() {
		machine.Update();
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

	private enum SpringGrenadeState {
		Flying = 1,
		Deployed
	}

	private class FlyingState : IState {
		private float _timeOut;
		private SpringGrenadeQuickItem behaviour;

		public FlyingState(SpringGrenadeQuickItem behaviour) {
			this.behaviour = behaviour;
		}

		public void OnEnter() {
			_timeOut = Time.time + behaviour._config.LifeTime;
			var springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Combine(springGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
			var gameObject = behaviour.gameObject;

			if (gameObject && GameState.Current.Avatar.Decorator && gameObject.collider) {
				var collider = gameObject.collider;

				foreach (var characterHitArea in GameState.Current.Avatar.Decorator.HitAreas) {
					if (gameObject.activeInHierarchy && characterHitArea.gameObject.activeInHierarchy) {
						Physics.IgnoreCollision(collider, characterHitArea.collider);
					}
				}
			}
		}

		public void OnResume() { }

		public void OnExit() {
			var springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Remove(springGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
		}

		public void OnUpdate() {
			if (_timeOut < Time.time) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}

		private void OnCollisionEnterEvent(Collision c) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, true);
			} else if (!(c.transform.tag == "MovableObject")) {
				if (behaviour._config.IsSticky) {
					if (c.contacts.Length > 0) {
						behaviour.transform.position = c.contacts[0].point + c.contacts[0].normal * behaviour.collider.bounds.extents.sqrMagnitude;
					}

					behaviour.machine.PopState();
					behaviour.machine.PushState(SpringGrenadeState.Deployed);
				}
			}

			PlayBounceSound(c.transform.position);
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
		private SpringGrenadeQuickItem behaviour;

		public DeployedState(SpringGrenadeQuickItem behaviour) {
			this.behaviour = behaviour;
			behaviour.OnProjectileExploded = null;
		}

		public void OnEnter() {
			_timeOut = Time.time + behaviour._config.LifeTime;
			var springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(springGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));

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
			var springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(springGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
		}

		public void OnUpdate() {
			if (_timeOut < Time.time) {
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}

		public void OnTriggerEnterEvent(Collider c) {
			if (TagUtil.GetTag(c) == "Player") {
				behaviour.machine.PopState();
				GameState.Current.Player.MoveController.ApplyForce(behaviour._config.JumpDirection.normalized * behaviour._config.Force, CharacterMoveController.ForceType.Additive);
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(behaviour.JumpSound);
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, true);
			} else if (behaviour.collider.gameObject.layer == 20) {
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.JumpPad, behaviour.transform.position);
			}
		}
	}
}
