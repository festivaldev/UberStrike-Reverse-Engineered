using System.Collections.Generic;
using UberStrike.Core.Models;

public abstract class GameDataBaseComparer : IComparer<GameRoomData> {
	public int Compare(GameRoomData x, GameRoomData y) {
		var num = GameRoomHelper.CanJoinGame(y).CompareTo(GameRoomHelper.CanJoinGame(x));

		return (num != 0) ? num : OnCompare(x, y);
	}

	protected abstract int OnCompare(GameRoomData x, GameRoomData y);
}
