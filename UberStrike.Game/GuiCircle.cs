using UnityEngine;

public static class GuiCircle {
	public enum Direction {
		Clockwise,
		CounterClockwise
	}

	private static Vector3 TexShift = new Vector3(0.5f, 0.5f, 0.5f);
	private static Vector3 Normal = new Vector3(0f, 0f, 1f);

	public static void DrawArc(Vector2 position, float angle, float radius, Material material) {
		DrawArc(position, 0f, angle, radius, material, Direction.Clockwise);
	}

	public static void DrawArc(Vector2 position, float startAngle, float fillAngle, float radius, Material material, Direction dir) {
		if (Event.current.type == EventType.Repaint) {
			GL.PushMatrix();
			material.SetPass(0);
			DrawSolidArc(new Vector3(position.x, position.y, 0f), fillAngle, radius, Quaternion.Euler(0f, 0f, startAngle), dir);
			GL.PopMatrix();
		}
	}

	private static void DrawSolidArc(Vector3 center, float angle, float radius, Quaternion rot, Direction dir) {
		var vector = rot * Vector3.down;
		var num = (int)Mathf.Clamp(angle * 0.1f, 5f, 30f);
		var num2 = 1f / (num - 1);
		var quaternion = Quaternion.AngleAxis(angle * num2, (dir != Direction.Clockwise) ? (-Normal) : Normal);
		var vector2 = vector * radius;
		var num3 = 1f / (2f * radius);
		var vector3 = new Vector3(num3, -num3, num3);
		GL.Begin(4);

		for (var i = 0; i < num - 1; i++) {
			var vector4 = vector2;
			vector2 = quaternion * vector2;
			GL.TexCoord(TexShift);
			GL.Vertex(center);

			if (dir == Direction.Clockwise) {
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector4, vector3));
				GL.Vertex(center + vector4);
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector2, vector3));
				GL.Vertex(center + vector2);
			} else {
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector2, vector3));
				GL.Vertex(center + vector2);
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector4, vector3));
				GL.Vertex(center + vector4);
			}
		}

		GL.End();
	}

	public static void DrawArcLine(Vector2 position, float startAngle, float fillAngle, float radius, float width, Material material, Direction dir) {
		if (Event.current.type == EventType.Repaint) {
			material.SetPass(0);
			DrawSolidArc(new Vector3(position.x, position.y, 0f), fillAngle, radius, width, Quaternion.Euler(0f, 0f, startAngle) * Vector3.down, dir);
		}
	}

	private static void DrawSolidArc(Vector3 center, float angle, float radius, float width, Vector3 from, Direction dir) {
		if (radius > 0f) {
			var num = (int)Mathf.Clamp(angle * 0.1f, 5f, 30f);
			var num2 = 1f / (num - 1);
			var num3 = 1f - Mathf.Clamp(width / radius, 0.001f, 1f);
			var quaternion = Quaternion.AngleAxis(angle * num2, (dir != Direction.Clockwise) ? (-Normal) : Normal);
			var vector = from * radius;
			GL.Begin(7);

			for (var i = 0; i < num - 1; i++) {
				var vector2 = vector;
				vector = quaternion * vector;

				if (dir == Direction.Clockwise) {
					GL.Vertex(center + vector2);
					GL.Vertex(center + vector);
					GL.Vertex(center + vector * num3);
					GL.Vertex(center + vector2 * num3);
				} else {
					GL.Vertex(center + vector);
					GL.Vertex(center + vector2);
					GL.Vertex(center + vector2 * num3);
					GL.Vertex(center + vector * num3);
				}
			}

			GL.End();
		}
	}
}
