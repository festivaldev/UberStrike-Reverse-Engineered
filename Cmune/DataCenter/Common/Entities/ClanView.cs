using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ClanView : BasicClanView {
		public List<ClanMemberView> Members { get; set; }

		public ClanView() {
			Members = new List<ClanMemberView>();
		}

		public ClanView(int groupId, int membersCount, string description, string name, string motto, string address, DateTime foundingDate, string picture, GroupType type, DateTime lastUpdated, string tag, int membersLimit, GroupColor colorStyle, GroupFontStyle fontStyle, int applicationId, int ownerCmid, string ownerName, List<ClanMemberView> members) : base(groupId, membersCount, description, name, motto, address, foundingDate, picture, type, lastUpdated, tag, membersLimit, colorStyle, fontStyle, applicationId, ownerCmid, ownerName) {
			Members = members;
		}

		public override string ToString() {
			var text = "[Clan: " + base.ToString();
			text += "[Members:";

			foreach (var clanMemberView in Members) {
				text += clanMemberView.ToString();
			}

			text += "]";

			return text;
		}
	}
}
