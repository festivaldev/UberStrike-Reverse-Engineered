using System;
using UnityEngine;

public static class AchievementIcons {
	public static Texture2D Achievement1MostValuablePlayer { get; private set; }
	public static Texture2D Achievement2MostAggressive { get; private set; }
	public static Texture2D Achievement3SharpestShooter { get; private set; }
	public static Texture2D Achievement4TriggerHappy { get; private set; }
	public static Texture2D Achievement5HardestHitter { get; private set; }
	public static Texture2D Achievement6CostEffective { get; private set; }
	public static Texture2D AchievementDefault { get; private set; }
	public static Texture2D RecommendationGear { get; private set; }
	public static Texture2D RecommendationMostEfficientWeapon { get; private set; }
	public static Texture2D RecommendationSale { get; private set; }
	public static Texture2D RecommendationWeapon { get; private set; }

	static AchievementIcons() {
		var component = GameObject.Find("AchievementIcons").GetComponent<Texture2DConfigurator>();

		if (component == null) {
			throw new Exception("Missing instance of the prefab with name: AchievementIcons!");
		}

		Achievement1MostValuablePlayer = component.Assets[0];
		Achievement2MostAggressive = component.Assets[1];
		Achievement3SharpestShooter = component.Assets[2];
		Achievement4TriggerHappy = component.Assets[3];
		Achievement5HardestHitter = component.Assets[4];
		Achievement6CostEffective = component.Assets[5];
		AchievementDefault = component.Assets[6];
		RecommendationGear = component.Assets[7];
		RecommendationMostEfficientWeapon = component.Assets[8];
		RecommendationSale = component.Assets[9];
		RecommendationWeapon = component.Assets[10];
	}
}
