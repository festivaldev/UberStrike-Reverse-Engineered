using UnityEngine;

public static class ColorExtensions {
	public static Color SetAlpha(this Color color, float alpha) {
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static Color SetAlphaInt(this Color color, int alpha) {
		return new Color(color.r, color.g, color.b, alpha / 255f);
	}

	public static Color MultiplyAlpha(this Color color, float alpha) {
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static Color SetIntensity(this Color color, float intensity, float alpha) {
		return new Color(color.r * intensity, color.g * intensity, color.b * intensity, alpha);
	}
}
