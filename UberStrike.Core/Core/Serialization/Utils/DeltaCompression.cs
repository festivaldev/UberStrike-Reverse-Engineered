using System.IO;

namespace UberStrike.Core.Serialization.Utils {
	public class DeltaCompression {
		public static byte[] Deflate(byte[] baseData, byte[] newData) {
			byte[] array;

			using (var memoryStream = new MemoryStream()) {
				byte b = 0;

				for (var i = 0; i < newData.Length; i++) {
					if (i < baseData.Length) {
						if (baseData[i] == newData[i]) {
							b += 1;
						} else {
							memoryStream.WriteByte(b);
							memoryStream.WriteByte(newData[i]);
							b = 0;
						}
					} else {
						memoryStream.WriteByte(newData[i]);
					}
				}

				array = memoryStream.ToArray();
			}

			return array;
		}

		public static byte[] Inflate(byte[] baseData, byte[] delta) {
			if (delta.Length == 0) {
				return baseData;
			}

			byte[] array;

			using (var memoryStream = new MemoryStream()) {
				var num = 0;
				var i = 0;

				while (i < delta.Length) {
					if (num < baseData.Length) {
						var j = 0;

						while (j < delta[i]) {
							memoryStream.WriteByte(baseData[num]);
							j++;
							num++;
						}

						memoryStream.WriteByte(delta[i + 1]);
						num++;
						i += 2;
					} else {
						memoryStream.WriteByte(delta[i]);
						i++;
					}
				}

				array = memoryStream.ToArray();
			}

			return array;
		}
	}
}
