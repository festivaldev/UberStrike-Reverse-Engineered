using System;
using UnityEngine;

public class GuiLockController : AutoMonoBehaviour<GuiLockController> {
	public static bool IsApplicationLocked { get; private set; }
	public static float Alpha { get; private set; }
	public static GuiDepth LockingDepth { get; private set; }
	public static bool IsEnabled { get; private set; }

	private void Awake() {
		enabled = false;
		Alpha = 0.6f;
	}

	public static void LockApplication() {
		IsApplicationLocked = true;
		LockingDepth = GuiDepth.Popup;
		Instance.enabled = true;
		EnableNguiControls(false);
	}

	public static bool IsLocked(params GuiDepth[] levels) {
		if (IsEnabled) {
			return Array.Exists(levels, l => l == LockingDepth);
		}

		return false;
	}

	public static void EnableLock(GuiDepth depth) {
		if (IsApplicationLocked) {
			return;
		}

		if (!IsEnabled || LockingDepth > depth) {
			LockingDepth = depth;
			IsEnabled = true;
			Instance.enabled = IsEnabled;
			EnableNguiControls(false);
		}
	}

	public static void ReleaseLock(GuiDepth depth) {
		if (IsApplicationLocked) {
			return;
		}

		if (IsEnabled && LockingDepth == depth) {
			IsEnabled = false;
			Instance.enabled = IsEnabled;
			EnableNguiControls(true);
		}
	}

	private static void EnableNguiControls(bool enable) {
		if (UICamera.eventHandler) {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
				UICamera.eventHandler.useTouch = enable;
			} else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360) {
				UICamera.eventHandler.useController = enable;
			} else {
				UICamera.eventHandler.useMouse = enable;
				UICamera.eventHandler.useKeyboard = enable;
			}
		}
	}

	private void OnGUI() {
		GUI.depth = (int)(LockingDepth + 1);

		if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) {
			Event.current.Use();
		}

		GUI.color = new Color(1f, 1f, 1f, Alpha);
		GUI.Button(new Rect(0f, 0f, Screen.width + 5, Screen.height + 5), string.Empty, BlueStonez.box_grey31);
		GUI.color = Color.white;
	}
}
