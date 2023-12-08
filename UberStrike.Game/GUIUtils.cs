using UnityEngine;

public static class GUIUtils {
	public static Color ColorBlack = ColorFromInt(0, 0, 0, 140);
	public static Color ColorBlackActive = ColorFromInt(40, 40, 40, 140);
	public static Color ColorBlackPressed = ColorFromInt(0, 0, 0, 70);
	public static Color ColorRed = ColorFromInt(255, 60, 48);
	public static Color ColorRedActive = ColorFromInt(255, 77, 77);
	public static Color ColorRedPressed = ColorFromInt(255, 60, 48, 140);
	public static Color ColorYellow = ColorFromInt(247, 148, 29);
	public static Color ColorYellowActive = ColorFromInt(255, 202, 42);
	public static Color ColorYellowPressed = ColorFromInt(247, 148, 29, 140);
	public static Color ColorBlue = ColorFromInt(0, 167, 209);
	public static Color ColorBlueActive = ColorFromInt(0, 204, 255);
	public static Color ColorBluePressed = ColorFromInt(0, 167, 209, 140);
	public static Color ColorGreen = ColorFromInt(0, 180, 97);
	public static Color ColorGreenActive = ColorFromInt(0, 207, 110);
	public static Color ColorGreenPressed = ColorFromInt(0, 180, 97, 140);

	public static Color ColorFromInt(int r, int g, int b, int alpha = 255) {
		return new Color(r / 255f, g / 255f, b / 255f, alpha / 255f);
	}

	public static string ColorToNGuiModifier(Color color) {
		var num = (int)(255f * color.r);
		var num2 = (int)(255f * color.g);
		var num3 = (int)(255f * color.b);

		return string.Concat("[", num.ToString("X2"), num2.ToString("X2"), num3.ToString("X2"), "]");
	}
}
