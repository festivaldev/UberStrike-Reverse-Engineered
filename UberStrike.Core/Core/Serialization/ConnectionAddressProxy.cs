using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class ConnectionAddressProxy {
		public static void Serialize(Stream stream, ConnectionAddress instance) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.Ipv4);
				UInt16Proxy.Serialize(memoryStream, instance.Port);
				memoryStream.WriteTo(stream);
			}
		}

		public static ConnectionAddress Deserialize(Stream bytes) {
			return new ConnectionAddress {
				Ipv4 = Int32Proxy.Deserialize(bytes),
				Port = UInt16Proxy.Deserialize(bytes)
			};
		}
	}
}
