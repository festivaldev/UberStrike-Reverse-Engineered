using System;
using UnityEngine;

public abstract class BasePopupDialog : IPopupDialog {
	protected PopupSystem.ActionType _actionType;
	protected PopupSystem.AlertType _alertType;
	protected bool _allowAudio = true;
	protected Action _callbackCancel;
	protected Action _callbackOk;
	protected string _cancelCaption = string.Empty;
	protected string _okCaption = string.Empty;
	protected Action _onGUIAction;
	protected Vector2 _size = new Vector2(320f, 240f);

	protected virtual bool IsOkButtonEnabled {
		get { return true; }
	}

	public string Text { get; set; }
	public string Title { get; set; }

	public GuiDepth Depth {
		get { return GuiDepth.Popup; }
	}

	public virtual void OnHide() { }

	public void OnGUI() {
		var rect = new Rect((Screen.width - _size.x) * 0.5f, (Screen.height - _size.y - 56f) * 0.5f, _size.x, _size.y);
		GUI.BeginGroup(rect, GUIContent.none, PopupSkin.window);
		GUI.Label(new Rect(0f, 0f, _size.x, 56f), Title, PopupSkin.title);
		DrawPopupWindow();

		switch (_alertType) {
			case PopupSystem.AlertType.OK:
				DoOKButton();

				break;
			case PopupSystem.AlertType.OKCancel:
				DoOKCancelButtons();

				break;
			case PopupSystem.AlertType.Cancel:
				DoCancelButton();

				break;
		}

		GUI.EndGroup();
	}

	public void SetAlertType(PopupSystem.AlertType type) {
		_alertType = type;
	}

	protected abstract void DrawPopupWindow();

	private void DoOKButton() {
		var guistyle = PopupSkin.button;
		var actionType = _actionType;

		if (actionType != PopupSystem.ActionType.Negative) {
			if (actionType == PopupSystem.ActionType.Positive) {
				guistyle = PopupSkin.button_green;
			}
		} else {
			guistyle = PopupSkin.button_red;
		}

		var rect = new Rect((_size.x - 120f) * 0.5f, _size.y - 40f, 120f, 32f);
		var guicontent = new GUIContent((!string.IsNullOrEmpty(_okCaption)) ? _okCaption : LocalizedStrings.OkCaps);
		bool flag;

		if (_allowAudio) {
			flag = GUITools.Button(rect, guicontent, guistyle);
		} else {
			flag = GUI.Button(rect, guicontent, guistyle);
		}

		if (flag) {
			PopupSystem.HideMessage(this);

			if (_callbackOk != null) {
				_callbackOk();
			}
		}
	}

	private void DoOKCancelButtons() {
		var rect = new Rect(_size.x * 0.5f + 5f, _size.y - 40f, 120f, 32f);
		var guicontent = new GUIContent((!string.IsNullOrEmpty(_cancelCaption)) ? _cancelCaption : LocalizedStrings.CancelCaps);
		GUI.color = Color.white;
		bool flag;

		if (_allowAudio) {
			flag = GUITools.Button(rect, guicontent, PopupSkin.button);
		} else {
			flag = GUI.Button(rect, guicontent, PopupSkin.button);
		}

		if (flag) {
			PopupSystem.HideMessage(this);

			if (_callbackCancel != null) {
				_callbackCancel();
			}
		}

		var guistyle = PopupSkin.button;
		var actionType = _actionType;

		if (actionType != PopupSystem.ActionType.Negative) {
			if (actionType == PopupSystem.ActionType.Positive) {
				guistyle = PopupSkin.button_green;
			}
		} else {
			guistyle = PopupSkin.button_red;
		}

		rect = new Rect(_size.x * 0.5f - 125f, _size.y - 40f, 120f, 32f);
		guicontent = new GUIContent((!string.IsNullOrEmpty(_okCaption)) ? _okCaption : LocalizedStrings.OkCaps);
		GUI.enabled = IsOkButtonEnabled;

		if (_allowAudio) {
			flag = GUITools.Button(rect, guicontent, guistyle);
		} else {
			flag = GUI.Button(rect, guicontent, guistyle);
		}

		if (flag) {
			PopupSystem.HideMessage(this);

			if (_callbackOk != null) {
				_callbackOk();
			}
		}
	}

	private void DoCancelButton() {
		var guistyle = PopupSkin.button;
		var actionType = _actionType;

		if (actionType != PopupSystem.ActionType.Negative) {
			if (actionType == PopupSystem.ActionType.Positive) {
				guistyle = PopupSkin.button_green;
			}
		} else {
			guistyle = PopupSkin.button_red;
		}

		var rect = new Rect((_size.x - 120f) * 0.5f, _size.y - 40f, 120f, 32f);
		var guicontent = new GUIContent((!string.IsNullOrEmpty(_cancelCaption)) ? _cancelCaption : LocalizedStrings.CancelCaps);
		bool flag;

		if (_allowAudio) {
			flag = GUITools.Button(rect, guicontent, guistyle);
		} else {
			flag = GUI.Button(rect, guicontent, guistyle);
		}

		if (flag) {
			PopupSystem.HideMessage(this);

			if (_callbackCancel != null) {
				_callbackCancel();
			}
		}
	}
}
