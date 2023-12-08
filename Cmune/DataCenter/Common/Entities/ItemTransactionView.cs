using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ItemTransactionView {
		public int WithdrawalId { get; set; }
		public DateTime WithdrawalDate { get; set; }
		public int Points { get; set; }
		public int Credits { get; set; }
		public int Cmid { get; set; }
		public bool IsAdminAction { get; set; }
		public int ItemId { get; set; }
		public BuyingDurationType Duration { get; set; }
		public ItemTransactionView() { }

		public ItemTransactionView(int withdrawalId, DateTime withdrawalDate, int points, int credits, int cmid, bool isAdminAction, int itemId, BuyingDurationType duration) {
			WithdrawalId = withdrawalId;
			WithdrawalDate = withdrawalDate;
			Points = points;
			Credits = credits;
			Cmid = cmid;
			IsAdminAction = isAdminAction;
			ItemId = itemId;
			Duration = duration;
		}
	}
}
