using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class CurrencyDepositView {
		public int CreditsDepositId { get; set; }
		public DateTime DepositDate { get; set; }
		public int Credits { get; set; }
		public int Points { get; set; }
		public decimal Cash { get; set; }
		public string CurrencyLabel { get; set; }
		public int Cmid { get; set; }
		public bool IsAdminAction { get; set; }
		public PaymentProviderType PaymentProviderId { get; set; }
		public string TransactionKey { get; set; }
		public int ApplicationId { get; set; }
		public ChannelType ChannelId { get; set; }
		public decimal UsdAmount { get; set; }
		public int? BundleId { get; set; }
		public string BundleName { get; set; }
		public CurrencyDepositView() { }

		public CurrencyDepositView(int creditsDepositId, DateTime depositDate, int credits, int points, decimal cash, string currencyLabel, int cmid, bool isAdminAction, PaymentProviderType paymentProviderId, string transactionKey, int applicationId, ChannelType channelId, decimal usdAmount, int? bundleId, string bundleName) {
			CreditsDepositId = creditsDepositId;
			DepositDate = depositDate;
			Credits = credits;
			Points = points;
			Cash = cash;
			CurrencyLabel = currencyLabel;
			Cmid = cmid;
			IsAdminAction = isAdminAction;
			PaymentProviderId = paymentProviderId;
			TransactionKey = transactionKey;
			ApplicationId = applicationId;
			ChannelId = channelId;
			UsdAmount = usdAmount;
			BundleId = bundleId;
			BundleName = bundleName;
		}
	}
}
