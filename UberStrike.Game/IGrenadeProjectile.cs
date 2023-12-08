using System;
using UnityEngine;

public interface IGrenadeProjectile : IProjectile {
	Vector3 Position { get; }
	Vector3 Velocity { get; }
	event Action<IGrenadeProjectile> OnProjectileEmitted;
	event Action<IGrenadeProjectile> OnProjectileExploded;
	IGrenadeProjectile Throw(Vector3 position, Vector3 velocity);
	void SetLayer(UberstrikeLayer layer);
}
