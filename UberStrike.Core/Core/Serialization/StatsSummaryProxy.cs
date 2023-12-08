using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class StatsSummaryProxy {
		public static void Serialize(Stream stream, StatsSummary instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				if (instance.Achievements != null) {
					DictionaryProxy<byte, ushort>.Serialize(memoryStream, instance.Achievements, ByteProxy.Serialize, UInt16Proxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				Int32Proxy.Serialize(memoryStream, instance.Deaths);
				Int32Proxy.Serialize(memoryStream, instance.Kills);
				Int32Proxy.Serialize(memoryStream, instance.Level);

				if (instance.Name != null) {
					StringProxy.Serialize(memoryStream, instance.Name);
				} else {
					num |= 2;
				}

				EnumProxy<TeamID>.Serialize(memoryStream, instance.Team);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static StatsSummary Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var statsSummary = new StatsSummary();

			if ((num & 1) != 0) {
				statsSummary.Achievements = DictionaryProxy<byte, ushort>.Deserialize(bytes, ByteProxy.Deserialize, UInt16Proxy.Deserialize);
			}

			statsSummary.Cmid = Int32Proxy.Deserialize(bytes);
			statsSummary.Deaths = Int32Proxy.Deserialize(bytes);
			statsSummary.Kills = Int32Proxy.Deserialize(bytes);
			statsSummary.Level = Int32Proxy.Deserialize(bytes);

			if ((num & 2) != 0) {
				statsSummary.Name = StringProxy.Deserialize(bytes);
			}

			statsSummary.Team = EnumProxy<TeamID>.Deserialize(bytes);

			return statsSummary;
		}
	}
}
