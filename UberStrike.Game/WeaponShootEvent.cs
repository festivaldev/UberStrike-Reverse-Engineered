using UnityEngine;

public class WeaponShootEvent {
	public float Angle;
	public Vector3 Force;
	public float Noise;

	public WeaponShootEvent(Vector3 force, float noise, float angle) {
		Force = force;
		Noise = noise;
		Angle = angle;
	}
}
