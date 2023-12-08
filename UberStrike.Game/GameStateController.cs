using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameStateController : Singleton<GameStateController> {
	private IGameMode currentGameMode;

	public GameMode CurrentGameMode {
		get { return (currentGameMode == null) ? GameMode.None : currentGameMode.Type; }
	}

	public GamePeer Client { get; private set; }

	private GameStateController() {
		Client = new GamePeer();
	}

	public void CreateNetworkGame(string server, int mapId, GameModeType mode, string name, string password, int timeMinutes, int killLimit, int playerLimit, int minLevel, int maxLevel, GameFlags.GAME_FLAGS flags) {
		var gameRoomData = new GameRoomData {
			Name = name,
			Server = new ConnectionAddress(server),
			MapID = mapId,
			TimeLimit = timeMinutes,
			PlayerLimit = playerLimit,
			GameMode = mode,
			GameFlags = (int)flags,
			KillLimit = killLimit,
			LevelMin = (byte)Mathf.Clamp(minLevel, 0, 255),
			LevelMax = (byte)Mathf.Clamp(maxLevel, 0, 255)
		};

		var time = Time.time;
		var dialog = PopupSystem.ShowProgress("Authentication", "Connecting to Server", () => Mathf.Clamp(Time.time - time, 0f, 3f));
		dialog.SetCancelable(delegate { PopupSystem.HideMessage(dialog); });
		Client.CreateGame(gameRoomData, password);
	}

	public void JoinNetworkGame(GameRoomData data) {
		if (data.Server != null) {
			var time = Time.time;
			var dialog = PopupSystem.ShowProgress("Authentication", "Connecting to Server", () => Mathf.Clamp(Time.time - time, 0f, 3f));
			dialog.SetCancelable(delegate { PopupSystem.HideMessage(dialog); });
			Singleton<ChatManager>.Instance.InGameDialog.Clear();
			Client.JoinGame(data.Server.ConnectionString, data.Number, string.Empty);
		} else {
			PopupSystem.ShowError("Game not found", "The game doesn't exist anymore.", PopupSystem.AlertType.OK);
		}
	}

	public void JoinNetworkGame(GameRoom data) {
		if (data.Server != null) {
			var time = Time.time;
			var dialog = PopupSystem.ShowProgress("Authentication", "Connecting to Server", () => Mathf.Clamp(Time.time - time, 0f, 3f));
			dialog.SetCancelable(delegate { PopupSystem.HideMessage(dialog); });
			Singleton<ChatManager>.Instance.InGameDialog.Clear();
			Client.JoinGame(data.Server.ConnectionString, data.Number, string.Empty);
		} else {
			PopupSystem.ShowError("Game not found", "The game doesn't exist anymore.", PopupSystem.AlertType.OK);
		}
	}

	public void LeaveGame(bool warnBeforeLeaving = false) {
		if (warnBeforeLeaving && GameState.Current.IsMultiplayer && GameState.Current.IsMatchRunning) {
			PopupSystem.ShowMessage(LocalizedStrings.LeavingGame, LocalizedStrings.LeaveGameWarningMsg, PopupSystem.AlertType.OKCancel, BackToMenu, LocalizedStrings.LeaveCaps, null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Negative);
		} else {
			BackToMenu();
		}
	}

	public void ResetClient() {
		Client.Dispose();
		Client = new GamePeer();
	}

	private void BackToMenu() {
		GamePageManager.Instance.UnloadCurrentPage();
		UnloadGameMode();

		if (Singleton<SceneLoader>.Instance.CurrentScene != "Menu") {
			Singleton<SceneLoader>.Instance.LoadLevel("Menu");
		}
	}

	public void UnloadGameMode() {
		SetGameMode(null);
	}

	public void SetGameMode(IGameMode mode) {
		if (currentGameMode != null) {
			Client.LeaveGame();
			currentGameMode.Dispose();
		}

		currentGameMode = mode;
	}
}
