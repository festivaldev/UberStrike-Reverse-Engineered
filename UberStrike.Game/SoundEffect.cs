using System;
using UnityEngine;

[Serializable]
public class SoundEffect {
	public AudioClip Clip;
	public float Pitch = 1f;
	public float Volume = 1f;

	public SoundEffect(AudioClip clip, float volume = 1f, float pitch = 1f) {
		Clip = clip;
		Volume = volume;
		Pitch = pitch;
	}
}
