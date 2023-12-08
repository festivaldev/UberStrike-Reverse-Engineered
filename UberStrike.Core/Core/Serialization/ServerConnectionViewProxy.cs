using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class ServerConnectionViewProxy {
		public static void Serialize(Stream stream, ServerConnectionView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<MemberAccessLevel>.Serialize(memoryStream, instance.AccessLevel);

				if (instance.ApiVersion != null) {
					StringProxy.Serialize(memoryStream, instance.ApiVersion);
				} else {
					num |= 1;
				}

				EnumProxy<ChannelType>.Serialize(memoryStream, instance.Channel);
				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ServerConnectionView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var serverConnectionView = new ServerConnectionView();
			serverConnectionView.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);

			if ((num & 1) != 0) {
				serverConnectionView.ApiVersion = StringProxy.Deserialize(bytes);
			}

			serverConnectionView.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
			serverConnectionView.Cmid = Int32Proxy.Deserialize(bytes);

			return serverConnectionView;
		}
	}
}
