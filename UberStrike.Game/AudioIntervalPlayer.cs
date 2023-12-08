using System.Collections;
using UnityEngine;

public class AudioIntervalPlayer : MonoBehaviour {
	[SerializeField]
	private bool waitForClipLength;

	[SerializeField]
	private float waitTime = 30f;

	private IEnumerator Start() {
		audio.loop = false;

		for (;;) {
			audio.Play();

			if (waitForClipLength && audio.clip != null) {
				yield return new WaitForSeconds(audio.clip.length);
			}

			yield return new WaitForSeconds(waitTime);
		}

		yield break;
	}
}
