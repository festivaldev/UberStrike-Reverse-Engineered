using UberStrike.Core.Types;

public class TemporaryLoadoutManager : Singleton<TemporaryLoadoutManager> {
	public bool IsGearLoadoutModified {
		get { return !Singleton<LoadoutManager>.Instance.Loadout.Compare(GameState.Current.Avatar.Loadout); }
	}

	private TemporaryLoadoutManager() { }

	public void SetLoadoutItem(LoadoutSlotType slot, IUnityItem item) {
		if (item != null) {
			IUnityItem unityItem;

			if (GameState.Current.Avatar.Loadout.TryGetItem(slot, out unityItem) && unityItem != item && !Singleton<InventoryManager>.Instance.Contains(unityItem.View.ID) && unityItem.View.ItemType != UberstrikeItemType.QuickUse) {
				unityItem.Unload();
			}

			GameState.Current.Avatar.Loadout.SetSlot(slot, item);
		}
	}

	public bool IsGearLoadoutModifiedOnSlot(LoadoutSlotType slot) {
		IUnityItem unityItem;

		return GameState.Current.Avatar.Loadout.TryGetItem(slot, out unityItem) && unityItem != Singleton<LoadoutManager>.Instance.GetItemOnSlot(slot);
	}

	public void ResetLoadout(LoadoutSlotType slot) {
		GameState.Current.Avatar.Loadout.ClearSlot(slot);
	}

	public void ResetLoadout() {
		if (!Singleton<LoadoutManager>.Instance.Loadout.Compare(GameState.Current.Avatar.Loadout)) {
			GameState.Current.Avatar.Loadout.ClearAllSlots();
			GameState.Current.Avatar.SetLoadout(new Loadout(Singleton<LoadoutManager>.Instance.Loadout));
		}
	}

	public void TryGear(IUnityItem item) {
		if (item.View.ItemType == UberstrikeItemType.Gear) {
			if (item.View.ItemClass == UberstrikeItemClass.GearHolo) {
				SetLoadoutItem(LoadoutSlotType.GearHolo, item);
			} else {
				SetLoadoutItem(InventoryManager.GetSlotTypeForGear(item), item);
			}

			GameState.Current.Avatar.Decorator.AnimationController.TriggerGearAnimation(item.View.ItemClass);

			switch (item.View.ItemType) {
				case UberstrikeItemType.Weapon:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.Weapons
					});

					break;
				case UberstrikeItemType.Gear:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.Gear
					});

					break;
				case UberstrikeItemType.QuickUse:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.QuickItems
					});

					break;
			}
		}
	}
}
