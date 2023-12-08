using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class ClanCreationReturnViewProxy {
		public static void Serialize(Stream stream, ClanCreationReturnView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.ClanView != null) {
					ClanViewProxy.Serialize(memoryStream, instance.ClanView);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.ResultCode);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ClanCreationReturnView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var clanCreationReturnView = new ClanCreationReturnView();

			if ((num & 1) != 0) {
				clanCreationReturnView.ClanView = ClanViewProxy.Deserialize(bytes);
			}

			clanCreationReturnView.ResultCode = Int32Proxy.Deserialize(bytes);

			return clanCreationReturnView;
		}
	}
}
