using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class ContactGroupViewProxy {
		public static void Serialize(Stream stream, ContactGroupView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Contacts != null) {
					ListProxy<PublicProfileView>.Serialize(memoryStream, instance.Contacts, PublicProfileViewProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.GroupId);

				if (instance.GroupName != null) {
					StringProxy.Serialize(memoryStream, instance.GroupName);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static ContactGroupView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var contactGroupView = new ContactGroupView();

			if ((num & 1) != 0) {
				contactGroupView.Contacts = ListProxy<PublicProfileView>.Deserialize(bytes, PublicProfileViewProxy.Deserialize);
			}

			contactGroupView.GroupId = Int32Proxy.Deserialize(bytes);

			if ((num & 2) != 0) {
				contactGroupView.GroupName = StringProxy.Deserialize(bytes);
			}

			return contactGroupView;
		}
	}
}
