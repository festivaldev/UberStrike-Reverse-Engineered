using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu {
	private const int Height = 24;
	private const int Width = 105;
	private List<MenuItem> _items;
	private Rect _position;
	private CommUser _selectedUser;

	public CommUser SelectedUser {
		get { return _selectedUser; }
	}

	public InstantMessage msg { get; set; }
	public static PopupMenu Current { get; private set; }

	public static bool IsEnabled {
		get { return Current != null; }
	}

	public PopupMenu() {
		_items = new List<MenuItem>();
	}

	public void AddMenuCopyItem(string caption, Action<CommUser, InstantMessage> action, Func<CommUser, bool> isEnabledForUser) {
		var menuItem = new MenuItem {
			Caption = caption,
			CopyMsgCallback = action,
			CopyMsg = action,
			CheckItem = isEnabledForUser
		};

		_items.Add(menuItem);
	}

	public void AddMenuCopyPlayerName(string caption, Action<CommUser, InstantMessage> action, Func<CommUser, bool> isEnabledForUser) {
		var menuItem = new MenuItem {
			Caption = caption,
			CopyPlayerName = action,
			CopyMsg = action,
			CheckItem = isEnabledForUser
		};

		_items.Add(menuItem);
	}

	public void AddMenuItem(Func<CommUser, string> caption, Action<CommUser> action, Func<CommUser, bool> isEnabledForUser) {
		var menuItem = new MenuItem {
			DynamicCaption = caption,
			Caption = string.Empty,
			Callback = action,
			CheckItem = isEnabledForUser
		};

		_items.Add(menuItem);
	}

	public void AddMenuItem(string caption, Action<CommUser> action, Func<CommUser, bool> isEnabledForUser) {
		var menuItem = new MenuItem {
			Caption = caption,
			Callback = action,
			CheckItem = isEnabledForUser
		};

		_items.Add(menuItem);
	}

	private void Configure() {
		foreach (var menuItem in _items) {
			menuItem.Enabled = menuItem.CheckItem(_selectedUser);

			if (menuItem.DynamicCaption != null) {
				menuItem.Caption = menuItem.DynamicCaption(_selectedUser);
			}
		}
	}

	public static void Hide() {
		Current = null;
	}

	public void Show(Vector2 screenPos, CommUser user) {
		Show(screenPos, user, this);
	}

	public static void Show(Vector2 screenPos, CommUser user, PopupMenu menu) {
		if (menu != null) {
			menu._selectedUser = user;
			menu.Configure();
			menu._position.height = 24 * menu._items.FindAll(i => i.Enabled).Count;
			menu._position.width = 105f;
			menu._position.x = screenPos.x - 1f;

			if (screenPos.y + menu._position.height > Screen.height) {
				menu._position.y = screenPos.y - menu._position.height + 1f;
			} else {
				menu._position.y = screenPos.y - 1f;
			}

			Current = menu;
		}
	}

	public void Draw() {
		GUI.BeginGroup(new Rect(_position.x, _position.y, _position.width, _position.height + 6f), BlueStonez.window);
		GUI.Label(new Rect(1f, 1f, _position.width - 2f, _position.height + 4f), GUIContent.none, BlueStonez.box_grey50);
		GUI.Label(new Rect(0f, 0f, _position.width, _position.height + 6f), GUIContent.none, BlueStonez.box_grey50);
		GUITools.PushGUIState();
		var num = 0;

		foreach (var menuItem in _items) {
			if (menuItem.Enabled) {
				if (menuItem.CopyMsgCallback != null) {
					GUI.enabled = menuItem.CopyMsgCallback != null;
				} else {
					GUI.enabled = menuItem.Callback != null;
				}

				GUI.Label(new Rect(8f, 8 + num * 24, _position.width - 8f, 24f), menuItem.Caption, BlueStonez.label_interparkmed_11pt_left);

				if (menuItem.Callback != null && GUI.Button(new Rect(2f, 3 + num * 24, _position.width - 4f, 24f), GUIContent.none, BlueStonez.dropdown_list)) {
					Current = null;
					menuItem.Callback(_selectedUser);
				} else if (menuItem.CopyMsgCallback != null && GUI.Button(new Rect(2f, 3 + num * 24, _position.width - 4f, 24f), GUIContent.none, BlueStonez.dropdown_list)) {
					Current = null;
					menuItem.CopyMsgCallback(_selectedUser, msg);
				}

				num++;
			}
		}

		GUITools.PopGUIState();
		GUI.EndGroup();

		if (Event.current.type == EventType.MouseUp && !_position.Contains(Event.current.mousePosition)) {
			Current = null;
		}
	}

	private class MenuItem {
		public Action<CommUser> Callback;
		public string Caption;
		public Func<CommUser, bool> CheckItem;
		public Action<CommUser, InstantMessage> CopyMsg;
		public Action<CommUser, InstantMessage> CopyMsgCallback;
		public Action<CommUser, InstantMessage> CopyPlayerName;
		public Func<CommUser, string> DynamicCaption;
		public bool Enabled;
	}
}
