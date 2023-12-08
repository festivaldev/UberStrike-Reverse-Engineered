using UberStrike.Core.Models;

public class GameDataMapComparer : GameDataBaseComparer {
	protected override int OnCompare(GameRoomData a, GameRoomData b) {
		var num = a.MapID - b.MapID;

		return (num != 0) ? ((!GameDataComparer.SortAscending) ? (-num) : num) : GameDataNameComparer.StaticCompare(a, b);
	}
}
