﻿using System;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class Int64Proxy {
		public static void Serialize(Stream bytes, long instance) {
			var bytes2 = BitConverter.GetBytes(instance);
			bytes.Write(bytes2, 0, bytes2.Length);
		}

		public static long Deserialize(Stream bytes) {
			var array = new byte[8];
			bytes.Read(array, 0, 8);

			return BitConverter.ToInt64(array, 0);
		}
	}
}
