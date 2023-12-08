using System;
using UnityEngine;

public static class ShopIcons {
	public static Texture2D StatsMostWeaponSplatsMelee { get; private set; }
	public static Texture2D StatsMostWeaponSplatsHandgun { get; private set; }
	public static Texture2D StatsMostWeaponSplatsMachinegun { get; private set; }
	public static Texture2D StatsMostWeaponSplatsShotgun { get; private set; }
	public static Texture2D StatsMostWeaponSplatsSniperRifle { get; private set; }
	public static Texture2D StatsMostWeaponSplatsCannon { get; private set; }
	public static Texture2D StatsMostWeaponSplatsSplattergun { get; private set; }
	public static Texture2D StatsMostWeaponSplatsLauncher { get; private set; }
	public static Texture2D Boots { get; private set; }
	public static Texture2D Head { get; private set; }
	public static Texture2D Face { get; private set; }
	public static Texture2D Upperbody { get; private set; }
	public static Texture2D Lowerbody { get; private set; }
	public static Texture2D Gloves { get; private set; }
	public static Texture2D Holos { get; private set; }
	public static Texture2D RecentItems { get; private set; }
	public static Texture2D FunctionalItems { get; private set; }
	public static Texture2D WeaponItems { get; private set; }
	public static Texture2D GearItems { get; private set; }
	public static Texture2D QuickItems { get; private set; }
	public static Texture2D NewItems { get; private set; }
	public static Texture2D LoadoutTabWeapons { get; private set; }
	public static Texture2D LoadoutTabGear { get; private set; }
	public static Texture2D LoadoutTabItems { get; private set; }
	public static Texture2D LabsInventory { get; private set; }
	public static Texture2D LabsShop { get; private set; }
	public static Texture2D LabsUndergroundIcon { get; private set; }
	public static Texture2D BundleIcon32x32 { get; private set; }
	public static Texture2D IconLottery { get; private set; }
	public static Texture2D CreditsIcon32x32 { get; private set; }
	public static Texture2D CreditsIcon48x48 { get; private set; }
	public static Texture2D CreditsIcon75x75 { get; private set; }
	public static Texture2D Points48x48 { get; private set; }
	public static Texture2D IconPoints20x20 { get; private set; }
	public static Texture2D IconCredits20x20 { get; private set; }
	public static Texture2D Stats1Kills20x20 { get; private set; }
	public static Texture2D Stats2Smackdowns20x20 { get; private set; }
	public static Texture2D Stats3Headshots20x20 { get; private set; }
	public static Texture2D Stats4Nutshots20x20 { get; private set; }
	public static Texture2D Stats5Damage20x20 { get; private set; }
	public static Texture2D Stats6Deaths20x20 { get; private set; }
	public static Texture2D Stats7Kdr20x20 { get; private set; }
	public static Texture2D Stats8Suicides20x20 { get; private set; }
	public static Texture2D New { get; private set; }
	public static Texture2D Hot { get; private set; }
	public static Texture2D Sale { get; private set; }
	public static Texture2D BlankItemFrame { get; private set; }
	public static Texture2D CheckMark { get; private set; }
	public static Texture2D ItemexpirationIcon { get; private set; }
	public static Texture2D ItemarmorpointsIcon { get; private set; }
	public static Texture2D ArrowBigShop { get; private set; }
	public static Texture2D ArrowSmallDownWhite { get; private set; }
	public static Texture2D ArrowSmallUpWhite { get; private set; }
	public static Texture2D ItemSlotSelected { get; private set; }

	static ShopIcons() {
		var component = GameObject.Find("ShopIcons").GetComponent<Texture2DConfigurator>();

		if (component == null) {
			throw new Exception("Missing instance of the prefab with name: ShopIcons!");
		}

		StatsMostWeaponSplatsMelee = component.Assets[0];
		StatsMostWeaponSplatsHandgun = component.Assets[1];
		StatsMostWeaponSplatsMachinegun = component.Assets[2];
		StatsMostWeaponSplatsShotgun = component.Assets[3];
		StatsMostWeaponSplatsSniperRifle = component.Assets[4];
		StatsMostWeaponSplatsCannon = component.Assets[5];
		StatsMostWeaponSplatsSplattergun = component.Assets[6];
		StatsMostWeaponSplatsLauncher = component.Assets[7];
		Boots = component.Assets[8];
		Head = component.Assets[9];
		Face = component.Assets[10];
		Upperbody = component.Assets[11];
		Lowerbody = component.Assets[12];
		Gloves = component.Assets[13];
		Holos = component.Assets[14];
		RecentItems = component.Assets[15];
		FunctionalItems = component.Assets[16];
		WeaponItems = component.Assets[17];
		GearItems = component.Assets[18];
		QuickItems = component.Assets[19];
		NewItems = component.Assets[20];
		LoadoutTabWeapons = component.Assets[21];
		LoadoutTabGear = component.Assets[22];
		LoadoutTabItems = component.Assets[23];
		LabsInventory = component.Assets[24];
		LabsShop = component.Assets[25];
		LabsUndergroundIcon = component.Assets[26];
		BundleIcon32x32 = component.Assets[27];
		IconLottery = component.Assets[28];
		CreditsIcon32x32 = component.Assets[29];
		CreditsIcon48x48 = component.Assets[30];
		CreditsIcon75x75 = component.Assets[31];
		Points48x48 = component.Assets[32];
		IconPoints20x20 = component.Assets[33];
		IconCredits20x20 = component.Assets[34];
		Stats1Kills20x20 = component.Assets[35];
		Stats2Smackdowns20x20 = component.Assets[36];
		Stats3Headshots20x20 = component.Assets[37];
		Stats4Nutshots20x20 = component.Assets[38];
		Stats5Damage20x20 = component.Assets[39];
		Stats6Deaths20x20 = component.Assets[40];
		Stats7Kdr20x20 = component.Assets[41];
		Stats8Suicides20x20 = component.Assets[42];
		New = component.Assets[43];
		Hot = component.Assets[44];
		Sale = component.Assets[45];
		BlankItemFrame = component.Assets[46];
		CheckMark = component.Assets[47];
		ItemexpirationIcon = component.Assets[48];
		ItemarmorpointsIcon = component.Assets[49];
		ArrowBigShop = component.Assets[50];
		ArrowSmallDownWhite = component.Assets[51];
		ArrowSmallUpWhite = component.Assets[52];
		ItemSlotSelected = component.Assets[53];
	}
}
