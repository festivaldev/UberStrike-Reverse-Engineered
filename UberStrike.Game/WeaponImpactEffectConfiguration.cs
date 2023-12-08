using System;

[Serializable]
public class WeaponImpactEffectConfiguration {
	public ExplosionParameterSet ExplosionParameterSet;
	public FireParticleConfiguration FireParticleConfigurationForInstantHit;
	public SurfaceParameters SurfaceParameterSet;
	public TrailParticleConfiguration TrailParticleConfigurationForInstantHit;
	public MoveTrailrendererObject TrailrendererTrailPrefab;
	public bool UseTrailrendererForTrail;
}
