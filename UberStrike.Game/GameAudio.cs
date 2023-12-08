using UnityEngine;

public static class GameAudio {
	public static AudioClip HomeSceneBackground { get; private set; }
	public static AudioClip BigSplash { get; private set; }
	public static AudioClip ImpactCement1 { get; private set; }
	public static AudioClip ImpactCement2 { get; private set; }
	public static AudioClip ImpactCement3 { get; private set; }
	public static AudioClip ImpactCement4 { get; private set; }
	public static AudioClip ImpactGlass1 { get; private set; }
	public static AudioClip ImpactGlass2 { get; private set; }
	public static AudioClip ImpactGlass3 { get; private set; }
	public static AudioClip ImpactGlass4 { get; private set; }
	public static AudioClip ImpactGlass5 { get; private set; }
	public static AudioClip ImpactGrass1 { get; private set; }
	public static AudioClip ImpactGrass2 { get; private set; }
	public static AudioClip ImpactGrass3 { get; private set; }
	public static AudioClip ImpactGrass4 { get; private set; }
	public static AudioClip ImpactMetal1 { get; private set; }
	public static AudioClip ImpactMetal2 { get; private set; }
	public static AudioClip ImpactMetal3 { get; private set; }
	public static AudioClip ImpactMetal4 { get; private set; }
	public static AudioClip ImpactMetal5 { get; private set; }
	public static AudioClip ImpactSand1 { get; private set; }
	public static AudioClip ImpactSand2 { get; private set; }
	public static AudioClip ImpactSand3 { get; private set; }
	public static AudioClip ImpactSand4 { get; private set; }
	public static AudioClip ImpactSand5 { get; private set; }
	public static AudioClip ImpactStone1 { get; private set; }
	public static AudioClip ImpactStone2 { get; private set; }
	public static AudioClip ImpactStone3 { get; private set; }
	public static AudioClip ImpactStone4 { get; private set; }
	public static AudioClip ImpactStone5 { get; private set; }
	public static AudioClip ImpactWater1 { get; private set; }
	public static AudioClip ImpactWater2 { get; private set; }
	public static AudioClip ImpactWater3 { get; private set; }
	public static AudioClip ImpactWater4 { get; private set; }
	public static AudioClip ImpactWater5 { get; private set; }
	public static AudioClip ImpactWood1 { get; private set; }
	public static AudioClip ImpactWood2 { get; private set; }
	public static AudioClip ImpactWood3 { get; private set; }
	public static AudioClip ImpactWood4 { get; private set; }
	public static AudioClip ImpactWood5 { get; private set; }
	public static AudioClip MediumSplash { get; private set; }
	public static AudioClip BlueWins { get; private set; }
	public static AudioClip CountdownTonal1 { get; private set; }
	public static AudioClip CountdownTonal2 { get; private set; }
	public static AudioClip Draw { get; private set; }
	public static AudioClip Fight { get; private set; }
	public static AudioClip FocusEnemy { get; private set; }
	public static AudioClip GameOver { get; private set; }
	public static AudioClip GetPoints { get; private set; }
	public static AudioClip GetXP { get; private set; }
	public static AudioClip LevelUp { get; private set; }
	public static AudioClip LostLead { get; private set; }
	public static AudioClip MatchEndingCountdown1 { get; private set; }
	public static AudioClip MatchEndingCountdown2 { get; private set; }
	public static AudioClip MatchEndingCountdown3 { get; private set; }
	public static AudioClip MatchEndingCountdown4 { get; private set; }
	public static AudioClip MatchEndingCountdown5 { get; private set; }
	public static AudioClip RedWins { get; private set; }
	public static AudioClip TakenLead { get; private set; }
	public static AudioClip TiedLead { get; private set; }
	public static AudioClip YouWin { get; private set; }
	public static AudioClip AmmoPickup2D { get; private set; }
	public static AudioClip ArmorShard2D { get; private set; }
	public static AudioClip BigHealth2D { get; private set; }
	public static AudioClip GoldArmor2D { get; private set; }
	public static AudioClip MediumHealth2D { get; private set; }
	public static AudioClip MegaHealth2D { get; private set; }
	public static AudioClip SilverArmor2D { get; private set; }
	public static AudioClip SmallHealth2D { get; private set; }
	public static AudioClip WeaponPickup2D { get; private set; }
	public static AudioClip HealthCriticalHeartbeat { get; private set; }
	public static AudioClip HealthCriticalNoise { get; private set; }
	public static AudioClip HealthOver100Decrease { get; private set; }
	public static AudioClip HealthOver100Increase { get; private set; }
	public static AudioClip FootStepDirt1 { get; private set; }
	public static AudioClip FootStepDirt2 { get; private set; }
	public static AudioClip FootStepDirt3 { get; private set; }
	public static AudioClip FootStepDirt4 { get; private set; }
	public static AudioClip FootStepGlass1 { get; private set; }
	public static AudioClip FootStepGlass2 { get; private set; }
	public static AudioClip FootStepGlass3 { get; private set; }
	public static AudioClip FootStepGlass4 { get; private set; }
	public static AudioClip FootStepGrass1 { get; private set; }
	public static AudioClip FootStepGrass2 { get; private set; }
	public static AudioClip FootStepGrass3 { get; private set; }
	public static AudioClip FootStepGrass4 { get; private set; }
	public static AudioClip FootStepHeavyMetal1 { get; private set; }
	public static AudioClip FootStepHeavyMetal2 { get; private set; }
	public static AudioClip FootStepHeavyMetal3 { get; private set; }
	public static AudioClip FootStepHeavyMetal4 { get; private set; }
	public static AudioClip FootStepMetal1 { get; private set; }
	public static AudioClip FootStepMetal2 { get; private set; }
	public static AudioClip FootStepMetal3 { get; private set; }
	public static AudioClip FootStepMetal4 { get; private set; }
	public static AudioClip FootStepRock1 { get; private set; }
	public static AudioClip FootStepRock2 { get; private set; }
	public static AudioClip FootStepRock3 { get; private set; }
	public static AudioClip FootStepRock4 { get; private set; }
	public static AudioClip FootStepSand1 { get; private set; }
	public static AudioClip FootStepSand2 { get; private set; }
	public static AudioClip FootStepSand3 { get; private set; }
	public static AudioClip FootStepSand4 { get; private set; }
	public static AudioClip FootStepSnow1 { get; private set; }
	public static AudioClip FootStepSnow2 { get; private set; }
	public static AudioClip FootStepSnow3 { get; private set; }
	public static AudioClip FootStepSnow4 { get; private set; }
	public static AudioClip FootStepWater1 { get; private set; }
	public static AudioClip FootStepWater2 { get; private set; }
	public static AudioClip FootStepWater3 { get; private set; }
	public static AudioClip FootStepWood1 { get; private set; }
	public static AudioClip FootStepWood2 { get; private set; }
	public static AudioClip FootStepWood3 { get; private set; }
	public static AudioClip FootStepWood4 { get; private set; }
	public static AudioClip GotHeadshotKill { get; private set; }
	public static AudioClip GotNutshotKill { get; private set; }
	public static AudioClip KilledBySplatbat { get; private set; }
	public static AudioClip LandingGrunt { get; private set; }
	public static AudioClip LocalPlayerHitArmorRemaining { get; private set; }
	public static AudioClip LocalPlayerHitNoArmor { get; private set; }
	public static AudioClip LocalPlayerHitNoArmorLowHealth { get; private set; }
	public static AudioClip NormalKill1 { get; private set; }
	public static AudioClip NormalKill2 { get; private set; }
	public static AudioClip NormalKill3 { get; private set; }
	public static AudioClip PlayerJump2D { get; private set; }
	public static AudioClip QuickItemRecharge { get; private set; }
	public static AudioClip SwimAboveWater1 { get; private set; }
	public static AudioClip SwimAboveWater2 { get; private set; }
	public static AudioClip SwimAboveWater3 { get; private set; }
	public static AudioClip SwimAboveWater4 { get; private set; }
	public static AudioClip SwimUnderWater { get; private set; }
	public static AudioClip AmmoPickup { get; private set; }
	public static AudioClip ArmorShard { get; private set; }
	public static AudioClip BigHealth { get; private set; }
	public static AudioClip GoldArmor { get; private set; }
	public static AudioClip JumpPad { get; private set; }
	public static AudioClip JumpPad2D { get; private set; }
	public static AudioClip MediumHealth { get; private set; }
	public static AudioClip MegaHealth { get; private set; }
	public static AudioClip SilverArmor { get; private set; }
	public static AudioClip SmallHealth { get; private set; }
	public static AudioClip TargetDamage { get; private set; }
	public static AudioClip TargetPopup { get; private set; }
	public static AudioClip WeaponPickup { get; private set; }
	public static AudioClip ButtonClick { get; private set; }
	public static AudioClip ClickReady { get; private set; }
	public static AudioClip ClickUnready { get; private set; }
	public static AudioClip ClosePanel { get; private set; }
	public static AudioClip CreateGame { get; private set; }
	public static AudioClip DoubleKill { get; private set; }
	public static AudioClip EndOfRound { get; private set; }
	public static AudioClip EquipGear { get; private set; }
	public static AudioClip EquipItem { get; private set; }
	public static AudioClip EquipWeapon { get; private set; }
	public static AudioClip FBScreenshot { get; private set; }
	public static AudioClip HeadShot { get; private set; }
	public static AudioClip JoinServer { get; private set; }
	public static AudioClip KillLeft1 { get; private set; }
	public static AudioClip KillLeft2 { get; private set; }
	public static AudioClip KillLeft3 { get; private set; }
	public static AudioClip KillLeft4 { get; private set; }
	public static AudioClip KillLeft5 { get; private set; }
	public static AudioClip LeaveServer { get; private set; }
	public static AudioClip MegaKill { get; private set; }
	public static AudioClip NewMessage { get; private set; }
	public static AudioClip NewRequest { get; private set; }
	public static AudioClip NutShot { get; private set; }
	public static AudioClip Objective { get; private set; }
	public static AudioClip ObjectiveTick { get; private set; }
	public static AudioClip OpenPanel { get; private set; }
	public static AudioClip QuadKill { get; private set; }
	public static AudioClip RibbonClick { get; private set; }
	public static AudioClip Smackdown { get; private set; }
	public static AudioClip SubObjective { get; private set; }
	public static AudioClip TripleKill { get; private set; }
	public static AudioClip UberKill { get; private set; }
	public static AudioClip LauncherBounce1 { get; private set; }
	public static AudioClip LauncherBounce2 { get; private set; }
	public static AudioClip OutOfAmmoClick { get; private set; }
	public static AudioClip SniperScopeIn { get; private set; }
	public static AudioClip SniperScopeOut { get; private set; }
	public static AudioClip SniperZoomIn { get; private set; }
	public static AudioClip SniperZoomOut { get; private set; }
	public static AudioClip UnderwaterExplosion1 { get; private set; }
	public static AudioClip UnderwaterExplosion2 { get; private set; }
	public static AudioClip WeaponSwitch { get; private set; }

	static GameAudio() {
		AudioClipConfigurator component;

		try {
			component = GameObject.Find("GameAudio").GetComponent<AudioClipConfigurator>();
		} catch {
			Debug.LogError("Missing instance of the prefab with name: GameAudio!");

			return;
		}

		HomeSceneBackground = component.Assets[0];
		BigSplash = component.Assets[1];
		ImpactCement1 = component.Assets[2];
		ImpactCement2 = component.Assets[3];
		ImpactCement3 = component.Assets[4];
		ImpactCement4 = component.Assets[5];
		ImpactGlass1 = component.Assets[6];
		ImpactGlass2 = component.Assets[7];
		ImpactGlass3 = component.Assets[8];
		ImpactGlass4 = component.Assets[9];
		ImpactGlass5 = component.Assets[10];
		ImpactGrass1 = component.Assets[11];
		ImpactGrass2 = component.Assets[12];
		ImpactGrass3 = component.Assets[13];
		ImpactGrass4 = component.Assets[14];
		ImpactMetal1 = component.Assets[15];
		ImpactMetal2 = component.Assets[16];
		ImpactMetal3 = component.Assets[17];
		ImpactMetal4 = component.Assets[18];
		ImpactMetal5 = component.Assets[19];
		ImpactSand1 = component.Assets[20];
		ImpactSand2 = component.Assets[21];
		ImpactSand3 = component.Assets[22];
		ImpactSand4 = component.Assets[23];
		ImpactSand5 = component.Assets[24];
		ImpactStone1 = component.Assets[25];
		ImpactStone2 = component.Assets[26];
		ImpactStone3 = component.Assets[27];
		ImpactStone4 = component.Assets[28];
		ImpactStone5 = component.Assets[29];
		ImpactWater1 = component.Assets[30];
		ImpactWater2 = component.Assets[31];
		ImpactWater3 = component.Assets[32];
		ImpactWater4 = component.Assets[33];
		ImpactWater5 = component.Assets[34];
		ImpactWood1 = component.Assets[35];
		ImpactWood2 = component.Assets[36];
		ImpactWood3 = component.Assets[37];
		ImpactWood4 = component.Assets[38];
		ImpactWood5 = component.Assets[39];
		MediumSplash = component.Assets[40];
		BlueWins = component.Assets[41];
		CountdownTonal1 = component.Assets[42];
		CountdownTonal2 = component.Assets[43];
		Draw = component.Assets[44];
		Fight = component.Assets[45];
		FocusEnemy = component.Assets[46];
		GameOver = component.Assets[47];
		GetPoints = component.Assets[48];
		GetXP = component.Assets[49];
		LevelUp = component.Assets[50];
		LostLead = component.Assets[51];
		MatchEndingCountdown1 = component.Assets[52];
		MatchEndingCountdown2 = component.Assets[53];
		MatchEndingCountdown3 = component.Assets[54];
		MatchEndingCountdown4 = component.Assets[55];
		MatchEndingCountdown5 = component.Assets[56];
		RedWins = component.Assets[57];
		TakenLead = component.Assets[58];
		TiedLead = component.Assets[59];
		YouWin = component.Assets[60];
		AmmoPickup2D = component.Assets[61];
		ArmorShard2D = component.Assets[62];
		BigHealth2D = component.Assets[63];
		GoldArmor2D = component.Assets[64];
		MediumHealth2D = component.Assets[65];
		MegaHealth2D = component.Assets[66];
		SilverArmor2D = component.Assets[67];
		SmallHealth2D = component.Assets[68];
		WeaponPickup2D = component.Assets[69];
		HealthCriticalHeartbeat = component.Assets[70];
		HealthCriticalNoise = component.Assets[71];
		HealthOver100Decrease = component.Assets[72];
		HealthOver100Increase = component.Assets[73];
		FootStepDirt1 = component.Assets[74];
		FootStepDirt2 = component.Assets[75];
		FootStepDirt3 = component.Assets[76];
		FootStepDirt4 = component.Assets[77];
		FootStepGlass1 = component.Assets[78];
		FootStepGlass2 = component.Assets[79];
		FootStepGlass3 = component.Assets[80];
		FootStepGlass4 = component.Assets[81];
		FootStepGrass1 = component.Assets[82];
		FootStepGrass2 = component.Assets[83];
		FootStepGrass3 = component.Assets[84];
		FootStepGrass4 = component.Assets[85];
		FootStepHeavyMetal1 = component.Assets[86];
		FootStepHeavyMetal2 = component.Assets[87];
		FootStepHeavyMetal3 = component.Assets[88];
		FootStepHeavyMetal4 = component.Assets[89];
		FootStepMetal1 = component.Assets[90];
		FootStepMetal2 = component.Assets[91];
		FootStepMetal3 = component.Assets[92];
		FootStepMetal4 = component.Assets[93];
		FootStepRock1 = component.Assets[94];
		FootStepRock2 = component.Assets[95];
		FootStepRock3 = component.Assets[96];
		FootStepRock4 = component.Assets[97];
		FootStepSand1 = component.Assets[98];
		FootStepSand2 = component.Assets[99];
		FootStepSand3 = component.Assets[100];
		FootStepSand4 = component.Assets[101];
		FootStepSnow1 = component.Assets[102];
		FootStepSnow2 = component.Assets[103];
		FootStepSnow3 = component.Assets[104];
		FootStepSnow4 = component.Assets[105];
		FootStepWater1 = component.Assets[106];
		FootStepWater2 = component.Assets[107];
		FootStepWater3 = component.Assets[108];
		FootStepWood1 = component.Assets[109];
		FootStepWood2 = component.Assets[110];
		FootStepWood3 = component.Assets[111];
		FootStepWood4 = component.Assets[112];
		GotHeadshotKill = component.Assets[113];
		GotNutshotKill = component.Assets[114];
		KilledBySplatbat = component.Assets[115];
		LandingGrunt = component.Assets[116];
		LocalPlayerHitArmorRemaining = component.Assets[117];
		LocalPlayerHitNoArmor = component.Assets[118];
		LocalPlayerHitNoArmorLowHealth = component.Assets[119];
		NormalKill1 = component.Assets[120];
		NormalKill2 = component.Assets[121];
		NormalKill3 = component.Assets[122];
		PlayerJump2D = component.Assets[123];
		QuickItemRecharge = component.Assets[124];
		SwimAboveWater1 = component.Assets[125];
		SwimAboveWater2 = component.Assets[126];
		SwimAboveWater3 = component.Assets[127];
		SwimAboveWater4 = component.Assets[128];
		SwimUnderWater = component.Assets[129];
		AmmoPickup = component.Assets[130];
		ArmorShard = component.Assets[131];
		BigHealth = component.Assets[132];
		GoldArmor = component.Assets[133];
		JumpPad = component.Assets[134];
		JumpPad2D = component.Assets[135];
		MediumHealth = component.Assets[136];
		MegaHealth = component.Assets[137];
		SilverArmor = component.Assets[138];
		SmallHealth = component.Assets[139];
		TargetDamage = component.Assets[140];
		TargetPopup = component.Assets[141];
		WeaponPickup = component.Assets[142];
		ButtonClick = component.Assets[143];
		ClickReady = component.Assets[144];
		ClickUnready = component.Assets[145];
		ClosePanel = component.Assets[146];
		CreateGame = component.Assets[147];
		DoubleKill = component.Assets[148];
		EndOfRound = component.Assets[149];
		EquipGear = component.Assets[150];
		EquipItem = component.Assets[151];
		EquipWeapon = component.Assets[152];
		FBScreenshot = component.Assets[153];
		HeadShot = component.Assets[154];
		JoinServer = component.Assets[155];
		KillLeft1 = component.Assets[156];
		KillLeft2 = component.Assets[157];
		KillLeft3 = component.Assets[158];
		KillLeft4 = component.Assets[159];
		KillLeft5 = component.Assets[160];
		LeaveServer = component.Assets[161];
		MegaKill = component.Assets[162];
		NewMessage = component.Assets[163];
		NewRequest = component.Assets[164];
		NutShot = component.Assets[165];
		Objective = component.Assets[166];
		ObjectiveTick = component.Assets[167];
		OpenPanel = component.Assets[168];
		QuadKill = component.Assets[169];
		RibbonClick = component.Assets[170];
		Smackdown = component.Assets[171];
		SubObjective = component.Assets[172];
		TripleKill = component.Assets[173];
		UberKill = component.Assets[174];
		LauncherBounce1 = component.Assets[175];
		LauncherBounce2 = component.Assets[176];
		OutOfAmmoClick = component.Assets[177];
		SniperScopeIn = component.Assets[178];
		SniperScopeOut = component.Assets[179];
		SniperZoomIn = component.Assets[180];
		SniperZoomOut = component.Assets[181];
		UnderwaterExplosion1 = component.Assets[182];
		UnderwaterExplosion2 = component.Assets[183];
		WeaponSwitch = component.Assets[184];
	}
}
