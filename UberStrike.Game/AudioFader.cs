using System.Collections;
using UnityEngine;

public class AudioFader : MonoBehaviour {
	public float FadeInLength = 1f;
	public float FadeOutLength = 1f;
	public float PlayLength = 5f;
	public float SilentLength = 5f;

	private void OnEnable() {
		StartCoroutine(PlayAudio());
	}

	private IEnumerator PlayAudio() {
		for (;;) {
			yield return new WaitForEndOfFrame();
		}

		yield break;
	}
}
