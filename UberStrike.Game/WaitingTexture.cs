using UnityEngine;

public static class WaitingTexture {
	public static int Angle {
		get { return Mathf.RoundToInt(Time.time * 10f) * 30; }
	}

	public static void Draw(Vector2 position, int size = 0) {
		if (size <= 0) {
			size = 32;
		} else {
			size = Mathf.Clamp(size, 1, 32);
		}

		GUIUtility.RotateAroundPivot(Angle, position);
		GUI.DrawTexture(new Rect(position.x - size * 0.5f, position.y - size * 0.5f, size, size), UberstrikeIcons.Waiting);
		GUI.matrix = Matrix4x4.identity;
	}
}
