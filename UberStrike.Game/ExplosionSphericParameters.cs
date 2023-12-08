using System;
using UnityEngine;

[Serializable]
public class ExplosionSphericParameters {
	public float MaxLifeTime;
	public float MaxSize;
	public float MinLifeTime;
	public float MinSize;
	public int ParticleCount;
	public ParticleEmitter ParticleEmitter;
	public float Speed;
}
