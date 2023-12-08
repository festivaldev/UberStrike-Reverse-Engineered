using System.Collections.Generic;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeItemShopView {
		public List<UberstrikeItemFunctionalView> FunctionalItems { get; set; }
		public List<UberstrikeItemGearView> GearItems { get; set; }
		public List<UberstrikeItemQuickUseView> QuickUseItems { get; set; }
		public List<UberstrikeItemSpecialView> SpecialItems { get; set; }
		public List<UberstrikeItemWeaponModView> WeaponModItems { get; set; }
		public List<UberstrikeItemWeaponView> WeaponItems { get; set; }
		public int DiscountPointsSevenDays { get; set; }
		public int DiscountPointsThirtyDays { get; set; }
		public int DiscountPointsNinetyDays { get; set; }
		public int DiscountCreditsSevenDays { get; set; }
		public int DiscountCreditsThirtyDays { get; set; }
		public int DiscountCreditsNinetyDays { get; set; }
		public Dictionary<int, int> ItemsRecommendationPerMap { get; set; }

		public UberstrikeItemShopView() {
			FunctionalItems = new List<UberstrikeItemFunctionalView>();
			GearItems = new List<UberstrikeItemGearView>();
			QuickUseItems = new List<UberstrikeItemQuickUseView>();
			SpecialItems = new List<UberstrikeItemSpecialView>();
			WeaponItems = new List<UberstrikeItemWeaponView>();
			WeaponModItems = new List<UberstrikeItemWeaponModView>();
		}

		public UberstrikeItemShopView(List<UberstrikeItemFunctionalView> functionalItems, List<UberstrikeItemGearView> gearItems, List<UberstrikeItemQuickUseView> quickUseItems, List<UberstrikeItemSpecialView> specialItems, List<UberstrikeItemWeaponView> weaponItems, List<UberstrikeItemWeaponModView> weaponModItems, int discoutPointsSevenDays, int discountPointsThirtyDays, int discountPointsNinetyDays, int discountCreditsSevenDays, int discountCreditsThirtyDays, int discountCreditsNinetyDays) {
			FunctionalItems = functionalItems;
			GearItems = gearItems;
			QuickUseItems = quickUseItems;
			SpecialItems = specialItems;
			WeaponItems = weaponItems;
			WeaponModItems = weaponModItems;
			DiscountPointsSevenDays = discoutPointsSevenDays;
			DiscountPointsThirtyDays = discountPointsThirtyDays;
			DiscountPointsNinetyDays = discountPointsNinetyDays;
			DiscountCreditsSevenDays = discountCreditsSevenDays;
			DiscountCreditsThirtyDays = discountCreditsThirtyDays;
			DiscountCreditsNinetyDays = discountCreditsNinetyDays;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeItemShopView: ");
			stringBuilder.Append("[FunctionalItems: ");

			if (FunctionalItems != null && FunctionalItems.Count > 0) {
				foreach (var uberstrikeItemFunctionalView in FunctionalItems) {
					stringBuilder.Append(uberstrikeItemFunctionalView);
				}
			}

			stringBuilder.Append("][GearItems: ");

			if (GearItems != null && GearItems.Count > 0) {
				foreach (var uberstrikeItemGearView in GearItems) {
					stringBuilder.Append(uberstrikeItemGearView);
				}
			}

			stringBuilder.Append("][QuickUseItems: ");

			if (QuickUseItems != null && QuickUseItems.Count > 0) {
				foreach (var uberstrikeItemQuickUseView in QuickUseItems) {
					stringBuilder.Append(uberstrikeItemQuickUseView);
				}
			}

			stringBuilder.Append("][SpecialItems: ");

			if (SpecialItems != null && SpecialItems.Count > 0) {
				foreach (var uberstrikeItemSpecialView in SpecialItems) {
					stringBuilder.Append(uberstrikeItemSpecialView);
				}
			}

			stringBuilder.Append("][WeaponItems: ");

			if (WeaponItems != null && WeaponItems.Count > 0) {
				foreach (var uberstrikeItemWeaponView in WeaponItems) {
					stringBuilder.Append(uberstrikeItemWeaponView);
				}
			}

			stringBuilder.Append("][WeaponModItems: ");

			if (WeaponModItems != null && WeaponModItems.Count > 0) {
				foreach (var uberstrikeItemWeaponModView in WeaponModItems) {
					stringBuilder.Append(uberstrikeItemWeaponModView);
				}
			}

			stringBuilder.Append("[DiscountPointsSevenDays: ");
			stringBuilder.Append(DiscountPointsSevenDays);
			stringBuilder.Append("%][DiscountPointsThirtyDays: ");
			stringBuilder.Append(DiscountPointsThirtyDays);
			stringBuilder.Append("%][DiscountPointsNinetyDays: ");
			stringBuilder.Append(DiscountPointsNinetyDays);
			stringBuilder.Append("%][DiscountCreditsSevenDays: ");
			stringBuilder.Append(DiscountCreditsSevenDays);
			stringBuilder.Append("%][DiscountCreditsThirtyDays: ");
			stringBuilder.Append(DiscountCreditsThirtyDays);
			stringBuilder.Append("%][DiscountCreditsNinetyDays: ");
			stringBuilder.Append(DiscountCreditsNinetyDays);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
