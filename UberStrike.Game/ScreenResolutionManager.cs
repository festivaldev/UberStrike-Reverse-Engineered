using System.Collections.Generic;
using UnityEngine;

public static class ScreenResolutionManager {
	private static List<Resolution> resolutions = new List<Resolution>();

	private static bool IsHighestResolution {
		get { return CurrentResolutionIndex == resolutions.Count - 1; }
	}

	public static List<Resolution> Resolutions {
		get { return resolutions; }
	}

	public static int InitialResolutionIndex {
		get { return resolutions.Count - 1; }
	}

	public static int CurrentResolutionIndex {
		get { return resolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height); }
	}

	public static bool IsFullScreen {
		get { return Screen.fullScreen; }
		set {
			if (!Application.isWebPlayer && !value && IsHighestResolution) {
				SetTwoMinusMaxResolution();
			} else {
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value);
			}

			ApplicationDataManager.ApplicationOptions.IsFullscreen = value;
			ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
		}
	}

	static ScreenResolutionManager() {
		foreach (var resolution in Screen.resolutions) {
			if (resolution.width > 800) {
				resolutions.Add(resolution);
			}
		}

		if (resolutions.Count == 0) {
			resolutions.Add(Screen.currentResolution);
		}
	}

	public static void SetResolution(int index, bool fullscreen) {
		var num = resolutions.Count - 1;
		var num2 = Mathf.Clamp(index, 0, num);

		if (!Application.isWebPlayer && num2 == num && !fullscreen) {
			fullscreen = true;
		}

		if (num2 >= 0 && num2 < resolutions.Count) {
			Screen.SetResolution(resolutions[num2].width, resolutions[num2].height, fullscreen);
			ApplicationDataManager.ApplicationOptions.ScreenResolution = num2;
		}
	}

	public static void SetTwoMinusMaxResolution() {
		if (Application.isWebPlayer) {
			Debug.LogError("SetOneMinusMaxResolution() should only be called from the desktop client");

			return;
		}

		if (resolutions.Count > 2) {
			var vector = new Vector2(resolutions[resolutions.Count - 3].width, resolutions[resolutions.Count - 3].height);
			Screen.SetResolution((int)vector.x, (int)vector.y, false);
		} else if (resolutions.Count == 2) {
			var vector2 = new Vector2(resolutions[1].width, resolutions[1].height);
			Screen.SetResolution((int)vector2.x, (int)vector2.y, false);
		} else if (resolutions.Count == 1) {
			var vector3 = new Vector2(resolutions[0].width, resolutions[0].height);
			Screen.SetResolution((int)vector3.x, (int)vector3.y, false);
		} else {
			Debug.LogError("ScreenResolutionManager: Screen.resolutions does not contain any supported resolutions.");
		}
	}

	public static void SetFullScreenMaxResolution() {
		if (resolutions.Count == 0) {
			Debug.LogError("SetFullScreenMaxResolution: No suitable resolution available in the Resolutions array.");

			return;
		}

		var num = resolutions.Count - 1;
		var vector = new Vector2(resolutions[num].width, resolutions[num].height);

		if (!Screen.fullScreen) {
			Screen.SetResolution((int)vector.x, (int)vector.y, true);
			ApplicationDataManager.ApplicationOptions.ScreenResolution = num;
		}
	}

	public static void DecreaseResolution() {
		SetResolution(CurrentResolutionIndex - 1, Screen.fullScreen);
	}

	public static void IncreaseResolution() {
		SetResolution(CurrentResolutionIndex + 1, Screen.fullScreen);
	}
}
