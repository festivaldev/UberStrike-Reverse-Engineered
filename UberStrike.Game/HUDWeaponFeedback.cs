using System.Collections;
using UnityEngine;

public class HUDWeaponFeedback : MonoBehaviour {
	[SerializeField]
	private float fadeInTime = 0.2f;

	[SerializeField]
	private float fadeOutTime = 1f;

	[SerializeField]
	private UILabel feedbackLabel;

	[SerializeField]
	private float onScreenTime = 2.5f;

	private void OnEnable() {
		GameState.Current.PlayerData.ActiveWeapon.Fire();
	}

	private void Start() {
		GameState.Current.PlayerData.ActiveWeapon.AddEventAndFire(HandleSelectedLoadoutChanged, this);
	}

	private void HandleSelectedLoadoutChanged(WeaponSlot weapon) {
		if (weapon != null) {
			var slot = weapon.Slot;

			if (!GameState.Current.PlayerData.LoadoutWeapons.Value.ContainsKey(slot)) {
				return;
			}

			feedbackLabel.text = GameState.Current.PlayerData.LoadoutWeapons.Value[slot].Name;
			StopAllCoroutines();
			StartCoroutine(FadeAnimation());
		}
	}

	private IEnumerator FadeAnimation() {
		TweenAlpha.Begin(feedbackLabel.gameObject, fadeInTime, 1f);

		yield return new WaitForSeconds(onScreenTime);

		TweenAlpha.Begin(feedbackLabel.gameObject, fadeOutTime, 0f);
	}
}
