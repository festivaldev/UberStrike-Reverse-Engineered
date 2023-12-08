using UberStrike.Core.Types;
using UnityEngine;

public class WeaponPickupItem : PickupItem {
	[SerializeField]
	private UberstrikeItemClass _weaponType;

	protected override bool OnPlayerPickup() {
		return false;
	}

	protected override void OnRemotePickup() { }
}
