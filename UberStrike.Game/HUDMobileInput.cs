using UnityEngine;

public class HUDMobileInput : MonoBehaviour {
	[SerializeField]
	private HUDMultitouchController multitouchController;

	[SerializeField]
	private HUDSimpleTouchController simpleTouchController;

	[SerializeField]
	private HUDSniperControls sniperControls;

	private void Start() {
		TouchInput.ShowTouchControls.AddEventAndFire(delegate(bool el) {
			multitouchController.gameObject.SetActive(el && TouchInput.UseMultiTouch);
			simpleTouchController.gameObject.SetActive(el && !TouchInput.UseMultiTouch);
			sniperControls.gameObject.SetActive(el);
		}, this);
	}
}
