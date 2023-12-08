using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ItemInventoryView {
		public int Cmid { get; set; }
		public int ItemId { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public int AmountRemaining { get; set; }
		public ItemInventoryView() { }

		public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining) {
			ItemId = itemId;
			ExpirationDate = expirationDate;
			AmountRemaining = amountRemaining;
		}

		public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining, int cmid) : this(itemId, expirationDate, amountRemaining) {
			Cmid = cmid;
		}

		public override string ToString() {
			var text = "[LiveInventoryView: ";
			var text2 = text;
			text = string.Concat(text2, "[Item Id: ", ItemId, "]");
			text2 = text;
			text = string.Concat(text2, "[Expiration date: ", ExpirationDate, "]");
			text2 = text;
			text = string.Concat(text2, "[Amount remaining:", AmountRemaining, "]");

			return text + "]";
		}
	}
}
