using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class ClanMemberViewProxy {
		public static void Serialize(Stream stream, ClanMemberView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				DateTimeProxy.Serialize(memoryStream, instance.JoiningDate);
				DateTimeProxy.Serialize(memoryStream, instance.Lastlogin);

				if (instance.Name != null) {
					StringProxy.Serialize(memoryStream, instance.Name);
				} else {
					num |= 1;
				}

				EnumProxy<GroupPosition>.Serialize(memoryStream, instance.Position);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ClanMemberView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var clanMemberView = new ClanMemberView();
			clanMemberView.Cmid = Int32Proxy.Deserialize(bytes);
			clanMemberView.JoiningDate = DateTimeProxy.Deserialize(bytes);
			clanMemberView.Lastlogin = DateTimeProxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				clanMemberView.Name = StringProxy.Deserialize(bytes);
			}

			clanMemberView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);

			return clanMemberView;
		}
	}
}
