using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class MysteryBoxWonItemUnityViewProxy {
		public static void Serialize(Stream stream, MysteryBoxWonItemUnityView instance) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.CreditWon);
				Int32Proxy.Serialize(memoryStream, instance.ItemIdWon);
				Int32Proxy.Serialize(memoryStream, instance.PointWon);
				memoryStream.WriteTo(stream);
			}
		}

		public static MysteryBoxWonItemUnityView Deserialize(Stream bytes) {
			return new MysteryBoxWonItemUnityView {
				CreditWon = Int32Proxy.Deserialize(bytes),
				ItemIdWon = Int32Proxy.Deserialize(bytes),
				PointWon = Int32Proxy.Deserialize(bytes)
			};
		}
	}
}
