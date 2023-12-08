using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PenetrationDetector : MonoBehaviour {
	[SerializeField]
	private CharacterController controller;

	private void OnTriggerEnter(Collider c) {
		if (!c.isTrigger) {
			Invoke("KillPlayer", 0f);
		}
	}

	private void KillPlayer() {
		if (controller) {
			controller.transform.position -= 0.5f * controller.velocity.normalized;
		}

		GameState.Current.Actions.KillPlayer();
	}
}
