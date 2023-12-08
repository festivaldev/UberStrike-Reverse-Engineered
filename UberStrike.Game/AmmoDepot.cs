using System;
using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public static class AmmoDepot {
	private static Dictionary<AmmoType, int> _currentAmmo = new Dictionary<AmmoType, int>(7);
	private static Dictionary<AmmoType, int> _startAmmo = new Dictionary<AmmoType, int>(7);
	private static Dictionary<AmmoType, int> _maxAmmo = new Dictionary<AmmoType, int>(7);

	static AmmoDepot() {
		foreach (var obj in Enum.GetValues(typeof(AmmoType))) {
			var ammoType = (AmmoType)((int)obj);
			_startAmmo.Add(ammoType, 100);
			_currentAmmo.Add(ammoType, 0);
			_maxAmmo.Add(ammoType, 200);
		}
	}

	public static void Reset() {
		_currentAmmo[AmmoType.Handgun] = _startAmmo[AmmoType.Handgun];
		_currentAmmo[AmmoType.Machinegun] = _startAmmo[AmmoType.Machinegun];
		_currentAmmo[AmmoType.Launcher] = _startAmmo[AmmoType.Launcher];
		_currentAmmo[AmmoType.Shotgun] = _startAmmo[AmmoType.Shotgun];
		_currentAmmo[AmmoType.Cannon] = _startAmmo[AmmoType.Cannon];
		_currentAmmo[AmmoType.Splattergun] = _startAmmo[AmmoType.Splattergun];
		_currentAmmo[AmmoType.Snipergun] = _startAmmo[AmmoType.Snipergun];
	}

	public static void SetMaxAmmoForType(UberstrikeItemClass weaponClass, int maxAmmoCount) {
		if (PlayerDataManager.IsPlayerLoggedIn) {
			switch (weaponClass) {
				case UberstrikeItemClass.WeaponMachinegun:
					_maxAmmo[AmmoType.Machinegun] = maxAmmoCount;

					break;
				case UberstrikeItemClass.WeaponShotgun:
					_maxAmmo[AmmoType.Shotgun] = maxAmmoCount;

					break;
				case UberstrikeItemClass.WeaponSniperRifle:
					_maxAmmo[AmmoType.Snipergun] = maxAmmoCount;

					break;
				case UberstrikeItemClass.WeaponCannon:
					_maxAmmo[AmmoType.Cannon] = maxAmmoCount;

					break;
				case UberstrikeItemClass.WeaponSplattergun:
					_maxAmmo[AmmoType.Splattergun] = maxAmmoCount;

					break;
				case UberstrikeItemClass.WeaponLauncher:
					_maxAmmo[AmmoType.Launcher] = maxAmmoCount;

					break;
			}
		}
	}

	public static void SetStartAmmoForType(UberstrikeItemClass weaponClass, int startAmmoCount) {
		if (PlayerDataManager.IsPlayerLoggedIn) {
			switch (weaponClass) {
				case UberstrikeItemClass.WeaponMachinegun:
					_startAmmo[AmmoType.Machinegun] = startAmmoCount;

					break;
				case UberstrikeItemClass.WeaponShotgun:
					_startAmmo[AmmoType.Shotgun] = startAmmoCount;

					break;
				case UberstrikeItemClass.WeaponSniperRifle:
					_startAmmo[AmmoType.Snipergun] = startAmmoCount;

					break;
				case UberstrikeItemClass.WeaponCannon:
					_startAmmo[AmmoType.Cannon] = startAmmoCount;

					break;
				case UberstrikeItemClass.WeaponSplattergun:
					_startAmmo[AmmoType.Splattergun] = startAmmoCount;

					break;
				case UberstrikeItemClass.WeaponLauncher:
					_startAmmo[AmmoType.Launcher] = startAmmoCount;

					break;
			}
		}
	}

	public static bool CanAddAmmo(AmmoType t) {
		UberstrikeItemClass uberstrikeItemClass;

		return TryGetAmmoTypeFromItemClass(t, out uberstrikeItemClass) && Singleton<WeaponController>.Instance.HasWeaponOfClass(uberstrikeItemClass) && _currentAmmo[t] < _maxAmmo[t];
	}

	public static void AddAmmoOfClass(UberstrikeItemClass c) {
		AmmoType ammoType;

		if (TryGetAmmoType(c, out ammoType)) {
			AddDefaultAmmoOfType(ammoType);
		}
	}

	public static void AddDefaultAmmoOfType(AmmoType t) {
		AddAmmoOfType(t, _startAmmo[t]);
	}

	public static void AddAmmoOfType(AmmoType t, int bullets) {
		_currentAmmo[t] = Mathf.Clamp(_currentAmmo[t] + bullets, 0, _maxAmmo[t]);
		GameState.Current.PlayerData.Ammo.Value = _currentAmmo[t];
	}

	public static void AddStartAmmoOfType(AmmoType t, float percentage = 1f) {
		var num = Mathf.CeilToInt(_startAmmo[t] * percentage);
		_currentAmmo[t] = Mathf.Clamp(_currentAmmo[t] + num, 0, _maxAmmo[t]);
		GameState.Current.PlayerData.Ammo.Value = _currentAmmo[t];
	}

	public static void AddMaxAmmoOfType(AmmoType t, float percentage = 1f) {
		var num = Mathf.CeilToInt(_maxAmmo[t] * percentage);
		_currentAmmo[t] = Mathf.Clamp(_currentAmmo[t] + num, 0, _maxAmmo[t]);
		GameState.Current.PlayerData.Ammo.Value = _currentAmmo[t];
	}

	public static bool HasAmmoOfType(AmmoType t) {
		return _currentAmmo[t] > 0;
	}

	public static bool HasAmmoOfClass(UberstrikeItemClass t) {
		AmmoType ammoType;

		return t == UberstrikeItemClass.WeaponMelee || (TryGetAmmoType(t, out ammoType) && HasAmmoOfType(ammoType));
	}

	public static int AmmoOfType(AmmoType t) {
		return _currentAmmo[t];
	}

	public static int AmmoOfClass(UberstrikeItemClass t) {
		AmmoType ammoType;

		if (TryGetAmmoType(t, out ammoType)) {
			return AmmoOfType(ammoType);
		}

		return 0;
	}

	public static int MaxAmmoOfClass(UberstrikeItemClass t) {
		AmmoType ammoType;

		if (TryGetAmmoType(t, out ammoType)) {
			return _maxAmmo[ammoType];
		}

		return 0;
	}

	public static bool TryGetAmmoType(UberstrikeItemClass item, out AmmoType t) {
		switch (item) {
			case UberstrikeItemClass.WeaponMachinegun:
				t = AmmoType.Machinegun;

				return true;
			case UberstrikeItemClass.WeaponShotgun:
				t = AmmoType.Shotgun;

				return true;
			case UberstrikeItemClass.WeaponSniperRifle:
				t = AmmoType.Snipergun;

				return true;
			case UberstrikeItemClass.WeaponCannon:
				t = AmmoType.Cannon;

				return true;
			case UberstrikeItemClass.WeaponSplattergun:
				t = AmmoType.Splattergun;

				return true;
			case UberstrikeItemClass.WeaponLauncher:
				t = AmmoType.Launcher;

				return true;
			default:
				t = AmmoType.Handgun;

				return false;
		}
	}

	public static bool TryGetAmmoTypeFromItemClass(AmmoType t, out UberstrikeItemClass itemClass) {
		switch (t) {
			case AmmoType.Cannon:
				itemClass = UberstrikeItemClass.WeaponCannon;

				return true;
			case AmmoType.Launcher:
				itemClass = UberstrikeItemClass.WeaponLauncher;

				return true;
			case AmmoType.Machinegun:
				itemClass = UberstrikeItemClass.WeaponMachinegun;

				return true;
			case AmmoType.Shotgun:
				itemClass = UberstrikeItemClass.WeaponShotgun;

				return true;
			case AmmoType.Snipergun:
				itemClass = UberstrikeItemClass.WeaponSniperRifle;

				return true;
			case AmmoType.Splattergun:
				itemClass = UberstrikeItemClass.WeaponSplattergun;

				return true;
		}

		itemClass = UberstrikeItemClass.WeaponMachinegun;

		return false;
	}

	public static bool UseAmmoOfType(AmmoType t, int count = 1) {
		if (_currentAmmo[t] > 0) {
			_currentAmmo[t] = Mathf.Max(_currentAmmo[t] - count, 0);
			GameState.Current.PlayerData.Ammo.Value = _currentAmmo[t];

			return true;
		}

		return false;
	}

	public static bool UseAmmoOfClass(UberstrikeItemClass t, int count = 1) {
		AmmoType ammoType;

		return TryGetAmmoType(t, out ammoType) && UseAmmoOfType(ammoType, count);
	}

	public static string ToString(AmmoType t) {
		return _currentAmmo[t].ToString();
	}

	public static void RemoveExtraAmmoOfType(UberstrikeItemClass t) {
		AmmoType ammoType;

		if (TryGetAmmoType(t, out ammoType) && _currentAmmo[ammoType] > _maxAmmo[ammoType]) {
			_currentAmmo[ammoType] = _maxAmmo[ammoType];
		}
	}
}
