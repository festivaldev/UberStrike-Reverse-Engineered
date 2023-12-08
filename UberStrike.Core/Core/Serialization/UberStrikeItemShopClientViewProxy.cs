using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization {
	public static class UberStrikeItemShopClientViewProxy {
		public static void Serialize(Stream stream, UberStrikeItemShopClientView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.FunctionalItems != null) {
					ListProxy<UberStrikeItemFunctionalView>.Serialize(memoryStream, instance.FunctionalItems, UberStrikeItemFunctionalViewProxy.Serialize);
				} else {
					num |= 1;
				}

				if (instance.GearItems != null) {
					ListProxy<UberStrikeItemGearView>.Serialize(memoryStream, instance.GearItems, UberStrikeItemGearViewProxy.Serialize);
				} else {
					num |= 2;
				}

				if (instance.QuickItems != null) {
					ListProxy<UberStrikeItemQuickView>.Serialize(memoryStream, instance.QuickItems, UberStrikeItemQuickViewProxy.Serialize);
				} else {
					num |= 4;
				}

				if (instance.WeaponItems != null) {
					ListProxy<UberStrikeItemWeaponView>.Serialize(memoryStream, instance.WeaponItems, UberStrikeItemWeaponViewProxy.Serialize);
				} else {
					num |= 8;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static UberStrikeItemShopClientView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var uberStrikeItemShopClientView = new UberStrikeItemShopClientView();

			if ((num & 1) != 0) {
				uberStrikeItemShopClientView.FunctionalItems = ListProxy<UberStrikeItemFunctionalView>.Deserialize(bytes, UberStrikeItemFunctionalViewProxy.Deserialize);
			}

			if ((num & 2) != 0) {
				uberStrikeItemShopClientView.GearItems = ListProxy<UberStrikeItemGearView>.Deserialize(bytes, UberStrikeItemGearViewProxy.Deserialize);
			}

			if ((num & 4) != 0) {
				uberStrikeItemShopClientView.QuickItems = ListProxy<UberStrikeItemQuickView>.Deserialize(bytes, UberStrikeItemQuickViewProxy.Deserialize);
			}

			if ((num & 8) != 0) {
				uberStrikeItemShopClientView.WeaponItems = ListProxy<UberStrikeItemWeaponView>.Deserialize(bytes, UberStrikeItemWeaponViewProxy.Deserialize);
			}

			return uberStrikeItemShopClientView;
		}
	}
}
