using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public static class UberstrikeIconsHelper {
	private static Texture2D _white;

	public static Texture2D White {
		get {
			if (_white == null) {
				_white = new Texture2D(1, 1, TextureFormat.RGB24, false);
				_white.SetPixel(0, 0, Color.white);
				_white.Apply();
			}

			return _white;
		}
	}

	public static Texture2D GetIconForItemClass(UberstrikeItemClass itemClass) {
		switch (itemClass) {
			case UberstrikeItemClass.WeaponMelee:
				return ShopIcons.StatsMostWeaponSplatsMelee;
			case UberstrikeItemClass.WeaponMachinegun:
				return ShopIcons.StatsMostWeaponSplatsMachinegun;
			case UberstrikeItemClass.WeaponShotgun:
				return ShopIcons.StatsMostWeaponSplatsShotgun;
			case UberstrikeItemClass.WeaponSniperRifle:
				return ShopIcons.StatsMostWeaponSplatsSniperRifle;
			case UberstrikeItemClass.WeaponCannon:
				return ShopIcons.StatsMostWeaponSplatsCannon;
			case UberstrikeItemClass.WeaponSplattergun:
				return ShopIcons.StatsMostWeaponSplatsSplattergun;
			case UberstrikeItemClass.WeaponLauncher:
				return ShopIcons.StatsMostWeaponSplatsLauncher;
			case UberstrikeItemClass.GearBoots:
				return ShopIcons.Boots;
			case UberstrikeItemClass.GearHead:
				return ShopIcons.Head;
			case UberstrikeItemClass.GearFace:
				return ShopIcons.Face;
			case UberstrikeItemClass.GearUpperBody:
				return ShopIcons.Upperbody;
			case UberstrikeItemClass.GearLowerBody:
				return ShopIcons.Lowerbody;
			case UberstrikeItemClass.GearGloves:
				return ShopIcons.Gloves;
			case UberstrikeItemClass.QuickUseGeneral:
				return ShopIcons.QuickItems;
			case UberstrikeItemClass.QuickUseGrenade:
				return ShopIcons.QuickItems;
			case UberstrikeItemClass.QuickUseMine:
				return ShopIcons.QuickItems;
			case UberstrikeItemClass.FunctionalGeneral:
				return ShopIcons.FunctionalItems;
			case UberstrikeItemClass.SpecialGeneral:
				return ShopIcons.FunctionalItems;
			case UberstrikeItemClass.GearHolo:
				return ShopIcons.Holos;
		}

		return null;
	}

	public static Texture2D GetIconForChannel(ChannelType channel) {
		switch (channel) {
			case ChannelType.WebPortal:
				return CommunicatorIcons.ChannelPortal16x16;
			case ChannelType.WebFacebook:
				return CommunicatorIcons.ChannelFacebook16x16;
			case ChannelType.WindowsStandalone:
				return CommunicatorIcons.ChannelWindows16x16;
			case ChannelType.MacAppStore:
				return CommunicatorIcons.ChannelApple16x16;
			case ChannelType.OSXStandalone:
				return CommunicatorIcons.ChannelApple16x16;
			case ChannelType.IPhone:
			case ChannelType.IPad:
				return CommunicatorIcons.ChannelIos16x16;
			case ChannelType.Android:
				return CommunicatorIcons.ChannelAndroid16x16;
			case ChannelType.Steam:
				return CommunicatorIcons.ChannelSteam16x16;
		}

		return White;
	}

	public static Texture2D GetAchievementBadgeTexture(AchievementType achievement) {
		switch (achievement) {
			case AchievementType.MostValuable:
				return AchievementIcons.Achievement1MostValuablePlayer;
			case AchievementType.MostAggressive:
				return AchievementIcons.Achievement2MostAggressive;
			case AchievementType.SharpestShooter:
				return AchievementIcons.Achievement3SharpestShooter;
			case AchievementType.TriggerHappy:
				return AchievementIcons.Achievement4TriggerHappy;
			case AchievementType.HardestHitter:
				return AchievementIcons.Achievement5HardestHitter;
			case AchievementType.CostEffective:
				return AchievementIcons.Achievement6CostEffective;
			default:
				return AchievementIcons.AchievementDefault;
		}
	}
}
