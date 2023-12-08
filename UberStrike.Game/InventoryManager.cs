using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager> {
	private static readonly InventoryItem EmptyItem = new InventoryItem(null);
	private IDictionary<int, InventoryItem> _inventoryItems;
	public LoadoutSlotType CurrentFunctionalSlot = LoadoutSlotType.FunctionalItem1;
	public LoadoutSlotType CurrentQuickItemSot = LoadoutSlotType.QuickUseItem1;
	public LoadoutSlotType CurrentWeaponSlot = LoadoutSlotType.WeaponPrimary;

	public IEnumerable<InventoryItem> InventoryItems {
		get { return _inventoryItems.Values; }
	}

	private InventoryManager() {
		_inventoryItems = new Dictionary<int, InventoryItem>();
		OnInventoryUpdated = (Action)Delegate.Combine(OnInventoryUpdated, new Action(delegate { }));
	}

	public event Action OnInventoryUpdated;

	public IEnumerator StartUpdateInventoryAndEquipNewItem(IUnityItem item, bool equipNow = false) {
		if (item != null) {
			var popupDialog = PopupSystem.ShowMessage(LocalizedStrings.UpdatingInventory, LocalizedStrings.WereUpdatingYourInventoryPleaseWait, PopupSystem.AlertType.None);

			yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(false));

			PopupSystem.HideMessage(popupDialog);

			if (equipNow) {
				EquipItem(item);
			} else if (GameState.Current.HasJoinedGame && GameState.Current.IsInGame) {
				EquipItem(item);
			} else if (item.View.ItemProperties.ContainsKey(ItemPropertyType.PointsBoost) || item.View.ItemProperties.ContainsKey(ItemPropertyType.XpBoost)) {
				var invItem = GetItem(item.View.ID);
				PopupSystem.ShowItem(item, "\nYou just bought the boost item!\nThis item is activated and expires in " + invItem.DaysRemaining + " days");
			} else {
				PopupSystem.ShowItem(item, string.Empty);
			}

			yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetMember());
		}
	}

	public bool EquipItem(IUnityItem item) {
		var loadoutSlotType = LoadoutSlotType.None;
		InventoryItem inventoryItem;

		if (TryGetInventoryItem(item.View.ID, out inventoryItem) && inventoryItem.IsValid && inventoryItem.Item.View.ItemType == UberstrikeItemType.Weapon && GameState.Current.Map != null) {
			loadoutSlotType = FindBestSlotToEquipWeapon(item);
		}

		return EquipItemOnSlot(item.View.ID, loadoutSlotType);
	}

	public static LoadoutSlotType FindBestSlotToEquipWeapon(IUnityItem weapon) {
		var itemClass = weapon.View.ItemClass;

		if (itemClass == UberstrikeItemClass.WeaponMelee) {
			return LoadoutSlotType.WeaponMelee;
		}

		var itemClassSlotType = Singleton<LoadoutManager>.Instance.Loadout.GetItemClassSlotType(itemClass);

		if (itemClassSlotType != LoadoutSlotType.None) {
			return itemClassSlotType;
		}

		var firstEmptyWeaponSlot = Singleton<LoadoutManager>.Instance.Loadout.GetFirstEmptyWeaponSlot();

		if (firstEmptyWeaponSlot != LoadoutSlotType.None) {
			return firstEmptyWeaponSlot;
		}

		return LoadoutSlotType.WeaponPrimary;
	}

	public void UnequipWeaponSlot(LoadoutSlotType slotType) {
		Singleton<LoadoutManager>.Instance.ResetSlot(slotType);
		GameState.Current.Avatar.UnassignWeapon(slotType);
	}

	public bool EquipItemOnSlot(int itemId, LoadoutSlotType slotType) {
		InventoryItem inventoryItem;

		if (TryGetInventoryItem(itemId, out inventoryItem) && inventoryItem.IsValid) {
			if (Singleton<LoadoutManager>.Instance.IsItemEquipped(itemId)) {
				LoadoutSlotType loadoutSlotType;

				if (Singleton<LoadoutManager>.Instance.TryGetSlotForItem(inventoryItem.Item, out loadoutSlotType)) {
					EventHandler.Global.Fire(new ShopEvents.ShopHighlightSlot {
						SlotType = loadoutSlotType
					});

					Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout(loadoutSlotType);
				}
			} else {
				HighlightItem(itemId, false);

				switch (inventoryItem.Item.View.ItemType) {
					case UberstrikeItemType.Weapon:
						if (inventoryItem.Item.View.ItemClass == UberstrikeItemClass.WeaponMelee) {
							slotType = LoadoutSlotType.WeaponMelee;
							Singleton<LoadoutManager>.Instance.RemoveDuplicateWeaponClass(inventoryItem);
							Singleton<LoadoutManager>.Instance.SetLoadoutItem(slotType, inventoryItem.Item);
							Singleton<LoadoutManager>.Instance.EquipWeapon(slotType, inventoryItem.Item);
							AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.WeaponSwitch);
						} else {
							if (slotType == LoadoutSlotType.None || slotType == LoadoutSlotType.WeaponMelee) {
								slotType = GetNextFreeWeaponSlot();
							}

							var loadoutSlotType2 = slotType;

							if (Singleton<LoadoutManager>.Instance.RemoveDuplicateWeaponClass(inventoryItem, ref loadoutSlotType2) && slotType != loadoutSlotType2) {
								Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, loadoutSlotType2);
							}

							Singleton<LoadoutManager>.Instance.SetLoadoutItem(slotType, inventoryItem.Item);
							Singleton<LoadoutManager>.Instance.EquipWeapon(slotType, inventoryItem.Item);
							AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.EquipWeapon);
						}

						goto IL_2B6;
					case UberstrikeItemType.Gear:
						slotType = ItemUtil.SlotFromItemClass(inventoryItem.Item.View.ItemClass);
						Singleton<LoadoutManager>.Instance.SetLoadoutItem(slotType, inventoryItem.Item);
						AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.EquipGear);

						if (GameState.Current.Avatar != null) {
							GameState.Current.Avatar.HideWeapons();
						}

						GameState.Current.Avatar.Decorator.AnimationController.TriggerGearAnimation(inventoryItem.Item.View.ItemClass);

						goto IL_2B6;
					case UberstrikeItemType.QuickUse:
						EquipQuickItemOnSlot(inventoryItem, slotType);

						goto IL_2B6;
					case UberstrikeItemType.Functional:
						if (inventoryItem.Item.Equippable) {
							if (slotType == LoadoutSlotType.None) {
								slotType = GetNextFreeFunctionalSlot();
							}

							var loadoutSlotType3 = slotType;

							if (Singleton<LoadoutManager>.Instance.RemoveDuplicateFunctionalItemClass(inventoryItem, ref loadoutSlotType3) && slotType != loadoutSlotType3) {
								Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, loadoutSlotType3);
							}

							Singleton<LoadoutManager>.Instance.SetLoadoutItem(slotType, inventoryItem.Item);
							AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.EquipItem);
						}

						goto IL_2B6;
				}

				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.EquipItem);
				Debug.LogError("Equip item of type: " + inventoryItem.Item.View.ItemType);
				IL_2B6:
				Singleton<TemporaryLoadoutManager>.Instance.SetLoadoutItem(slotType, inventoryItem.Item);

				EventHandler.Global.Fire(new ShopEvents.ShopHighlightSlot {
					SlotType = slotType
				});
			}

			return true;
		}

		return false;
	}

	private void EquipQuickItemOnSlot(InventoryItem item, LoadoutSlotType slotType) {
		if (slotType < LoadoutSlotType.QuickUseItem1 || slotType > LoadoutSlotType.QuickUseItem3) {
			slotType = GetNextFreeQuickItemSlot();
		}

		var loadoutSlotType = slotType;

		if (slotType != loadoutSlotType && Singleton<LoadoutManager>.Instance.RemoveDuplicateQuickItemClass(item.Item.View as UberStrikeItemQuickView, ref loadoutSlotType)) {
			Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, loadoutSlotType);
		}

		AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.EquipItem);
		Singleton<LoadoutManager>.Instance.SetLoadoutItem(slotType, item.Item);
	}

	private LoadoutSlotType GetNextFreeWeaponSlot() {
		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponPrimary)) {
			return LoadoutSlotType.WeaponPrimary;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponSecondary)) {
			return LoadoutSlotType.WeaponSecondary;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponTertiary)) {
			return LoadoutSlotType.WeaponTertiary;
		}

		if (CurrentWeaponSlot == LoadoutSlotType.WeaponPrimary || CurrentWeaponSlot == LoadoutSlotType.WeaponSecondary || CurrentWeaponSlot == LoadoutSlotType.WeaponTertiary) {
			return CurrentWeaponSlot;
		}

		return LoadoutSlotType.WeaponPrimary;
	}

	private LoadoutSlotType GetNextFreeFunctionalSlot() {
		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem1)) {
			return LoadoutSlotType.FunctionalItem1;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem2)) {
			return LoadoutSlotType.FunctionalItem2;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem3)) {
			return LoadoutSlotType.FunctionalItem3;
		}

		switch (CurrentFunctionalSlot) {
			case LoadoutSlotType.FunctionalItem1:
				return LoadoutSlotType.FunctionalItem2;
			case LoadoutSlotType.FunctionalItem2:
				return LoadoutSlotType.FunctionalItem3;
			case LoadoutSlotType.FunctionalItem3:
				return LoadoutSlotType.FunctionalItem1;
			default:
				return CurrentFunctionalSlot;
		}
	}

	private LoadoutSlotType GetNextFreeQuickItemSlot() {
		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem1)) {
			return LoadoutSlotType.QuickUseItem1;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem2)) {
			return LoadoutSlotType.QuickUseItem2;
		}

		if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem3)) {
			return LoadoutSlotType.QuickUseItem3;
		}

		switch (CurrentQuickItemSot) {
			case LoadoutSlotType.QuickUseItem1:
				return LoadoutSlotType.QuickUseItem2;
			case LoadoutSlotType.QuickUseItem2:
				return LoadoutSlotType.QuickUseItem3;
			case LoadoutSlotType.QuickUseItem3:
				return LoadoutSlotType.QuickUseItem1;
			default:
				return CurrentQuickItemSot;
		}
	}

	public static LoadoutSlotType GetSlotTypeForGear(IUnityItem gearItem) {
		if (gearItem != null) {
			switch (gearItem.View.ItemClass) {
				case UberstrikeItemClass.GearBoots:
					return LoadoutSlotType.GearBoots;
				case UberstrikeItemClass.GearHead:
					return LoadoutSlotType.GearHead;
				case UberstrikeItemClass.GearFace:
					return LoadoutSlotType.GearFace;
				case UberstrikeItemClass.GearUpperBody:
					return LoadoutSlotType.GearUpperBody;
				case UberstrikeItemClass.GearLowerBody:
					return LoadoutSlotType.GearLowerBody;
				case UberstrikeItemClass.GearGloves:
					return LoadoutSlotType.GearGloves;
				case UberstrikeItemClass.GearHolo:
					return LoadoutSlotType.GearHolo;
			}

			return LoadoutSlotType.None;
		}

		return LoadoutSlotType.None;
	}

	public List<InventoryItem> GetAllItems(bool ignoreEquippedItems) {
		var list = new List<InventoryItem>();

		foreach (var inventoryItem in _inventoryItems.Values) {
			var flag = inventoryItem.DaysRemaining <= 0 && inventoryItem.Item.View.Prices != null && inventoryItem.Item.View.Prices.Count > 0;

			if (inventoryItem.DaysRemaining > 0 || inventoryItem.IsPermanent || flag) {
				if (ignoreEquippedItems) {
					if (!Singleton<LoadoutManager>.Instance.IsItemEquipped(inventoryItem.Item.View.ID)) {
						list.Add(inventoryItem);
					}
				} else {
					list.Add(inventoryItem);
				}
			}
		}

		return list;
	}

	public int GetGearItem(int itemID, UberstrikeItemClass itemClass) {
		InventoryItem inventoryItem;

		if (_inventoryItems.TryGetValue(itemID, out inventoryItem) && inventoryItem != null && inventoryItem.Item.View.ItemType == UberstrikeItemType.Gear) {
			return inventoryItem.Item.View.ID;
		}

		IUnityItem unityItem;

		if (Singleton<ItemManager>.Instance.TryGetDefaultItem(itemClass, out unityItem)) {
			return unityItem.View.ID;
		}

		return 0;
	}

	public InventoryItem GetItem(int itemID) {
		InventoryItem inventoryItem;

		if (_inventoryItems.TryGetValue(itemID, out inventoryItem) && inventoryItem != null) {
			return inventoryItem;
		}

		return EmptyItem;
	}

	public InventoryItem GetWeaponItem(int itemId) {
		InventoryItem inventoryItem;

		if (_inventoryItems.TryGetValue(itemId, out inventoryItem) && inventoryItem != null && inventoryItem.Item.View.ItemType == UberstrikeItemType.Weapon) {
			return inventoryItem;
		}

		return EmptyItem;
	}

	public bool TryGetInventoryItem(int itemID, out InventoryItem item) {
		return _inventoryItems.TryGetValue(itemID, out item) && item != null && item.Item != null;
	}

	public bool HasClanLicense() {
		return Contains(1234);
	}

	public bool IsItemValidForDays(InventoryItem item, int days) {
		return item != null && (item.DaysRemaining > days || item.IsPermanent);
	}

	public bool Contains(int itemId) {
		InventoryItem inventoryItem;

		return _inventoryItems.TryGetValue(itemId, out inventoryItem) && IsItemValidForDays(inventoryItem, 0);
	}

	public void UpdateInventoryItems(List<ItemInventoryView> inventory) {
		if (Singleton<ItemManager>.Instance.ShopItemCount == 0) {
			Debug.LogWarning("Stopped updating inventory because shop is empty!");

			return;
		}

		var hashSet = new HashSet<int>(_inventoryItems.Keys);
		_inventoryItems.Clear();

		foreach (var itemInventoryView in inventory) {
			var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemInventoryView.ItemId);

			if (itemInShop != null && itemInShop.View.ID == itemInventoryView.ItemId) {
				var inventoryItems = _inventoryItems;
				var id = itemInShop.View.ID;
				var inventoryItem = new InventoryItem(itemInShop);
				inventoryItem.IsPermanent = itemInventoryView.ExpirationDate == null;
				inventoryItem.AmountRemaining = itemInventoryView.AmountRemaining;
				var inventoryItem2 = inventoryItem;
				var expirationDate = itemInventoryView.ExpirationDate;
				inventoryItem2.ExpirationDate = (expirationDate == null) ? DateTime.MinValue : expirationDate.Value;
				inventoryItem.IsHighlighted = hashSet.Count > 0 && !hashSet.Contains(itemInShop.View.ID);
				inventoryItems[id] = inventoryItem;
			} else {
				Debug.LogWarning(string.Concat("Inventory Item not found: ", itemInventoryView.ItemId, " ", itemInShop == null));
			}
		}

		OnInventoryUpdated();
	}

	internal void HighlightItem(int itemId, bool isHighlighted) {
		InventoryItem inventoryItem;

		if (_inventoryItems.TryGetValue(itemId, out inventoryItem) && inventoryItem != null) {
			inventoryItem.IsHighlighted = isHighlighted;
		}
	}

	public void EnableAllItems() {
		Debug.Log("PopulateCompleteInventory");
		_inventoryItems.Clear();

		foreach (var unityItem in Singleton<ItemManager>.Instance.ShopItems) {
			_inventoryItems.Add(unityItem.View.ID, new InventoryItem(unityItem) {
				IsPermanent = true,
				AmountRemaining = 0,
				ExpirationDate = DateTime.MaxValue
			});
		}
	}
}
