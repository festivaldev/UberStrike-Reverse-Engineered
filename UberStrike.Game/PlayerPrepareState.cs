using UnityEngine;

internal class PlayerPrepareState : IState {
	public PlayerPrepareState(StateMachine<PlayerStateId> stateMachine) { }

	public void OnEnter() {
		GameState.Current.Player.InitializePlayer();
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		GameState.Current.Player.EnableWeaponControl = false;
		Screen.lockCursor = true;
		LevelCamera.SetMode(LevelCamera.CameraMode.FirstPerson);
		EventHandler.Global.Fire(new GameEvents.PlayerIngame());
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate += GameState.Current.Player.MoveController.UpdatePlayerMovement;
	}

	public void OnResume() { }

	public void OnExit() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate -= GameState.Current.Player.MoveController.UpdatePlayerMovement;
	}

	public void OnUpdate() { }
}
