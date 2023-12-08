namespace UberStrike.DataCenter.Common.Entities {
	public class EsnsBasicStatisticView {
		public string Name { get; protected set; }
		public int SocialRank { get; protected set; }
		public int XP { get; protected set; }
		public int Level { get; protected set; }
		public int Cmid { get; protected set; }

		public EsnsBasicStatisticView(string name, int xp, int level, int cmid) {
			Name = name;
			XP = xp;
			Level = level;
			Cmid = cmid;
		}

		public EsnsBasicStatisticView() {
			Name = string.Empty;
			XP = 0;
			Level = 0;
			Cmid = 0;
		}
	}
}
