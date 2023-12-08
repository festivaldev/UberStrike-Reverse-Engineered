using Cmune.DataCenter.Common.Entities;

public static class ItemPackGuiUtil {
	public const int Columns = 6;
	public const int Rows = 2;

	public static BuyingDurationType GetDuration(IUnityItem item) {
		var buyingDurationType = BuyingDurationType.None;

		if (item != null && item.View != null && item.View.Prices != null) {
			var enumerator = item.View.Prices.GetEnumerator();

			if (enumerator.MoveNext()) {
				buyingDurationType = enumerator.Current.Duration;
			}
		}

		return buyingDurationType;
	}
}
