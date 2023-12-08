using UnityEngine;

public class WeaponShootAnimation : BaseWeaponEffect {
	[SerializeField]
	private Animation _shootAnimation;

	private void Awake() {
		if (_shootAnimation) {
			_shootAnimation.playAutomatically = false;
		}
	}

	public override void OnShoot() {
		if (_shootAnimation) {
			_shootAnimation.Rewind();
			_shootAnimation.Play();
		}
	}

	public override void OnPostShoot() { }
	public override void OnHits(RaycastHit[] hits) { }

	public override void Hide() {
		if (_shootAnimation && _shootAnimation.clip) {
			gameObject.SampleAnimation(_shootAnimation.clip, 0f);
			_shootAnimation.Stop();
		}
	}
}
