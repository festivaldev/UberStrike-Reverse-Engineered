using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization {
	public static class ItemAssetBundleViewProxy {
		public static void Serialize(Stream stream, ItemAssetBundleView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Url != null) {
					StringProxy.Serialize(memoryStream, instance.Url);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ItemAssetBundleView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var itemAssetBundleView = new ItemAssetBundleView();

			if ((num & 1) != 0) {
				itemAssetBundleView.Url = StringProxy.Deserialize(bytes);
			}

			return itemAssetBundleView;
		}
	}
}
