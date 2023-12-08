using System.Collections.Generic;
using UnityEngine;

public class SfxManager : AutoMonoBehaviour<SfxManager> {
	private AudioClip[] _footStepDirt;
	private AudioClip[] _footStepGlass;
	private AudioClip[] _footStepGrass;
	private AudioClip[] _footStepHeavyMetal;
	private AudioClip[] _footStepMetal;
	private AudioClip[] _footStepRock;
	private AudioClip[] _footStepSand;
	private AudioClip[] _footStepSnow;
	private AudioClip[] _footStepWater;
	private AudioClip[] _footStepWood;
	private Dictionary<string, AudioClip[]> _surfaceImpactSoundMap;
	private AudioClip[] _swimAboveWater;
	private AudioClip[] _swimUnderWater;
	private AudioPool audioPool;
	private AudioSource gameAudioSource;
	private AudioSource uiAudioSource;
	private AudioSource uiAudioSourceLooped;

	public static float EffectsAudioVolume {
		get { return ApplicationDataManager.ApplicationOptions.AudioEffectsVolume; }
	}

	public static float MusicAudioVolume {
		get { return ApplicationDataManager.ApplicationOptions.AudioMusicVolume; }
	}

	public static float MasterAudioVolume {
		get { return ApplicationDataManager.ApplicationOptions.AudioMasterVolume; }
	}

	private void Awake() {
		audioPool = new AudioPool();
		gameAudioSource = gameObject.AddComponent<AudioSource>();
		gameAudioSource.loop = false;
		gameAudioSource.playOnAwake = false;
		gameAudioSource.rolloffMode = AudioRolloffMode.Linear;
		gameAudioSource.priority = 100;
		uiAudioSource = gameObject.AddComponent<AudioSource>();
		uiAudioSource.loop = false;
		uiAudioSource.playOnAwake = false;
		uiAudioSource.rolloffMode = AudioRolloffMode.Linear;
		uiAudioSource.priority = 100;
		uiAudioSourceLooped = gameObject.AddComponent<AudioSource>();
		uiAudioSourceLooped.loop = true;
		uiAudioSourceLooped.playOnAwake = false;
		uiAudioSourceLooped.rolloffMode = AudioRolloffMode.Linear;

		_footStepDirt = new[] {
			GameAudio.FootStepDirt1,
			GameAudio.FootStepDirt2,
			GameAudio.FootStepDirt3,
			GameAudio.FootStepDirt4
		};

		_footStepGrass = new[] {
			GameAudio.FootStepGrass1,
			GameAudio.FootStepGrass2,
			GameAudio.FootStepGrass3,
			GameAudio.FootStepGrass4
		};

		_footStepMetal = new[] {
			GameAudio.FootStepMetal1,
			GameAudio.FootStepMetal2,
			GameAudio.FootStepMetal3,
			GameAudio.FootStepMetal4
		};

		_footStepHeavyMetal = new[] {
			GameAudio.FootStepHeavyMetal1,
			GameAudio.FootStepHeavyMetal2,
			GameAudio.FootStepHeavyMetal3,
			GameAudio.FootStepHeavyMetal4
		};

		_footStepRock = new[] {
			GameAudio.FootStepRock1,
			GameAudio.FootStepRock2,
			GameAudio.FootStepRock3,
			GameAudio.FootStepRock4
		};

		_footStepSand = new[] {
			GameAudio.FootStepSand1,
			GameAudio.FootStepSand2,
			GameAudio.FootStepSand3,
			GameAudio.FootStepSand4
		};

		_footStepWater = new[] {
			GameAudio.FootStepWater1,
			GameAudio.FootStepWater2,
			GameAudio.FootStepWater3
		};

		_footStepWood = new[] {
			GameAudio.FootStepWood1,
			GameAudio.FootStepWood2,
			GameAudio.FootStepWood3,
			GameAudio.FootStepWood4
		};

		_swimAboveWater = new[] {
			GameAudio.SwimAboveWater1,
			GameAudio.SwimAboveWater2,
			GameAudio.SwimAboveWater3,
			GameAudio.SwimAboveWater4
		};

		_swimUnderWater = new[] {
			GameAudio.SwimUnderWater
		};

		_footStepSnow = new[] {
			GameAudio.FootStepSnow1,
			GameAudio.FootStepSnow2,
			GameAudio.FootStepSnow3,
			GameAudio.FootStepSnow4
		};

		_footStepGlass = new[] {
			GameAudio.FootStepGlass1,
			GameAudio.FootStepGlass2,
			GameAudio.FootStepGlass3,
			GameAudio.FootStepGlass4
		};

		AudioClip[] array = {
			GameAudio.ImpactCement1,
			GameAudio.ImpactCement2,
			GameAudio.ImpactCement3,
			GameAudio.ImpactCement4
		};

		AudioClip[] array2 = {
			GameAudio.ImpactGlass1,
			GameAudio.ImpactGlass2,
			GameAudio.ImpactGlass3,
			GameAudio.ImpactGlass4,
			GameAudio.ImpactGlass5
		};

		AudioClip[] array3 = {
			GameAudio.ImpactGrass1,
			GameAudio.ImpactGrass2,
			GameAudio.ImpactGrass3,
			GameAudio.ImpactGrass4
		};

		AudioClip[] array4 = {
			GameAudio.ImpactMetal1,
			GameAudio.ImpactMetal2,
			GameAudio.ImpactMetal3,
			GameAudio.ImpactMetal4,
			GameAudio.ImpactMetal5
		};

		AudioClip[] array5 = {
			GameAudio.ImpactSand1,
			GameAudio.ImpactSand2,
			GameAudio.ImpactSand3,
			GameAudio.ImpactSand4,
			GameAudio.ImpactSand5
		};

		AudioClip[] array6 = {
			GameAudio.ImpactStone1,
			GameAudio.ImpactStone2,
			GameAudio.ImpactStone3,
			GameAudio.ImpactStone4,
			GameAudio.ImpactStone5
		};

		AudioClip[] array7 = {
			GameAudio.ImpactWater1,
			GameAudio.ImpactWater2,
			GameAudio.ImpactWater3,
			GameAudio.ImpactWater4,
			GameAudio.ImpactWater5
		};

		AudioClip[] array8 = {
			GameAudio.ImpactWood1,
			GameAudio.ImpactWood2,
			GameAudio.ImpactWood3,
			GameAudio.ImpactWood4,
			GameAudio.ImpactWood5
		};

		_surfaceImpactSoundMap = new Dictionary<string, AudioClip[]> {
			{
				"Wood", array8
			}, {
				"SolidWood", array8
			}, {
				"Stone", array6
			}, {
				"Metal", array4
			}, {
				"Sand", array5
			}, {
				"Grass", array3
			}, {
				"Glass", array2
			}, {
				"Cement", array
			}, {
				"Water", array7
			}
		};
	}

	public void PlayInGameAudioClip(AudioClip audioClip, ulong delay = 0UL) {
		if (audioClip != null) {
			Instance.gameAudioSource.PlayOneShot(audioClip);
		}
	}

	public void Play2dAudioClip(SoundEffect sound) {
		Play2dAudioClip(sound.Clip, 0UL, sound.Volume, sound.Pitch);
	}

	public void Play2dAudioClip(AudioClip audioClip, ulong delay = 0UL, float volume = 1f, float pitch = 1f) {
		if (audioClip == null) {
			return;
		}

		if (delay > 0UL) {
			Instance.uiAudioSource.clip = audioClip;
			Instance.uiAudioSource.PlayDelayed(delay);
		} else {
			Instance.uiAudioSource.PlayOneShot(audioClip);
		}

		var applicationOptions = ApplicationDataManager.ApplicationOptions;
		var num = ((!applicationOptions.AudioEnabled) ? 0f : (applicationOptions.AudioEffectsVolume * applicationOptions.AudioMasterVolume));
		Instance.uiAudioSource.volume = num * volume;
		Instance.uiAudioSource.pitch = pitch;
	}

	public void PlayLoopedAudioClip(SoundEffect sound) {
		PlayLoopedAudioClip(sound.Clip, sound.Volume, sound.Pitch);
	}

	public void PlayLoopedAudioClip(AudioClip audioClip, float volume = 1f, float pitch = 1f) {
		if (audioClip == null) {
			return;
		}

		var applicationOptions = ApplicationDataManager.ApplicationOptions;
		var num = ((!applicationOptions.AudioEnabled) ? 0f : applicationOptions.AudioEffectsVolume);
		uiAudioSourceLooped.volume = num * Mathf.Clamp01(volume);
		uiAudioSourceLooped.pitch = Mathf.Clamp(pitch, -3f, 3f);

		if (uiAudioSourceLooped.clip == audioClip && uiAudioSourceLooped.isPlaying) {
			return;
		}

		uiAudioSourceLooped.clip = audioClip;
		uiAudioSourceLooped.Play();
	}

	public void StopLoopedAudioClip() {
		uiAudioSourceLooped.Stop();
	}

	public void Play3dAudioClip(AudioClip clip, Vector3 position, float volume = 1f) {
		var applicationOptions = ApplicationDataManager.ApplicationOptions;
		var num = ((!applicationOptions.AudioEnabled) ? 0f : (applicationOptions.AudioEffectsVolume * applicationOptions.AudioMasterVolume));
		volume *= num;
		audioPool.PlayClipAtPoint(clip, position, volume);
	}

	public AudioClip GetFootStepAudioClip(FootStepSoundType footStep) {
		AudioClip[] array;

		switch (footStep) {
			case FootStepSoundType.Grass:
				array = _footStepGrass;

				break;
			case FootStepSoundType.Metal:
				array = _footStepMetal;

				break;
			case FootStepSoundType.Rock:
				array = _footStepRock;

				break;
			case FootStepSoundType.Sand:
				array = _footStepSand;

				break;
			case FootStepSoundType.Water:
				array = _footStepWater;

				break;
			case FootStepSoundType.Wood:
				array = _footStepWood;

				break;
			case FootStepSoundType.Swim:
				array = _swimAboveWater;

				break;
			case FootStepSoundType.Dive:
				array = _swimUnderWater;

				break;
			case FootStepSoundType.Snow:
				array = _footStepSnow;

				break;
			case FootStepSoundType.HeavyMetal:
				array = _footStepHeavyMetal;

				break;
			case FootStepSoundType.Glass:
				array = _footStepGlass;

				break;
			default:
				array = _footStepDirt;

				break;
		}

		if (array.Length > 1) {
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		return array[0];
	}

	public void PlayImpactSound(string surfaceType, Vector3 position) {
		AudioClip[] array = null;

		if (_surfaceImpactSoundMap.TryGetValue(surfaceType, out array)) {
			Play3dAudioClip(array[UnityEngine.Random.Range(0, array.Length)], position);
		}
	}

	public void EnableAudio(bool enabled) {
		AudioListener.volume = ((!enabled) ? 0f : ApplicationDataManager.ApplicationOptions.AudioMasterVolume);
	}

	public void UpdateMasterVolume() {
		if (ApplicationDataManager.ApplicationOptions.AudioEnabled) {
			AudioListener.volume = ApplicationDataManager.ApplicationOptions.AudioMasterVolume;
		}
	}

	public void UpdateMusicVolume() {
		if (ApplicationDataManager.ApplicationOptions.AudioEnabled) {
			AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Volume = ApplicationDataManager.ApplicationOptions.AudioMusicVolume;

			if (GameState.Current.Map != null) {
				GameState.Current.Map.UpdateVolumes(ApplicationDataManager.ApplicationOptions.AudioMusicVolume);
			}
		}
	}

	public void UpdateEffectsVolume() {
		var applicationOptions = ApplicationDataManager.ApplicationOptions;
		var num = ((!applicationOptions.AudioEnabled) ? 0f : applicationOptions.AudioEffectsVolume);
		Instance.uiAudioSource.volume = num;
		Instance.gameAudioSource.volume = num;
		Instance.uiAudioSourceLooped.volume = num;
	}

	private class AudioPool {
		private Queue<AudioSource> audioPool;

		public AudioPool(int size = 10) {
			var transform = new GameObject("AudioPool").transform;
			DontDestroyOnLoad(transform);
			audioPool = new Queue<AudioSource>();

			for (var i = 0; i < size; i++) {
				var component = new GameObject("AudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
				component.gameObject.transform.parent = transform;
				component.playOnAwake = false;
				component.minDistance = 0f;
				component.maxDistance = 80f;
				component.rolloffMode = AudioRolloffMode.Custom;
				component.loop = false;
				audioPool.Enqueue(component);
			}
		}

		public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume) {
			if (clip != null) {
				var audioSource = audioPool.Dequeue();
				audioSource.transform.position = position;
				audioSource.clip = clip;
				audioSource.volume = volume;
				audioSource.Play();
				audioPool.Enqueue(audioSource);
			}
		}
	}
}
