using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupSystem : AutoMonoBehaviour<PopupSystem> {
	public enum ActionType {
		None,
		Negative,
		Positive
	}

	public enum AlertType {
		OK,
		OKCancel,
		Cancel,
		None
	}

	private readonly PopupStack<IPopupDialog> _popups = new PopupStack<IPopupDialog>();
	private GuiDepth _lastLockDepth;

	public static bool IsAnyPopupOpen {
		get { return Instance._popups.Count > 0; }
	}

	public static string CurrentPopupName {
		get { return (Instance._popups.Count <= 0) ? string.Empty : Instance._popups.Peek().ToString(); }
	}

	private void OnGUI() {
		ReleaseOldLock();

		if (_popups.Count > 0) {
			var popupDialog = _popups.Peek();
			_lastLockDepth = popupDialog.Depth;
			GUI.depth = (int)_lastLockDepth;
			popupDialog.OnGUI();

			if (Event.current.type == EventType.Layout) {
				GuiLockController.EnableLock(_lastLockDepth);
			}
		}

		GuiManager.DrawTooltip();
	}

	private void ReleaseOldLock() {
		if (Event.current.type == EventType.Layout) {
			if (_popups.Count > 0) {
				if (_lastLockDepth != _popups.Peek().Depth) {
					GuiLockController.ReleaseLock(_lastLockDepth);
				}
			} else {
				GuiLockController.ReleaseLock(_lastLockDepth);
				enabled = false;
			}
		}
	}

	public static void Show(IPopupDialog popup) {
		Instance._popups.Push(popup);
		Instance.enabled = true;
	}

	public static void ShowMessage(string title, string text, AlertType flag, Action ok) {
		ShowMessage(title, text, flag, ok, null);
	}

	public static void ShowError(string title, string text, AlertType flag, Action ok) {
		ShowError(title, text, flag, ok, null);
	}

	public static void ShowMessage(string title, string text, AlertType flag, Action ok, Action cancel) {
		Show(new GeneralPopupDialog(title, text, flag, ok, cancel));
	}

	public static void ShowError(string title, string text, AlertType flag, Action ok, Action cancel) {
		Show(new GeneralPopupDialog(title, text, flag, ok, cancel, false));
	}

	public static IPopupDialog ShowMessage(string title, string text, AlertType flag, Action ok, string okCaption, Action cancel, string cancelCaption, ActionType type) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, flag, ok, okCaption, cancel, cancelCaption, type);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowMessage(string title, string text, AlertType flag, Action ok, string okCaption, Action cancel, string cancelCaption) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, flag, ok, okCaption, cancel, cancelCaption, ActionType.None);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowMessage(string title, string text, AlertType flag, string okCaption, Action ok) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, flag, ok, okCaption);
		Show(popupDialog);

		return popupDialog;
	}

	public static ProgressPopupDialog ShowProgress(string title, string text, Func<float> progress = null) {
		var progressPopupDialog = new ProgressPopupDialog(title, text, progress);
		Show(progressPopupDialog);

		return progressPopupDialog;
	}

	public static IPopupDialog ShowItems(string title, string text, List<IUnityItem> items) {
		IPopupDialog popupDialog = new ItemListPopupDialog(title, text, items);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowItem(IUnityItem item, string customMessage = "") {
		IPopupDialog popupDialog = new ItemListPopupDialog(item, customMessage);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowMessage(string title, string text) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, AlertType.OK);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowMessage(string title, string text, AlertType flag) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, flag);
		Show(popupDialog);

		return popupDialog;
	}

	public static IPopupDialog ShowError(string title, string text, AlertType flag) {
		IPopupDialog popupDialog = new GeneralPopupDialog(title, text, flag);
		Show(popupDialog);

		return popupDialog;
	}

	public static void HideMessage(IPopupDialog dialog) {
		if (dialog != null) {
			Instance._popups.Remove(dialog);
			dialog.OnHide();
		}
	}

	public static void ClearAll() {
		Instance._popups.Clear();
	}

	private static bool IsCurrentPopup(IPopupDialog dialog) {
		return Instance._popups.Count > 0 && Instance._popups.Peek() == dialog;
	}
}
