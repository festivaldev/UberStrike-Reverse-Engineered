using System.Collections;
using UnityEngine;

public class MusicFader {
	private AudioSource _audioSource;
	private bool _isFading;
	private float _targetVolume;

	public AudioSource Source {
		get { return _audioSource; }
	}

	public MusicFader(AudioSource audio) {
		_audioSource = audio;
	}

	public void FadeIn(float volume) {
		_targetVolume = volume;

		if (!_isFading) {
			if (!_audioSource.isPlaying) {
				_audioSource.Play();
			}

			UnityRuntime.StartRoutine(StartFading());
		}
	}

	public void FadeOut() {
		_targetVolume = 0f;

		if (!_isFading) {
			UnityRuntime.StartRoutine(StartFading());
		}
	}

	private IEnumerator StartFading() {
		_isFading = true;

		while (Mathf.Abs(_audioSource.volume - _targetVolume) > 0.05f) {
			_audioSource.volume = Mathf.Lerp(_audioSource.volume, _targetVolume, Time.deltaTime * 3f);

			yield return new WaitForEndOfFrame();
		}

		if (_targetVolume == 0f) {
			_audioSource.volume = 0f;
			_audioSource.Stop();
		}

		_isFading = false;
	}
}
