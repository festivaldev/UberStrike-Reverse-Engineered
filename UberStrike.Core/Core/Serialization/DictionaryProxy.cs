using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization {
	public static class DictionaryProxy<S, T> {
		public delegate U Deserializer<U>(Stream stream);

		public delegate void Serializer<U>(Stream stream, U instance);

		public static void Serialize(Stream bytes, Dictionary<S, T> instance, Serializer<S> keySerialization, Serializer<T> valueSerialization) {
			Int32Proxy.Serialize(bytes, instance.Count);

			foreach (var keyValuePair in instance) {
				keySerialization(bytes, keyValuePair.Key);
				valueSerialization(bytes, keyValuePair.Value);
			}
		}

		public static Dictionary<S, T> Deserialize(Stream bytes, Deserializer<S> keySerialization, Deserializer<T> valueSerialization) {
			var num = Int32Proxy.Deserialize(bytes);
			var dictionary = new Dictionary<S, T>(num);

			for (var i = 0; i < num; i++) {
				dictionary.Add(keySerialization(bytes), valueSerialization(bytes));
			}

			return dictionary;
		}
	}
}
