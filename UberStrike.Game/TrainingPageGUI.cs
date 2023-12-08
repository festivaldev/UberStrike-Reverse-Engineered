using UberStrike.Core.Models;
using UnityEngine;

public class TrainingPageGUI : MonoBehaviour {
	private const int PageWidth = 700;
	private const int PageHeight = 480;
	private const int MapsPerRow = 4;
	private Vector2 _mapScroll;

	private void OnGUI() {
		GUI.depth = 11;
		GUI.skin = BlueStonez.Skin;
		GUI.BeginGroup(new Rect((Screen.width - 700) * 0.5f, (Screen.height - GlobalUIRibbon.Instance.Height() - 480) * 0.5f, 700f, 480f), string.Empty, BlueStonez.window);
		GUI.Label(new Rect(10f, 20f, 670f, 48f), LocalizedStrings.ExploreMaps, BlueStonez.label_interparkbold_48pt);
		GUI.Label(new Rect(30f, 50f, 640f, 120f), LocalizedStrings.TrainingModeDesc, BlueStonez.label_interparkbold_13pt);
		GUI.Box(new Rect(12f, 160f, 670f, 20f), string.Empty, BlueStonez.box_grey50);
		GUI.Label(new Rect(16f, 160f, 120f, 20f), LocalizedStrings.ChooseAMap, BlueStonez.label_interparkbold_18pt_left);
		var num = 280;
		GUI.Box(new Rect(12f, 179f, 670f, num), string.Empty, BlueStonez.window);
		var num2 = 0;

		if (Singleton<MapManager>.Instance.Count > 0) {
			num2 = (Singleton<MapManager>.Instance.Count - 1) / 4 + 1;
		}

		_mapScroll = GUITools.BeginScrollView(new Rect(0f, 179f, 682f, num), _mapScroll, new Rect(0f, 0f, 655f, 10 + 80 * num2));
		var vector = new Vector2(163f, 80f);
		var num3 = 0;

		foreach (var uberstrikeMap in Singleton<MapManager>.Instance.AllMaps) {
			if (uberstrikeMap.IsVisible) {
				var white = Color.white;
				var num4 = num3 / 4;
				var num5 = num3 % 4;
				var rect = new Rect(13f + num5 * vector.Width(), num4 * vector.y + 4f, vector.x, vector.y);

				if (GUI.Button(rect, string.Empty, BlueStonez.gray_background) && !GUITools.IsScrolling && !Singleton<SceneLoader>.Instance.IsLoading && uberstrikeMap != null) {
					Singleton<MapManager>.Instance.LoadMap(uberstrikeMap, delegate {
						Singleton<GameStateController>.Instance.SetGameMode(new TrainingRoom());
						GameState.Current.Actions.JoinTeam(TeamID.NONE);
					});
				}

				GUI.BeginGroup(rect);
				uberstrikeMap.Icon.Draw(rect.CenterHorizontally(2f, 100f, 64f));
				var vector2 = BlueStonez.label_interparkbold_11pt.CalcSize(new GUIContent(uberstrikeMap.Name));
				GUI.contentColor = white;
				GUI.Label(rect.CenterHorizontally(rect.height - vector2.y, vector2.x, vector2.y), uberstrikeMap.Name, BlueStonez.label_interparkbold_11pt);
				GUI.contentColor = Color.white;
				GUI.EndGroup();
				num3++;
			}
		}

		GUITools.EndScrollView();
		GUI.EndGroup();
		GUI.enabled = true;
	}
}
