public class PlayerDropPickupItem : PickupItem {
	protected override bool OnPlayerPickup() {
		return false;
	}

	protected override void OnRemotePickup() { }
}
