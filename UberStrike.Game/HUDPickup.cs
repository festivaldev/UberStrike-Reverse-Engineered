using System.Collections;
using UnityEngine;

public class HUDPickup : MonoBehaviour {
	[SerializeField]
	private float fadeoutSpeed = 1f;

	[SerializeField]
	private UILabel label;

	private int lastCount;
	private PickUpMessageType lastItem;
	private float lastTime;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private float pickupMiltiplicationMaxTime = 3f;

	[SerializeField]
	private float showDuration = 2f;

	private void Start() {
		panel.alpha = 0f;

		GameData.Instance.OnItemPickup.AddEvent(delegate(string itemName, PickUpMessageType el) {
			StopAllCoroutines();
			StartCoroutine(ShowCrt(itemName, el));
		}, this);
	}

	private IEnumerator ShowCrt(string itemName, PickUpMessageType item) {
		if (lastItem == item && Time.time <= lastTime + pickupMiltiplicationMaxTime) {
			lastCount++;
		} else {
			lastCount = 1;
		}

		lastTime = Time.time;
		lastItem = item;
		panel.alpha = 1f;

		if (lastCount > 1) {
			label.text = itemName + " x " + lastCount;
		} else {
			label.text = itemName;
		}

		yield return new WaitForSeconds(showDuration);

		while (panel.alpha > 0f) {
			panel.alpha = Mathf.MoveTowards(panel.alpha, 0f, Time.deltaTime * fadeoutSpeed);

			yield return 0;
		}
	}
}
