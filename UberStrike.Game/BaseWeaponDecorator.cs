using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseWeaponDecorator : MonoBehaviour {
	private Vector3 _defaultPosition;
	private Dictionary<string, SurfaceEffectType> _effectMap;
	private List<BaseWeaponEffect> _effects = new List<BaseWeaponEffect>();
	private ParticleConfigurationType _effectType;
	private Vector3 _ironSightPosition;
	private bool _isEnabled = true;
	private bool _isShootAnimationEnabled;
	protected AudioSource _mainAudioSource;

	[SerializeField]
	private Transform _muzzlePosition;

	private Transform _parent;
	private ParticleSystem _particles;

	[SerializeField]
	private AudioClip[] _shootSounds;

	private MoveTrailrendererObject _trailRenderer;

	public bool IsEnabled {
		get { return _isEnabled; }
		set {
			if (gameObject.activeSelf != value) {
				_isEnabled = value;
				gameObject.SetActive(_isEnabled);
				HideAllWeaponEffect();
			}
		}
	}

	public bool EnableShootAnimation {
		get { return _isShootAnimationEnabled; }
		set {
			_isShootAnimationEnabled = value;

			if (!_isShootAnimationEnabled) {
				var weaponShootAnimation = _effects.Find(p => p is WeaponShootAnimation) as WeaponShootAnimation;

				if (weaponShootAnimation) {
					_effects.Remove(weaponShootAnimation);
					Destroy(weaponShootAnimation);
				}
			}
		}
	}

	public bool HasShootAnimation { get; private set; }

	public Vector3 MuzzlePosition {
		get { return (!_muzzlePosition) ? Vector3.zero : _muzzlePosition.position; }
	}

	public Vector3 DefaultPosition {
		get { return _defaultPosition; }
		set {
			_defaultPosition = value;
			transform.localPosition = _defaultPosition;
		}
	}

	public Vector3 CurrentPosition {
		get { return transform.localPosition; }
		set { transform.localPosition = value; }
	}

	public Quaternion CurrentRotation {
		get { return transform.localRotation; }
		set { transform.localRotation = value; }
	}

	public Vector3 IronSightPosition {
		get { return _ironSightPosition; }
		set { _ironSightPosition = value; }
	}

	public Vector3 DefaultAngles { get; set; }
	public UberstrikeItemClass WeaponClass { get; set; }

	public MoveTrailrendererObject TrailRenderer {
		get { return _trailRenderer; }
	}

	public bool IsMelee { get; protected set; }

	public void HideAllWeaponEffect() {
		if (_effects != null) {
			foreach (var baseWeaponEffect in _effects) {
				baseWeaponEffect.Hide();
			}
		}
	}

	protected virtual void Awake() {
		_parent = transform.parent;
		_mainAudioSource = GetComponent<AudioSource>();

		if (_mainAudioSource) {
			_mainAudioSource.priority = 0;
		}

		_effects.AddRange(GetComponentsInChildren<BaseWeaponEffect>(true));

		if (_muzzlePosition) {
			_particles = _muzzlePosition.GetComponent<ParticleSystem>();
		}

		HasShootAnimation = _effects.Exists(e => e is WeaponShootAnimation);
		InitEffectMap();
	}

	protected virtual void Start() {
		HideAllWeaponEffect();
	}

	public BaseWeaponDecorator Clone() {
		return Instantiate(this) as BaseWeaponDecorator;
	}

	public virtual void ShowShootEffect(RaycastHit[] hits) {
		if (IsEnabled) {
			if (_muzzlePosition) {
				var position = _muzzlePosition.position;

				for (var i = 0; i < hits.Length; i++) {
					var normalized = (hits[i].point - position).normalized;
					var num = Vector3.Distance(position, hits[i].point);
					ShowImpactEffects(hits[i], normalized, position, num, i == 0);
				}
			}

			foreach (var baseWeaponEffect in _effects) {
				baseWeaponEffect.OnShoot();
				baseWeaponEffect.OnHits(hits);
			}

			if (_particles) {
				_particles.Stop();
				_particles.Play(_isShootAnimationEnabled);
			}

			PlayShootSound();
		}
	}

	public virtual void PostShoot() {
		if (IsEnabled && _effects != null) {
			foreach (var baseWeaponEffect in _effects) {
				baseWeaponEffect.OnPostShoot();
			}
		}
	}

	protected virtual void ShowImpactEffects(RaycastHit hit, Vector3 direction, Vector3 muzzlePosition, float distance, bool playSound) {
		EmitImpactParticles(hit, direction, muzzlePosition, distance, playSound);
	}

	private static void Play3dAudioClip(AudioSource audioSource, AudioClip soundEffect) {
		Play3dAudioClip(audioSource, soundEffect, 0f);
	}

	private static void Play3dAudioClip(AudioSource audioSource, AudioClip soundEffect, float delay) {
		try {
			audioSource.clip = soundEffect;
			var num = (ulong)(delay * audioSource.clip.frequency);
			audioSource.Play(num);
		} catch {
			Debug.LogError("Play3dAudioClip: " + soundEffect + " failed.");
		}
	}

	public virtual void StopSound() {
		_mainAudioSource.Stop();
	}

	public void PlayShootSound() {
		if (_mainAudioSource && _shootSounds != null && _shootSounds.Length > 0) {
			var num = UnityEngine.Random.Range(0, _shootSounds.Length);
			var audioClip = _shootSounds[num];

			if (audioClip) {
				_mainAudioSource.volume = ((!ApplicationDataManager.ApplicationOptions.AudioEnabled) ? 0f : ApplicationDataManager.ApplicationOptions.AudioEffectsVolume);
				_mainAudioSource.PlayOneShot(audioClip);
			}
		}
	}

	private void InitEffectMap() {
		_effectMap = new Dictionary<string, SurfaceEffectType>();
		_effectMap.Add("Wood", SurfaceEffectType.WoodEffect);
		_effectMap.Add("SolidWood", SurfaceEffectType.WoodEffect);
		_effectMap.Add("Stone", SurfaceEffectType.StoneEffect);
		_effectMap.Add("Metal", SurfaceEffectType.MetalEffect);
		_effectMap.Add("Sand", SurfaceEffectType.SandEffect);
		_effectMap.Add("Grass", SurfaceEffectType.GrassEffect);
		_effectMap.Add("Avatar", SurfaceEffectType.Splat);
		_effectMap.Add("Water", SurfaceEffectType.WaterEffect);
		_effectMap.Add("NoTarget", SurfaceEffectType.None);
		_effectMap.Add("Cement", SurfaceEffectType.StoneEffect);
	}

	public void SetSurfaceEffect(ParticleConfigurationType effect) {
		_effectType = effect;
	}

	public virtual void PlayEquipSound() {
		AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.WeaponSwitch);
	}

	public virtual void PlayHitSound() {
		Debug.LogError("Not Implemented: Should play WeaponHit sound!");
	}

	public void PlayOutOfAmmoSound() {
		Play3dAudioClip(_mainAudioSource, GameAudio.OutOfAmmoClick);
	}

	public void PlayImpactSoundAt(HitPoint point) {
		if (point == null) {
			return;
		}

		var num = ((!_muzzlePosition) ? 0f : _muzzlePosition.position.y);
		var num2 = ((!(GameState.Current.Map != null) || !GameState.Current.Map.HasWaterPlane) ? num : GameState.Current.Map.WaterPlaneHeight);

		if ((num > num2 && point.Point.y < num2) || (num < num2 && point.Point.y > num2)) {
			var point2 = point.Point;
			point2.y = 0f;
			AutoMonoBehaviour<SfxManager>.Instance.PlayImpactSound("Water", point2);
		} else {
			EmitImpactSound(point.Tag, point.Point);
		}
	}

	protected virtual void EmitImpactSound(string impactType, Vector3 position) {
		AutoMonoBehaviour<SfxManager>.Instance.PlayImpactSound(impactType, position);
	}

	protected void EmitImpactParticles(RaycastHit hit, Vector3 direction, Vector3 muzzlePosition, float distance, bool playSound) {
		var tag = TagUtil.GetTag(hit.collider);
		var point = hit.point;
		var vector = hit.normal;
		var surfaceEffectType = SurfaceEffectType.Default;

		if (_effectMap.TryGetValue(tag, out surfaceEffectType)) {
			if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane && ((_muzzlePosition.position.y > GameState.Current.Map.WaterPlaneHeight && point.y < GameState.Current.Map.WaterPlaneHeight) || (_muzzlePosition.position.y < GameState.Current.Map.WaterPlaneHeight && point.y > GameState.Current.Map.WaterPlaneHeight))) {
				surfaceEffectType = SurfaceEffectType.WaterEffect;
				vector = Vector3.up;
				point.y = GameState.Current.Map.WaterPlaneHeight;

				if (!Mathf.Approximately(direction.y, 0f)) {
					point.x = (GameState.Current.Map.WaterPlaneHeight - hit.point.y) / direction.y * direction.x + hit.point.x;
					point.z = (GameState.Current.Map.WaterPlaneHeight - hit.point.y) / direction.y * direction.z + hit.point.z;
				}
			}

			ParticleEffectController.ShowHitEffect(_effectType, surfaceEffectType, direction, point, vector, muzzlePosition, distance, ref _trailRenderer, _parent);
		}
	}

	public void SetMuzzlePosition(Transform muzzle) {
		_muzzlePosition = muzzle;
	}

	public void SetWeaponSounds(AudioClip[] sounds) {
		if (sounds != null) {
			_shootSounds = new AudioClip[sounds.Length];
			sounds.CopyTo(_shootSounds, 0);
		}
	}
}
