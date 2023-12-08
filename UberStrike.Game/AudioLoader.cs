using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : Singleton<AudioLoader> {
	private Dictionary<string, AudioClip> cachedAudioClips;

	public IEnumerable<KeyValuePair<string, AudioClip>> AllClips {
		get { return cachedAudioClips; }
	}

	private AudioLoader() {
		cachedAudioClips = new Dictionary<string, AudioClip>();
	}

	public AudioClip Get(string name) {
		if (!cachedAudioClips.ContainsKey(name)) {
			Debug.LogWarning("AudioClip was not found : " + name);
		}

		return cachedAudioClips[name];
	}
}
