using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class ListProxy<T> {
		public delegate U Deserializer<U>(Stream stream);

		public delegate void Serializer<U>(Stream stream, U instance);

		public static void Serialize(Stream bytes, ICollection<T> instance, Serializer<T> serialization) {
			UShortProxy.Serialize(bytes, (ushort)instance.Count);

			foreach (var t in instance) {
				serialization(bytes, t);
			}
		}

		public static List<T> Deserialize(Stream bytes, Deserializer<T> serialization) {
			var num = UShortProxy.Deserialize(bytes);
			var list = new List<T>(num);

			for (var i = 0; i < num; i++) {
				list.Add(serialization(bytes));
			}

			return list;
		}
	}
}
