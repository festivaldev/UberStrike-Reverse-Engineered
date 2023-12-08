using UberStrike.Core.Models;
using UberStrike.Core.Types;

internal static class ExtensionMethods {
	public static void Copy(this CommActorInfo original, CommActorInfo data) {
		original.ClanTag = data.ClanTag;
		original.CurrentRoom = data.CurrentRoom;
		original.ModerationFlag = data.ModerationFlag;
		original.ModInformation = data.ModInformation;
		original.PlayerName = data.PlayerName;
	}

	public static GameModeType GetGameMode(int id) {
		if (id == 100) {
			return GameModeType.TeamDeathMatch;
		}

		if (id == 101) {
			return GameModeType.DeathMatch;
		}

		if (id != 106) {
			return GameModeType.None;
		}

		return GameModeType.EliminationMode;
	}
}
