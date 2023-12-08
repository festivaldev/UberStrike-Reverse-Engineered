using UnityEngine;

public static class HudTextures {
	public static Texture2D WhiteBlur128 { get; private set; }
	public static Texture2D DamageFeedbackMark { get; private set; }

	static HudTextures() {
		Texture2DConfigurator component;

		try {
			component = GameObject.Find("HudTextures").GetComponent<Texture2DConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: HudTextures!");

			return;
		}

		WhiteBlur128 = component.Assets[0];
		DamageFeedbackMark = component.Assets[1];
	}
}
