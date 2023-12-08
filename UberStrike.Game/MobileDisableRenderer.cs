using UnityEngine;

public class MobileDisableRenderer : MonoBehaviour {
	private void OnEnable() {
		if (ApplicationDataManager.IsMobile) {
			renderer.enabled = false;
		}
	}
}
