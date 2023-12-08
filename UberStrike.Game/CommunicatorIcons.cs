using UnityEngine;

public static class CommunicatorIcons {
	public static Texture2D ChannelPortal16x16 { get; private set; }
	public static Texture2D ChannelFacebook16x16 { get; private set; }
	public static Texture2D ChannelWindows16x16 { get; private set; }
	public static Texture2D ChannelApple16x16 { get; private set; }
	public static Texture2D ChannelKongregate16x16 { get; private set; }
	public static Texture2D ChannelAndroid16x16 { get; private set; }
	public static Texture2D ChannelIos16x16 { get; private set; }
	public static Texture2D PresenceOffline { get; private set; }
	public static Texture2D PresenceOnline { get; private set; }
	public static Texture2D PresencePlaying { get; private set; }
	public static Texture2D NewInboxMessage { get; private set; }
	public static Texture2D TagLightningBolt { get; private set; }
	public static Texture2D SkullCrossbonesIcon { get; private set; }
	public static Texture2D ChannelSteam16x16 { get; private set; }

	static CommunicatorIcons() {
		Texture2DConfigurator component;

		try {
			component = GameObject.Find("CommunicatorIcons").GetComponent<Texture2DConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: CommunicatorIcons!");

			return;
		}

		ChannelPortal16x16 = component.Assets[0];
		ChannelFacebook16x16 = component.Assets[1];
		ChannelWindows16x16 = component.Assets[2];
		ChannelApple16x16 = component.Assets[3];
		ChannelKongregate16x16 = component.Assets[4];
		ChannelAndroid16x16 = component.Assets[5];
		ChannelIos16x16 = component.Assets[6];
		PresenceOffline = component.Assets[7];
		PresenceOnline = component.Assets[8];
		PresencePlaying = component.Assets[9];
		NewInboxMessage = component.Assets[10];
		TagLightningBolt = component.Assets[11];
		SkullCrossbonesIcon = component.Assets[12];
		ChannelSteam16x16 = component.Assets[13];
	}
}
