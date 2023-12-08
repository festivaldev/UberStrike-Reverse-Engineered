using UnityEngine;

public class MathUtils {
	public static float GetQuatLength(Quaternion q) {
		return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
	}

	public static Quaternion GetQuatConjugate(Quaternion q) {
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	public static Quaternion GetQuatLog(Quaternion q) {
		var quaternion = q;
		quaternion.w = 0f;

		if (Mathf.Abs(q.w) < 1f) {
			var num = Mathf.Acos(q.w);
			var num2 = Mathf.Sin(num);

			if (Mathf.Abs(num2) > 0.0001) {
				var num3 = num / num2;
				quaternion.x = q.x * num3;
				quaternion.y = q.y * num3;
				quaternion.z = q.z * num3;
			}
		}

		return quaternion;
	}

	public static Quaternion GetQuatExp(Quaternion q) {
		var quaternion = q;
		var num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
		var num2 = Mathf.Sin(num);
		quaternion.w = Mathf.Cos(num);

		if (Mathf.Abs(num2) > 0.0001) {
			var num3 = num2 / num;
			quaternion.x = num3 * q.x;
			quaternion.y = num3 * q.y;
			quaternion.z = num3 * q.z;
		}

		return quaternion;
	}

	public static Quaternion GetQuatSquad(float t, Quaternion q0, Quaternion q1, Quaternion a0, Quaternion a1) {
		var num = 2f * t * (1f - t);
		var quaternion = Slerp(q0, q1, t);
		var quaternion2 = Slerp(a0, a1, t);

		return Slerp(quaternion, quaternion2, num);
	}

	public static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2) {
		var quatConjugate = GetQuatConjugate(q1);
		var quatLog = GetQuatLog(quatConjugate * q0);
		var quatLog2 = GetQuatLog(quatConjugate * q2);
		var quaternion = new Quaternion(-0.25f * (quatLog.x + quatLog2.x), -0.25f * (quatLog.y + quatLog2.y), -0.25f * (quatLog.z + quatLog2.z), -0.25f * (quatLog.w + quatLog2.w));

		return q1 * GetQuatExp(quaternion);
	}

	public static float Ease(float t, float k1, float k2) {
		var num = k1 * 2f / 3.1415927f + k2 - k1 + (1f - k2) * 2f / 3.1415927f;
		float num2;

		if (t < k1) {
			num2 = k1 * 0.63661975f * (Mathf.Sin(t / k1 * 3.1415927f / 2f - 1.5707964f) + 1f);
		} else if (t < k2) {
			num2 = 2f * k1 / 3.1415927f + t - k1;
		} else {
			num2 = 2f * k1 / 3.1415927f + k2 - k1 + (1f - k2) * 0.63661975f * Mathf.Sin((t - k2) / (1f - k2) * 3.1415927f / 2f);
		}

		return num2 / num;
	}

	public static Quaternion Slerp(Quaternion p, Quaternion q, float t) {
		var num = Quaternion.Dot(p, q);
		Quaternion quaternion;

		if (1f + num > 1E-05) {
			float num4;
			float num5;

			if (1f - num > 1E-05) {
				var num2 = Mathf.Acos(num);
				var num3 = 1f / Mathf.Sin(num2);
				num4 = Mathf.Sin((1f - t) * num2) * num3;
				num5 = Mathf.Sin(t * num2) * num3;
			} else {
				num4 = 1f - t;
				num5 = t;
			}

			quaternion.x = num4 * p.x + num5 * q.x;
			quaternion.y = num4 * p.y + num5 * q.y;
			quaternion.z = num4 * p.z + num5 * q.z;
			quaternion.w = num4 * p.w + num5 * q.w;
		} else {
			var num6 = Mathf.Sin((1f - t) * 3.1415927f * 0.5f);
			var num7 = Mathf.Sin(t * 3.1415927f * 0.5f);
			quaternion.x = num6 * p.x - num7 * p.y;
			quaternion.y = num6 * p.y + num7 * p.x;
			quaternion.z = num6 * p.z - num7 * p.w;
			quaternion.w = p.z;
		}

		return quaternion;
	}
}
