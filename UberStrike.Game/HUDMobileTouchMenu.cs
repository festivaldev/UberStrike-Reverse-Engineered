using UnityEngine;

public class HUDMobileTouchMenu : MonoBehaviour {
	[SerializeField]
	private UIButton chatButton;

	[SerializeField]
	private UIButton pauseButton;

	[SerializeField]
	private UIButton takeScreenshotButton;

	private void Start() {
		pauseButton.OnRelease = delegate { EventHandler.Global.Fire(new GameEvents.PlayerPause()); };
		chatButton.OnRelease = delegate { GameData.Instance.OnHUDChatStartTyping.Fire(); };

		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) {
			var flag = el == PlayerStateId.Paused;
			pauseButton.gameObject.SetActive(!flag);
		}, this);
	}
}
