using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDMobileWeaponSelector : MonoBehaviour {
	[SerializeField]
	private GameObject cannon;

	[SerializeField]
	private GameObject disableSlot;

	[SerializeField]
	private GameObject launcher;

	[SerializeField]
	private GameObject machinegun;

	[SerializeField]
	private GameObject melee;

	[SerializeField]
	private GameObject meleeSlot;

	[SerializeField]
	private GameObject primarySlot;

	[SerializeField]
	private NGUIScrollList scrollList;

	[SerializeField]
	private GameObject secondarySlot;

	[SerializeField]
	private GameObject selectorBackground;

	[SerializeField]
	private GameObject shotgun;

	private Dictionary<GameObject, LoadoutSlotType> slots = new Dictionary<GameObject, LoadoutSlotType>();

	[SerializeField]
	private GameObject sniper;

	[SerializeField]
	private GameObject splattergun;

	[SerializeField]
	private GameObject tertiarySlot;

	private Dictionary<UberstrikeItemClass, GameObject> weapons = new Dictionary<UberstrikeItemClass, GameObject>();

	private void OnEnable() {
		GameState.Current.PlayerData.LoadoutWeapons.Fire();
		GameState.Current.PlayerData.ActiveWeapon.Fire();
	}

	private void Start() {
		AutoMonoBehaviour<TouchInput>.Instance.Shooter.IgnoreRect(new Rect(Screen.width - 240f, 0f, 240f, 200f));
		slots.Add(meleeSlot, LoadoutSlotType.WeaponMelee);
		slots.Add(primarySlot, LoadoutSlotType.WeaponPrimary);
		slots.Add(secondarySlot, LoadoutSlotType.WeaponSecondary);
		slots.Add(tertiarySlot, LoadoutSlotType.WeaponTertiary);
		weapons.Add(UberstrikeItemClass.WeaponMelee, melee);
		weapons.Add(UberstrikeItemClass.WeaponMachinegun, machinegun);
		weapons.Add(UberstrikeItemClass.WeaponShotgun, shotgun);
		weapons.Add(UberstrikeItemClass.WeaponSniperRifle, sniper);
		weapons.Add(UberstrikeItemClass.WeaponCannon, cannon);
		weapons.Add(UberstrikeItemClass.WeaponSplattergun, splattergun);
		weapons.Add(UberstrikeItemClass.WeaponLauncher, launcher);

		scrollList.SelectedElement.AddEvent(delegate(GameObject el) {
			if (el != null && slots.ContainsKey(el)) {
				GameInputKey gameInputKey;

				switch (slots[el]) {
					case LoadoutSlotType.WeaponMelee:
						gameInputKey = GameInputKey.WeaponMelee;

						break;
					case LoadoutSlotType.WeaponPrimary:
						gameInputKey = GameInputKey.Weapon1;

						break;
					case LoadoutSlotType.WeaponSecondary:
						gameInputKey = GameInputKey.Weapon2;

						break;
					case LoadoutSlotType.WeaponTertiary:
						gameInputKey = GameInputKey.Weapon3;

						break;
					default:
						Debug.LogError("Cannot switch to unknown slot!");

						return;
				}

				EventHandler.Global.Fire(new GlobalEvents.InputChanged(gameInputKey, 1f));
			}
		}, this);

		GameState.Current.PlayerData.LoadoutWeapons.AddEventAndFire(LoadWeaponList, this);

		GameState.Current.PlayerData.ActiveWeapon.AddEventAndFire(delegate(WeaponSlot el) {
			if (el != null) {
				scrollList.SpringToElement(ElementAtSlot(el.Slot));
			}
		}, this);
	}

	private void LoadWeaponList(Dictionary<LoadoutSlotType, IUnityItem> loadoutWeapons) {
		if (loadoutWeapons == null) {
			return;
		}

		foreach (var gameObject in weapons.Values) {
			UnloadWeapon(gameObject);
		}

		var list = new List<GameObject>();

		foreach (var keyValuePair in loadoutWeapons) {
			var gameObject2 = ElementAtSlot(keyValuePair.Key);
			LoadWeapon(keyValuePair.Value.View.ItemClass, gameObject2);

			if (keyValuePair.Value != null && gameObject2 != null) {
				list.Add(gameObject2);
			}
		}

		scrollList.SetActiveElements(list);
	}

	private void UnloadWeapon(GameObject weapon) {
		weapon.transform.parent = disableSlot.transform;
	}

	private void LoadWeapon(UberstrikeItemClass weaponClass, GameObject slot) {
		if (weapons.ContainsKey(weaponClass)) {
			weapons[weaponClass].transform.parent = slot.transform;
			weapons[weaponClass].transform.localPosition = Vector3.zero;
		}
	}

	private GameObject ElementAtSlot(LoadoutSlotType slot) {
		switch (slot) {
			case LoadoutSlotType.WeaponMelee:
				return meleeSlot;
			case LoadoutSlotType.WeaponPrimary:
				return primarySlot;
			case LoadoutSlotType.WeaponSecondary:
				return secondarySlot;
			case LoadoutSlotType.WeaponTertiary:
				return tertiarySlot;
			default:
				return null;
		}
	}

	public void Show(bool show) {
		scrollList.Panel.panel.alpha = (float)((!show) ? 0 : 1);
		selectorBackground.SetActive(show);
	}
}
