using UnityEngine;

public static class Mathfx {
	public const float PI = 3.14159f;
	public const float FOUR_PI = 12.56636f;
	public const float TWO_PI = 6.28318f;
	public const float PI_HALF = 1.570795f;
	public const float PI_FOURTH = 0.7853975f;
	private static readonly Quaternion _rotate90 = Quaternion.AngleAxis(90f, Vector3.up);

	public static float NormAmplitude(float a) {
		return (a + 1f) * 0.5f;
	}

	public static float Hermite(float start, float end, float value) {
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	public static float Gauss(float start, float end, float value) {
		return Mathf.Lerp(start, end, (1f + Mathf.Cos(value - 3.1415927f)) / 2f);
	}

	public static float Sinerp(float start, float end, float value) {
		return Mathf.Lerp(start, end, Mathf.Sin(value * 3.1415927f * 0.5f));
	}

	public static float Berp(float start, float end, float value) {
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));

		return start + (end - start) * value;
	}

	public static float SmoothStep(float x, float min, float max) {
		x = Mathf.Clamp(x, min, max);
		var num = (x - min) / (max - min);
		var num2 = (x - min) / (max - min);

		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static float Lerp(float start, float end, float value) {
		return (1f - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
		var vector = Vector3.Normalize(lineEnd - lineStart);
		var num = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);

		return lineStart + num * vector;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
		var vector = lineEnd - lineStart;
		var vector2 = Vector3.Normalize(vector);
		var num = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);

		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	public static float Bounce(float x) {
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	public static bool Approx(float val, float about, float range) {
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range) {
		return (val - about).sqrMagnitude < range * range;
	}

	public static float ProjectedAngle(Vector3 a, Vector3 b) {
		var num = Vector3.Angle(a, b);

		return (Vector3.Dot(a, _rotate90 * b) >= 0f) ? num : (360f - num);
	}

	public static Vector3 ProjectVector3(Vector3 v, Vector3 normal) {
		return v - Vector3.Dot(v, normal) * normal;
	}

	public static float ClampAngle(float angle, float min, float max) {
		return Mathf.Clamp(angle % 360f, min, max);
	}

	public static int Sign(float s) {
		return (s != 0f) ? ((s >= 0f) ? 1 : (-1)) : 0;
	}

	public static short Clamp(short v, short min, short max) {
		if (v < min) {
			return min;
		}

		if (v > max) {
			return max;
		}

		return v;
	}

	public static int Clamp(int v, int min, int max) {
		if (v < min) {
			return min;
		}

		if (v > max) {
			return max;
		}

		return v;
	}

	public static float Clamp(float v, float min, float max) {
		if (v < min) {
			return min;
		}

		if (v > max) {
			return max;
		}

		return v;
	}

	public static byte Clamp(byte v, byte min, byte max) {
		if (v < min) {
			return min;
		}

		if (v > max) {
			return max;
		}

		return v;
	}

	public static float Ease(float t, EaseType easeType) {
		switch (easeType) {
			case EaseType.In:
				return Mathf.Lerp(0f, 1f, 1f - Mathf.Cos(t * 3.1415927f * 0.5f));
			case EaseType.Out:
				return Mathf.Lerp(0f, 1f, Mathf.Sin(t * 3.1415927f * 0.5f));
			case EaseType.InOut:
				return Mathf.SmoothStep(0f, 1f, t);
			case EaseType.Berp:
				return Berp(0f, 1f, t);
			default:
				return t;
		}
	}

	public static Vector2 RotateVector2AboutPoint(Vector2 input, Vector2 center, float degRotate) {
		var vector = Quaternion.AngleAxis(degRotate, new Vector3(0f, 0f, 1f)) * (input - center);

		return center + new Vector2(vector.x, vector.y);
	}
}
