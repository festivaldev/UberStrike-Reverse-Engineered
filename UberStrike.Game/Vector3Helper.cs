using UnityEngine;

public static class Vector3Helper {
	public static Vector3 SetX(this Vector3 vector, float x) {
		return new Vector3(x, vector.y, vector.z);
	}

	public static Vector3 SetY(this Vector3 vector, float y) {
		return new Vector3(vector.x, y, vector.z);
	}

	public static Vector3 SetZ(this Vector3 vector, float z) {
		return new Vector3(vector.x, vector.y, z);
	}

	public static Vector3 Add(this Vector3 vector, float x, float y, float z) {
		return new Vector3(vector.x + x, vector.y + y, vector.z + z);
	}

	public static Vector3 AddX(this Vector3 vector, float x) {
		return new Vector3(vector.x + x, vector.y, vector.z);
	}

	public static Vector3 AddY(this Vector3 vector, float y) {
		return new Vector3(vector.x, vector.y + y, vector.z);
	}

	public static Vector3 AddZ(this Vector3 vector, float z) {
		return new Vector3(vector.x, vector.y, vector.z + z);
	}
}
