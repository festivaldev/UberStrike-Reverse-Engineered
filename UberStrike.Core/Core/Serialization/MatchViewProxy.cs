using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class MatchViewProxy {
		public static void Serialize(Stream stream, MatchView instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<GameModeType>.Serialize(memoryStream, instance.GameModeId);
				Int32Proxy.Serialize(memoryStream, instance.MapId);

				if (instance.PlayersCompleted != null) {
					ListProxy<PlayerStatisticsView>.Serialize(memoryStream, instance.PlayersCompleted, PlayerStatisticsViewProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.PlayersLimit);

				if (instance.PlayersNonCompleted != null) {
					ListProxy<PlayerStatisticsView>.Serialize(memoryStream, instance.PlayersNonCompleted, PlayerStatisticsViewProxy.Serialize);
				} else {
					num |= 2;
				}

				Int32Proxy.Serialize(memoryStream, instance.TimeLimit);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static MatchView Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var matchView = new MatchView();
			matchView.GameModeId = EnumProxy<GameModeType>.Deserialize(bytes);
			matchView.MapId = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				matchView.PlayersCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, PlayerStatisticsViewProxy.Deserialize);
			}

			matchView.PlayersLimit = Int32Proxy.Deserialize(bytes);

			if ((num & 2) != 0) {
				matchView.PlayersNonCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, PlayerStatisticsViewProxy.Deserialize);
			}

			matchView.TimeLimit = Int32Proxy.Deserialize(bytes);

			return matchView;
		}
	}
}
