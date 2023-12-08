using UnityEngine;

public class MinigunWeaponDecorator : BaseWeaponDecorator {
	private AudioSource _duringShootAudioSource;

	[SerializeField]
	private AudioClip _duringShootSound;

	private WeaponHeadAnimation _headAnim;
	private float _maxWarmDownTime;
	private float _maxWarmUpTime;
	private AudioSource _warmDownAudioSource;

	[SerializeField]
	private AudioClip _warmDownSound;

	private AudioSource _warmUpAudioSource;

	[SerializeField]
	private AudioClip _warmUpSound;

	public float MaxWarmUpTime {
		get { return _maxWarmUpTime; }
	}

	public float MaxWarmDownTime {
		get { return _maxWarmDownTime; }
	}

	protected override void Awake() {
		base.Awake();

		if (_warmUpSound == null) {
			throw new MissingReferenceException("MinigunWeaponDecorator - _warmUpSound is NULL");
		}

		if (_warmDownSound == null) {
			throw new MissingReferenceException("MinigunWeaponDecorator - _warmDownSound is NULL");
		}

		InitAudioSource();
		_headAnim = GetComponentInChildren<WeaponHeadAnimation>();
	}

	private void InitAudioSource() {
		if (_duringShootSound) {
			_duringShootAudioSource = gameObject.AddComponent<AudioSource>();

			if (_duringShootAudioSource) {
				_duringShootAudioSource.loop = true;
				_duringShootAudioSource.priority = 0;
				_duringShootAudioSource.playOnAwake = false;
				_duringShootAudioSource.clip = _duringShootSound;
			}
		}

		_warmUpAudioSource = gameObject.AddComponent<AudioSource>();

		if (_warmUpAudioSource) {
			_warmUpAudioSource.priority = 0;
			_warmUpAudioSource.playOnAwake = false;
			_maxWarmUpTime = _warmUpSound.length;
			_warmUpAudioSource.clip = _warmUpSound;
		}

		if (_warmDownSound) {
			_warmDownAudioSource = gameObject.AddComponent<AudioSource>();

			if (_warmDownAudioSource) {
				_warmDownAudioSource.priority = 0;
				_warmDownAudioSource.playOnAwake = false;
				_maxWarmDownTime = _warmDownSound.length;
				_warmDownAudioSource.clip = _warmDownSound;
			}
		}
	}

	public override void ShowShootEffect(RaycastHit[] hits) {
		base.ShowShootEffect(hits);
	}

	public void PlayWindUpSound(float time) {
		if (_warmDownAudioSource) {
			_warmDownAudioSource.Stop();
		}

		if (_warmUpAudioSource) {
			_warmUpAudioSource.time = time;
			_warmUpAudioSource.Play();
		}
	}

	public void PlayWindDownSound(float time) {
		if (_duringShootAudioSource) {
			_duringShootAudioSource.Stop();
		}

		if (_warmUpAudioSource) {
			_warmUpAudioSource.Stop();
		}

		if (_warmDownAudioSource) {
			_warmDownAudioSource.time = time;
			_warmDownAudioSource.Play();
		}
	}

	public void PlayDuringSound() {
		if (!_duringShootAudioSource.isPlaying) {
			_duringShootAudioSource.Play();
		}
	}

	public override void StopSound() {
		base.StopSound();
		_duringShootAudioSource.Stop();
	}

	public void SpinWeaponHead() {
		if (_headAnim) {
			_headAnim.OnShoot();
		}
	}
}
