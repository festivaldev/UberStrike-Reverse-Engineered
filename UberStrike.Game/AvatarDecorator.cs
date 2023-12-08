using UnityEngine;

public class AvatarDecorator : MonoBehaviour {
	private AudioSource _audio;

	[SerializeField]
	private CharacterHitArea[] _hitAreas;

	private float _nextFootStepTime;
	private Transform _transform;

	[SerializeField]
	private Transform _weaponAttachPoint;

	public Animation Animation { get; private set; }
	public Animator Animator { get; private set; }
	public AvatarAnimationController AnimationController { get; private set; }
	public FootStepSoundType CurrentFootStep { get; set; }
	public AvatarHudInformation HudInformation { get; private set; }
	public AvatarDecoratorConfig Configuration { get; private set; }

	public CharacterHitArea[] HitAreas {
		get { return _hitAreas; }
		set { _hitAreas = value; }
	}

	public Transform WeaponAttachPoint {
		get { return _weaponAttachPoint; }
		set { _weaponAttachPoint = value; }
	}

	private void Awake() {
		_transform = transform;
		_audio = GetComponent<AudioSource>();
		Animator = GetComponent<Animator>();
		AnimationController = GetComponent<AvatarAnimationController>();
		HudInformation = GetComponentInChildren<AvatarHudInformation>();
		Configuration = GetComponent<AvatarDecoratorConfig>();
	}

	public void SetLayers(UberstrikeLayer layer) {
		LayerUtil.SetLayerRecursively(transform, layer);
	}

	public Transform GetBone(BoneIndex bone) {
		return Configuration.GetBone(bone);
	}

	public void SetPosition(Vector3 position, Quaternion rotation) {
		transform.localPosition = position;
		transform.localRotation = rotation;
	}

	public void PlayFootSound(float walkingSpeed) {
		PlayFootSound(walkingSpeed, CurrentFootStep);
	}

	public void PlayJumpSound() {
		_nextFootStepTime = Time.time + 0.3f;
	}

	public void PlayFootSound(float walkingSpeed, FootStepSoundType sound) {
		if (_nextFootStepTime < Time.time) {
			_nextFootStepTime = Time.time + walkingSpeed;
			_audio.clip = AutoMonoBehaviour<SfxManager>.Instance.GetFootStepAudioClip(sound);
			AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(_audio.clip, _transform.position);
		}
	}

	public void PlayDieSound() {
		var num = UnityEngine.Random.Range(0, 3);
		var audioClip = GameAudio.NormalKill1;

		switch (num) {
			case 0:
				audioClip = GameAudio.NormalKill1;

				break;
			case 1:
				audioClip = GameAudio.NormalKill2;

				break;
			case 3:
				audioClip = GameAudio.NormalKill3;

				break;
		}

		AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(audioClip, _transform.position);
	}
}
