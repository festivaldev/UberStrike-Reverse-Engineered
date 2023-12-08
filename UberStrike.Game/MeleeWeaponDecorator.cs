using System.Collections;
using UnityEngine;

public sealed class MeleeWeaponDecorator : BaseWeaponDecorator {
	[SerializeField]
	private Animation _animation;

	[SerializeField]
	private AudioClip _equipSound;

	[SerializeField]
	private AudioClip[] _impactSounds;

	[SerializeField]
	private AnimationClip[] _shootAnimClips;

	protected override void Awake() {
		base.Awake();
		IsMelee = true;
	}

	public override void ShowShootEffect(RaycastHit[] hits) {
		base.ShowShootEffect(hits);

		if (EnableShootAnimation && _animation && _shootAnimClips.Length > 0) {
			var num = UnityEngine.Random.Range(0, _shootAnimClips.Length);
			_animation.clip = _shootAnimClips[num];
			_animation.Rewind();
			_animation.Play();
		}
	}

	public override void PlayHitSound() { }

	public override void PlayEquipSound() {
		if (_mainAudioSource && _equipSound) {
			_mainAudioSource.volume = ((!ApplicationDataManager.ApplicationOptions.AudioEnabled) ? 0f : ApplicationDataManager.ApplicationOptions.AudioEffectsVolume);
			_mainAudioSource.PlayOneShot(_equipSound);
		}
	}

	protected override void EmitImpactSound(string impactType, Vector3 position) {
		if (_impactSounds != null && _impactSounds.Length > 0) {
			var num = UnityEngine.Random.Range(0, _impactSounds.Length);
			var audioClip = _impactSounds[num];

			if (audioClip) {
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(audioClip, position);
			} else {
				Debug.LogError("Missing impact sound for melee weapon!");
			}
		} else {
			Debug.LogError("Melee impact sound is not signed!");
		}
	}

	protected override void ShowImpactEffects(RaycastHit hit, Vector3 direction, Vector3 muzzlePosition, float distance, bool playSound) {
		StartCoroutine(StartShowImpactEffects(hit, direction, muzzlePosition, distance, playSound));
	}

	private IEnumerator StartShowImpactEffects(RaycastHit hit, Vector3 direction, Vector3 muzzlePosition, float distance, bool playSound) {
		yield return new WaitForSeconds(0.2f);

		EmitImpactParticles(hit, direction, muzzlePosition, distance, playSound);
	}
}
