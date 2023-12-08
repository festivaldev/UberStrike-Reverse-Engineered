using UnityEngine;

public class ParticleConfigurationPerWeapon : MonoBehaviour {
	[SerializeField]
	private WeaponImpactEffectConfiguration _weaponImpactEffectConfiguration;

	public WeaponImpactEffectConfiguration WeaponImpactEffectConfiguration {
		get { return _weaponImpactEffectConfiguration; }
	}
}
