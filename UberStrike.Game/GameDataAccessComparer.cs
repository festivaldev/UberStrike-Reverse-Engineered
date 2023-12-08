using UberStrike.Core.Models;

public class GameDataAccessComparer : GameDataBaseComparer {
	protected override int OnCompare(GameRoomData a, GameRoomData b) {
		var num = 0;

		if (GameDataComparer.SortAscending) {
			if (!a.IsPasswordProtected && !b.IsPasswordProtected) {
				num = 2;
			} else if (!a.IsPasswordProtected) {
				num = 1;
			} else if (!b.IsPasswordProtected) {
				num = -1;
			}
		} else if (a.IsPasswordProtected && b.IsPasswordProtected) {
			num = 2;
		} else if (a.IsPasswordProtected) {
			num = 1;
		} else if (b.IsPasswordProtected) {
			num = -1;
		}

		return num;
	}
}
