using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class CurrencyDepositsViewModelProxy {
		public static void Serialize(Stream stream, CurrencyDepositsViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.CurrencyDeposits != null) {
					ListProxy<CurrencyDepositView>.Serialize(memoryStream, instance.CurrencyDeposits, CurrencyDepositViewProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.TotalCount);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static CurrencyDepositsViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var currencyDepositsViewModel = new CurrencyDepositsViewModel();

			if ((num & 1) != 0) {
				currencyDepositsViewModel.CurrencyDeposits = ListProxy<CurrencyDepositView>.Deserialize(bytes, CurrencyDepositViewProxy.Deserialize);
			}

			currencyDepositsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);

			return currencyDepositsViewModel;
		}
	}
}
