using UberStrike.Core.Models;

public class GameDataPlayerComparer : GameDataBaseComparer {
	protected override int OnCompare(GameRoomData a, GameRoomData b) {
		var num = a.ConnectedPlayers - b.ConnectedPlayers;

		return (num != 0) ? ((!GameDataComparer.SortAscending) ? (-num) : num) : GameDataNameComparer.StaticCompare(a, b);
	}
}
