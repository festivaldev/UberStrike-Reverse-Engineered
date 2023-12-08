using System;
using UnityEngine;

public class ProgressPopupDialog : GeneralPopupDialog {
	private Func<float> _progress;
	public float Progress { get; set; }

	public ProgressPopupDialog(string title, string text, Func<float> progress = null) : base(title, text) {
		_progress = progress;
	}

	protected override void DrawPopupWindow() {
		GUI.Label(new Rect(17f, 95f, _size.x - 34f, 32f), Text, BlueStonez.label_interparkbold_11pt);

		if (_progress != null) {
			DrawLevelBar(new Rect(17f, 125f, _size.x - 34f, 16f), _progress(), ColorScheme.ProgressBar);
		} else {
			DrawLevelBar(new Rect(17f, 125f, _size.x - 34f, 16f), Progress, ColorScheme.ProgressBar);
		}
	}

	private void DrawLevelBar(Rect position, float amount, Color barColor) {
		GUI.BeginGroup(position);
		GUI.Label(new Rect(0f, 0f, position.width, 12f), GUIContent.none, BlueStonez.progressbar_background);
		GUI.color = barColor;
		GUI.Label(new Rect(2f, 2f, (position.width - 4f) * Mathf.Clamp01(amount), 8f), string.Empty, BlueStonez.progressbar_thumb);
		GUI.color = Color.white;
		GUI.EndGroup();
	}

	public void SetCancelable(Action action) {
		_callbackCancel = action;
		_cancelCaption = LocalizedStrings.Cancel;
		_alertType = ((action == null) ? PopupSystem.AlertType.None : PopupSystem.AlertType.Cancel);
		_actionType = PopupSystem.ActionType.None;
	}
}
