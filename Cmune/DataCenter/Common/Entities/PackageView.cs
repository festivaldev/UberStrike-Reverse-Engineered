using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class PackageView {
		public int Bonus { get; set; }
		public decimal Price { get; set; }
		public List<int> Items { get; set; }
		public string Name { get; set; }

		public PackageView() {
			Bonus = 0;
			Price = 0m;
			Items = new List<int>();
			Name = string.Empty;
		}

		public PackageView(int bonus, decimal price, List<int> items, string name) {
			Bonus = bonus;
			Price = price;
			Items = items;
			Name = name;
		}
	}
}
