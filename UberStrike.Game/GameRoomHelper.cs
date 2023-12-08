using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;

public static class GameRoomHelper {
	public static bool HasLevelRestriction(GameRoomData room) {
		return room.LevelMin != 0 || room.LevelMax != 0;
	}

	public static bool IsLevelAllowed(int min, int max, int level) {
		return level >= min && (max == 0 || level <= max);
	}

	public static bool IsLevelAllowed(GameRoomData room, int level) {
		return IsLevelAllowed(room.LevelMin, room.LevelMax, level);
	}

	public static bool CanJoinGame(GameRoomData game) {
		var flag = !game.IsFull && IsLevelAllowed(game, PlayerDataManager.PlayerLevel);
		flag |= PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator;

		return flag & Singleton<MapManager>.Instance.HasMapWithId(game.MapID);
	}
}
