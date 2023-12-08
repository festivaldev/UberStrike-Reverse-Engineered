using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

internal class DailyPointsPopupDialog : BaseEventPopup {
	private DailyPointsView _points;

	public DailyPointsPopupDialog(DailyPointsView dailypoints) {
		if (dailypoints != null) {
			_points = dailypoints;
		} else {
			_points = new DailyPointsView {
				Current = 700,
				PointsTomorrow = 800,
				PointsMax = 1000
			};
		}

		Width = 500;
		Height = 330;
	}

	protected override void DrawGUI(Rect rect) {
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, -20f, rect.width + 100f, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 50f), "Daily Reward", BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		var num = 230;
		GUI.DrawTexture(new Rect((rect.width - num) / 2f, rect.height - num - 25f, num, num), ShopIcons.Points48x48);
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, 25f, rect.width + 100f, 120f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 35f, rect.width, 100f), _points.Current + " POINTS", BlueStonez.label_interparkbold_48pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		GUITools.OutlineLabel(new Rect(0f, rect.height - 50f, rect.width, 50f), string.Format("Come back tomorrow for {0} points!", GetPointsForTomorrow()), BlueStonez.label_interparkbold_13pt, 1, new Color(0.9f, 0.9f, 0.9f), ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
	}

	private int GetPointsForTomorrow() {
		return _points.PointsTomorrow;
	}
}
