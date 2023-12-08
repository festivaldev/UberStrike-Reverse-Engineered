using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class ClaimFacebookGiftViewProxy {
		public static void Serialize(Stream stream, ClaimFacebookGiftView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<ClaimFacebookGiftResult>.Serialize(memoryStream, instance.ClaimResult);

				if (instance.ItemId != null) {
					Stream stream2 = memoryStream;
					var itemId = instance.ItemId;
					Int32Proxy.Serialize(stream2, (itemId == null) ? 0 : itemId.Value);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ClaimFacebookGiftView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var claimFacebookGiftView = new ClaimFacebookGiftView();
			claimFacebookGiftView.ClaimResult = EnumProxy<ClaimFacebookGiftResult>.Deserialize(bytes);

			if ((num & 1) != 0) {
				claimFacebookGiftView.ItemId = Int32Proxy.Deserialize(bytes);
			}

			return claimFacebookGiftView;
		}
	}
}
