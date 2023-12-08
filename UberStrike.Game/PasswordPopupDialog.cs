using System;
using UnityEngine;

public class PasswordPopupDialog : BasePopupDialog {
	private string password = string.Empty;

	protected override bool IsOkButtonEnabled {
		get { return !string.IsNullOrEmpty(password); }
	}

	public PasswordPopupDialog(string title, string text, Action<string> ok, Action cancel) {
		var f__this = this;
		Text = text;
		Title = title;
		_alertType = PopupSystem.AlertType.OKCancel;
		_actionType = PopupSystem.ActionType.None;
		_callbackOk = () => ok(password);
		_callbackCancel = cancel;
		_okCaption = "OK";
		_cancelCaption = "Cancel";
		_allowAudio = true;
		_size = new Vector2(320f, 200f);
	}

	protected override void DrawPopupWindow() {
		if (IsOkButtonEnabled && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)) {
			PopupSystem.HideMessage(this);

			if (_callbackOk != null) {
				_callbackOk();
			}
		}

		GUI.Label(new Rect(17f, 55f, _size.x - 34f, 40f), Text, PopupSkin.label);
		GUI.SetNextControlName("JoinPassword");
		password = GUI.PasswordField(new Rect(25f, 100f, _size.x - 50f, 24f), password, '*', 64, BlueStonez.textField);

		if (string.IsNullOrEmpty(password)) {
			GUI.color = Color.white.SetAlpha(0.3f);
			GUI.Label(new Rect(25f, 100f, _size.x - 50f, 24f), "  " + LocalizedStrings.Password, BlueStonez.label_interparkbold_13pt_left);
			GUI.color = Color.white;
		}

		if (GUI.GetNameOfFocusedControl() == string.Empty) {
			GUI.FocusControl("JoinPassword");
		}
	}
}
