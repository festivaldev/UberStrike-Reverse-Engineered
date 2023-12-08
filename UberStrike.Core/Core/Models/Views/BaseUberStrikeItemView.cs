using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Core.Models.Views {
	public abstract class BaseUberStrikeItemView {
		[SerializeField]
		private UberstrikeItemClass _itemClass;

		public abstract UberstrikeItemType ItemType { get; }

		public UberstrikeItemClass ItemClass {
			get { return _itemClass; }
			set { _itemClass = value; }
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string PrefabName { get; set; }
		public string Description { get; set; }
		public int LevelLock { get; set; }
		public int MaxDurationDays { get; set; }
		public bool IsConsumable { get; set; }
		public ICollection<ItemPrice> Prices { get; set; }

		public bool IsForSale {
			get { return Prices != null && Prices.Count > 0; }
		}

		public ItemShopHighlightType ShopHighlightType { get; set; }
		public Dictionary<string, string> CustomProperties { get; set; }
		public Dictionary<ItemPropertyType, int> ItemProperties { get; set; }
	}
}
