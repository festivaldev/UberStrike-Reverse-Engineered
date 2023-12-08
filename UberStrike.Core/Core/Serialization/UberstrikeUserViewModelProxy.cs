using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class UberstrikeUserViewModelProxy {
		public static void Serialize(Stream stream, UberstrikeUserViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.CmuneMemberView != null) {
					MemberViewProxy.Serialize(memoryStream, instance.CmuneMemberView);
				} else {
					num |= 1;
				}

				if (instance.UberstrikeMemberView != null) {
					UberstrikeMemberViewProxy.Serialize(memoryStream, instance.UberstrikeMemberView);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static UberstrikeUserViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var uberstrikeUserViewModel = new UberstrikeUserViewModel();

			if ((num & 1) != 0) {
				uberstrikeUserViewModel.CmuneMemberView = MemberViewProxy.Deserialize(bytes);
			}

			if ((num & 2) != 0) {
				uberstrikeUserViewModel.UberstrikeMemberView = UberstrikeMemberViewProxy.Deserialize(bytes);
			}

			return uberstrikeUserViewModel;
		}
	}
}
