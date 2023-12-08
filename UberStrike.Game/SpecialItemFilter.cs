using UberStrike.Core.Types;

public class SpecialItemFilter : IShopItemFilter {
	public bool CanPass(IUnityItem item) {
		return item.View.ShopHighlightType != ItemShopHighlightType.None;
	}
}
