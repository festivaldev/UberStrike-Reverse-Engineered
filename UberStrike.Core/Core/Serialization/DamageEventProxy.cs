using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class DamageEventProxy {
		public static void Serialize(Stream stream, DamageEvent instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				ByteProxy.Serialize(memoryStream, instance.BodyPartFlag);

				if (instance.Damage != null) {
					DictionaryProxy<byte, byte>.Serialize(memoryStream, instance.Damage, ByteProxy.Serialize, ByteProxy.Serialize);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.DamageEffectFlag);
				SingleProxy.Serialize(memoryStream, instance.DamgeEffectValue);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static DamageEvent Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var damageEvent = new DamageEvent();
			damageEvent.BodyPartFlag = ByteProxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				damageEvent.Damage = DictionaryProxy<byte, byte>.Deserialize(bytes, ByteProxy.Deserialize, ByteProxy.Deserialize);
			}

			damageEvent.DamageEffectFlag = Int32Proxy.Deserialize(bytes);
			damageEvent.DamgeEffectValue = SingleProxy.Deserialize(bytes);

			return damageEvent;
		}
	}
}
