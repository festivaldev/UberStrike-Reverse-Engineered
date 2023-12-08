using System;
using System.Collections;
using UnityEngine;

public class GUIPageBase : MonoBehaviour {
	[SerializeField]
	public float bringInDuration = 0.8f;

	[SerializeField]
	public float dismissDuration = 0.2f;

	public IEnumerator AnimateAlpha(float to, float duration, params UIButton[] buttons) {
		yield return StartCoroutine(this.AnimateAlpha(to, duration, Array.ConvertAll<UIButton, UIPanel>(buttons, el => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params UIEventReceiver[] buttons) {
		yield return StartCoroutine(this.AnimateAlpha(to, duration, Array.ConvertAll<UIEventReceiver, UIPanel>(buttons, el => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params GameObject[] objects) {
		yield return StartCoroutine(this.AnimateAlpha(to, duration, Array.ConvertAll<GameObject, UIPanel>(objects, el => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params UIPanel[] buttons) {
		TweenAlpha[] tweens = Array.ConvertAll<UIPanel, TweenAlpha>(buttons, el => TweenAlpha.Begin(el.gameObject, duration, to));

		foreach (TweenAlpha el2 in tweens) {
			while (el2.enabled) {
				yield return 0;
			}
		}
	}

	public void Dismiss(Action onFinished) {
		StopAllCoroutines();
		StartCoroutine(DismissCrt(onFinished));
	}

	private IEnumerator DismissCrt(Action onFinished) {
		yield return StartCoroutine(OnDismiss());

		if (onFinished != null) {
			onFinished();
		}
	}

	protected virtual IEnumerator OnDismiss() {
		yield return 0;
	}

	public void BringIn() {
		StopAllCoroutines();
		StartCoroutine(OnBringIn());
	}

	protected virtual IEnumerator OnBringIn() {
		yield return 0;
	}
}
