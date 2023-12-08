using UnityEngine;

public class HUDDesktopFPS : MonoBehaviour {
	[SerializeField]
	private UILabel label;

	private void Start() {
		GameData.Instance.VideoShowFps.AddEventAndFire(delegate(Tuple el) { label.enabled = ApplicationDataManager.ApplicationOptions.VideoShowFps; }, this);
	}

	private void OnEnable() {
		GameData.Instance.VideoShowFps.Fire();
	}

	private void Update() {
		if (label.enabled) {
			label.text = ApplicationDataManager.FrameRate;
		}
	}
}
