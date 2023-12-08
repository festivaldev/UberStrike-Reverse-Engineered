using System;
using UnityEngine;

public static class TagUtil {
	public static string GetTag(Collider c) {
		var text = "Cement";

		try {
			if (c) {
				text = c.tag;
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to get tag from collider: " + ex.Message);
		}

		return text;
	}
}
