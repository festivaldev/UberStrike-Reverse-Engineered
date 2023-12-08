using System;

namespace UberStrike.Realtime.UnitySdk {
	public static class ConvertBytes {
		private const double toKilo = 0.0009765625;

		public static float ToKiloBytes(ulong bytes) {
			return Convert.ToSingle(bytes * 0.0009765625);
		}

		public static float ToKiloBytes(int bytes) {
			return Convert.ToSingle(bytes * 0.0009765625);
		}

		public static float ToKiloBytes(long bytes) {
			return Convert.ToSingle(bytes * 0.0009765625);
		}

		public static float ToMegaBytes(ulong bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625);
		}

		public static float ToMegaBytes(long bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625);
		}

		public static float ToMegaBytes(int bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625);
		}

		public static float ToGigaBytes(ulong bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);
		}

		public static float ToGigaBytes(long bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);
		}

		public static float ToGigaBytes(int bytes) {
			return Convert.ToSingle(bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);
		}
	}
}
