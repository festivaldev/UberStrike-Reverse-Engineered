using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathArea : MonoBehaviour {
	private void Awake() {
		if (collider) {
			collider.isTrigger = true;
		}
	}

	private void OnTriggerEnter(Collider c) {
		if (c.tag == "Player") {
			GameState.Current.Actions.KillPlayer();
		}
	}
}
