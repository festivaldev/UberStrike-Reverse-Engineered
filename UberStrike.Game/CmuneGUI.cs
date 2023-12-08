using UnityEngine;

public static class CmuneGUI {
	public static int HorizontalScrollbar(string title, int value, int min, int max) {
		float num = value;
		GUILayout.BeginHorizontal();
		GUILayout.Label(title);
		GUILayout.Space(10f);
		num = GUILayout.HorizontalScrollbar(value, 1f, min, max + 1);
		GUILayout.Space(10f);
		GUILayout.Label(string.Format("{0} [{1},{2}]", value, min, max));
		GUILayout.EndHorizontal();

		return (int)num;
	}

	public static float HorizontalScrollbar(string title, float value, int min, int max) {
		GUILayout.BeginHorizontal();
		GUILayout.Label(title);
		GUILayout.Space(10f);
		var num = GUILayout.HorizontalScrollbar(value, 1f, min, max + 1);
		GUILayout.Space(10f);
		GUILayout.Label(string.Format("{0} [{1},{2}]", value, min, max));
		GUILayout.EndHorizontal();

		return num;
	}
}
