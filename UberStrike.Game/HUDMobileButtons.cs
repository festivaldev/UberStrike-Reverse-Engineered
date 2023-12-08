using UnityEngine;

public class HUDMobileButtons : MonoBehaviour {
	[SerializeField]
	private UIButton multitouchButton;

	[SerializeField]
	private UIButton simpleInputButton;

	private void Start() {
		simpleInputButton.gameObject.SetActive(false);
		multitouchButton.gameObject.SetActive(false);

		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) {
			var flag = el == PlayerStateId.Paused;
			simpleInputButton.gameObject.SetActive(flag && TouchInput.UseMultiTouch);
			multitouchButton.gameObject.SetActive(flag && !TouchInput.UseMultiTouch);
		}, this);

		TouchInput.UseMultiTouch.AddEvent(delegate(bool el) {
			simpleInputButton.gameObject.SetActive(el);
			multitouchButton.gameObject.SetActive(!el);
		}, this);

		simpleInputButton.OnRelease = delegate {
			ApplicationDataManager.ApplicationOptions.UseMultiTouch = false;
			ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
		};

		multitouchButton.OnRelease = delegate {
			ApplicationDataManager.ApplicationOptions.UseMultiTouch = true;
			ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
		};
	}
}
