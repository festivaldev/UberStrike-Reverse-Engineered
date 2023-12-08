using UnityEngine;

public class DebugGames : IDebugPage {
	private Vector2 scroll;

	public string Title {
		get { return "Games"; }
	}

	public void Draw() {
		if (Singleton<GameStateController>.Instance.Client.IsConnected) {
			if (Singleton<GameStateController>.Instance.Client.IsConnectedToLobby) {
				scroll = GUILayout.BeginScrollView(scroll);

				foreach (var gameRoomData in Singleton<GameListManager>.Instance.GameList) {
					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Concat("[ID: ", gameRoomData.Number, "] [Name: ", gameRoomData.Name, "] [Players: ", gameRoomData.ConnectedPlayers, "/", gameRoomData.PlayerLimit, "] [Time: ", gameRoomData.TimeLimit, "]"));

					if (GUILayout.Button("Close", GUILayout.Width(200f))) {
						Singleton<GameStateController>.Instance.Client.CloseGame(gameRoomData.Number);
					}

					GUILayout.EndHorizontal();
				}

				GUILayout.EndScrollView();
			} else {
				GUILayout.FlexibleSpace();
				GUILayout.Label("Reconnect to the game lobby");

				if (GUILayout.Button(LocalizedStrings.Refresh, BlueStonez.buttondark_medium)) {
					Singleton<GameStateController>.Instance.Client.RefreshGameLobby();
				}

				GUILayout.FlexibleSpace();
			}
		} else {
			GUILayout.FlexibleSpace();
			GUILayout.Label("You're not connected to a game server");
			GUILayout.FlexibleSpace();
		}
	}
}
