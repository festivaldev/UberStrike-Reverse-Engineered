using System;
using System.Collections.Generic;

namespace UberStrike.WebService.Unity {
	public static class WebServiceStatistics {
		public static bool IsEnabled = true;
		public static readonly Dictionary<string, Statistics> Data = new Dictionary<string, Statistics>();
		public static long TotalBytesIn { get; private set; }
		public static long TotalBytesOut { get; private set; }

		public static void RecordWebServiceBegin(string method, int bytes) {
			var statistics = GetStatistics(method);
			statistics.Counter++;
			statistics.OutgoingBytes += bytes;
			TotalBytesOut += bytes;
			statistics.LastCall = DateTime.UtcNow;
		}

		public static void RecordWebServiceEnd(string method, int bytes, bool success) {
			var statistics = GetStatistics(method);
			statistics.IncomingBytes += bytes;
			TotalBytesIn += bytes;

			if (!success) {
				statistics.FailCounter++;
			}

			statistics.Time = (float)DateTime.UtcNow.Subtract(statistics.LastCall).TotalSeconds;
		}

		private static Statistics GetStatistics(string method) {
			Statistics statistics;

			if (!Data.TryGetValue(method, out statistics)) {
				statistics = new Statistics();
				Data[method] = statistics;
			}

			return statistics;
		}

		public class Statistics {
			public int Counter { get; set; }
			public int IncomingBytes { get; set; }
			public int OutgoingBytes { get; set; }
			public int FailCounter { get; set; }
			public float Time { get; set; }
			internal DateTime LastCall { get; set; }

			public Statistics() {
				LastCall = DateTime.UtcNow;
			}

			public override string ToString() {
				return string.Format("\tcount:{0}({1}) | time:{2:N2} | data:{3:F0}/{4:F0}", Counter, FailCounter, Time, IncomingBytes / 1024f, OutgoingBytes / 1024f);
			}
		}
	}
}
