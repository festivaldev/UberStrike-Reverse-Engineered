using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models {
	[Serializable]
	public class DamageEvent {
		public Dictionary<byte, byte> Damage { get; set; }
		public byte BodyPartFlag { get; set; }
		public int DamageEffectFlag { get; set; }
		public float DamgeEffectValue { get; set; }

		public int Count {
			get { return (Damage == null) ? 0 : Damage.Count; }
		}

		public void Clear() {
			if (Damage == null) {
				Damage = new Dictionary<byte, byte>();
			}

			BodyPartFlag = 0;
			Damage.Clear();
		}

		public void AddDamage(byte angle, short damage, byte bodyPart, int damageEffectFlag, float damageEffectValue) {
			if (Damage == null)
				Damage = new Dictionary<byte, byte>();

			if (Damage.ContainsKey(angle)) {
				Dictionary<byte, byte> damage1;
				byte key;
				(damage1 = Damage)[key = angle] = (byte)(damage1[key] + (uint)(byte)damage);
			} else
				Damage[angle] = (byte)damage;

			BodyPartFlag |= bodyPart;
			DamageEffectFlag = damageEffectFlag;
			DamgeEffectValue = damageEffectValue;
		}
	}
}
