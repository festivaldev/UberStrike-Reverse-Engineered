using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class UberstrikeLevelViewModelProxy {
		public static void Serialize(Stream stream, UberstrikeLevelViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Maps != null) {
					ListProxy<MapView>.Serialize(memoryStream, instance.Maps, MapViewProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static UberstrikeLevelViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var uberstrikeLevelViewModel = new UberstrikeLevelViewModel();

			if ((num & 1) != 0) {
				uberstrikeLevelViewModel.Maps = ListProxy<MapView>.Deserialize(bytes, MapViewProxy.Deserialize);
			}

			return uberstrikeLevelViewModel;
		}
	}
}
