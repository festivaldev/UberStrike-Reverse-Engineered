using System.Collections;
using UnityEngine;

public class HUDNotificationWarning : MonoBehaviour {
	[SerializeField]
	private float defaultFadeInSpeed = 20f;

	[SerializeField]
	private float defaultFadeOutSpeed = 5f;

	[SerializeField]
	private UILabel label;

	[SerializeField]
	private UIPanel panel;

	private void Start() {
		GameData.Instance.OnWarningNotification.AddEvent(delegate(string el) { Show(el); }, this);
		panel.alpha = 0f;
	}

	private void OnEnable() {
		panel.alpha = 0f;
	}

	public void Show(string text) {
		StopAllCoroutines();
		StartCoroutine(ShowCrt(text, defaultFadeInSpeed, defaultFadeOutSpeed, 1f));
	}

	public IEnumerator ShowCrt(string text, float fadeInSpeed, float fadeOutSpeed, float duration) {
		panel.alpha = 0f;
		label.text = text;

		while (panel.alpha < 1f) {
			panel.alpha = Mathf.MoveTowards(panel.alpha, 1f, Time.deltaTime * fadeInSpeed);

			yield return 0;
		}

		if (duration > 0f) {
			yield return new WaitForSeconds(duration);
		}

		while (panel.alpha > 0f) {
			panel.alpha = Mathf.MoveTowards(panel.alpha, 0f, Time.deltaTime * fadeOutSpeed);

			yield return 0;
		}
	}
}
