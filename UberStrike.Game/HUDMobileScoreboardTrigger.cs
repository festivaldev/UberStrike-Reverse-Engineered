using UnityEngine;

public class HUDMobileScoreboardTrigger : MonoBehaviour {
	[SerializeField]
	private UIEventReceiver scoreboardButton;

	[SerializeField]
	private GameObject visualAid;

	private void Start() {
		scoreboardButton.OnPressed = delegate(bool el) { TabScreenPanelGUI.ForceShow = el; };
		GameData.Instance.GameState.AddEventAndFire(delegate(GameStateId el) { visualAid.SetActive(el == GameStateId.PrepareNextRound); }, this);
	}
}
