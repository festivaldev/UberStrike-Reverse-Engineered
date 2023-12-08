using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class DamageInfo {
	public byte Bullets { get; set; }
	public short Damage { get; set; }
	public Vector3 Force { get; set; }
	public float UpwardsForceMultiplier { get; set; }
	public Vector3 Hitpoint { get; set; }
	public BodyPart BodyPart { get; set; }
	public int ProjectileID { get; set; }
	public int WeaponID { get; set; }
	public UberstrikeItemClass WeaponClass { get; set; }
	public byte SlotId { get; set; }
	public float CriticalStrikeBonus { get; set; }
	public DamageEffectType DamageEffectFlag { get; set; }
	public float DamageEffectValue { get; set; }
	public byte Distance { get; set; }

	public bool IsExplosion {
		get { return WeaponClass == UberstrikeItemClass.WeaponCannon || WeaponClass == UberstrikeItemClass.WeaponLauncher; }
	}

	public DamageInfo(short damage) {
		Damage = damage;
		Force = Vector3.zero;
		BodyPart = BodyPart.Body;
		Bullets = 1;
	}

	public DamageInfo(Vector3 force, BodyPart bodyPart) {
		Force = force;
		BodyPart = bodyPart;
		Bullets = 1;
	}
}
