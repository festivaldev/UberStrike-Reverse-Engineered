using System;
using UnityEngine;

[Obsolete("To be removed")]
public class WeaponKickback : BaseWeaponEffect {
	public override void OnShoot() { }
	public override void OnPostShoot() { }
	public override void Hide() { }
	public override void OnHits(RaycastHit[] hits) { }
}
