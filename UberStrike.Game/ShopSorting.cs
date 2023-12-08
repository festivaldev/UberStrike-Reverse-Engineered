using System.Collections.Generic;
using UberStrike.Core.Types;

public static class ShopSorting {
	private static int CompareDuration(InventoryItem a, InventoryItem b, bool ascending) {
		if (a.IsPermanent && b.IsPermanent) {
			return CompareName(a.Item, b.Item, ascending);
		}

		if (a.IsPermanent) {
			return (!ascending) ? (-1) : 1;
		}

		if (b.IsPermanent) {
			return (!ascending) ? 1 : (-1);
		}

		if (a.DaysRemaining > b.DaysRemaining) {
			return (!ascending) ? (-1) : 1;
		}

		if (a.DaysRemaining < b.DaysRemaining) {
			return (!ascending) ? 1 : (-1);
		}

		return CompareName(a.Item, b.Item, ascending);
	}

	private static int CompareLevel(IUnityItem a, IUnityItem b, bool ascending) {
		if (a.View.LevelLock < b.View.LevelLock) {
			return (!ascending) ? 1 : (-1);
		}

		if (a.View.LevelLock > b.View.LevelLock) {
			return (!ascending) ? (-1) : 1;
		}

		return CompareName(a, b, ascending);
	}

	private static int CompareClass(IUnityItem a, IUnityItem b, bool ascending) {
		var num = ((!ascending) ? (-1) : 1);
		var num2 = ((a.View.ItemType != UberstrikeItemType.Weapon) ? 100 : 10);
		var num3 = ((b.View.ItemType != UberstrikeItemType.Weapon) ? 100 : 10);

		switch (a.View.ItemClass) {
			case UberstrikeItemClass.WeaponMelee:
				num2 += 7;

				break;
			case UberstrikeItemClass.WeaponMachinegun:
				num2 = num2;

				break;
			case UberstrikeItemClass.WeaponShotgun:
				num2 += 4;

				break;
			case UberstrikeItemClass.WeaponSniperRifle:
				num2 += 2;

				break;
			case UberstrikeItemClass.WeaponCannon:
				num2++;

				break;
			case UberstrikeItemClass.WeaponSplattergun:
				num2 += 5;

				break;
			case UberstrikeItemClass.WeaponLauncher:
				num2 += 3;

				break;
			case UberstrikeItemClass.GearBoots:
				num2 += 4;

				break;
			case UberstrikeItemClass.GearHead:
				num2 = num2;

				break;
			case UberstrikeItemClass.GearFace:
				num2++;

				break;
			case UberstrikeItemClass.GearUpperBody:
				num2 += 2;

				break;
			case UberstrikeItemClass.GearLowerBody:
				num2 += 3;

				break;
			case UberstrikeItemClass.GearGloves:
				num2 += 5;

				break;
			case UberstrikeItemClass.GearHolo:
				num2 += 6;

				break;
		}

		switch (b.View.ItemClass) {
			case UberstrikeItemClass.WeaponMelee:
				num3 += 7;

				break;
			case UberstrikeItemClass.WeaponMachinegun:
				num3 = num3;

				break;
			case UberstrikeItemClass.WeaponShotgun:
				num3 += 4;

				break;
			case UberstrikeItemClass.WeaponSniperRifle:
				num3 += 2;

				break;
			case UberstrikeItemClass.WeaponCannon:
				num3++;

				break;
			case UberstrikeItemClass.WeaponSplattergun:
				num3 += 5;

				break;
			case UberstrikeItemClass.WeaponLauncher:
				num3 += 3;

				break;
			case UberstrikeItemClass.GearBoots:
				num3 += 4;

				break;
			case UberstrikeItemClass.GearHead:
				num3 = num3;

				break;
			case UberstrikeItemClass.GearFace:
				num3++;

				break;
			case UberstrikeItemClass.GearUpperBody:
				num3 += 2;

				break;
			case UberstrikeItemClass.GearLowerBody:
				num3 += 3;

				break;
			case UberstrikeItemClass.GearGloves:
				num3 += 5;

				break;
			case UberstrikeItemClass.GearHolo:
				num3 += 6;

				break;
		}

		if (num2 == num3) {
			return 0;
		}

		return (num2 <= num3) ? (num * -1) : num;
	}

	private static int CompareName(IUnityItem a, IUnityItem b, bool ascending) {
		if (ascending) {
			return string.Compare(a.View.Name, b.View.Name);
		}

		return string.Compare(b.View.Name, a.View.Name);
	}

	public abstract class ItemComparer<T> : IComparer<T> {
		public bool Ascending { get; protected set; }
		public ShopSortedColumns Column { get; set; }
		public abstract int Compare(T a, T b);

		public void SwitchOrder() {
			Ascending = !Ascending;
		}
	}

	public class LevelComparer : ItemComparer<IShopItemGUI> {
		public LevelComparer() {
			Column = ShopSortedColumns.Level;
			Ascending = true;
		}

		public override int Compare(IShopItemGUI a, IShopItemGUI b) {
			return CompareLevel(a.Item, b.Item, Ascending);
		}
	}

	public class ItemClassComparer : ItemComparer<IShopItemGUI> {
		public ItemClassComparer() {
			Column = ShopSortedColumns.Duration;
			Ascending = false;
		}

		public override int Compare(IShopItemGUI a, IShopItemGUI b) {
			if (a.Item.View.ItemClass == b.Item.View.ItemClass) {
				var inventoryItemGUI = a as InventoryItemGUI;
				var inventoryItemGUI2 = b as InventoryItemGUI;

				return CompareDuration(inventoryItemGUI.InventoryItem, inventoryItemGUI2.InventoryItem, Ascending);
			}

			return (!Ascending) ? b.Item.View.ItemClass.CompareTo(a.Item.View.ItemClass) : a.Item.View.ItemClass.CompareTo(b.Item.View.ItemClass);
		}
	}
}
