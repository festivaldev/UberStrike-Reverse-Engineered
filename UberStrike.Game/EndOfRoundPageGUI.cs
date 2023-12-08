using UberStrike.Core.Models;
using UnityEngine;

public class EndOfRoundPageGUI : PageGUI {
	private const float WeaponRecommendHeight = 265f;
	private ValuablePlayerDetailGUI _playerDetailGui;
	private ValuablePlayerListGUI _playerListGui;

	public override void DrawGUI(Rect rect) {
		var num = Mathf.Min(_playerListGui.Height, rect.height - 265f) - 2f;
		var num2 = Mathf.Min(_playerListGui.Height, rect.height - 265f);
		GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
		_playerListGui.Draw(new Rect(2f, 2f, rect.width - 4f, num));
		DrawDetails(new Rect(2f, 2f + num2, rect.width - 4f, 265f));
		GUI.EndGroup();
	}

	private void Awake() {
		_playerListGui = new ValuablePlayerListGUI();
		_playerDetailGui = new ValuablePlayerDetailGUI();
		_playerListGui.OnSelectionChange = OnValuablePlayerListSelectionChange;
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

	private void DrawDetails(Rect rect) {
		GUI.BeginGroup(rect);
		GUI.Label(new Rect(0f, 2f, rect.width, 20f), LocalizedStrings.RecommendedLoadoutCaps, BlueStonez.label_interparkbold_18pt);
		rect = new Rect(0f, 25f, rect.width, rect.height - 25f);
		GUI.BeginGroup(rect);
		_playerDetailGui.Draw(new Rect(0f, 0f, 200f, rect.height));
		GUI.EndGroup();
		GUI.EndGroup();
	}

	private void OnRecomListSelectionChange(IUnityItem item, RecommendType type) {
		_playerListGui.ClearSelection();
		_playerDetailGui.StopBadgeShow();
	}

	private void OnValuablePlayerListSelectionChange(StatsSummary playerStats) {
		_playerDetailGui.SetValuablePlayer(playerStats);
	}
}
