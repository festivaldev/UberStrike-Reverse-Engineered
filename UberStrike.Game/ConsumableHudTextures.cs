using UnityEngine;

public static class ConsumableHudTextures {
	public static Texture2D TooltipDown { get; private set; }
	public static Texture2D TooltipLeft { get; private set; }
	public static Texture2D TooltipRight { get; private set; }
	public static Texture2D TooltipUp { get; private set; }
	public static Texture2D AmmoBlue { get; private set; }
	public static Texture2D AmmoRed { get; private set; }
	public static Texture2D ArmorBlue { get; private set; }
	public static Texture2D ArmorRed { get; private set; }
	public static Texture2D HealthBlue { get; private set; }
	public static Texture2D HealthRed { get; private set; }
	public static Texture2D OffensiveGrenadeBlue { get; private set; }
	public static Texture2D OffensiveGrenadeRed { get; private set; }
	public static Texture2D SpringGrenadeBlue { get; private set; }
	public static Texture2D SpringGrenadeRed { get; private set; }
	public static Texture2D CircleBlue { get; private set; }
	public static Texture2D CircleRed { get; private set; }
	public static Texture2D CircleWhite { get; private set; }

	static ConsumableHudTextures() {
		Texture2DConfigurator component;

		try {
			component = GameObject.Find("ConsumableHudTextures").GetComponent<Texture2DConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: ConsumableHudTextures!");

			return;
		}

		TooltipDown = component.Assets[0];
		TooltipLeft = component.Assets[1];
		TooltipRight = component.Assets[2];
		TooltipUp = component.Assets[3];
		AmmoBlue = component.Assets[4];
		AmmoRed = component.Assets[5];
		ArmorBlue = component.Assets[6];
		ArmorRed = component.Assets[7];
		HealthBlue = component.Assets[8];
		HealthRed = component.Assets[9];
		OffensiveGrenadeBlue = component.Assets[10];
		OffensiveGrenadeRed = component.Assets[11];
		SpringGrenadeBlue = component.Assets[12];
		SpringGrenadeRed = component.Assets[13];
		CircleBlue = component.Assets[14];
		CircleRed = component.Assets[15];
		CircleWhite = component.Assets[16];
	}
}
