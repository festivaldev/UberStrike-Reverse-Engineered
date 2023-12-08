using System;
using UnityEngine;

public class AudioFadeArea : MonoBehaviour {
	private static AudioFadeArea Current;

	[SerializeField]
	private AudioArea indoorAudio;

	[SerializeField]
	private AudioArea outdoorAudio;

	private void Awake() {
		collider.isTrigger = true;
	}

	private void Update() {
		if (Current == this) {
			outdoorAudio.Update();
			indoorAudio.Update();
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			Current = this;

			if (outdoorAudio != null) {
				outdoorAudio.Enabled = false;
			}

			if (indoorAudio != null) {
				indoorAudio.Enabled = true;
			}
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.tag == "Player") {
			if (outdoorAudio != null) {
				outdoorAudio.Enabled = Current == this;
			}

			if (indoorAudio != null) {
				indoorAudio.Enabled = false;
			}
		}
	}

	[Serializable]
	private class AudioArea {
		[SerializeField]
		private float currentVolume = 1f;

		[SerializeField]
		private bool enabled;

		[SerializeField]
		private float fadeSpeed = 3f;

		[SerializeField]
		private float maxVolume = 1f;

		[SerializeField]
		private AudioSource[] sources;

		public bool Enabled {
			get { return enabled; }
			set { enabled = value; }
		}

		public AudioArea() {
			currentVolume = ((!enabled) ? 0f : maxVolume);
		}

		public bool Update() {
			if (enabled && currentVolume < maxVolume) {
				currentVolume = Mathf.Lerp(currentVolume, maxVolume, Time.deltaTime * fadeSpeed);

				if (Mathf.Abs(currentVolume - maxVolume) < 0.01f) {
					currentVolume = maxVolume;
				}

				Array.ForEach(sources, delegate(AudioSource s) { s.volume = currentVolume; });

				return true;
			}

			if (!enabled && currentVolume > 0f) {
				currentVolume = Mathf.Lerp(currentVolume, 0f, Time.deltaTime * fadeSpeed);

				if (currentVolume < 0.01f) {
					currentVolume = 0f;
				}

				Array.ForEach(sources, delegate(AudioSource s) { s.volume = currentVolume; });

				return true;
			}

			return false;
		}
	}
}
