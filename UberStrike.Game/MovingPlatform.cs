using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	private Vector3 lastPosition;
	private Transform player;

	private void OnTriggerEnter(Collider c) {
		if (c.tag == "Player") {
			lastPosition = transform.position;
			player = c.transform;
		}
	}

	private void OnTriggerExit(Collider c) {
		if (c.tag == "Player") {
			player = null;
		}
	}

	private void LateUpdate() {
		if (player) {
			player.localPosition += transform.position - lastPosition;
			lastPosition = transform.position;
		}
	}
}
