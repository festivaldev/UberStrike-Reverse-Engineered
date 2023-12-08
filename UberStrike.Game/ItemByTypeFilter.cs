using UberStrike.Core.Types;

public class ItemByTypeFilter : IShopItemFilter {
	private UberstrikeItemType _itemType;

	public ItemByTypeFilter(UberstrikeItemType itemType) {
		_itemType = itemType;
	}

	public bool CanPass(IUnityItem item) {
		return item.View.ItemType == _itemType;
	}
}
