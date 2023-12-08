using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class ClanRequestAcceptViewProxy {
		public static void Serialize(Stream stream, ClanRequestAcceptView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.ActionResult);
				Int32Proxy.Serialize(memoryStream, instance.ClanRequestId);

				if (instance.ClanView != null) {
					ClanViewProxy.Serialize(memoryStream, instance.ClanView);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ClanRequestAcceptView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var clanRequestAcceptView = new ClanRequestAcceptView();
			clanRequestAcceptView.ActionResult = Int32Proxy.Deserialize(bytes);
			clanRequestAcceptView.ClanRequestId = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				clanRequestAcceptView.ClanView = ClanViewProxy.Deserialize(bytes);
			}

			return clanRequestAcceptView;
		}
	}
}
