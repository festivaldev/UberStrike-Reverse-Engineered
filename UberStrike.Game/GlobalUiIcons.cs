using UnityEngine;

public static class GlobalUiIcons {
	public static Texture2D QuadpanelButtonFullscreen { get; private set; }
	public static Texture2D QuadpanelButtonNormalize { get; private set; }
	public static Texture2D QuadpanelButtonModerate { get; private set; }
	public static Texture2D QuadpanelButtonSoundoff { get; private set; }
	public static Texture2D QuadpanelButtonSoundon { get; private set; }
	public static Texture2D QuadpanelButtonReportplayer { get; private set; }
	public static Texture2D QuadpanelButtonOptions { get; private set; }
	public static Texture2D QuadpanelButtonHelp { get; private set; }
	public static Texture2D NewInboxMessage { get; private set; }
	public static Texture2D QuadpanelButtonLogout { get; private set; }

	static GlobalUiIcons() {
		Texture2DConfigurator component;

		try {
			component = GameObject.Find("GlobalUiIcons").GetComponent<Texture2DConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: GlobalUiIcons!");

			return;
		}

		QuadpanelButtonFullscreen = component.Assets[0];
		QuadpanelButtonNormalize = component.Assets[1];
		QuadpanelButtonModerate = component.Assets[2];
		QuadpanelButtonSoundoff = component.Assets[3];
		QuadpanelButtonSoundon = component.Assets[4];
		QuadpanelButtonReportplayer = component.Assets[5];
		QuadpanelButtonOptions = component.Assets[6];
		QuadpanelButtonHelp = component.Assets[7];
		NewInboxMessage = component.Assets[8];
		QuadpanelButtonLogout = component.Assets[9];
	}
}
