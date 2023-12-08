using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class Projectile : MonoBehaviour, IProjectile {
	public const int DefaultTimeout = 30;

	[SerializeField]
	private Collider _collider;

	[SerializeField]
	private GameObject _explosionEffect;

	protected AudioClip _explosionSound;
	private float _positionSign;
	private Rigidbody _rigidbody;

	[SerializeField]
	private bool _showHeatwave;

	protected AudioSource _source;
	private Transform _transform;

	[SerializeField]
	private Collider _trigger;

	public ParticleConfigurationType ExplosionEffect { get; set; }

	public Rigidbody Rigidbody {
		get { return _rigidbody; }
	}

	public ProjectileDetonator Detonator { get; set; }
	public bool IsProjectileExploded { get; protected set; }
	public float TimeOut { get; set; }

	protected int CollisionMask {
		get {
			if (gameObject && gameObject.layer == 24) {
				return UberstrikeLayerMasks.RemoteRocketMask;
			}

			return UberstrikeLayerMasks.LocalRocketMask;
		}
	}

	public int ID { get; set; }

	public void Destroy() {
		if (!IsProjectileExploded) {
			IsProjectileExploded = true;
			gameObject.SetActive(false);
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public Vector3 Explode() {
		var vector = Vector3.zero;

		try {
			RaycastHit raycastHit;

			if (Physics.Raycast(transform.position - transform.forward, transform.forward, out raycastHit, 2f, CollisionMask)) {
				vector = raycastHit.point - transform.forward * 0.01f;
				Explode(vector, raycastHit.normal, TagUtil.GetTag(raycastHit.collider));
			} else {
				vector = transform.position;
				Explode(vector, -transform.forward, string.Empty);
			}
		} catch (Exception ex) {
			Debug.LogWarning(ex);
		}

		return vector;
	}

	protected virtual void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
		_source = GetComponent<AudioSource>();

		if (_collider == null && _trigger == null) {
			Debug.LogError("The Projectile " + gameObject.name + " has not assigned Collider or Trigger! Check your Inspector settings.");
		}

		if (_collider && _collider.isTrigger) {
			Debug.LogError("The Projectile " + gameObject.name + " has a Collider attached that is configured as Trigger! Check your Inspector settings.");
		}

		if (_trigger && !_trigger.isTrigger) {
			Debug.LogError("The Projectile " + gameObject.name + " has a Trigger attached that is configured as Collider! Check your Inspector settings.");
		}

		_transform = transform;
		_positionSign = Mathf.Sign(_transform.position.y);
	}

	protected virtual void Start() {
		if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane) {
			_positionSign = Mathf.Sign(_transform.position.y - GameState.Current.Map.WaterPlaneHeight);
		}

		StartCoroutine(StartTimeout());
	}

	public void MoveInDirection(Vector3 direction) {
		Rigidbody.isKinematic = false;
		Rigidbody.velocity = direction;
	}

	protected virtual IEnumerator StartTimeout() {
		yield return new WaitForSeconds((TimeOut <= 0f) ? 30f : TimeOut);

		Singleton<ProjectileManager>.Instance.RemoveProjectile(ID);
	}

	protected abstract void OnTriggerEnter(Collider c);
	protected abstract void OnCollisionEnter(Collision c);

	protected virtual void Update() {
		if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane && _positionSign != Mathf.Sign(_transform.position.y - GameState.Current.Map.WaterPlaneHeight)) {
			_positionSign = Mathf.Sign(_transform.position.y - GameState.Current.Map.WaterPlaneHeight);
			ParticleEffectController.ProjectileWaterRipplesEffect(ExplosionEffect, _transform.position);
		}
	}

	protected void Explode(Vector3 point, Vector3 normal, string tag) {
		Destroy();

		if (Detonator != null) {
			Detonator.Explode(point);
		}

		Singleton<ExplosionManager>.Instance.PlayExplosionSound(point, _explosionSound);
		Singleton<ExplosionManager>.Instance.ShowExplosionEffect(point, normal, tag, ExplosionEffect);

		if (_showHeatwave) {
			ParticleEffectController.ShowHeatwaveEffect(transform.position);
		}

		if (_explosionEffect) {
			Instantiate(_explosionEffect, point, Quaternion.LookRotation(normal));
		}
	}

	public void SetExplosionSound(AudioClip clip) {
		_explosionSound = clip;
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
