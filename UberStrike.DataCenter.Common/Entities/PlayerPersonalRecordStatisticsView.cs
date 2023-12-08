using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class PlayerPersonalRecordStatisticsView {
		public int MostHeadshots { get; set; }
		public int MostNutshots { get; set; }
		public int MostConsecutiveSnipes { get; set; }
		public int MostXPEarned { get; set; }
		public int MostSplats { get; set; }
		public int MostDamageDealt { get; set; }
		public int MostDamageReceived { get; set; }
		public int MostArmorPickedUp { get; set; }
		public int MostHealthPickedUp { get; set; }
		public int MostMeleeSplats { get; set; }
		public int MostHandgunSplats { get; set; } // # LEGACY # //
		public int MostMachinegunSplats { get; set; }
		public int MostShotgunSplats { get; set; }
		public int MostSniperSplats { get; set; }
		public int MostSplattergunSplats { get; set; }
		public int MostCannonSplats { get; set; }
		public int MostLauncherSplats { get; set; }
		public PlayerPersonalRecordStatisticsView() { }

		public PlayerPersonalRecordStatisticsView(int mostHeadshots, int mostNutshots, int mostConsecutiveSnipes, int mostXPEarned, int mostSplats, int mostDamageDealt, int mostDamageReceived, int mostArmorPickedUp, int mostHealthPickedUp, int mostMeleeSplats, int mostMachinegunSplats, int mostShotgunSplats, int mostSniperSplats, int mostSplattergunSplats, int mostCannonSplats, int mostLauncherSplats) {
			MostArmorPickedUp = mostArmorPickedUp;
			MostCannonSplats = mostCannonSplats;
			MostConsecutiveSnipes = mostConsecutiveSnipes;
			MostDamageDealt = mostDamageDealt;
			MostDamageReceived = mostDamageReceived;
			MostHeadshots = mostHeadshots;
			MostHealthPickedUp = mostHealthPickedUp;
			MostLauncherSplats = mostLauncherSplats;
			MostMachinegunSplats = mostMachinegunSplats;
			MostMeleeSplats = mostMeleeSplats;
			MostNutshots = mostNutshots;
			MostShotgunSplats = mostShotgunSplats;
			MostSniperSplats = mostSniperSplats;
			MostSplats = mostSplats;
			MostSplattergunSplats = mostSplattergunSplats;
			MostXPEarned = mostXPEarned;
		}

		// # LEGACY # //
		public PlayerPersonalRecordStatisticsView(int mostHeadshots, int mostNutshots, int mostConsecutiveSnipes, int mostXPEarned, int mostSplats, int mostDamageDealt, int mostDamageReceived, int mostArmorPickedUp, int mostHealthPickedUp, int mostMeleeSplats, int mostHandgunSplats, int mostMachinegunSplats, int mostShotgunSplats, int mostSniperSplats, int mostSplattergunSplats, int mostCannonSplats, int mostLauncherSplats) {
			MostArmorPickedUp = mostArmorPickedUp;
			MostCannonSplats = mostCannonSplats;
			MostConsecutiveSnipes = mostConsecutiveSnipes;
			MostDamageDealt = mostDamageDealt;
			MostDamageReceived = mostDamageReceived;
			MostHandgunSplats = mostHandgunSplats;
			MostHeadshots = mostHeadshots;
			MostHealthPickedUp = mostHealthPickedUp;
			MostLauncherSplats = mostLauncherSplats;
			MostMachinegunSplats = mostMachinegunSplats;
			MostMeleeSplats = mostMeleeSplats;
			MostNutshots = mostNutshots;
			MostShotgunSplats = mostShotgunSplats;
			MostSniperSplats = mostSniperSplats;
			MostSplats = mostSplats;
			MostSplattergunSplats = mostSplattergunSplats;
			MostXPEarned = mostXPEarned;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[PlayerPersonalRecordStatisticsView: ");
			stringBuilder.Append("[MostArmorPickedUp: ");
			stringBuilder.Append(MostArmorPickedUp);
			stringBuilder.Append("][MostCannonSplats: ");
			stringBuilder.Append(MostCannonSplats);
			stringBuilder.Append("][MostConsecutiveSnipes: ");
			stringBuilder.Append(MostConsecutiveSnipes);
			stringBuilder.Append("][MostDamageDealt: ");
			stringBuilder.Append(MostDamageDealt);
			stringBuilder.Append("][MostDamageReceived: ");
			stringBuilder.Append(MostDamageReceived);
			stringBuilder.Append("][MostHandgunSplats: ");
			stringBuilder.Append(MostHandgunSplats);
			stringBuilder.Append("][MostHeadshots: ");
			stringBuilder.Append(MostHeadshots);
			stringBuilder.Append("][MostHealthPickedUp: ");
			stringBuilder.Append(MostHealthPickedUp);
			stringBuilder.Append("][MostLauncherSplats: ");
			stringBuilder.Append(MostLauncherSplats);
			stringBuilder.Append("][MostMachinegunSplats: ");
			stringBuilder.Append(MostMachinegunSplats);
			stringBuilder.Append("][MostMeleeSplats: ");
			stringBuilder.Append(MostMeleeSplats);
			stringBuilder.Append("][MostNutshots: ");
			stringBuilder.Append(MostNutshots);
			stringBuilder.Append("][MostShotgunSplats: ");
			stringBuilder.Append(MostShotgunSplats);
			stringBuilder.Append("][MostSniperSplats: ");
			stringBuilder.Append(MostSniperSplats);
			stringBuilder.Append("][MostSplats: ");
			stringBuilder.Append(MostSplats);
			stringBuilder.Append("][MostSplattergunSplats: ");
			stringBuilder.Append(MostSplattergunSplats);
			stringBuilder.Append("][MostXPEarned: ");
			stringBuilder.Append(MostXPEarned);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
