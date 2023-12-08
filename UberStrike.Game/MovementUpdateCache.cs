using UnityEngine;

internal class MovementUpdateCache {
	private float lastUpdate;
	public Vector3 Position { get; private set; }
	public byte HRotation { get; private set; }
	public byte VRotation { get; private set; }
	public byte MovementState { get; private set; }

	public bool Update(Vector3 position, byte hAngle, byte vAngle, byte moveState) {
		if (Position != position || MovementState != moveState) {
			lastUpdate = Time.time;
		} else if ((HRotation != hAngle || VRotation != vAngle) && lastUpdate + 0.5f < Time.time) {
			lastUpdate = Time.time;
		}

		Position = position;
		MovementState = moveState;
		HRotation = hAngle;
		VRotation = vAngle;

		return lastUpdate < Time.time + 1f;
	}
}
