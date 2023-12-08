using System;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class UInt16Proxy {
		public static void Serialize(Stream bytes, ushort instance) {
			var bytes2 = BitConverter.GetBytes(instance);
			bytes.Write(bytes2, 0, bytes2.Length);
		}

		public static ushort Deserialize(Stream bytes) {
			var array = new byte[2];
			bytes.Read(array, 0, 2);

			return BitConverter.ToUInt16(array, 0);
		}
	}
}
