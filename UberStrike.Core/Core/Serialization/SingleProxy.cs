using System;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class SingleProxy {
		public static void Serialize(Stream bytes, float instance) {
			var bytes2 = BitConverter.GetBytes(instance);
			bytes.Write(bytes2, 0, bytes2.Length);
		}

		public static float Deserialize(Stream bytes) {
			var array = new byte[4];
			bytes.Read(array, 0, 4);

			return BitConverter.ToSingle(array, 0);
		}
	}
}
