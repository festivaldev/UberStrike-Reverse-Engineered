using UnityEngine;

public class BackgroundMusicPlayer : AutoMonoBehaviour<BackgroundMusicPlayer> {
	private MusicFader musicFaderA;
	private MusicFader musicFaderB;
	private bool toggle;

	public float Volume {
		set { Current.Source.volume = value; }
	}

	private MusicFader Current {
		get { return (!toggle) ? musicFaderB : musicFaderA; }
	}

	private void Awake() {
		var audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0f;
		audioSource.loop = true;
		var audioSource2 = gameObject.AddComponent<AudioSource>();
		audioSource2.volume = 0f;
		audioSource2.loop = true;
		musicFaderA = new MusicFader(audioSource);
		musicFaderB = new MusicFader(audioSource2);
	}

	public void Play(AudioClip clip) {
		if (Current.Source.clip != clip) {
			Current.FadeOut();
			toggle = !toggle;
			Current.Source.clip = clip;
			Current.FadeIn(SfxManager.MusicAudioVolume);
		} else {
			Current.FadeIn(SfxManager.MusicAudioVolume);
		}
	}

	public void Stop() {
		Current.FadeOut();
	}
}
