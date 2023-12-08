using System.Collections.Generic;
using System.Linq;
using UberStrike.Core.Models.Views;
using UnityEngine;

public static class WeaponConfigurationHelper {
	private static Dictionary<int, SecureMemory<int>> rateOfFireCache = new Dictionary<int, SecureMemory<int>>();
	private static Dictionary<int, SecureMemory<int>> spreadCache = new Dictionary<int, SecureMemory<int>>();
	private static Dictionary<int, SecureMemory<int>> speedCache = new Dictionary<int, SecureMemory<int>>();
	private static Dictionary<int, SecureMemory<int>> splashCache = new Dictionary<int, SecureMemory<int>>();
	public static float MaxAmmo { get; private set; } = 1f;
	public static float MaxDamage { get; private set; } = 1f;
	public static float MaxAccuracySpread { get; private set; } = 1f;
	public static float MaxProjectileSpeed { get; private set; } = 1f;
	public static float MaxRateOfFire { get; private set; } = 1f;
	public static float MaxRecoilKickback { get; private set; } = 1f;
	public static float MaxSplashRadius { get; private set; } = 1f;

	public static void UpdateWeaponStatistics(UberStrikeItemShopClientView shopView) {
		if (shopView != null && shopView.WeaponItems.Count > 0) {
			MaxAmmo = shopView.WeaponItems.OrderByDescending(item => item.MaxAmmo).First().MaxAmmo;
			MaxSplashRadius = shopView.WeaponItems.OrderByDescending(item => item.SplashRadius).First().SplashRadius / 100f;
			MaxRecoilKickback = shopView.WeaponItems.OrderByDescending(item => item.RecoilKickback).First().RecoilKickback;
			MaxRateOfFire = shopView.WeaponItems.OrderByDescending(item => item.RateOfFire).First().RateOfFire / 1000f;
			MaxProjectileSpeed = shopView.WeaponItems.OrderByDescending(item => item.ProjectileSpeed).First().ProjectileSpeed;
			MaxAccuracySpread = shopView.WeaponItems.OrderByDescending(item => item.AccuracySpread).First().AccuracySpread / 10;
			MaxDamage = shopView.WeaponItems.OrderByDescending(item => item.DamagePerProjectile).First().DamagePerProjectile;

			foreach (var uberStrikeItemWeaponView in shopView.WeaponItems) {
				rateOfFireCache[uberStrikeItemWeaponView.ID] = new SecureMemory<int>(uberStrikeItemWeaponView.RateOfFire);
				spreadCache[uberStrikeItemWeaponView.ID] = new SecureMemory<int>(uberStrikeItemWeaponView.AccuracySpread);
				speedCache[uberStrikeItemWeaponView.ID] = new SecureMemory<int>(uberStrikeItemWeaponView.ProjectileSpeed);
				splashCache[uberStrikeItemWeaponView.ID] = new SecureMemory<int>(uberStrikeItemWeaponView.SplashRadius);
			}
		}
	}

	public static float GetAmmoNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.MaxAmmo / MaxAmmo);
	}

	public static float GetDamageNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.DamagePerProjectile * view.ProjectilesPerShot / MaxDamage);
	}

	public static float GetAccuracySpreadNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.AccuracySpread / 10f / MaxAccuracySpread);
	}

	public static float GetProjectileSpeedNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.ProjectileSpeed / MaxProjectileSpeed);
	}

	public static float GetRateOfFireNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.RateOfFire / 1000f / MaxRateOfFire);
	}

	public static float GetRecoilKickbackNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.RecoilKickback / MaxRecoilKickback);
	}

	public static float GetSplashRadiusNormalized(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.SplashRadius / 100f / MaxSplashRadius);
	}

	public static float GetAmmo(UberStrikeItemWeaponView view) {
		return (view == null) ? 0 : view.MaxAmmo;
	}

	public static float GetDamage(UberStrikeItemWeaponView view) {
		return (view == null) ? 0 : (view.DamagePerProjectile * view.ProjectilesPerShot);
	}

	public static float GetRecoilKickback(UberStrikeItemWeaponView view) {
		return (view == null) ? 0 : view.RecoilKickback;
	}

	public static float GetRecoilMovement(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.RecoilMovement / 100f);
	}

	public static float GetCriticalStrikeBonus(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (view.CriticalStrikeBonus / 100f);
	}

	public static float GetAccuracySpread(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (GetSecureSpread(view.ID) / 10f);
	}

	public static float GetRateOfFire(UberStrikeItemWeaponView view) {
		return (view == null) ? 1f : (GetSecureRateOfFire(view.ID) / 1000f);
	}

	public static float GetProjectileSpeed(UberStrikeItemWeaponView view) {
		return (view == null) ? 1 : GetSecureProjectileSpeed(view.ID);
	}

	public static float GetSplashRadius(UberStrikeItemWeaponView view) {
		return (view == null) ? 0f : (GetSecureSplashRadius(view.ID) / 100f);
	}

	public static int GetSecureRateOfFire(int itemId) {
		SecureMemory<int> secureMemory;

		if (rateOfFireCache.TryGetValue(itemId, out secureMemory)) {
			return secureMemory.ReadData(true);
		}

		return 1;
	}

	public static int GetSecureSpread(int itemId) {
		SecureMemory<int> secureMemory;

		if (spreadCache.TryGetValue(itemId, out secureMemory)) {
			return secureMemory.ReadData(true);
		}

		return Mathf.RoundToInt(MaxAccuracySpread * 10f);
	}

	public static int GetSecureProjectileSpeed(int itemId) {
		SecureMemory<int> secureMemory;

		if (speedCache.TryGetValue(itemId, out secureMemory)) {
			return secureMemory.ReadData(true);
		}

		return 1;
	}

	public static int GetSecureSplashRadius(int itemId) {
		SecureMemory<int> secureMemory;

		if (splashCache.TryGetValue(itemId, out secureMemory)) {
			return secureMemory.ReadData(true);
		}

		return 0;
	}
}
