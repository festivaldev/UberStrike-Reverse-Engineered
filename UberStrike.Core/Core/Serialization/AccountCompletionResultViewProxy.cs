using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class AccountCompletionResultViewProxy {
		public static void Serialize(Stream stream, AccountCompletionResultView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.ItemsAttributed != null) {
					DictionaryProxy<int, int>.Serialize(memoryStream, instance.ItemsAttributed, Int32Proxy.Serialize, Int32Proxy.Serialize);
				} else {
					num |= 1;
				}

				if (instance.NonDuplicateNames != null) {
					ListProxy<string>.Serialize(memoryStream, instance.NonDuplicateNames, StringProxy.Serialize);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(memoryStream, instance.Result);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static AccountCompletionResultView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var accountCompletionResultView = new AccountCompletionResultView();

			if ((num & 1) != 0) {
				accountCompletionResultView.ItemsAttributed = DictionaryProxy<int, int>.Deserialize(bytes, Int32Proxy.Deserialize, Int32Proxy.Deserialize);
			}

			if ((num & 2) != 0) {
				accountCompletionResultView.NonDuplicateNames = ListProxy<string>.Deserialize(bytes, StringProxy.Deserialize);
			}

			accountCompletionResultView.Result = Int32Proxy.Deserialize(bytes);

			return accountCompletionResultView;
		}
	}
}
