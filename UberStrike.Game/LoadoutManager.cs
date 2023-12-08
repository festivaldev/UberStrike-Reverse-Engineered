using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

public class LoadoutManager : Singleton<LoadoutManager> {
	public static readonly LoadoutSlotType[] QuickSlots = {
		LoadoutSlotType.QuickUseItem1,
		LoadoutSlotType.QuickUseItem2,
		LoadoutSlotType.QuickUseItem3
	};

	public static readonly LoadoutSlotType[] WeaponSlots = {
		LoadoutSlotType.WeaponMelee,
		LoadoutSlotType.WeaponPrimary,
		LoadoutSlotType.WeaponSecondary,
		LoadoutSlotType.WeaponTertiary
	};

	public static readonly LoadoutSlotType[] GearSlots = {
		LoadoutSlotType.GearHead,
		LoadoutSlotType.GearFace,
		LoadoutSlotType.GearGloves,
		LoadoutSlotType.GearUpperBody,
		LoadoutSlotType.GearLowerBody,
		LoadoutSlotType.GearBoots
	};

	public static readonly UberstrikeItemClass[] GearSlotClasses = {
		UberstrikeItemClass.GearHead,
		UberstrikeItemClass.GearFace,
		UberstrikeItemClass.GearGloves,
		UberstrikeItemClass.GearUpperBody,
		UberstrikeItemClass.GearLowerBody,
		UberstrikeItemClass.GearBoots
	};

	public static readonly string[] GearSlotNames = {
		LocalizedStrings.Head,
		LocalizedStrings.Face,
		LocalizedStrings.Gloves,
		LocalizedStrings.UpperBody,
		LocalizedStrings.LowerBody,
		LocalizedStrings.Boots
	};

	public Loadout Loadout { get; private set; }

	private LoadoutManager() {
		var dictionary = new Dictionary<LoadoutSlotType, IUnityItem>();

		LoadoutSlotType[] array = {
			LoadoutSlotType.GearHead,
			LoadoutSlotType.GearGloves,
			LoadoutSlotType.GearUpperBody,
			LoadoutSlotType.GearLowerBody,
			LoadoutSlotType.GearBoots
		};

		int[] array2 = {
			1084,
			1086,
			1087,
			1088,
			1089
		};

		for (var i = 0; i < array.Length; i++) {
			var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(array2[i]);

			if (itemInShop != null) {
				dictionary.Add(array[i], itemInShop);
			}
		}

		Loadout = new Loadout(dictionary);
	}

	public void EquipWeapon(LoadoutSlotType weaponSlot, IUnityItem itemWeapon) {
		if (itemWeapon != null) {
			var gameObject = itemWeapon.Create(Vector3.zero, Quaternion.identity);

			if (gameObject) {
				var component = gameObject.GetComponent<BaseWeaponDecorator>();
				component.EnableShootAnimation = false;
				GameState.Current.Avatar.AssignWeapon(weaponSlot, component, itemWeapon);
				GameState.Current.Avatar.ShowWeapon(weaponSlot);
			} else {
				GameState.Current.Avatar.UnassignWeapon(weaponSlot);
			}
		} else {
			GameState.Current.Avatar.UnassignWeapon(weaponSlot);
		}
	}

	public void UpdateLoadout(LoadoutView view) {
		if (view.Head == 0) {
			Loadout.SetSlot(LoadoutSlotType.GearHead, Singleton<ItemManager>.Instance.GetItemInShop(1084));
		} else {
			Loadout.SetSlot(LoadoutSlotType.GearHead, Singleton<ItemManager>.Instance.GetItemInShop(view.Head));
		}

		if (view.Gloves == 0) {
			Loadout.SetSlot(LoadoutSlotType.GearGloves, Singleton<ItemManager>.Instance.GetItemInShop(1086));
		} else {
			Loadout.SetSlot(LoadoutSlotType.GearGloves, Singleton<ItemManager>.Instance.GetItemInShop(view.Gloves));
		}

		if (view.UpperBody == 0) {
			Loadout.SetSlot(LoadoutSlotType.GearUpperBody, Singleton<ItemManager>.Instance.GetItemInShop(1087));
		} else {
			Loadout.SetSlot(LoadoutSlotType.GearUpperBody, Singleton<ItemManager>.Instance.GetItemInShop(view.UpperBody));
		}

		if (view.LowerBody == 0) {
			Loadout.SetSlot(LoadoutSlotType.GearLowerBody, Singleton<ItemManager>.Instance.GetItemInShop(1088));
		} else {
			Loadout.SetSlot(LoadoutSlotType.GearLowerBody, Singleton<ItemManager>.Instance.GetItemInShop(view.LowerBody));
		}

		if (view.Boots == 0) {
			Loadout.SetSlot(LoadoutSlotType.GearBoots, Singleton<ItemManager>.Instance.GetItemInShop(1089));
		} else {
			Loadout.SetSlot(LoadoutSlotType.GearBoots, Singleton<ItemManager>.Instance.GetItemInShop(view.Boots));
		}

		Loadout.SetSlot(LoadoutSlotType.GearFace, Singleton<ItemManager>.Instance.GetItemInShop(view.Face));
		Loadout.SetSlot(LoadoutSlotType.GearHolo, Singleton<ItemManager>.Instance.GetItemInShop(view.Webbing));
		Loadout.SetSlot(LoadoutSlotType.WeaponMelee, Singleton<ItemManager>.Instance.GetItemInShop(view.MeleeWeapon));
		Loadout.SetSlot(LoadoutSlotType.WeaponPrimary, Singleton<ItemManager>.Instance.GetItemInShop(view.Weapon1));
		Loadout.SetSlot(LoadoutSlotType.WeaponSecondary, Singleton<ItemManager>.Instance.GetItemInShop(view.Weapon2));
		Loadout.SetSlot(LoadoutSlotType.WeaponTertiary, Singleton<ItemManager>.Instance.GetItemInShop(view.Weapon3));
		Loadout.SetSlot(LoadoutSlotType.QuickUseItem1, Singleton<ItemManager>.Instance.GetItemInShop(view.QuickItem1));
		Loadout.SetSlot(LoadoutSlotType.QuickUseItem2, Singleton<ItemManager>.Instance.GetItemInShop(view.QuickItem2));
		Loadout.SetSlot(LoadoutSlotType.QuickUseItem3, Singleton<ItemManager>.Instance.GetItemInShop(view.QuickItem3));
		Loadout.SetSlot(LoadoutSlotType.FunctionalItem1, Singleton<ItemManager>.Instance.GetItemInShop(view.FunctionalItem1));
		Loadout.SetSlot(LoadoutSlotType.FunctionalItem2, Singleton<ItemManager>.Instance.GetItemInShop(view.FunctionalItem2));
		Loadout.SetSlot(LoadoutSlotType.FunctionalItem3, Singleton<ItemManager>.Instance.GetItemInShop(view.FunctionalItem3));
		UpdateArmor();
	}

	public bool RemoveDuplicateWeaponClass(InventoryItem baseItem) {
		var loadoutSlotType = LoadoutSlotType.None;

		return RemoveDuplicateWeaponClass(baseItem, ref loadoutSlotType);
	}

	public bool RemoveDuplicateWeaponClass(InventoryItem baseItem, ref LoadoutSlotType updatedSlot) {
		var flag = false;

		if (baseItem != null && baseItem.Item.View.ItemType == UberstrikeItemType.Weapon) {
			foreach (var loadoutSlotType in WeaponSlots) {
				InventoryItem inventoryItem;

				if (TryGetItemInSlot(loadoutSlotType, out inventoryItem) && inventoryItem.Item.View.ItemClass == baseItem.Item.View.ItemClass && inventoryItem.Item.View.ID != baseItem.Item.View.ID) {
					GameState.Current.Avatar.UnassignWeapon(loadoutSlotType);
					ResetSlot(loadoutSlotType);
					updatedSlot = loadoutSlotType;
					flag = true;

					break;
				}
			}
		}

		return flag;
	}

	public bool RemoveDuplicateQuickItemClass(UberStrikeItemQuickView view, ref LoadoutSlotType lastRemovedSlot) {
		var flag = false;

		if (view.ItemType == UberstrikeItemType.QuickUse) {
			LoadoutSlotType[] array = {
				LoadoutSlotType.QuickUseItem1,
				LoadoutSlotType.QuickUseItem2,
				LoadoutSlotType.QuickUseItem3
			};

			foreach (var loadoutSlotType in array) {
				InventoryItem inventoryItem;

				if (TryGetItemInSlot(loadoutSlotType, out inventoryItem)) {
					var uberStrikeItemQuickView = inventoryItem.Item as UberStrikeItemQuickView;

					if (inventoryItem.Item.View.ItemType == UberstrikeItemType.QuickUse && uberStrikeItemQuickView.BehaviourType == view.BehaviourType) {
						ResetSlot(loadoutSlotType);
						flag = true;
						lastRemovedSlot = loadoutSlotType;
					}
				}
			}
		}

		return flag;
	}

	public bool RemoveDuplicateFunctionalItemClass(InventoryItem inventoryItem, ref LoadoutSlotType lastRemovedSlot) {
		var flag = false;

		if (inventoryItem != null && inventoryItem.Item.View.ItemType == UberstrikeItemType.Functional) {
			LoadoutSlotType[] array = {
				LoadoutSlotType.FunctionalItem1,
				LoadoutSlotType.FunctionalItem2,
				LoadoutSlotType.FunctionalItem3
			};

			foreach (var loadoutSlotType in array) {
				if (HasLoadoutItem(loadoutSlotType) && GetItemOnSlot(loadoutSlotType).View.ItemClass == inventoryItem.Item.View.ItemClass) {
					ResetSlot(loadoutSlotType);
					flag = true;
					lastRemovedSlot = loadoutSlotType;
				}
			}
		}

		return flag;
	}

	public void SwitchItemInSlot(LoadoutSlotType slot1, LoadoutSlotType slot2) {
		IUnityItem unityItem;
		var flag = Loadout.TryGetItem(slot1, out unityItem);
		IUnityItem unityItem2;
		var flag2 = Loadout.TryGetItem(slot2, out unityItem2);

		if (flag) {
			if (flag2) {
				Loadout.SetSlot(slot1, unityItem2);
				Loadout.SetSlot(slot2, unityItem);
			} else {
				Loadout.SetSlot(slot2, unityItem);
				Loadout.ClearSlot(slot1);
			}
		} else if (flag2) {
			Loadout.SetSlot(slot1, unityItem2);
			Loadout.ClearSlot(slot2);
		}
	}

	public bool IsWeaponSlotType(LoadoutSlotType slot) {
		return slot >= LoadoutSlotType.WeaponMelee && slot <= LoadoutSlotType.WeaponTertiary;
	}

	public bool IsQuickItemSlotType(LoadoutSlotType slot) {
		return slot >= LoadoutSlotType.QuickUseItem1 && slot <= LoadoutSlotType.QuickUseItem3;
	}

	public bool IsFunctionalItemSlotType(LoadoutSlotType slot) {
		return slot >= LoadoutSlotType.FunctionalItem1 && slot <= LoadoutSlotType.FunctionalItem3;
	}

	public bool SwapLoadoutItems(LoadoutSlotType slotA, LoadoutSlotType slotB) {
		var flag = false;

		if (slotA != slotB) {
			if (IsWeaponSlotType(slotA) && IsWeaponSlotType(slotB)) {
				InventoryItem inventoryItem = null;
				InventoryItem inventoryItem2 = null;
				TryGetItemInSlot(slotA, out inventoryItem);
				TryGetItemInSlot(slotB, out inventoryItem2);

				if (inventoryItem != null || inventoryItem2 != null) {
					IUnityItem unityItem2;

					if (inventoryItem2 != null) {
						var unityItem = inventoryItem2.Item;
						unityItem2 = unityItem;
					} else {
						unityItem2 = null;
					}

					SetLoadoutItem(slotA, unityItem2);
					IUnityItem unityItem3;

					if (inventoryItem != null) {
						var unityItem = inventoryItem.Item;
						unityItem3 = unityItem;
					} else {
						unityItem3 = null;
					}

					SetLoadoutItem(slotB, unityItem3);
					IUnityItem unityItem4;

					if (inventoryItem2 != null) {
						var unityItem = inventoryItem2.Item;
						unityItem4 = unityItem;
					} else {
						unityItem4 = null;
					}

					EquipWeapon(slotA, unityItem4);
					IUnityItem unityItem5;

					if (inventoryItem != null) {
						var unityItem = inventoryItem.Item;
						unityItem5 = unityItem;
					} else {
						unityItem5 = null;
					}

					EquipWeapon(slotB, unityItem5);
					flag = true;
				}
			} else if ((IsQuickItemSlotType(slotA) && IsQuickItemSlotType(slotB)) || (IsFunctionalItemSlotType(slotA) && IsFunctionalItemSlotType(slotB))) {
				InventoryItem inventoryItem3 = null;
				InventoryItem inventoryItem4 = null;
				TryGetItemInSlot(slotA, out inventoryItem3);
				TryGetItemInSlot(slotB, out inventoryItem4);

				if (inventoryItem3 != null || inventoryItem4 != null) {
					IUnityItem unityItem6;

					if (inventoryItem4 != null) {
						var unityItem = inventoryItem4.Item;
						unityItem6 = unityItem;
					} else {
						unityItem6 = null;
					}

					SetLoadoutItem(slotA, unityItem6);
					IUnityItem unityItem7;

					if (inventoryItem3 != null) {
						var unityItem = inventoryItem3.Item;
						unityItem7 = unityItem;
					} else {
						unityItem7 = null;
					}

					SetLoadoutItem(slotB, unityItem7);
					flag = true;
				}
			}
		}

		return flag;
	}

	public void SetLoadoutItem(LoadoutSlotType loadoutSlotType, IUnityItem item) {
		if (item == null) {
			ResetSlot(loadoutSlotType);
		} else {
			InventoryItem inventoryItem;

			if (Singleton<InventoryManager>.Instance.TryGetInventoryItem(item.View.ID, out inventoryItem) && inventoryItem.IsValid) {
				if (item.View.ItemType == UberstrikeItemType.Weapon) {
					RemoveDuplicateWeaponClass(inventoryItem);
				}

				Loadout.SetSlot(loadoutSlotType, item);
			} else if (item.View != null) {
				var buyPanelGUI = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;

				if (buyPanelGUI) {
					buyPanelGUI.SetItem(item, BuyingLocationType.Shop, BuyingRecommendationType.None);
				}
			}

			UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
			UpdateArmor();
		}
	}

	public void ResetSlot(LoadoutSlotType loadoutSlotType) {
		Loadout.ClearSlot(loadoutSlotType);
		UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
		UpdateArmor();
	}

	public void GetArmorValues(out int armorPoints) {
		armorPoints = 0;
		InventoryItem inventoryItem;

		if (TryGetItemInSlot(LoadoutSlotType.GearLowerBody, out inventoryItem) && inventoryItem.Item.View.ItemType == UberstrikeItemType.Gear) {
			var uberStrikeItemGearView = inventoryItem.Item.View as UberStrikeItemGearView;
			armorPoints += uberStrikeItemGearView.ArmorPoints;
		}

		if (TryGetItemInSlot(LoadoutSlotType.GearUpperBody, out inventoryItem) && inventoryItem.Item.View.ItemType == UberstrikeItemType.Gear) {
			var uberStrikeItemGearView2 = inventoryItem.Item.View as UberStrikeItemGearView;
			armorPoints += uberStrikeItemGearView2.ArmorPoints;
		}

		if (TryGetItemInSlot(LoadoutSlotType.GearHolo, out inventoryItem) && inventoryItem.Item.View.ItemType == UberstrikeItemType.Gear) {
			var uberStrikeItemGearView3 = inventoryItem.Item.View as UberStrikeItemGearView;
			armorPoints += uberStrikeItemGearView3.ArmorPoints;
		}
	}

	public bool HasLoadoutItem(LoadoutSlotType loadoutSlotType) {
		IUnityItem unityItem;

		return Loadout.TryGetItem(loadoutSlotType, out unityItem);
	}

	public int GetItemIdOnSlot(LoadoutSlotType loadoutSlotType) {
		var num = 0;
		IUnityItem unityItem;

		if (Loadout.TryGetItem(loadoutSlotType, out unityItem)) {
			num = unityItem.View.ID;
		}

		return num;
	}

	public IUnityItem GetItemOnSlot(LoadoutSlotType loadoutSlotType) {
		IUnityItem unityItem = null;
		Loadout.TryGetItem(loadoutSlotType, out unityItem);

		return unityItem;
	}

	public bool IsItemEquipped(int itemId) {
		return Loadout.Contains(itemId);
	}

	public bool HasItemInSlot(LoadoutSlotType slot) {
		InventoryItem inventoryItem;

		return TryGetItemInSlot(slot, out inventoryItem);
	}

	public bool TryGetItemInSlot(LoadoutSlotType slot, out InventoryItem item) {
		item = null;
		IUnityItem unityItem;

		return Loadout.TryGetItem(slot, out unityItem) && Singleton<InventoryManager>.Instance.TryGetInventoryItem(unityItem.View.ID, out item);
	}

	public bool TryGetSlotForItem(IUnityItem item, out LoadoutSlotType slot) {
		slot = LoadoutSlotType.None;
		var enumerator = Loadout.GetEnumerator();

		while (enumerator.MoveNext()) {
			if (enumerator.Current.Value == item) {
				slot = enumerator.Current.Key;

				return true;
			}
		}

		return false;
	}

	public bool ValidateLoadout() {
		return Loadout.ItemCount > 0;
	}

	public void UpdateArmor() {
		int num;
		GetArmorValues(out num);
		GameState.Current.PlayerData.ArmorCarried.Value = num;
	}

	public List<int> GetWeapons() {
		return new List<int> {
			GetItemIdOnSlot(LoadoutSlotType.WeaponMelee),
			GetItemIdOnSlot(LoadoutSlotType.WeaponPrimary),
			GetItemIdOnSlot(LoadoutSlotType.WeaponSecondary),
			GetItemIdOnSlot(LoadoutSlotType.WeaponTertiary)
		};
	}

	public List<int> GetGear() {
		return new List<int> {
			GetItemIdOnSlot(LoadoutSlotType.GearHead),
			GetItemIdOnSlot(LoadoutSlotType.GearFace),
			GetItemIdOnSlot(LoadoutSlotType.GearGloves),
			GetItemIdOnSlot(LoadoutSlotType.GearUpperBody),
			GetItemIdOnSlot(LoadoutSlotType.GearLowerBody),
			GetItemIdOnSlot(LoadoutSlotType.GearBoots),
			GetItemIdOnSlot(LoadoutSlotType.GearHolo)
		};
	}

	public List<int> GetQuickItems() {
		return new List<int> {
			GetItemIdOnSlot(LoadoutSlotType.QuickUseItem1),
			GetItemIdOnSlot(LoadoutSlotType.QuickUseItem2),
			GetItemIdOnSlot(LoadoutSlotType.QuickUseItem3)
		};
	}
}
