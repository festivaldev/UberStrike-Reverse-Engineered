namespace UberStrike.DataCenter.Common.Entities {
	public class LinkedMemberView {
		public int Cmid { get; private set; }
		public string Name { get; private set; }

		public LinkedMemberView(int cmid, string name) {
			Cmid = cmid;
			Name = name;
		}
	}
}
