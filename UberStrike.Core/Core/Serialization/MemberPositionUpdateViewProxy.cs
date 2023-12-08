using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class MemberPositionUpdateViewProxy {
		public static void Serialize(Stream stream, MemberPositionUpdateView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.AuthToken != null) {
					StringProxy.Serialize(memoryStream, instance.AuthToken);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.GroupId);
				Int32Proxy.Serialize(memoryStream, instance.MemberCmid);
				EnumProxy<GroupPosition>.Serialize(memoryStream, instance.Position);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static MemberPositionUpdateView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var memberPositionUpdateView = new MemberPositionUpdateView();

			if ((num & 1) != 0) {
				memberPositionUpdateView.AuthToken = StringProxy.Deserialize(bytes);
			}

			memberPositionUpdateView.GroupId = Int32Proxy.Deserialize(bytes);
			memberPositionUpdateView.MemberCmid = Int32Proxy.Deserialize(bytes);
			memberPositionUpdateView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);

			return memberPositionUpdateView;
		}
	}
}
