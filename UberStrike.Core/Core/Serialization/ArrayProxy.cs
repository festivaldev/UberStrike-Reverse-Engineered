using System;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class ArrayProxy<T> {
		public delegate U Deserializer<U>(Stream stream);

		public delegate void Serializer<U>(Stream stream, U instance);

		public static void Serialize(Stream bytes, T[] instance, Action<Stream, T> serialization) {
			UShortProxy.Serialize(bytes, (ushort)instance.Length);

			foreach (var t in instance) {
				serialization(bytes, t);
			}
		}

		public static T[] Deserialize(Stream bytes, Deserializer<T> serialization) {
			var num = UShortProxy.Deserialize(bytes);
			var array = new T[num];

			for (var i = 0; i < num; i++) {
				array[i] = serialization(bytes);
			}

			return array;
		}
	}
}
