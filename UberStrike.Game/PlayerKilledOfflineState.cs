using UnityEngine;

internal class PlayerKilledOfflineState : IState {
	public PlayerKilledOfflineState(StateMachine<PlayerStateId> stateMachine) { }

	public void OnEnter() {
		Screen.lockCursor = false;
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
		LevelCamera.SetMode(LevelCamera.CameraMode.Ragdoll);
	}

	public void OnResume() { }
	public void OnExit() { }

	public void OnUpdate() {
		if (Input.GetKeyDown(KeyCode.L) && !GameData.Instance.HUDChatIsTyping) {
			if (GamePageManager.IsCurrentPage(IngamePageType.None)) {
				GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
			} else {
				GamePageManager.Instance.UnloadCurrentPage();
			}
		}
	}
}
