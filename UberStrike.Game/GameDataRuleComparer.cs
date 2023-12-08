using UberStrike.Core.Models;

public class GameDataRuleComparer : GameDataBaseComparer {
	protected override int OnCompare(GameRoomData a, GameRoomData b) {
		var num = (short)a.GameMode - (short)b.GameMode;

		return (num != 0) ? ((!GameDataComparer.SortAscending) ? (-num) : num) : GameDataNameComparer.StaticCompare(a, b);
	}
}
