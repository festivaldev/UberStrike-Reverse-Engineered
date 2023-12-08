using UnityEngine;

internal class PlayerAfterRoundState : IState {
	public PlayerAfterRoundState(StateMachine<PlayerStateId> stateMachine) { }

	public void OnEnter() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate += GameState.Current.Player.MoveController.UpdatePlayerMovement;
	}

	public void OnExit() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate -= GameState.Current.Player.MoveController.UpdatePlayerMovement;
	}

	public void OnResume() { }

	public void OnUpdate() {
		if (!Screen.lockCursor && !ApplicationDataManager.IsMobile) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		var flag = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetKey(KeyCode.Tab);

		if (Input.GetKeyDown(KeyCode.Escape) || flag) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		if (!GameData.Instance.HUDChatIsTyping && Input.GetKeyDown(KeyCode.Backspace)) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}
	}
}
