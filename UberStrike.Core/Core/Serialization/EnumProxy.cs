using System;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class EnumProxy<T> {
		public static void Serialize(Stream bytes, T instance) {
			var bytes2 = BitConverter.GetBytes(Convert.ToInt32(instance));
			bytes.Write(bytes2, 0, bytes2.Length);
		}

		public static T Deserialize(Stream bytes) {
			var array = new byte[4];
			bytes.Read(array, 0, 4);

			return (T)Enum.ToObject(typeof(T), BitConverter.ToInt32(array, 0));
		}
	}
}
