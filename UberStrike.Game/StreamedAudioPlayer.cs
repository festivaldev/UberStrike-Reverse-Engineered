using System.Collections;
using UnityEngine;

public class StreamedAudioPlayer : AutoMonoBehaviour<StreamedAudioPlayer> {
	private static int _playCounter;

	public void PlayMusic(AudioSource source, string clipName) {
		if (!string.IsNullOrEmpty(clipName)) {
			StartCoroutine(PlayMusic(source, Singleton<AudioLoader>.Instance.Get(clipName)));
		} else {
			StopMusic(source);
		}
	}

	public void StopMusic(AudioSource source) {
		_playCounter++;
		source.Stop();
	}

	private IEnumerator PlayMusic(AudioSource source, AudioClip clip) {
		var id = ++_playCounter;
		var isStreamed = !clip || !clip.isReadyToPlay;

		while (clip && !clip.isReadyToPlay) {
			yield return new WaitForEndOfFrame();
		}

		if (isStreamed) {
			yield return new WaitForSeconds(1f);
		}

		if (clip != null) {
			source.clip = clip;
			source.Play();
		}

		while (id == _playCounter) {
			while (source.isPlaying && id == _playCounter) {
				yield return new WaitForEndOfFrame();
			}

			if (id == _playCounter) {
				source.Play();

				yield return new WaitForEndOfFrame();
			} else {
				source.Stop();
			}
		}
	}
}
