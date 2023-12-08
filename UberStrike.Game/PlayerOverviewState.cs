using UnityEngine;

internal class PlayerOverviewState : IState {
	public PlayerOverviewState(StateMachine<PlayerStateId> stateMachine) { }

	public void OnEnter() {
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
		Screen.lockCursor = false;
		WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Idle);

		if (Singleton<WeaponController>.Instance.CurrentWeapon) {
			Singleton<WeaponController>.Instance.CurrentWeapon.StopSound();
		}

		GameState.Current.Player.EnableWeaponControl = false;
		LevelCamera.SetMode(LevelCamera.CameraMode.OrbitAround);

		if (GameState.Current.Player.Character != null) {
			GameState.Current.Player.Character.WeaponSimulator.UpdateWeaponSlot(GameState.Current.PlayerData.Player.CurrentWeaponSlot, true);
		}
	}

	public void OnResume() { }
	public void OnExit() { }
	public void OnUpdate() { }
}
