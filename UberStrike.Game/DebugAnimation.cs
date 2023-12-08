using UnityEngine;

public class DebugAnimation : IDebugPage {
	private CharacterConfig config;

	public string Title {
		get { return "Animation"; }
	}

	public void Draw() {
		GUILayout.BeginHorizontal();

		foreach (var characterConfig in GameState.Current.Avatars.Values) {
			if (GUILayout.Button(characterConfig.name)) {
				config = characterConfig;
			}
		}

		GUILayout.EndHorizontal();

		if (config == null) {
			GUILayout.Label("Select a player");
		} else if (config.Avatar == null) {
			GUILayout.Label("Missing Decorator");
		}
	}
}
