using UnityEngine;

public class DebugPlayersInGame : IDebugPage {
	private int gameId;
	private string gameIdString = "0";
	private Vector2 scroll;

	public string Title {
		get { return "Players"; }
	}

	public void Draw() {
		if (Singleton<GameStateController>.Instance.Client.IsConnected) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Get Players of Game: ", GUILayout.Width(150f));
			gameIdString = GUILayout.TextField(gameIdString, GUILayout.Width(50f));

			if (!int.TryParse(gameIdString, out gameId)) {
				gameIdString = "0";
			}

			GUI.enabled = gameId > 0;
			GUILayout.Space(10f);

			if (GUILayout.Button("Inspect", GUILayout.Width(100f))) {
				Singleton<GameStateController>.Instance.Client.InspectGame(gameId);
			}

			GUILayout.EndHorizontal();
		} else {
			GUILayout.Label("You're not connected to the game lobby");
		}
	}
}
