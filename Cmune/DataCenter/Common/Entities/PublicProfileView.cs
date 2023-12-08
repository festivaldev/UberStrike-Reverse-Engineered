using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class PublicProfileView {
		public int Cmid { get; set; }
		public string Name { get; set; }
		public bool IsChatDisabled { get; set; }
		public MemberAccessLevel AccessLevel { get; set; }
		public string GroupTag { get; set; }
		public DateTime LastLoginDate { get; set; }
		public EmailAddressStatus EmailAddressStatus { get; set; }
		public string FacebookId { get; set; }

		public PublicProfileView() {
			Cmid = 0;
			Name = string.Empty;
			IsChatDisabled = false;
			AccessLevel = MemberAccessLevel.Default;
			GroupTag = string.Empty;
			LastLoginDate = DateTime.UtcNow;
			EmailAddressStatus = EmailAddressStatus.Unverified;
			FacebookId = string.Empty;
		}

		public PublicProfileView(int cmid, string name, MemberAccessLevel accesLevel, bool isChatDisabled, DateTime lastLoginDate, EmailAddressStatus emailAddressStatus, string facebookId) {
			SetPublicProfile(cmid, name, accesLevel, isChatDisabled, string.Empty, lastLoginDate, emailAddressStatus, facebookId);
		}

		public PublicProfileView(int cmid, string name, MemberAccessLevel accesLevel, bool isChatDisabled, string groupTag, DateTime lastLoginDate, EmailAddressStatus emailAddressStatus, string facebookId) {
			SetPublicProfile(cmid, name, accesLevel, isChatDisabled, groupTag, lastLoginDate, emailAddressStatus, facebookId);
		}

		private void SetPublicProfile(int cmid, string name, MemberAccessLevel accesLevel, bool isChatDisabled, string groupTag, DateTime lastLoginDate, EmailAddressStatus emailAddressStatus, string facebookId) {
			Cmid = cmid;
			Name = name;
			AccessLevel = accesLevel;
			IsChatDisabled = isChatDisabled;
			GroupTag = groupTag;
			LastLoginDate = lastLoginDate;
			EmailAddressStatus = emailAddressStatus;
			FacebookId = facebookId;
		}

		public override string ToString() {
			var text = "[Public profile: ";
			var text2 = text;
			text = string.Concat(text2, "[Member name:", Name, "][CMID:", Cmid, "][Is banned from chat: ", IsChatDisabled, "]");
			text2 = text;
			text = string.Concat(text2, "[Access level:", AccessLevel, "][Group tag: ", GroupTag, "][Last login date: ", LastLoginDate, "]]");
			text2 = text;

			return string.Concat(text2, "[EmailAddressStatus:", EmailAddressStatus, "][FacebookId: ", FacebookId, "]");
		}
	}
}
