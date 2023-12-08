using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class MemberAuthenticationViewModelProxy {
		public static void Serialize(Stream stream, MemberAuthenticationViewModel instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<MemberAuthenticationResult>.Serialize(memoryStream, instance.MemberAuthenticationResult);

				if (instance.MemberView != null) {
					MemberViewProxy.Serialize(memoryStream, instance.MemberView);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static MemberAuthenticationViewModel Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var memberAuthenticationViewModel = new MemberAuthenticationViewModel();
			memberAuthenticationViewModel.MemberAuthenticationResult = EnumProxy<MemberAuthenticationResult>.Deserialize(bytes);

			if ((num & 1) != 0) {
				memberAuthenticationViewModel.MemberView = MemberViewProxy.Deserialize(bytes);
			}

			return memberAuthenticationViewModel;
		}
	}
}
