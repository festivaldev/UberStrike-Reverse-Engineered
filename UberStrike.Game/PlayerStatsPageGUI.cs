using UberStrike.Core.Models;
using UnityEngine;

internal class PlayerStatsPageGUI : PageGUI {
	private ValuablePlayerDetailGUI _playerDetailGui;
	private ValuablePlayerListGUI _playerListGui;

	private void Awake() {
		_playerDetailGui = new ValuablePlayerDetailGUI();
		_playerListGui = new ValuablePlayerListGUI();
		_playerListGui.OnSelectionChange = OnValuablePlayerListSelectionChange;
	}

	public override void DrawGUI(Rect rect) {
		var num = Mathf.Min(_playerListGui.Height, rect.height - 402f) - 2f;
		var num2 = Mathf.Min(_playerListGui.Height, rect.height - 402f);
		GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
		_playerListGui.Draw(new Rect(2f, 2f, rect.width - 4f, num));
		_playerDetailGui.Draw(new Rect(2f, num2 + 2f, 200f, 200f));
		DrawStats(new Rect(202f, num2 + 2f, rect.width - 200f - 4f, 200f));
		DrawRewards(new Rect(2f, num2 + 202f, rect.width - 4f, 200f));
		GUI.EndGroup();
	}

	private void DrawStats(Rect rect) {
		GUI.Button(new Rect(rect.x, rect.y, rect.width, 40f), string.Empty, BlueStonez.box_grey50);
		GUI.Label(new Rect(rect.x + 10f, rect.y + 2f, rect.width, 40f), "MY STATUS", BlueStonez.label_interparkbold_18pt_left);
		var width = rect.width;
		var num = 32f;
		GUI.BeginGroup(new Rect(rect.x, rect.y + 40f, rect.width, rect.height - 40f), GUIContent.none, BlueStonez.window);
		GUI.Label(new Rect(5f, num * 0f, width + 1f, num), new GUIContent(LocalizedStrings.PlayTime, UberstrikeIcons.Time20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 0f, width - 5f, num), GameState.Current.Statistics.PlayTime, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 1f, width + 1f, num), new GUIContent(LocalizedStrings.Kills, ShopIcons.Stats1Kills20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 1f, width - 5f, num), GameState.Current.Statistics.Kills, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 2f, width + 1f, num), new GUIContent(LocalizedStrings.Headshot, ShopIcons.Stats3Headshots20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 2f, width - 5f, num), GameState.Current.Statistics.Headshots, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 3f, width + 1f, num), new GUIContent(LocalizedStrings.Nutshot, ShopIcons.Stats4Nutshots20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 3f, width - 5f, num), GameState.Current.Statistics.Nutshots, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 4f, width + 1f, num), new GUIContent(LocalizedStrings.Smackdown, ShopIcons.Stats2Smackdowns20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 4f, width - 5f, num), GameState.Current.Statistics.Smackdowns, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 5f, width + 1f, num), new GUIContent(LocalizedStrings.DeathsCaps, ShopIcons.Stats6Deaths20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 5f, width - 5f, num), GameState.Current.Statistics.Deaths, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 6f, width + 1f, num), new GUIContent(LocalizedStrings.KDR, ShopIcons.Stats7Kdr20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 6f, width - 5f, num), GameState.Current.Statistics.KDR, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, num * 7f, width + 1f, num), new GUIContent(LocalizedStrings.SuicideXP, ShopIcons.Stats8Suicides20x20), BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, num * 7f, width - 5f, num), GameState.Current.Statistics.Suicides, BlueStonez.label_interparkbold_18pt_right);
		GUI.EndGroup();
	}

	private void DrawRewards(Rect rect) {
		GUI.Button(new Rect(rect.x, rect.y, rect.width, 40f), string.Empty, BlueStonez.box_grey50);
		GUI.Label(new Rect(rect.x + 10f, rect.y + 2f, rect.width, 40f), "MY REWARDS", BlueStonez.label_interparkbold_18pt_left);
		var num = rect.height - 40f;
		GUI.BeginGroup(new Rect(rect.x, rect.y + 40f, rect.width, num), GUIContent.none, BlueStonez.window);
		GUI.Label(new Rect(5f, 0f, rect.width, 32f), LocalizedStrings.PlayTime, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, 0f, rect.width - 100f, 32f), new GUIContent(GameState.Current.Statistics.PlayTimeXp, UberstrikeIcons.IconXP20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(0f, 0f, rect.width, 32f), new GUIContent(GameState.Current.Statistics.PlayTimePts, ShopIcons.IconPoints20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, 32f, rect.width, 32f), LocalizedStrings.SkillBonus, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, 32f, rect.width - 100f, 32f), new GUIContent(GameState.Current.Statistics.SkillBonusXp, UberstrikeIcons.IconXP20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(0f, 32f, rect.width, 32f), new GUIContent(GameState.Current.Statistics.SkillBonusPts, ShopIcons.IconPoints20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, 64f, rect.width, 32f), LocalizedStrings.Boost, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, 64f, rect.width - 100f, 32f), new GUIContent(GameState.Current.Statistics.BoostXp, UberstrikeIcons.IconXP20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(0f, 64f, rect.width, 32f), new GUIContent(GameState.Current.Statistics.BoostPts, ShopIcons.IconPoints20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(5f, 96f, rect.width, 32f), LocalizedStrings.TOTAL, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(0f, 96f, rect.width - 100f, 32f), new GUIContent(GameState.Current.Statistics.TotalXp, UberstrikeIcons.IconXP20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(0f, 96f, rect.width, 32f), new GUIContent(GameState.Current.Statistics.TotalPts, ShopIcons.IconPoints20x20), BlueStonez.label_interparkbold_18pt_right);
		GUI.EndGroup();
	}

	private void OnValuablePlayerListSelectionChange(StatsSummary playerStats) {
		_playerDetailGui.SetValuablePlayer(playerStats);
	}

	private void OnEnable() {
		if (GameState.Current.Statistics.Data.MostValuablePlayers != null && GameState.Current.Statistics.Data.MostValuablePlayers.Count > 0) {
			_playerListGui.SetSelection(0);
		} else {
			_playerDetailGui.SetValuablePlayer(null);
		}

		_playerListGui.Enabled = true;
	}

	private void OnDisabled() {
		_playerListGui.Enabled = false;
		_playerListGui.ClearSelection();
		_playerDetailGui.StopBadgeShow();
	}
}
