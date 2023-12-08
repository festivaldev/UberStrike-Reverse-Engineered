using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class CurrencyDepositViewProxy {
		public static void Serialize(Stream stream, CurrencyDepositView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.ApplicationId);

				if (instance.BundleId != null) {
					Stream stream2 = memoryStream;
					var bundleId = instance.BundleId;
					Int32Proxy.Serialize(stream2, (bundleId == null) ? 0 : bundleId.Value);
				} else {
					num |= 1;
				}

				if (instance.BundleName != null) {
					StringProxy.Serialize(memoryStream, instance.BundleName);
				} else {
					num |= 2;
				}

				DecimalProxy.Serialize(memoryStream, instance.Cash);
				EnumProxy<ChannelType>.Serialize(memoryStream, instance.ChannelId);
				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				Int32Proxy.Serialize(memoryStream, instance.Credits);
				Int32Proxy.Serialize(memoryStream, instance.CreditsDepositId);

				if (instance.CurrencyLabel != null) {
					StringProxy.Serialize(memoryStream, instance.CurrencyLabel);
				} else {
					num |= 4;
				}

				DateTimeProxy.Serialize(memoryStream, instance.DepositDate);
				BooleanProxy.Serialize(memoryStream, instance.IsAdminAction);
				EnumProxy<PaymentProviderType>.Serialize(memoryStream, instance.PaymentProviderId);
				Int32Proxy.Serialize(memoryStream, instance.Points);

				if (instance.TransactionKey != null) {
					StringProxy.Serialize(memoryStream, instance.TransactionKey);
				} else {
					num |= 8;
				}

				DecimalProxy.Serialize(memoryStream, instance.UsdAmount);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static CurrencyDepositView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var currencyDepositView = new CurrencyDepositView();
			currencyDepositView.ApplicationId = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				currencyDepositView.BundleId = Int32Proxy.Deserialize(bytes);
			}

			if ((num & 2) != 0) {
				currencyDepositView.BundleName = StringProxy.Deserialize(bytes);
			}

			currencyDepositView.Cash = DecimalProxy.Deserialize(bytes);
			currencyDepositView.ChannelId = EnumProxy<ChannelType>.Deserialize(bytes);
			currencyDepositView.Cmid = Int32Proxy.Deserialize(bytes);
			currencyDepositView.Credits = Int32Proxy.Deserialize(bytes);
			currencyDepositView.CreditsDepositId = Int32Proxy.Deserialize(bytes);

			if ((num & 4) != 0) {
				currencyDepositView.CurrencyLabel = StringProxy.Deserialize(bytes);
			}

			currencyDepositView.DepositDate = DateTimeProxy.Deserialize(bytes);
			currencyDepositView.IsAdminAction = BooleanProxy.Deserialize(bytes);
			currencyDepositView.PaymentProviderId = EnumProxy<PaymentProviderType>.Deserialize(bytes);
			currencyDepositView.Points = Int32Proxy.Deserialize(bytes);

			if ((num & 8) != 0) {
				currencyDepositView.TransactionKey = StringProxy.Deserialize(bytes);
			}

			currencyDepositView.UsdAmount = DecimalProxy.Deserialize(bytes);

			return currencyDepositView;
		}
	}
}
