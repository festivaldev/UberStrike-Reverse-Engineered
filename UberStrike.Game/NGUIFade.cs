using System;
using System.Collections;
using UnityEngine;

public class NGUIFade : MonoBehaviour {
	private static NGUIFade instance;

	[SerializeField]
	private float fadeSpeed = 1f;

	[SerializeField]
	private UISprite sprite;

	public static void FadeIn(Action onFinished = null, bool immediate = false) {
		Fade(1f, onFinished, immediate);
	}

	public static void FadeOut(Action onFinished = null, bool immediate = false) {
		Fade(0f, onFinished, immediate);
	}

	public static void Fade(float targetAlpha, Action onFinished = null, bool immediate = false) {
		if (instance == null) {
			UICamera.eventHandler.gameObject.AddComponent<NGUIFade>();
		}

		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.Animate(immediate, targetAlpha, onFinished));
	}

	private void Awake() {
		instance = this;
	}

	private void OnDestroy() {
		instance = null;
	}

	private IEnumerator Animate(bool immediate, float targetAlpha, Action onFinished) {
		UICamera uiCamera = null;
		var tr = transform;

		while (uiCamera == null && tr != null) {
			uiCamera = tr.GetComponent<UICamera>();
			tr = tr.parent;
		}

		uiCamera.enabled = false;

		if (sprite != null) {
			if (sprite.alpha == 1f && transform.localScale == Vector3.one) {
				sprite.alpha = 0f;
			}

			transform.localScale = new Vector3(10000f, 10000f, 1f);

			if (!immediate) {
				while (sprite.alpha != targetAlpha) {
					sprite.alpha = Mathf.MoveTowards(sprite.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

					yield return 0;
				}
			} else {
				sprite.alpha = targetAlpha;
			}

			if (targetAlpha == 0f) {
				transform.localScale = Vector3.one;
			}
		}

		uiCamera.enabled = targetAlpha == 1f;

		if (onFinished != null) {
			onFinished();
		}
	}
}
