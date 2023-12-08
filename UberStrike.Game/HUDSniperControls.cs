using UnityEngine;

public class HUDSniperControls : MonoBehaviour {
	private Rect ignoreRect = default(Rect);

	[SerializeField]
	private UIEventReceiver sniperButton;

	private ZoomInfo zoomInfo;

	[SerializeField]
	private UISlider zoomSlider;

	private void OnEnable() {
		sniperButton.gameObject.SetActive(false);
		zoomSlider.gameObject.SetActive(false);
	}

	private void Start() {
		ignoreRect = new Rect(Screen.width - 100f, 400f, 100f, 300f);
		sniperButton.OnClicked = delegate { };
		zoomSlider.onValueChange = delegate(float el) { EventHandler.Global.Fire(new GlobalEvents.InputChanged((el != 0f) ? GameInputKey.NextWeapon : GameInputKey.PrevWeapon, 1f)); };

		GameState.Current.PlayerData.ActiveWeapon.AddEventAndFire(delegate(WeaponSlot el) {
			if (el != null) {
				sniperButton.gameObject.SetActive(el.View.WeaponSecondaryAction != 0);
				zoomInfo = new ZoomInfo(el.View.DefaultZoomMultiplier, el.View.MinZoomMultiplier, el.View.MaxZoomMultiplier);
			}
		}, this);

		TouchInput.OnSecondaryFire.AddEventAndFire(delegate(bool el) {
			var flag = el && zoomInfo != null && zoomInfo.DefaultMultiplier != 1f && zoomInfo.MaxMultiplier != zoomInfo.MinMultiplier;
			zoomSlider.gameObject.SetActive(flag);

			if (flag) {
				zoomSlider.sliderValue = 0f;
				AutoMonoBehaviour<TouchInput>.Instance.Shooter.IgnoreRect(ignoreRect);
			} else {
				AutoMonoBehaviour<TouchInput>.Instance.Shooter.UnignoreRect(ignoreRect);
			}
		}, this);
	}
}
