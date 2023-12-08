using UnityEngine;

public class DebugProjectiles : IDebugPage {
	private Vector2 scroll1;
	private Vector2 scroll2;

	public string Title {
		get { return "Projectiles"; }
	}

	public void Draw() {
		scroll1 = GUILayout.BeginScrollView(scroll1);

		foreach (var keyValuePair in Singleton<ProjectileManager>.Instance.AllProjectiles) {
			GUILayout.Label((keyValuePair.Key + " - " + keyValuePair.Value == null) ? (ProjectileManager.PrintID(keyValuePair.Key) + " (exploded zombie)") : ProjectileManager.PrintID(keyValuePair.Key));
		}

		GUILayout.EndScrollView();
		GUILayout.Space(30f);
		scroll2 = GUILayout.BeginScrollView(scroll2);

		foreach (var num in Singleton<ProjectileManager>.Instance.LimitedProjectiles) {
			GUILayout.Label("Limited " + ProjectileManager.PrintID(num));
		}

		GUILayout.EndScrollView();
	}
}
