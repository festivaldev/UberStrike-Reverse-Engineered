using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class PlayerStatisticsView {
		public int Cmid { get; set; }
		public int Splats { get; set; }
		public int Splatted { get; set; }
		public long Shots { get; set; }
		public long Hits { get; set; }
		public int Headshots { get; set; }
		public int Nutshots { get; set; }
		public int Xp { get; set; }
		public int Points { get; set; } // # LEGACY # //
		public int Level { get; set; }
		public int TimeSpentInGame { get; set; }
		public PlayerPersonalRecordStatisticsView PersonalRecord { get; set; }
		public PlayerWeaponStatisticsView WeaponStatistics { get; set; }

		public PlayerStatisticsView() {
			PersonalRecord = new PlayerPersonalRecordStatisticsView();
			WeaponStatistics = new PlayerWeaponStatisticsView();
		}

		public PlayerStatisticsView(int cmid, int splats, int splatted, long shots, long hits, int headshots, int nutshots, PlayerPersonalRecordStatisticsView personalRecord, PlayerWeaponStatisticsView weaponStatistics) {
			Cmid = cmid;
			Hits = hits;
			Level = 0;
			Shots = shots;
			Splats = splats;
			Splatted = splatted;
			Headshots = headshots;
			Nutshots = nutshots;
			Xp = 0;
			PersonalRecord = personalRecord;
			WeaponStatistics = weaponStatistics;
		}

		public PlayerStatisticsView(int cmid, int splats, int splatted, long shots, long hits, int headshots, int nutshots, int xp, int level, PlayerPersonalRecordStatisticsView personalRecord, PlayerWeaponStatisticsView weaponStatistics) {
			Cmid = cmid;
			Hits = hits;
			Level = level;
			Shots = shots;
			Splats = splats;
			Splatted = splatted;
			Headshots = headshots;
			Nutshots = nutshots;
			Xp = xp;
			PersonalRecord = personalRecord;
			WeaponStatistics = weaponStatistics;
		}

		// # LEGACY # //
		public PlayerStatisticsView(int cmid, int splats, int splatted, long shots, long hits, int headshots, int nutshots, int xp, int points, int level, PlayerPersonalRecordStatisticsView personalRecord, PlayerWeaponStatisticsView weaponStatistics) {
			Cmid = cmid;
			Hits = hits;
			Level = level;
			Shots = shots;
			Splats = splats;
			Splatted = splatted;
			Headshots = headshots;
			Nutshots = nutshots;
			Xp = xp;
			Points = points;
			PersonalRecord = personalRecord;
			WeaponStatistics = weaponStatistics;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[PlayerStatisticsView: ");
			stringBuilder.Append("[Cmid: ");
			stringBuilder.Append(Cmid);
			stringBuilder.Append("][Hits: ");
			stringBuilder.Append(Hits);
			stringBuilder.Append("][Level: ");
			stringBuilder.Append(Level);
			stringBuilder.Append("][Shots: ");
			stringBuilder.Append(Shots);
			stringBuilder.Append("][Splats: ");
			stringBuilder.Append(Splats);
			stringBuilder.Append("][Splatted: ");
			stringBuilder.Append(Splatted);
			stringBuilder.Append("][Headshots: ");
			stringBuilder.Append(Headshots);
			stringBuilder.Append("][Nutshots: ");
			stringBuilder.Append(Nutshots);
			stringBuilder.Append("][Xp: ");
			stringBuilder.Append(Xp);
			stringBuilder.Append("][Points: ");
			stringBuilder.Append(Points);
			stringBuilder.Append("]");
			stringBuilder.Append(PersonalRecord);
			stringBuilder.Append(WeaponStatistics);
			stringBuilder.Append("]");

			return stringBuilder.ToString();
		}
	}
}
