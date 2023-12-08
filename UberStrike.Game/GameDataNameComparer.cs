using UberStrike.Core.Models;

public class GameDataNameComparer : GameDataBaseComparer {
	protected override int OnCompare(GameRoomData a, GameRoomData b) {
		return StaticCompare(a, b);
	}

	public static int StaticCompare(GameRoomData a, GameRoomData b) {
		return (!GameDataComparer.SortAscending) ? string.Compare(a.Name, b.Name) : string.Compare(b.Name, a.Name);
	}
}
