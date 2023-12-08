using UnityEngine;

public static class MouseInput {
	public const float DoubleClickInterval = 0.3f;
	private static Click Current;
	private static Click Previous;

	static MouseInput() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGUI;
	}

	public static bool IsDoubleClick() {
		return Time.time - Previous.Time < 0.3f && Current.Point == Previous.Point;
	}

	public static bool IsMouseClickIn(Rect rect, int mouse = 0) {
		return Event.current.type == EventType.MouseDown && Event.current.button == mouse && rect.Contains(Event.current.mousePosition);
	}

	private static void OnGUI() {
		if (Event.current.type == EventType.MouseDown) {
			Previous = Current;
			Current.Time = Time.time;
			Current.Point = Event.current.mousePosition;
			Current.Button = Event.current.button;
		}
	}

	private struct Click {
		public float Time;
		public Vector2 Point;
		public int Button;
	}
}
