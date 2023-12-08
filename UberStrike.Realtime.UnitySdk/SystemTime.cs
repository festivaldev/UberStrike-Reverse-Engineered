using System;

namespace UberStrike.Realtime.UnitySdk {
	public static class SystemTime {
		public static int Running {
			get { return Environment.TickCount & int.MaxValue; }
		}
	}
}
