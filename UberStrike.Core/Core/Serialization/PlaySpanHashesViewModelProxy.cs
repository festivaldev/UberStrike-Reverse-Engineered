using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class PlaySpanHashesViewModelProxy {
		public static void Serialize(Stream stream, PlaySpanHashesViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Hashes != null) {
					DictionaryProxy<decimal, string>.Serialize(memoryStream, instance.Hashes, DecimalProxy.Serialize, StringProxy.Serialize);
				} else {
					num |= 1;
				}

				if (instance.MerchTrans != null) {
					StringProxy.Serialize(memoryStream, instance.MerchTrans);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static PlaySpanHashesViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var playSpanHashesViewModel = new PlaySpanHashesViewModel();

			if ((num & 1) != 0) {
				playSpanHashesViewModel.Hashes = DictionaryProxy<decimal, string>.Deserialize(bytes, DecimalProxy.Deserialize, StringProxy.Deserialize);
			}

			if ((num & 2) != 0) {
				playSpanHashesViewModel.MerchTrans = StringProxy.Deserialize(bytes);
			}

			return playSpanHashesViewModel;
		}
	}
}
