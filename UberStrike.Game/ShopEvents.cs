using UberStrike.Core.Types;

public static class ShopEvents {
	public class ShopHighlightSlot {
		public LoadoutSlotType SlotType { get; set; }
	}

	public class SelectShopArea {
		public ShopArea ShopArea { get; set; }
		public UberstrikeItemClass ItemClass { get; set; }
		public UberstrikeItemType ItemType { get; set; }
	}

	public class SelectLoadoutArea {
		public LoadoutArea Area { get; set; }
	}

	public class LoadoutAreaChanged {
		public LoadoutArea Area { get; set; }
	}

	public class SelectShopItem {
		public IUnityItem Item { get; set; }
	}

	public class RefreshCurrentItemList {
		public bool UseCurrentSelection { get; private set; }
		public UberstrikeItemClass ItemClass { get; private set; }
		public UberstrikeItemType ItemType { get; private set; }

		public RefreshCurrentItemList() {
			UseCurrentSelection = true;
		}

		public RefreshCurrentItemList(UberstrikeItemClass itemClass, UberstrikeItemType itemType) {
			UseCurrentSelection = false;
			ItemClass = itemClass;
			ItemType = itemType;
		}
	}
}
