using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class EndOfMatchDataProxy {
		public static void Serialize(Stream stream, EndOfMatchData instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				BooleanProxy.Serialize(memoryStream, instance.HasWonMatch);

				if (instance.MatchGuid != null) {
					StringProxy.Serialize(memoryStream, instance.MatchGuid);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.MostEffecientWeaponId);

				if (instance.MostValuablePlayers != null) {
					ListProxy<StatsSummary>.Serialize(memoryStream, instance.MostValuablePlayers, StatsSummaryProxy.Serialize);
				} else {
					num |= 2;
				}

				if (instance.PlayerStatsBestPerLife != null) {
					StatsCollectionProxy.Serialize(memoryStream, instance.PlayerStatsBestPerLife);
				} else {
					num |= 4;
				}

				if (instance.PlayerStatsTotal != null) {
					StatsCollectionProxy.Serialize(memoryStream, instance.PlayerStatsTotal);
				} else {
					num |= 8;
				}

				if (instance.PlayerXpEarned != null) {
					DictionaryProxy<byte, ushort>.Serialize(memoryStream, instance.PlayerXpEarned, ByteProxy.Serialize, UInt16Proxy.Serialize);
				} else {
					num |= 16;
				}

				Int32Proxy.Serialize(memoryStream, instance.TimeInGameMinutes);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static EndOfMatchData Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var endOfMatchData = new EndOfMatchData();
			endOfMatchData.HasWonMatch = BooleanProxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				endOfMatchData.MatchGuid = StringProxy.Deserialize(bytes);
			}

			endOfMatchData.MostEffecientWeaponId = Int32Proxy.Deserialize(bytes);

			if ((num & 2) != 0) {
				endOfMatchData.MostValuablePlayers = ListProxy<StatsSummary>.Deserialize(bytes, StatsSummaryProxy.Deserialize);
			}

			if ((num & 4) != 0) {
				endOfMatchData.PlayerStatsBestPerLife = StatsCollectionProxy.Deserialize(bytes);
			}

			if ((num & 8) != 0) {
				endOfMatchData.PlayerStatsTotal = StatsCollectionProxy.Deserialize(bytes);
			}

			if ((num & 16) != 0) {
				endOfMatchData.PlayerXpEarned = DictionaryProxy<byte, ushort>.Deserialize(bytes, ByteProxy.Deserialize, UInt16Proxy.Deserialize);
			}

			endOfMatchData.TimeInGameMinutes = Int32Proxy.Deserialize(bytes);

			return endOfMatchData;
		}
	}
}
