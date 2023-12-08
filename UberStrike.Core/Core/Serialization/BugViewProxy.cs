using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class BugViewProxy {
		public static void Serialize(Stream stream, BugView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Content != null) {
					StringProxy.Serialize(memoryStream, instance.Content);
				} else {
					num |= 1;
				}

				if (instance.Subject != null) {
					StringProxy.Serialize(memoryStream, instance.Subject);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static BugView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var bugView = new BugView();

			if ((num & 1) != 0) {
				bugView.Content = StringProxy.Deserialize(bytes);
			}

			if ((num & 2) != 0) {
				bugView.Subject = StringProxy.Deserialize(bytes);
			}

			return bugView;
		}
	}
}
