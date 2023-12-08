using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ClanMemberView {
		public string Name { get; set; }
		public int Cmid { get; set; }
		public GroupPosition Position { get; set; }
		public DateTime JoiningDate { get; set; }
		public DateTime Lastlogin { get; set; }
		public ClanMemberView() { }

		public ClanMemberView(string name, int cmid, GroupPosition position, DateTime joiningDate, DateTime lastLogin) {
			Cmid = cmid;
			Name = name;
			Position = position;
			JoiningDate = joiningDate;
			Lastlogin = lastLogin;
		}

		public override string ToString() {
			return string.Concat("[Clan member: [Name: ", Name, "][Cmid: ", Cmid, "][Position: ", Position, "][JoiningDate: ", JoiningDate, "][Lastlogin: ", Lastlogin, "]]");
		}
	}
}
