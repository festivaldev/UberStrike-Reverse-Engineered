using UnityEngine;

public class DebugMaps : IDebugPage {
	private Vector2 scroll;

	public string Title {
		get { return "Maps"; }
	}

	public void Draw() {
		scroll = GUILayout.BeginScrollView(scroll);

		foreach (var uberstrikeMap in Singleton<MapManager>.Instance.AllMaps) {
			GUILayout.Label(string.Concat(uberstrikeMap.Id, ", Modes: ", uberstrikeMap.View.SupportedGameModes, ", Item: ", uberstrikeMap.View.RecommendedItemId, ", Scene: ", uberstrikeMap.SceneName));
		}

		GUILayout.EndScrollView();
	}
}
