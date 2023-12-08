using System;
using UnityEngine;

[Serializable]
public class EnviromentSettings {
	public enum TYPE {
		NONE,
		WATER,
		SURFACE,
		LATTER
	}

	public float AirAcceleration = 3f;
	public Bounds EnviromentBounds;
	public float FlyAcceleration = 8f;
	public float FlyFriction = 3f;
	public float Gravity = 50f;
	public float GroundAcceleration = 15f;
	public float GroundFriction = 8f;
	public float SpectatorFriction = 5f;
	public float StopSpeed = 8f;
	public TYPE Type;
	public float WaterAcceleration = 6f;
	public float WaterFriction = 2f;

	public void CheckPlayerEnclosure(Vector3 position, float height, out float enclosure) {
		if (EnviromentBounds.Contains(position)) {
			var vector = position + Vector3.up * height;
			float num;

			if (EnviromentBounds.IntersectRay(new Ray(vector, Vector3.down), out num)) {
				enclosure = (height - num) / height;
			} else {
				enclosure = 0f;
			}
		} else {
			enclosure = 0f;
		}
	}

	public override string ToString() {
		return string.Format("Type: ", Type);
	}
}
