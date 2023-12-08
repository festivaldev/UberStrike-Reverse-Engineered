using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class MatchStatsProxy {
		public static void Serialize(Stream stream, MatchStats instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<GameModeType>.Serialize(memoryStream, instance.GameModeId);
				Int32Proxy.Serialize(memoryStream, instance.MapId);

				if (instance.Players != null) {
					ListProxy<PlayerMatchStats>.Serialize(memoryStream, instance.Players, PlayerMatchStatsProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.PlayersLimit);
				Int32Proxy.Serialize(memoryStream, instance.TimeLimit);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static MatchStats Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var matchStats = new MatchStats();
			matchStats.GameModeId = EnumProxy<GameModeType>.Deserialize(bytes);
			matchStats.MapId = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				matchStats.Players = ListProxy<PlayerMatchStats>.Deserialize(bytes, PlayerMatchStatsProxy.Deserialize);
			}

			matchStats.PlayersLimit = Int32Proxy.Deserialize(bytes);
			matchStats.TimeLimit = Int32Proxy.Deserialize(bytes);

			return matchStats;
		}
	}
}
