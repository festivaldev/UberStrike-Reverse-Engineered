using System;
using UnityEngine;

[Serializable]
public class ExplosionDustParameters {
	public float MaxLifeTime;
	public float MaxSize;
	public float MaxStartPositionSize;
	public float MinLifeTime;
	public float MinSize;
	public float MinStartPositionSize;
	public int ParticleCount;
	public ParticleEmitter ParticleEmitter;
}
