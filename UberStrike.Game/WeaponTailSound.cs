using UnityEngine;

public class WeaponTailSound : BaseWeaponEffect {
	[SerializeField]
	private WeaponHeadAnimation _headAnimation;

	private AudioSource _tailAudioSource;

	[SerializeField]
	private AudioClip _tailSound;

	private float _tailSoundLength;
	private float _tailSoundMaxLength;

	private void Awake() {
		if (_tailSound) {
			_tailAudioSource = gameObject.AddComponent<AudioSource>();

			if (_tailAudioSource) {
				_tailAudioSource.clip = _tailSound;
				_tailAudioSource.playOnAwake = false;
			}

			_tailSoundMaxLength = _tailSound.length * 0.8f;
		} else {
			Debug.LogError("There is no audio clip signed for WeaponTailSound!");
		}
	}

	private void Update() {
		if (_tailSoundLength > 0f) {
			if (_headAnimation) {
				_headAnimation.OnShoot();
			}

			_tailSoundLength -= Time.deltaTime;
		}
	}

	public override void OnShoot() {
		if (_tailAudioSource) {
			_tailAudioSource.Stop();
		}

		_tailSoundLength = _tailSoundMaxLength;
	}

	public override void OnPostShoot() {
		if (_tailAudioSource) {
			_tailAudioSource.Stop();
			_tailAudioSource.Play();
		}
	}

	public override void Hide() {
		if (_tailAudioSource) {
			_tailAudioSource.Stop();
		}
	}

	public override void OnHits(RaycastHit[] hits) { }
}
