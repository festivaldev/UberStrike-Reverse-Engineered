using UnityEngine;

public static class CmunePrefs {
	public enum Key {
		Player_Name = 50,
		Player_Email,
		Player_Password,
		Player_AutoLogin = 54,
		Options_VideoIsUsingCustom = 95,
		Options_VideoMaxQueuedFrames,
		Options_VideoTextureQuality,
		Options_VideoVSyncCount,
		Options_VideoAntiAliasing,
		Options_VideoWaterMode = 101,
		Options_VideoCurrentQualityLevel,
		Options_VideoAdvancedWater,
		Options_VideoBloomAndFlares,
		Options_VideoColorCorrection,
		Options_VideoMotionBlur,
		Options_VideoPostProcessing = 118,
		Options_VideoShowFps,
		Options_InputXMouseSensitivity = 107,
		Options_InputYMouseSensitivity,
		Options_InputMouseRotationMaxX,
		Options_InputMouseRotationMaxY,
		Options_InputMouseRotationMinX,
		Options_InputMouseRotationMinY,
		Options_InputInvertMouse,
		Options_GameplayAutoPickupEnabled,
		Options_GameplayAutoEquipEnabled,
		Options_GameplayRagdollEnabled,
		Options_InputEnableGamepad,
		Options_AudioEnabled = 120,
		Options_AudioEffectsVolume,
		Options_AudioMusicVolume,
		Options_AudioMasterVolume,
		Options_VideoHardcoreMode,
		Options_VideoScreenRes,
		Options_VideoIsFullscreen,
		Keymap_None = 300,
		Keymap_HorizontalLook,
		Keymap_VerticalLook,
		Keymap_Forward,
		Keymap_Backward,
		Keymap_Left,
		Keymap_Right,
		Keymap_Jump,
		Keymap_Crouch,
		Keymap_PrimaryFire,
		Keymap_SecondaryFire,
		Keymap_Weapon1,
		Keymap_Weapon2,
		Keymap_Weapon3,
		Keymap_WeaponMelee = 315,
		Keymap_QuickItem1,
		Keymap_QuickItem2,
		Keymap_QuickItem3,
		Keymap_NextWeapon,
		Keymap_PrevWeapon,
		Keymap_Pause,
		Keymap_Fullscreen,
		Keymap_Tabscreen,
		Keymap_Chat,
		Keymap_Inventory,
		Keymap_UseQuickItem,
		Keymap_ChangeTeam,
		Keymap_NextQuickItem,
		Keymap_SendScreenshotToFacebook,
		Shop_RecentlyUsedItems = 400,
		App_ClientRegistered = 500
	}

	public static void Reset() {
		PlayerPrefs.DeleteAll();
	}

	public static bool TryGetKey<T>(Key k, out T value) {
		if (PlayerPrefs.HasKey(k.ToString())) {
			value = ReadKey(k, default(T));

			return true;
		}

		value = default(T);

		return false;
	}

	public static bool HasKey(Key k) {
		return PlayerPrefs.HasKey(k.ToString());
	}

	public static T ReadKey<T>(Key k, T defaultValue) {
		var t = defaultValue;

		if (typeof(T) == typeof(bool)) {
			t = (T)((object)(PlayerPrefs.GetInt(k.ToString(), (!(bool)((object)defaultValue)) ? 0 : 1) == 1));
		} else if (typeof(T) == typeof(int)) {
			t = (T)((object)PlayerPrefs.GetInt(k.ToString(), (int)((object)defaultValue)));
		} else if (typeof(T) == typeof(float)) {
			t = (T)((object)PlayerPrefs.GetFloat(k.ToString(), (float)((object)defaultValue)));
		} else if (typeof(T) == typeof(string)) {
			t = (T)((object)PlayerPrefs.GetString(k.ToString(), (string)((object)defaultValue)));
		} else {
			Debug.LogError(string.Format("Key {0} couldn't be read because type {1} not supported.", k, typeof(T)));
		}

		return t;
	}

	public static T ReadKey<T>(Key k) {
		return ReadKey(k, default(T));
	}

	public static void WriteKey<T>(Key k, T val) {
		if (typeof(T) == typeof(bool)) {
			PlayerPrefs.SetInt(k.ToString(), (!(bool)((object)val)) ? 0 : 1);
		} else if (typeof(T) == typeof(int)) {
			PlayerPrefs.SetInt(k.ToString(), (int)((object)val));
		} else if (typeof(T) == typeof(float)) {
			PlayerPrefs.SetFloat(k.ToString(), (float)((object)val));
		} else if (typeof(T) == typeof(string)) {
			PlayerPrefs.SetString(k.ToString(), (string)((object)val));
		} else {
			Debug.LogError(string.Format("Key {0} couldn't be read because type {1} not supported.", k, typeof(T)));
		}
	}
}
