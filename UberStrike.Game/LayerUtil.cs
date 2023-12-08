using System;
using UnityEngine;

public static class LayerUtil {
	public static int LayerMaskEverything {
		get { return -1; }
	}

	public static int LayerMaskNothing {
		get { return 0; }
	}

	public static void ValidateUberstrikeLayers() {
		for (var i = 0; i < 32; i++) {
			if (i != 2) {
				if (!string.IsNullOrEmpty(LayerMask.LayerToName(i))) {
					if (Enum.GetName(typeof(UberstrikeLayer), i) != LayerMask.LayerToName(i)) {
						Debug.LogError("Editor layer '" + LayerMask.LayerToName(i) + "' is not defined in the UberstrikeLayer enum!");
					}
				} else if (!string.IsNullOrEmpty(Enum.GetName(typeof(UberstrikeLayer), i))) {
					throw new Exception("UberstrikeLayer mismatch with Editor on layer: " + Enum.GetName(typeof(UberstrikeLayer), i));
				}
			}
		}
	}

	public static int CreateLayerMask(params UberstrikeLayer[] layers) {
		var num = 0;

		foreach (int num2 in layers) {
			num |= 1 << num2;
		}

		return num;
	}

	public static int AddToLayerMask(int mask, params UberstrikeLayer[] layers) {
		foreach (int num in layers) {
			mask |= 1 << num;
		}

		return mask;
	}

	public static int RemoveFromLayerMask(int mask, params UberstrikeLayer[] layers) {
		foreach (int num in layers) {
			mask &= ~(1 << num);
		}

		return mask;
	}

	public static void SetLayerRecursively(Transform transform, UberstrikeLayer layer) {
		SetLayerRecursively(transform, (int)layer);
	}

	public static void SetLayerRecursively(Transform transform, int layer) {
		foreach (var transform2 in transform.GetComponentsInChildren<Transform>(true)) {
			transform2.gameObject.layer = layer;
		}
	}

	public static int GetLayer(UberstrikeLayer layer) {
		return (int)layer;
	}

	public static bool IsLayerInMask(int mask, int layer) {
		return (mask & (1 << layer)) != 0;
	}

	public static bool IsLayerInMask(int mask, UberstrikeLayer layer) {
		return (mask & (1 << (int)layer)) != 0;
	}
}
