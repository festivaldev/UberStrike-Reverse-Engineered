using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities {
	public class MapUsageView {
		public DateTime PlayDate { get; private set; }
		public int MapId { get; private set; }
		public GameModeType GameModeId { get; private set; }
		public int TimeLimit { get; private set; }
		public int PlayerLimit { get; private set; }
		public int PlayersTotal { get; private set; }
		public int PlayersCompleted { get; private set; }

		public MapUsageView(DateTime playDate, int mapId, GameModeType gameModeId, int timeLimit, int playerLimit, int playersTotal, int playersCompleted) {
			PlayDate = playDate;
			MapId = mapId;
			GameModeId = gameModeId;
			TimeLimit = timeLimit;
			PlayerLimit = playerLimit;
			PlayersTotal = playersTotal;
			PlayersCompleted = playersCompleted;
		}
	}
}
