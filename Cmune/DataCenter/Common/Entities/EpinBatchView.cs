using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities {
	public class EpinBatchView {
		public int BatchId { get; private set; }
		public int ApplicationId { get; private set; }
		public PaymentProviderType EpinProvider { get; private set; }
		public int Amount { get; private set; }
		public int CreditAmount { get; private set; }
		public DateTime BatchDate { get; private set; }
		public bool IsAdmin { get; private set; }
		public bool IsRetired { get; private set; }
		public List<EpinView> Epins { get; private set; }

		public EpinBatchView(int batchId, int applicationId, PaymentProviderType epinProvider, int amount, int creditAmount, DateTime batchDate, bool isAdmin, bool isRetired, List<EpinView> epins) {
			BatchId = batchId;
			ApplicationId = applicationId;
			EpinProvider = epinProvider;
			Amount = amount;
			CreditAmount = creditAmount;
			BatchDate = batchDate;
			IsAdmin = isAdmin;
			Epins = epins;
			IsRetired = isRetired;
		}
	}
}
