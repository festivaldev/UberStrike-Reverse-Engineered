using UnityEngine;

public class DebugGameState : IDebugPage {
	private Vector2 v1;

	public string Title {
		get { return "States"; }
	}

	public void Draw() {
		if (GameState.Current != null) {
			v1 = GUILayout.BeginScrollView(v1);
			GUILayout.Label(string.Concat("Mode:", GameState.Current.RoomData.GameMode, "/", Singleton<GameStateController>.Instance.CurrentGameMode));
			GUILayout.Label("MatchState:" + GameState.Current.MatchState.CurrentStateId);
			GUILayout.Label("PlayerState:" + GameState.Current.PlayerState.CurrentStateId);

			if (GameState.Current.RoomData.Server != null) {
				GUILayout.Label(string.Concat("Server:", GameState.Current.RoomData.Server, "/", GameState.Current.RoomData.Number));
			}

			GUILayout.Label("IsSpectator:" + GameState.Current.PlayerData.IsSpectator);
			GUILayout.Label("HasJoinedGame:" + GameState.Current.HasJoinedGame);
			GUILayout.Label("IsMatchRunning:" + GameState.Current.IsMatchRunning);
			GUILayout.Label("CameraMode:" + LevelCamera.CurrentMode);
			GUILayout.Space(10f);
			GUILayout.Label("IsInputEnabled:" + AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled);
			GUILayout.Label("lockCursor:" + Screen.lockCursor);
			GUILayout.Label(string.Concat("Mouse:", UserInput.Mouse, " ", UserInput.Rotation));
			GUILayout.Label("KeyState:" + GameState.Current.PlayerData.KeyState);
			GUILayout.Label("MovementState:" + GameState.Current.PlayerData.MovementState);
			GUILayout.Label("IsWalkingEnabled:" + GameState.Current.Player.IsWalkingEnabled);
			GUILayout.Label("WeaponCamera:" + GameState.Current.Player.WeaponCamera.IsEnabled);
			GUILayout.Label("Weapons:" + GameState.Current.Player.EnableWeaponControl);
			GUILayout.Space(10f);
			GUILayout.Label("GameTime:" + GameState.Current.GameTime.ToString("N2"));
			GUILayout.Label("Latency:" + Singleton<GameStateController>.Instance.Client.Peer.RoundTripTime.ToString("N0"));
			GUILayout.EndScrollView();
		}
	}
}
