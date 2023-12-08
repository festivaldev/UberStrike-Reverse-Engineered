using UnityEngine;

public class TouchFinger {
	public int FingerId;
	public Vector2 LastPos;
	public Vector2 StartPos;
	public float StartTouchTime;

	public TouchFinger() {
		Reset();
	}

	public void Reset() {
		StartPos = Vector2.zero;
		LastPos = Vector2.zero;
		StartTouchTime = 0f;
		FingerId = -1;
	}
}
