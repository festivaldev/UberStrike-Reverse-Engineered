using System;
using UnityEngine;

[Serializable]
public class SoundEffectTunable {
	public AudioClip Clip;
	public float PitchLeft = 1f;
	public float PitchRight = 1f;
	public float VolumeLeft = 1f;
	public float VolumeRight = 1f;

	public SoundEffect Interpolate(float value, float valueFrom, float valueTo) {
		return new SoundEffect(Clip, LinearScale(value, valueFrom, valueTo, VolumeLeft, VolumeRight), LinearScale(value, valueFrom, valueTo, PitchLeft, PitchRight));
	}

	private float LinearScale(float value, float sourceFrom, float sourceTo, float targetFrom, float targetTo) {
		if (sourceFrom == sourceTo) {
			return sourceFrom;
		}

		var num = (targetTo - targetFrom) / (sourceTo - sourceFrom);
		var num2 = targetFrom - num * sourceFrom;

		return num * value + num2;
	}
}
