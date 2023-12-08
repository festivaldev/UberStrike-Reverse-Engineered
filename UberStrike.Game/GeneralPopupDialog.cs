using System;
using UnityEngine;

public class GeneralPopupDialog : BasePopupDialog {
	public GeneralPopupDialog(string title, string text, PopupSystem.AlertType flag, Action ok, string okCaption, Action cancel, string cancelCaption, PopupSystem.ActionType actionType, bool allowAudio = true) {
		Text = text;
		Title = title;
		_alertType = flag;
		_actionType = actionType;
		_callbackOk = ok;
		_callbackCancel = cancel;
		_okCaption = okCaption;
		_cancelCaption = cancelCaption;
		_allowAudio = allowAudio;
	}

	public GeneralPopupDialog(string title, string text) : this(title, text, PopupSystem.AlertType.None, null, string.Empty, null, string.Empty, PopupSystem.ActionType.None) { }
	public GeneralPopupDialog(string title, string text, PopupSystem.AlertType flag, bool allowAudio = true) : this(title, text, flag, null, string.Empty, null, string.Empty, PopupSystem.ActionType.None, allowAudio) { }
	public GeneralPopupDialog(string title, string text, PopupSystem.AlertType flag, Action ok, string okCaption, bool allowAudio = true) : this(title, text, flag, ok, okCaption, null, string.Empty, PopupSystem.ActionType.None, allowAudio) { }
	public GeneralPopupDialog(string title, string text, PopupSystem.AlertType flag, Action action, bool allowAudio = true) : this(title, text, flag, action, string.Empty, null, string.Empty, PopupSystem.ActionType.None, allowAudio) { }
	public GeneralPopupDialog(string title, string text, PopupSystem.AlertType flag, Action ok, Action cancel, bool allowAudio = true) : this(title, text, flag, ok, string.Empty, cancel, string.Empty, PopupSystem.ActionType.None, allowAudio) { }

	protected override void DrawPopupWindow() {
		GUI.Label(new Rect(17f, 55f, _size.x - 34f, _size.y - 100f), Text, PopupSkin.label);
	}
}
