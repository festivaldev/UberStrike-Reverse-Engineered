using System;

namespace UberStrike.Realtime.UnitySdk {
	[Flags]
	public enum PlayerTagFlag {
		None = 0,
		Speedhacker = 1,
		Ammohacker = 2,
		ReportedCheater = 4,
		ReportedAbuser = 8
	}
}
