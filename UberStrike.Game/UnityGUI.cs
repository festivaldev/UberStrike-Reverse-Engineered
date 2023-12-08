using UnityEngine;

public static class UnityGUI {
	public static int Toolbar(Rect position, int selected, GUIContent[] contents, int xCount, GUIStyle style) {
		var num = GUI.Toolbar(position, selected, contents, style);
		var controlID = GUIUtility.GetControlID(FocusType.Native, position);
		var typeForControl = Event.current.GetTypeForControl(controlID);

		if (typeForControl == EventType.Repaint) {
			GUIStyle guistyle = null;
			GUIStyle guistyle2 = null;
			GUIStyle guistyle3 = null;
			FindStyles(ref style, out guistyle, out guistyle2, out guistyle3, "left", "mid", "right");
			var num2 = contents.Length;
			var num3 = num2 / xCount;

			if (num2 % xCount != 0) {
				num3++;
			}

			float num4 = CalcTotalHorizSpacing(xCount, style, guistyle, guistyle2, guistyle3);
			float num5 = Mathf.Max(style.margin.top, style.margin.bottom) * (num3 - 1);
			var num6 = (position.width - num4) / xCount;
			var num7 = (position.height - num5) / num3;
			var array = CalcMouseRects(position, num2, xCount, num6, num7, style, guistyle, guistyle2, guistyle3, false);
			var buttonGridMouseSelection = GetButtonGridMouseSelection(array, Event.current.mousePosition, controlID == GUIUtility.hotControl);

			if (buttonGridMouseSelection >= 0) {
				GUI.tooltip = contents[buttonGridMouseSelection].tooltip;
			}
		}

		return num;
	}

	internal static GUIContent[] Temp(string[] texts) {
		var array = new GUIContent[texts.Length];

		for (var i = 0; i < texts.Length; i++) {
			array[i] = new GUIContent(texts[i]);
		}

		return array;
	}

	public static int Toolbar(Rect position, int selected, string[] contents, int length, GUIStyle style) {
		return Toolbar(position, selected, Temp(contents), length, style);
	}

	public static int Toolbar(Rect position, int selected, GUIContent[] contents, GUIStyle style) {
		return Toolbar(position, selected, contents, contents.Length, style);
	}

	internal static void FindStyles(ref GUIStyle style, out GUIStyle firstStyle, out GUIStyle midStyle, out GUIStyle lastStyle, string first, string mid, string last) {
		if (style == null) {
			style = GUI.skin.button;
		}

		var name = style.name;
		midStyle = GUI.skin.FindStyle(name + mid);

		if (midStyle == null) {
			midStyle = style;
		}

		firstStyle = GUI.skin.FindStyle(name + first);

		if (firstStyle == null) {
			firstStyle = midStyle;
		}

		lastStyle = GUI.skin.FindStyle(name + last);

		if (lastStyle == null) {
			lastStyle = midStyle;
		}
	}

	private static Rect[] CalcMouseRects(Rect position, int count, int xCount, float elemWidth, float elemHeight, GUIStyle style, GUIStyle firstStyle, GUIStyle midStyle, GUIStyle lastStyle, bool addBorders) {
		var num = 0;
		var num2 = 0;
		var num3 = position.xMin;
		var num4 = position.yMin;
		var guistyle = style;
		var array = new Rect[count];

		if (count > 1) {
			guistyle = firstStyle;
		}

		for (var i = 0; i < count; i++) {
			if (addBorders) {
				array[i] = guistyle.margin.Add(new Rect(num3, num4, elemWidth, elemHeight));
			} else {
				array[i] = new Rect(num3, num4, elemWidth, elemHeight);
			}

			array[i].width = Mathf.Round(array[i].xMax) - Mathf.Round(array[i].x);
			array[i].x = Mathf.Round(array[i].x);
			var guistyle2 = midStyle;

			if (i == count - 2) {
				guistyle2 = lastStyle;
			}

			num3 = num3 + elemWidth + Mathf.Max(guistyle.margin.right, guistyle2.margin.left);
			num2++;

			if (num2 >= xCount) {
				num++;
				num2 = 0;
				num4 = num4 + elemHeight + Mathf.Max(style.margin.top, style.margin.bottom);
				num3 = position.xMin;
			}
		}

		return array;
	}

	private static int GetButtonGridMouseSelection(Rect[] buttonRects, Vector2 mousePos, bool findNearest) {
		for (var i = 0; i < buttonRects.Length; i++) {
			if (buttonRects[i].Contains(mousePos)) {
				return i;
			}
		}

		if (findNearest) {
			var num = 10000000f;
			var num2 = -1;

			for (var j = 0; j < buttonRects.Length; j++) {
				var rect = buttonRects[j];
				var vector = new Vector2(Mathf.Clamp(mousePos.x, rect.xMin, rect.xMax), Mathf.Clamp(mousePos.y, rect.yMin, rect.yMax));
				var sqrMagnitude = (mousePos - vector).sqrMagnitude;

				if (sqrMagnitude < num) {
					num2 = j;
					num = sqrMagnitude;
				}
			}

			return num2;
		}

		return -1;
	}

	internal static int CalcTotalHorizSpacing(int xCount, GUIStyle style, GUIStyle firstStyle, GUIStyle midStyle, GUIStyle lastStyle) {
		if (xCount < 2) {
			return 0;
		}

		if (xCount != 2) {
			var num = Mathf.Max(midStyle.margin.left, midStyle.margin.right);

			return Mathf.Max(firstStyle.margin.right, midStyle.margin.left) + Mathf.Max(midStyle.margin.right, lastStyle.margin.left) + num * (xCount - 3);
		}

		return Mathf.Max(firstStyle.margin.right, lastStyle.margin.left);
	}
}
