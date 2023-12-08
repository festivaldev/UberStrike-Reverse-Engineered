using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public static class ShopUtils {
	public static ItemPrice GetLowestPrice(IUnityItem item, UberStrikeCurrencyType currency = UberStrikeCurrencyType.None) {
		ItemPrice itemPrice = null;

		if (item != null && item.View != null && item.View.Prices != null) {
			foreach (var itemPrice2 in item.View.Prices) {
				if ((currency == UberStrikeCurrencyType.None || itemPrice2.Currency == currency) && (itemPrice == null || itemPrice.Price > itemPrice2.Price)) {
					itemPrice = itemPrice2;
				}
			}
		}

		return itemPrice;
	}

	public static string PrintDuration(BuyingDurationType duration) {
		switch (duration) {
			case BuyingDurationType.OneDay:
				return " 1 " + LocalizedStrings.Day;
			case BuyingDurationType.SevenDays:
				return " 1 " + LocalizedStrings.Week;
			case BuyingDurationType.ThirtyDays:
				return " 1 " + LocalizedStrings.Month;
			case BuyingDurationType.NinetyDays:
				return " " + LocalizedStrings.ThreeMonths;
			case BuyingDurationType.Permanent:
				return " " + LocalizedStrings.Permanent;
			default:
				return string.Empty;
		}
	}

	public static Texture2D CurrencyIcon(UberStrikeCurrencyType currency) {
		if (currency == UberStrikeCurrencyType.Credits) {
			return ShopIcons.IconCredits20x20;
		}

		if (currency != UberStrikeCurrencyType.Points) {
			return null;
		}

		return ShopIcons.IconPoints20x20;
	}

	public static bool IsMeleeWeapon(IUnityItem view) {
		return view != null && view.View != null && view.View.ItemClass == UberstrikeItemClass.WeaponMelee;
	}

	public static bool IsInstantHitWeapon(IUnityItem view) {
		return view != null && view.View != null && (view.View.ItemClass == UberstrikeItemClass.WeaponMachinegun || view.View.ItemClass == UberstrikeItemClass.WeaponShotgun || view.View.ItemClass == UberstrikeItemClass.WeaponSniperRifle);
	}

	public static bool IsProjectileWeapon(IUnityItem view) {
		return view != null && view.View != null && (view.View.ItemClass == UberstrikeItemClass.WeaponCannon || view.View.ItemClass == UberstrikeItemClass.WeaponLauncher || view.View.ItemClass == UberstrikeItemClass.WeaponSplattergun);
	}

	public class PriceComparer<T> : IComparer<KeyValuePair<T, ItemPrice>> {
		public int Compare(KeyValuePair<T, ItemPrice> x, KeyValuePair<T, ItemPrice> y) {
			var num = x.Value.Price + ((x.Value.Currency != UberStrikeCurrencyType.Credits) ? 0 : 1000000);

			return (y.Value.Price + ((y.Value.Currency != UberStrikeCurrencyType.Credits) ? 0 : 1000000)).CompareTo(num);
		}
	}

	private class DescendedComparer : IComparer<int> {
		public int Compare(int x, int y) {
			return y - x;
		}
	}
}
