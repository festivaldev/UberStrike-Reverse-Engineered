using UnityEngine;

public static class MobileIcons {
	public static Material TextureAtlas { get; private set; }
	public static Rect TouchArrowLeft { get; private set; }
	public static Rect TouchArrowRight { get; private set; }
	public static Rect TouchChatButton { get; private set; }
	public static Rect TouchCrouchButton { get; private set; }
	public static Rect TouchCrouchButtonActive { get; private set; }
	public static Rect TouchFireButton { get; private set; }
	public static Rect TouchJumpButton { get; private set; }
	public static Rect TouchKeyboardDpad { get; private set; }
	public static Rect TouchMenuButton { get; private set; }
	public static Rect TouchMoveInner { get; private set; }
	public static Rect TouchMoveOuter { get; private set; }
	public static Rect TouchScoreboardButton { get; private set; }
	public static Rect TouchSecondFireButton { get; private set; }
	public static Rect TouchWeaponCannon { get; private set; }
	public static Rect TouchWeaponHandgun { get; private set; }
	public static Rect TouchWeaponLauncher { get; private set; }
	public static Rect TouchWeaponMachinegun { get; private set; }
	public static Rect TouchWeaponMelee { get; private set; }
	public static Rect TouchWeaponShotgun { get; private set; }
	public static Rect TouchWeaponSniperrifle { get; private set; }
	public static Rect TouchWeaponSplattergun { get; private set; }
	public static Rect TouchZoomScrollbar { get; private set; }

	static MobileIcons() {
		Texture2DAtlasHolder component;

		try {
			component = GameObject.Find("MobileIcons").GetComponent<Texture2DAtlasHolder>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: MobileIcons!");

			return;
		}

		TextureAtlas = component.Atlas;
		TouchArrowLeft = new Rect(0.1582031f, 0.8828125f, 0.01074219f, 0.015625f);
		TouchArrowRight = new Rect(0.1582031f, 0.89941406f, 0.01074219f, 0.015625f);
		TouchChatButton = new Rect(0.1132813f, 0.8828125f, 0.04394531f, 0.043945316f);
		TouchCrouchButton = new Rect(0.6962891f, 0.5f, 0.07421875f, 0.0732422f);
		TouchCrouchButtonActive = new Rect(0.7714844f, 0.5f, 0.07421875f, 0.0732422f);
		TouchFireButton = new Rect(0f, 0.8828125f, 0.1123047f, 0.11132815f);
		TouchJumpButton = new Rect(0.5439453f, 0.5f, 0.07617188f, 0.07421875f);
		TouchKeyboardDpad = new Rect(0f, 0.5f, 0.390625f, 0.20410155f);
		TouchMenuButton = new Rect(0.1132813f, 0.9277344f, 0.04394531f, 0.043945316f);
		TouchMoveInner = new Rect(0.3916016f, 0.8808594f, 0.09082031f, 0.0908203f);
		TouchMoveOuter = new Rect(0f, 0.7050781f, 0.1767578f, 0.1767578f);
		TouchScoreboardButton = new Rect(0.3300781f, 0.7050781f, 0.04394531f, 0.043945316f);
		TouchSecondFireButton = new Rect(0.6210938f, 0.5f, 0.07421875f, 0.07421875f);
		TouchWeaponCannon = new Rect(0.1777344f, 0.7050781f, 0.1513672f, 0.0751953f);
		TouchWeaponHandgun = new Rect(0.1777344f, 0.78125f, 0.1513672f, 0.0751953f);
		TouchWeaponLauncher = new Rect(0.1777344f, 0.8574219f, 0.1513672f, 0.0751953f);
		TouchWeaponMachinegun = new Rect(0.3916016f, 0.5f, 0.1513672f, 0.0751953f);
		TouchWeaponMelee = new Rect(0.3916016f, 0.5761719f, 0.1513672f, 0.0751953f);
		TouchWeaponShotgun = new Rect(0.3916016f, 0.65234375f, 0.1513672f, 0.0751953f);
		TouchWeaponSniperrifle = new Rect(0.3916016f, 0.7285156f, 0.1513672f, 0.0751953f);
		TouchWeaponSplattergun = new Rect(0.3916016f, 0.8046875f, 0.1513672f, 0.0751953f);
		TouchZoomScrollbar = new Rect(0.5439453f, 0.5751953f, 0.02636719f, 0.18652345f);
	}
}
