using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class MemberPositionUpdateView {
		public int GroupId { get; set; }
		public string AuthToken { get; set; }
		public int MemberCmid { get; set; }
		public GroupPosition Position { get; set; }
		public int CmidMakingAction { get; set; } // # LEGACY # //
		public MemberPositionUpdateView() { }

		public MemberPositionUpdateView(int groupId, string authToken, int memberCmid, GroupPosition position) {
			GroupId = groupId;
			AuthToken = authToken;
			MemberCmid = memberCmid;
			Position = position;
		}

		public override string ToString() {
			var text = string.Concat("[MemberPositionUpdateView: [GroupId:", GroupId, "][AuthToken:", AuthToken, "][MemberCmid:", MemberCmid);
			var text2 = text;

			return string.Concat(text2, "][Position:", Position, "]]");
		}
	}
}
