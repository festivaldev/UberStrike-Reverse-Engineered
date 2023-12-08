using System.Globalization;
using UnityEngine;

public static class ColorConverter {
	public static float GetHue(Color c) {
		float num;

		if (c.r == 0f) {
			num = 2f;
			num += ((c.b >= 1f) ? (2f - c.g) : c.b);
		} else if (c.g == 0f) {
			num = 4f;
			num += ((c.r >= 1f) ? (2f - c.b) : c.r);
		} else {
			num = 0f;
			num += ((c.g >= 1f) ? (2f - c.r) : c.g);
		}

		return num;
	}

	public static Color GetColor(float hue) {
		hue %= 6f;
		var white = Color.white;

		if (hue < 1f) {
			white = new Color(1f, hue, 0f);
		} else if (hue < 2f) {
			white = new Color(2f - hue, 1f, 0f);
		} else if (hue < 3f) {
			white = new Color(0f, 1f, hue - 2f);
		} else if (hue < 4f) {
			white = new Color(0f, 4f - hue, 1f);
		} else if (hue < 5f) {
			white = new Color(hue - 4f, 0f, 1f);
		} else {
			white = new Color(1f, 0f, 6f - hue);
		}

		return white;
	}

	public static Color HexToColor(string hexString) {
		int num;

		try {
			num = int.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
		} catch {
			num = 255;
		}

		int num2;

		try {
			num2 = int.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
		} catch {
			num2 = 255;
		}

		int num3;

		try {
			num3 = int.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
		} catch {
			num3 = 255;
		}

		return new Color(num / 255f, num2 / 255f, num3 / 255f);
	}

	public static string ColorToHex(Color color) {
		var text = ((int)(color.r * 255f)).ToString("X2");
		var text2 = ((int)(color.g * 255f)).ToString("X2");
		var text3 = ((int)(color.b * 255f)).ToString("X2");

		return text + text2 + text3;
	}

	public static Color RgbToColor(float r, float g, float b) {
		return new Color(r / 255f, g / 255f, b / 255f);
	}

	public static Color RgbaToColor(float r, float g, float b, float a) {
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}
}
