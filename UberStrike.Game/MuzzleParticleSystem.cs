using UnityEngine;

public class MuzzleParticleSystem : BaseWeaponEffect {
	private ParticleSystem _particleSystem;

	private void Awake() {
		_particleSystem = GetComponent<ParticleSystem>();
	}

	public override void OnShoot() {
		if (_particleSystem) {
			_particleSystem.Play();
		}
	}

	public override void OnPostShoot() { }
	public override void Hide() { }
	public override void OnHits(RaycastHit[] hits) { }
}
