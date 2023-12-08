using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class AudioTriggerArea : MonoBehaviour {
	private AudioSource audioSource;

	[SerializeField]
	private bool loopClip;

	[SerializeField]
	private float maxVolume;

	private float wishVolume;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0f;
	}

	private void Update() {
		if (audioSource.isPlaying) {
			if (audioSource.volume < wishVolume) {
				audioSource.volume += Time.deltaTime;
			} else {
				audioSource.volume -= Time.deltaTime;
			}

			if (audioSource.volume <= 0f) {
				audioSource.Stop();
			}
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			audioSource.loop = loopClip;
			wishVolume = maxVolume;
			audioSource.Play();
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.tag == "Player" && audioSource.isPlaying) {
			wishVolume = 0f;
		}
	}
}
