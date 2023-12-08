using UnityEngine;

public static class GuiManager {
	public static void DrawTooltip() {
		if (!string.IsNullOrEmpty(GUI.tooltip)) {
			var matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			var vector = BlueStonez.tooltip.CalcSize(new GUIContent(GUI.tooltip));
			var rect = new Rect(Mathf.Clamp(Event.current.mousePosition.x, 14f, Screen.width - (vector.x + 14f)), Event.current.mousePosition.y + 24f, vector.x, vector.y + 16f);

			if (rect.yMax > Screen.height) {
				rect.x += 30f;
				rect.y += Screen.height - rect.yMax;
			}

			if (rect.xMax > Screen.width) {
				rect.x += Screen.width - rect.xMax;
			}

			GUI.Label(rect, GUI.tooltip, BlueStonez.tooltip);
			GUI.matrix = matrix;
		}
	}
}
