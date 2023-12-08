using System.Collections.Generic;
using System.Text;

namespace Cmune.DataCenter.Common.Entities {
	public class ItemView {
		public int ItemId { get; set; }
		public string Name { get; set; }
		public string PrefabName { get; set; }
		public string Description { get; set; }
		public int CreditsPerDay { get; set; }
		public int PointsPerDay { get; set; }
		public int PermanentPoints { get; set; }
		public int PermanentCredits { get; set; }
		public bool IsDisable { get; set; }
		public bool IsForSale { get; set; }
		public bool IsNew { get; set; }
		public bool IsPopular { get; set; }
		public bool IsFeatured { get; set; }
		public PurchaseType PurchaseType { get; set; }
		public int TypeId { get; set; }
		public int ClassId { get; set; }
		public int AmountRemaining { get; set; }
		public int PackOneAmount { get; set; }
		public int PackTwoAmount { get; set; }
		public int PackThreeAmount { get; set; }
		public bool Enable1Day { get; set; }
		public bool Enable7Days { get; set; }
		public bool Enable30Days { get; set; }
		public bool Enable90Days { get; set; }
		public int MaximumDurationDays { get; set; }
		public int MaximumOwnableAmount { get; set; }
		public Dictionary<string, string> CustomProperties { get; set; }
		public Dictionary<ItemPropertyType, int> ItemProperties { get; set; }

		public bool EnablePermanent {
			get { return PermanentCredits != -1 || PermanentPoints != -1; }
		}

		public ItemView() { }

		protected ItemView(ItemView itemView) {
			AmountRemaining = itemView.AmountRemaining;
			ClassId = itemView.ClassId;
			CreditsPerDay = itemView.CreditsPerDay;
			Description = itemView.Description;
			IsFeatured = itemView.IsFeatured;
			IsForSale = itemView.IsForSale;
			IsNew = itemView.IsNew;
			IsPopular = itemView.IsPopular;
			ItemId = itemView.ItemId;
			Name = itemView.Name;
			PrefabName = itemView.PrefabName;
			PermanentCredits = itemView.PermanentCredits;
			PointsPerDay = itemView.PointsPerDay;
			PurchaseType = itemView.PurchaseType;
			TypeId = itemView.TypeId;
			PackOneAmount = itemView.PackOneAmount;
			PackTwoAmount = itemView.PackTwoAmount;
			PackThreeAmount = itemView.PackThreeAmount;
			MaximumOwnableAmount = itemView.MaximumOwnableAmount;
			Enable1Day = itemView.Enable1Day;
			Enable7Days = itemView.Enable7Days;
			Enable30Days = itemView.Enable30Days;
			Enable90Days = itemView.Enable90Days;
			MaximumDurationDays = itemView.MaximumDurationDays;
			PermanentPoints = itemView.PermanentPoints;
			IsDisable = itemView.IsDisable;
			CustomProperties = ((itemView.CustomProperties == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(itemView.CustomProperties));
			ItemProperties = ((ItemProperties == null) ? new Dictionary<ItemPropertyType, int>() : new Dictionary<ItemPropertyType, int>(itemView.ItemProperties));
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[ItemView: [AmountRemaining: ");
			stringBuilder.Append(AmountRemaining);
			stringBuilder.Append("][ClassId: ");
			stringBuilder.Append(ClassId);
			stringBuilder.Append("][CreditsPerDayShop: ");
			stringBuilder.Append(CreditsPerDay);
			stringBuilder.Append("][Description: ");
			stringBuilder.Append(Description);
			stringBuilder.Append("][IsFeatured: ");
			stringBuilder.Append(IsFeatured);
			stringBuilder.Append("][IsForSale: ");
			stringBuilder.Append(IsForSale);
			stringBuilder.Append("][IsNew: ");
			stringBuilder.Append(IsNew);
			stringBuilder.Append("][IsPopular: ");
			stringBuilder.Append(IsPopular);
			stringBuilder.Append("][ItemId: ");
			stringBuilder.Append(ItemId);
			stringBuilder.Append("][Name: ");
			stringBuilder.Append(Name);
			stringBuilder.Append("][PrefabName: ");
			stringBuilder.Append(PrefabName);
			stringBuilder.Append("][PermanentCredits: ");
			stringBuilder.Append(PermanentCredits);
			stringBuilder.Append("][PointsPerDayShop: ");
			stringBuilder.Append(PointsPerDay);
			stringBuilder.Append("][PurchaseType: ");
			stringBuilder.Append(PurchaseType);
			stringBuilder.Append("][TypeId: ");
			stringBuilder.Append(TypeId);
			stringBuilder.Append("][PackOneAmount: ");
			stringBuilder.Append(PackOneAmount);
			stringBuilder.Append("][PackTwoAmount: ");
			stringBuilder.Append(PackTwoAmount);
			stringBuilder.Append("][PackThreeAmount: ");
			stringBuilder.Append(PackThreeAmount);
			stringBuilder.Append("][MaximumOwnableAmount: ");
			stringBuilder.Append(MaximumOwnableAmount);
			stringBuilder.Append("][Enable1Day: ");
			stringBuilder.Append(Enable1Day);
			stringBuilder.Append("][Enable7Days: ");
			stringBuilder.Append(Enable7Days);
			stringBuilder.Append("][Enable30Days: ");
			stringBuilder.Append(Enable30Days);
			stringBuilder.Append("][Enable90Days: ");
			stringBuilder.Append(Enable90Days);
			stringBuilder.Append("][MaximumDurationDays: ");
			stringBuilder.Append(MaximumDurationDays);
			stringBuilder.Append("][PermanentPoints: ");
			stringBuilder.Append(PermanentPoints);
			stringBuilder.Append("][IsDisable: ");
			stringBuilder.Append(IsDisable);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
