using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public static class DefaultItemUtil {
	public const string HeadName = "LutzDefaultGearHead";
	public const string GlovesName = "LutzDefaultGearGloves";
	public const string UpperbodyName = "LutzDefaultGearUpperBody";
	public const string LowerbodyName = "LutzDefaultGearLowerBody";
	public const string BootsName = "LutzDefaultGearBoots";
	public const string FaceName = "LutzDefaultGearFace";
	public const string MeleeName = "TheSplatbat";
	public const string HandGunName = "HandGun";
	public const string MachineGunName = "MachineGun";
	public const string SplatterGunName = "SplatterGun";
	public const string CannonName = "Cannon";
	public const string SniperRifleName = "SniperRifle";
	public const string LauncherName = "Launcher";
	public const string ShotGunName = "ShotGun";
	public const int HeadId = 1084;
	public const int GlovesId = 1086;
	public const int UpperbodyId = 1087;
	public const int LowerbodyId = 1088;
	public const int BootsId = 1089;
	public const int MeleeId = 1000;
	public const int HandgunId = 1001;
	public const int MachineGunId = 1002;
	public const int ShotGunId = 1003;
	public const int SniperRifleId = 1004;
	public const int CannonId = 1005;
	public const int SplatterGunId = 1006;
	public const int LauncherId = 1007;

	public static void ConfigureDefaultGearAndWeapons() {
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView {
			ID = 1084,
			PrefabName = "LutzDefaultGearHead"
		});

		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView {
			ID = 1086,
			PrefabName = "LutzDefaultGearGloves"
		});

		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView {
			ID = 1087,
			PrefabName = "LutzDefaultGearUpperBody"
		});

		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView {
			ID = 1088,
			PrefabName = "LutzDefaultGearLowerBody"
		});

		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView {
			ID = 1089,
			PrefabName = "LutzDefaultGearBoots"
		});

		for (var uberstrikeItemClass = UberstrikeItemClass.WeaponMelee; uberstrikeItemClass <= UberstrikeItemClass.WeaponLauncher; uberstrikeItemClass++) {
			Singleton<ItemManager>.Instance.AddDefaultItem(GetDefaultWeaponView(uberstrikeItemClass));
		}
	}

	public static UberStrikeItemWeaponView GetDefaultWeaponView(UberstrikeItemClass itemClass) {
		switch (itemClass) {
			case UberstrikeItemClass.WeaponMelee:
				return new UberStrikeItemWeaponView {
					ID = 1000,
					ItemClass = UberstrikeItemClass.WeaponMelee,
					PrefabName = "TheSplatbat",
					DamageKnockback = 1000,
					DamagePerProjectile = 99,
					AccuracySpread = 0,
					RecoilKickback = 0,
					StartAmmo = 0,
					MaxAmmo = 0,
					MissileTimeToDetonate = 0,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 500,
					SplashRadius = 100,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 0,
					RecoilMovement = 0,
					HasAutomaticFire = true,
					DefaultZoomMultiplier = 1,
					MinZoomMultiplier = 1,
					MaxZoomMultiplier = 1
				};
			case UberstrikeItemClass.WeaponMachinegun:
				return new UberStrikeItemWeaponView {
					ID = 1002,
					ItemClass = UberstrikeItemClass.WeaponMachinegun,
					PrefabName = "MachineGun",
					DamageKnockback = 50,
					DamagePerProjectile = 13,
					AccuracySpread = 3,
					RecoilKickback = 4,
					StartAmmo = 100,
					MaxAmmo = 300,
					MissileTimeToDetonate = 0,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 125,
					SplashRadius = 100,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 0,
					RecoilMovement = 5,
					WeaponSecondaryAction = 2,
					HasAutomaticFire = true,
					DefaultZoomMultiplier = 2,
					MinZoomMultiplier = 2,
					MaxZoomMultiplier = 2
				};
			case UberstrikeItemClass.WeaponShotgun:
				return new UberStrikeItemWeaponView {
					ID = 1003,
					ItemClass = UberstrikeItemClass.WeaponShotgun,
					PrefabName = "ShotGun",
					DamageKnockback = 160,
					DamagePerProjectile = 9,
					AccuracySpread = 8,
					RecoilKickback = 15,
					StartAmmo = 20,
					MaxAmmo = 50,
					MissileTimeToDetonate = 0,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 1000,
					SplashRadius = 100,
					ProjectilesPerShot = 11,
					ProjectileSpeed = 0,
					RecoilMovement = 10,
					DefaultZoomMultiplier = 1,
					MinZoomMultiplier = 1,
					MaxZoomMultiplier = 1
				};
			case UberstrikeItemClass.WeaponSniperRifle:
				return new UberStrikeItemWeaponView {
					ID = 1004,
					ItemClass = UberstrikeItemClass.WeaponSniperRifle,
					PrefabName = "SniperRifle",
					DamageKnockback = 150,
					DamagePerProjectile = 70,
					AccuracySpread = 0,
					RecoilKickback = 12,
					StartAmmo = 20,
					MaxAmmo = 50,
					MissileTimeToDetonate = 0,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 1500,
					SplashRadius = 100,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 0,
					RecoilMovement = 15,
					WeaponSecondaryAction = 1,
					DefaultZoomMultiplier = 2,
					MinZoomMultiplier = 2,
					MaxZoomMultiplier = 4
				};
			case UberstrikeItemClass.WeaponCannon:
				return new UberStrikeItemWeaponView {
					ID = 1005,
					ItemClass = UberstrikeItemClass.WeaponCannon,
					PrefabName = "Cannon",
					DamageKnockback = 600,
					DamagePerProjectile = 65,
					AccuracySpread = 0,
					RecoilKickback = 12,
					StartAmmo = 10,
					MaxAmmo = 25,
					MissileTimeToDetonate = 5000,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 1000,
					SplashRadius = 250,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 50,
					RecoilMovement = 32,
					DefaultZoomMultiplier = 1,
					MinZoomMultiplier = 1,
					MaxZoomMultiplier = 1
				};
			case UberstrikeItemClass.WeaponSplattergun:
				return new UberStrikeItemWeaponView {
					ID = 1006,
					ItemClass = UberstrikeItemClass.WeaponSplattergun,
					PrefabName = "SplatterGun",
					DamageKnockback = 150,
					DamagePerProjectile = 16,
					AccuracySpread = 0,
					RecoilKickback = 0,
					StartAmmo = 60,
					MaxAmmo = 200,
					MissileTimeToDetonate = 5000,
					MissileForceImpulse = 0,
					MissileBounciness = 80,
					RateOfFire = 90,
					SplashRadius = 80,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 70,
					RecoilMovement = 0,
					DefaultZoomMultiplier = 1,
					MinZoomMultiplier = 1,
					MaxZoomMultiplier = 1
				};
			case UberstrikeItemClass.WeaponLauncher:
				return new UberStrikeItemWeaponView {
					ID = 1007,
					ItemClass = UberstrikeItemClass.WeaponLauncher,
					PrefabName = "Launcher",
					DamageKnockback = 450,
					DamagePerProjectile = 70,
					AccuracySpread = 0,
					RecoilKickback = 15,
					StartAmmo = 15,
					MaxAmmo = 30,
					MissileTimeToDetonate = 1250,
					MissileForceImpulse = 0,
					MissileBounciness = 0,
					RateOfFire = 1000,
					SplashRadius = 400,
					ProjectilesPerShot = 1,
					ProjectileSpeed = 20,
					RecoilMovement = 9,
					DefaultZoomMultiplier = 1,
					MinZoomMultiplier = 1,
					MaxZoomMultiplier = 1
				};
		}

		return null;
	}
}
