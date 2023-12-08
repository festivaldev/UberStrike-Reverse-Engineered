using Cmune.DataCenter.Common.Entities;

public class ShopItemView {
	public BuyingDurationType Duration { get; private set; }
	public IUnityItem UnityItem { get; private set; }
	public int ItemId { get; private set; }
	public int Credits { get; private set; }
	public int Points { get; private set; }

	public ShopItemView(int itemId) {
		var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemId);
		Points = 0;
		Credits = 0;
		UnityItem = itemInShop;
		ItemId = itemId;
		Duration = BuyingDurationType.None;
	}

	public ShopItemView(BundleItemView itemView) {
		var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemView.ItemId);
		Points = 0;
		Credits = 0;
		UnityItem = itemInShop;
		ItemId = itemView.ItemId;
		Duration = itemView.Duration;
	}

	public ShopItemView(UberStrikeCurrencyType currency, int price) {
		if (currency != UberStrikeCurrencyType.Credits) {
			if (currency != UberStrikeCurrencyType.Points) {
				UnityItem = null;
				Points = 0;
				Credits = 0;
			} else {
				UnityItem = new PointsUnityItem(price);
				Credits = 0;
				Points = price;
			}
		} else {
			UnityItem = new CreditsUnityItem(price);
			Points = 0;
			Credits = price;
		}

		ItemId = 0;
		Duration = BuyingDurationType.None;
	}
}
