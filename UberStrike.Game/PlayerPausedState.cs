using UberStrike.Core.Models;
using UnityEngine;

internal class PlayerPausedState : IState {
	public PlayerPausedState(StateMachine<PlayerStateId> stateMachine) { }

	public void OnEnter() {
		Singleton<WeaponController>.Instance.StopInputHandler();
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
		Screen.lockCursor = false;
		WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Idle);
		GameState.Current.PlayerData.ResetKeys();
		GameState.Current.PlayerData.Set(PlayerStates.Shooting, false);
		GameState.Current.PlayerData.Set(PlayerStates.Paused, true);

		if (GameState.Current.IsLocalAvatarLoaded) {
			LevelCamera.SetMode(LevelCamera.CameraMode.Paused);
			GameState.Current.Player.Character.WeaponSimulator.UpdateWeaponSlot(GameState.Current.PlayerData.Player.CurrentWeaponSlot, true);
		}

		if (GameState.Current.IsMultiplayer) {
			Singleton<ChatManager>.Instance.SetGameSection(GameState.Current.RoomData.Server.ConnectionString, GameState.Current.RoomData.Number, GameState.Current.RoomData.MapID, GameState.Current.Players.Values);
		}
	}

	public void OnResume() { }

	public void OnExit() {
		GameState.Current.PlayerData.Set(PlayerStates.Paused, false);
	}

	public void OnUpdate() {
		if (Input.GetKeyDown(KeyCode.L) && !GameData.Instance.HUDChatIsTyping) {
			if (GamePageManager.IsCurrentPage(IngamePageType.None)) {
				if (GameState.Current.IsSinglePlayer) {
					GamePageManager.Instance.LoadPage(IngamePageType.PausedOffline);
				} else if (!GameState.Current.IsMatchRunning) {
					GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
				} else {
					GamePageManager.Instance.LoadPage(IngamePageType.Paused);
				}
			} else {
				GamePageManager.Instance.UnloadCurrentPage();
			}
		}
	}
}
