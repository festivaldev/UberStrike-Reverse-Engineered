using System;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Realtime.UnitySdk;

public class SecureMemory<T> {
	private const string pp = "h&dk2Ks901HenM";
	private const string iv = "huSj39Dl)2kJ4nat";
	private T _cachedValue;
	private byte[] _encryptedData;

	public SecureMemory(T value) {
		WriteData(value);
	}

	public void WriteData(T value) {
		try {
			_cachedValue = value;
			_encryptedData = Cryptography.RijndaelEncrypt(Serialize(value), "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat");
		} catch (Exception ex) {
			throw new Exception(string.Format("SecureMemory failed encrypting Data: {0}", ex.Message), ex.InnerException);
		}
	}

	public void ValidateData() {
		if (!Comparison.IsEqual(_cachedValue, DecryptValue())) {
			throw new Exception("Failed to validate data due to a corrupted memory");
		}
	}

	public object ReadObject(bool secure) {
		return ReadData(secure);
	}

	public T ReadData(bool secure) {
		if (secure) {
			_cachedValue = DecryptValue();
		}

		return _cachedValue;
	}

	private T DecryptValue() {
		T t;

		try {
			var array = Cryptography.RijndaelDecrypt(_encryptedData, "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat");

			if (array == null) {
				throw new Exception("SecureMemory failed decrypting Data becauase CmuneSecurity.Decrypt returned NULL");
			}

			object obj = Deserialize(array);

			if (obj == null) {
				throw new Exception("SecureMemory failed decrypting Data becauase RealtimeSerialization.ToObject returned NULL");
			}

			t = (T)obj;
		} catch (Exception ex) {
			throw new Exception(string.Format("SecureMemory failed decrypting Data: {0}", ex.Message), ex.InnerException);
		}

		return t;
	}

	private byte[] Serialize(T obj) {
		byte[] array;

		using (var memoryStream = new MemoryStream()) {
			var typeFromHandle = typeof(T);

			if (typeFromHandle == typeof(int)) {
				Int32Proxy.Serialize(memoryStream, (int)((object)obj));
			} else if (typeFromHandle == typeof(float)) {
				SingleProxy.Serialize(memoryStream, (float)((object)obj));
			} else if (typeFromHandle == typeof(string)) {
				StringProxy.Serialize(memoryStream, (string)((object)obj));
			}

			array = memoryStream.ToArray();
		}

		return array;
	}

	private T Deserialize(byte[] bytes) {
		T t;

		using (var memoryStream = new MemoryStream(bytes)) {
			var typeFromHandle = typeof(T);

			if (typeFromHandle == typeof(int)) {
				t = (T)((object)Int32Proxy.Deserialize(memoryStream));
			} else if (typeFromHandle == typeof(float)) {
				t = (T)((object)SingleProxy.Deserialize(memoryStream));
			} else if (typeFromHandle == typeof(string)) {
				t = (T)((object)StringProxy.Deserialize(memoryStream));
			} else {
				t = default(T);
			}
		}

		return t;
	}
}
