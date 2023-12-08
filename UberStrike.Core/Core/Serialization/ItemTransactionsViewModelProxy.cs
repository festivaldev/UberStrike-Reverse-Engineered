using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class ItemTransactionsViewModelProxy {
		public static void Serialize(Stream stream, ItemTransactionsViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.ItemTransactions != null) {
					ListProxy<ItemTransactionView>.Serialize(memoryStream, instance.ItemTransactions, ItemTransactionViewProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.TotalCount);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ItemTransactionsViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var itemTransactionsViewModel = new ItemTransactionsViewModel();

			if ((num & 1) != 0) {
				itemTransactionsViewModel.ItemTransactions = ListProxy<ItemTransactionView>.Deserialize(bytes, ItemTransactionViewProxy.Deserialize);
			}

			itemTransactionsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);

			return itemTransactionsViewModel;
		}
	}
}
