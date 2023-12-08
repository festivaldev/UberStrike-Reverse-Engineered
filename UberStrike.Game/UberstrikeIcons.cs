using UnityEngine;

public static class UberstrikeIcons {
	public static Texture2D Waiting { get; private set; }
	public static Texture2D IconXP20x20 { get; private set; }
	public static Texture2D BlueLevel32 { get; private set; }
	public static Texture2D LevelUpPopup { get; private set; }
	public static Texture2D FacebookCreditsIcon { get; private set; }
	public static Texture2D LevelMastered { get; private set; }
	public static Texture2D FBScreenshotWatermark { get; private set; }
	public static Texture2D Time20x20 { get; private set; }

	static UberstrikeIcons() {
		Texture2DConfigurator component;

		try {
			component = GameObject.Find("UberstrikeIcons").GetComponent<Texture2DConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: UberstrikeIcons!");

			return;
		}

		Waiting = component.Assets[0];
		IconXP20x20 = component.Assets[1];
		BlueLevel32 = component.Assets[2];
		LevelUpPopup = component.Assets[3];
		FacebookCreditsIcon = component.Assets[4];
		LevelMastered = component.Assets[5];
		FBScreenshotWatermark = component.Assets[6];
		Time20x20 = component.Assets[7];
	}
}
