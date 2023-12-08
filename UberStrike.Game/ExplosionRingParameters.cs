using System;
using UnityEngine;

[Serializable]
public class ExplosionRingParameters {
	public float MaxLifeTime;
	public float MinLifeTime;
	public ParticleEmitter ParticleEmitter;
	public float StartSize;
}
