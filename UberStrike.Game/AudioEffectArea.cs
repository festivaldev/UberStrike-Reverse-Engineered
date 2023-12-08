using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AudioEffectArea : MonoBehaviour {
	[SerializeField]
	private GameObject indoorEnvironment;

	[SerializeField]
	private GameObject outdoorEnvironment;

	private void Awake() {
		collider.isTrigger = true;

		if (indoorEnvironment != null) {
			indoorEnvironment.SetActive(true);
		}

		if (outdoorEnvironment != null) {
			outdoorEnvironment.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			if (outdoorEnvironment != null) {
				outdoorEnvironment.SetActive(false);
			}

			if (indoorEnvironment != null) {
				indoorEnvironment.SetActive(true);
			}
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.tag == "Player") {
			if (outdoorEnvironment != null) {
				outdoorEnvironment.SetActive(true);
			}

			if (indoorEnvironment != null) {
				indoorEnvironment.SetActive(false);
			}
		}
	}
}
