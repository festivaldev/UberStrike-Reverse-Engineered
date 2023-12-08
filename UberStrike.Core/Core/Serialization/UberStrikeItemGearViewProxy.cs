using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

namespace UberStrike.Core.Serialization {
	public static class UberStrikeItemGearViewProxy {
		public static void Serialize(Stream stream, UberStrikeItemGearView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.ArmorPoints);
				Int32Proxy.Serialize(memoryStream, instance.ArmorWeight);

				if (instance.CustomProperties != null) {
					DictionaryProxy<string, string>.Serialize(memoryStream, instance.CustomProperties, StringProxy.Serialize, StringProxy.Serialize);
				} else {
					num |= 1;
				}

				if (instance.Description != null) {
					StringProxy.Serialize(memoryStream, instance.Description);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(memoryStream, instance.ID);
				BooleanProxy.Serialize(memoryStream, instance.IsConsumable);
				EnumProxy<UberstrikeItemClass>.Serialize(memoryStream, instance.ItemClass);

				if (instance.ItemProperties != null) {
					DictionaryProxy<ItemPropertyType, int>.Serialize(memoryStream, instance.ItemProperties, EnumProxy<ItemPropertyType>.Serialize, Int32Proxy.Serialize);
				} else {
					num |= 4;
				}

				Int32Proxy.Serialize(memoryStream, instance.LevelLock);
				Int32Proxy.Serialize(memoryStream, instance.MaxDurationDays);

				if (instance.Name != null) {
					StringProxy.Serialize(memoryStream, instance.Name);
				} else {
					num |= 8;
				}

				if (instance.PrefabName != null) {
					StringProxy.Serialize(memoryStream, instance.PrefabName);
				} else {
					num |= 16;
				}

				if (instance.Prices != null) {
					ListProxy<ItemPrice>.Serialize(memoryStream, instance.Prices, ItemPriceProxy.Serialize);
				} else {
					num |= 32;
				}

				EnumProxy<ItemShopHighlightType>.Serialize(memoryStream, instance.ShopHighlightType);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static UberStrikeItemGearView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var uberStrikeItemGearView = new UberStrikeItemGearView();
			uberStrikeItemGearView.ArmorPoints = Int32Proxy.Deserialize(bytes);
			uberStrikeItemGearView.ArmorWeight = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				uberStrikeItemGearView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, StringProxy.Deserialize, StringProxy.Deserialize);
			}

			if ((num & 2) != 0) {
				uberStrikeItemGearView.Description = StringProxy.Deserialize(bytes);
			}

			uberStrikeItemGearView.ID = Int32Proxy.Deserialize(bytes);
			uberStrikeItemGearView.IsConsumable = BooleanProxy.Deserialize(bytes);
			uberStrikeItemGearView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);

			if ((num & 4) != 0) {
				uberStrikeItemGearView.ItemProperties = DictionaryProxy<ItemPropertyType, int>.Deserialize(bytes, EnumProxy<ItemPropertyType>.Deserialize, Int32Proxy.Deserialize);
			}

			uberStrikeItemGearView.LevelLock = Int32Proxy.Deserialize(bytes);
			uberStrikeItemGearView.MaxDurationDays = Int32Proxy.Deserialize(bytes);

			if ((num & 8) != 0) {
				uberStrikeItemGearView.Name = StringProxy.Deserialize(bytes);
			}

			if ((num & 16) != 0) {
				uberStrikeItemGearView.PrefabName = StringProxy.Deserialize(bytes);
			}

			if ((num & 32) != 0) {
				uberStrikeItemGearView.Prices = ListProxy<ItemPrice>.Deserialize(bytes, ItemPriceProxy.Deserialize);
			}

			uberStrikeItemGearView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);

			return uberStrikeItemGearView;
		}
	}
}
