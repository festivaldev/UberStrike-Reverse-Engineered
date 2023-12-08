using UnityEngine;

public class FriendRequestPanelGUI : PanelGuiBase {
	private const string FocusReceiver = "Message Receiver";
	private const string FocusContent = "Message Content";
	private Vector2 _friendDropdownScroll;
	private float _keyboardOffset;
	private string _lastMsgRcvName;
	private string _msgContent;
	private int _msgRcvCmid;
	private string _msgReceiver;
	private float _rcvDropdownHeight;
	private float _rcvDropdownWidth;
	private int _receiverCount;
	private bool _showComposeMessage;
	private bool _showReceiverDropdownList;
	private float _targetKeyboardOffset;
	private bool _useFixedReceiver;

	private void OnGUI() {
		if (Mathf.Abs(_keyboardOffset - _targetKeyboardOffset) > 2f) {
			_keyboardOffset = Mathf.Lerp(_keyboardOffset, _targetKeyboardOffset, Time.deltaTime * 4f);
		} else {
			_keyboardOffset = _targetKeyboardOffset;
		}

		if (!_showComposeMessage) {
			return;
		}

		GUI.depth = 3;
		GUI.skin = BlueStonez.Skin;
		var rect = new Rect((Screen.width - 480) / 2, (Screen.height - 320) / 2 - _keyboardOffset, 480f, 300f);
		GUI.Box(rect, GUIContent.none, BlueStonez.window);
		DoCompose(rect);

		if (_showReceiverDropdownList) {
			DoReceiverDropdownList(rect);
		}

		_rcvDropdownHeight = Mathf.Lerp(_rcvDropdownHeight, (!_showReceiverDropdownList) ? 0 : 146, Time.deltaTime * 9f);

		if (!_showReceiverDropdownList && Mathf.Approximately(_rcvDropdownHeight, 0f)) {
			_rcvDropdownHeight = 0f;
		}

		GUI.enabled = true;
	}

	private void HideKeyboard() { }

	private void DoCompose(Rect rect) {
		var rect2 = new Rect(rect.x + (rect.width - 480f) / 2f, rect.y + (rect.height - 300f) / 2f, 480f, 290f);
		GUI.BeginGroup(rect2, BlueStonez.window);
		var num = 35;
		var num2 = 120;
		var num3 = 320;
		var num4 = 70;
		var num5 = 100;
		GUI.Label(new Rect(0f, 0f, rect2.width, 0f), LocalizedStrings.FriendRequestCaps, BlueStonez.tab_strip);
		GUI.Box(new Rect(12f, 55f, rect2.width - 24f, rect2.height - 101f), GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.Label(new Rect(num, num4, 75f, 20f), LocalizedStrings.To, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(num, num5, 75f, 20f), LocalizedStrings.Message, BlueStonez.label_interparkbold_18pt_right);
		var enabled = GUI.enabled;
		GUI.enabled = enabled && !_useFixedReceiver;
		GUI.SetNextControlName("Message Receiver");
		_msgReceiver = GUI.TextField(new Rect(num2, num4, num3, 24f), _msgReceiver, BlueStonez.textField);

		if (string.IsNullOrEmpty(_msgReceiver) && !GUI.GetNameOfFocusedControl().Equals("Message Receiver")) {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(num2, num4, num3, 24f), " " + LocalizedStrings.StartTypingTheNameOfAFriend, BlueStonez.label_interparkbold_11pt_left);
			GUI.color = Color.white;
		}

		GUI.enabled = enabled && !_showReceiverDropdownList;
		GUI.SetNextControlName("Message Content");
		_msgContent = GUI.TextArea(new Rect(num2, num5, num3, 108f), _msgContent, 140, BlueStonez.textArea);
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.Label(new Rect(num2, num5 + 110, num3, 24f), (140 - _msgContent.Length).ToString(), BlueStonez.label_interparkbold_11pt_right);
		GUI.color = Color.white;
		GUI.enabled = enabled && !_showReceiverDropdownList && _msgRcvCmid != 0 && !string.IsNullOrEmpty(_msgContent);

		if (GUITools.Button(new Rect(rect2.width - 95f - 100f, rect2.height - 44f, 90f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_green)) {
			Singleton<CommsManager>.Instance.SendFriendRequest(_msgRcvCmid, _msgContent);
			_msgContent = string.Empty;
			_msgReceiver = string.Empty;
			_msgRcvCmid = 0;
			HideKeyboard();
			Hide();
		}

		GUI.enabled = enabled;

		if (GUITools.Button(new Rect(rect2.width - 100f, rect2.height - 44f, 90f, 32f), new GUIContent(LocalizedStrings.DiscardCaps), BlueStonez.button)) {
			HideKeyboard();
			Hide();
		}

		if (!_showReceiverDropdownList && GUI.GetNameOfFocusedControl().Equals("Message Receiver")) {
			_showReceiverDropdownList = true;
			_lastMsgRcvName = _msgReceiver;
			_msgReceiver = string.Empty;
		}

		GUI.EndGroup();

		if (_showReceiverDropdownList) {
			DoReceiverDropdownList(rect);
		}
	}

	private void DoReceiverDropdownList(Rect rect) {
		var rect2 = new Rect(rect.x + 120f, rect.y + 94f, 320f, _rcvDropdownHeight);
		GUI.BeginGroup(rect2, BlueStonez.window);

		if (Singleton<PlayerDataManager>.Instance.FriendsCount > 0) {
			var num = 0;
			_friendDropdownScroll = GUITools.BeginScrollView(new Rect(0f, 0f, rect2.width, rect2.height), _friendDropdownScroll, new Rect(0f, 0f, _rcvDropdownWidth, _receiverCount * 24));

			foreach (var publicProfileView in Singleton<PlayerDataManager>.Instance.MergedFriends) {
				if (_msgReceiver.Length <= 0 || publicProfileView.Name.ToLower().Contains(_msgReceiver.ToLower())) {
					var rect3 = new Rect(0f, num * 24, rect2.width, 24f);

					if (GUI.enabled && rect3.Contains(Event.current.mousePosition) && GUI.Button(rect3, GUIContent.none, BlueStonez.box_grey50)) {
						_msgRcvCmid = publicProfileView.Cmid;
						_msgReceiver = publicProfileView.Name;
						_showReceiverDropdownList = false;
						GUI.FocusControl("Message Content");
					}

					GUI.Label(new Rect(8f, num * 24 + 4, rect2.width, rect2.height), publicProfileView.Name, BlueStonez.label_interparkmed_11pt_left);
					num++;
				}
			}

			_receiverCount = num;

			if (_receiverCount * 24 > rect2.height) {
				_rcvDropdownWidth = rect2.width - 22f;
			} else {
				_rcvDropdownWidth = rect2.width - 8f;
			}

			GUITools.EndScrollView();
		} else {
			GUI.Label(new Rect(0f, 0f, rect2.width, rect2.height), LocalizedStrings.YouHaveNoFriends, BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();

		if (Event.current.type == EventType.MouseDown && !rect2.Contains(Event.current.mousePosition)) {
			_showReceiverDropdownList = false;

			if (_msgRcvCmid == 0) {
				_msgReceiver = _lastMsgRcvName;
			}
		}
	}

	public override void Show() {
		base.Show();
		_msgRcvCmid = 0;
		_msgContent = string.Empty;
		_msgReceiver = string.Empty;
		_showComposeMessage = true;
		_showReceiverDropdownList = false;
		_useFixedReceiver = false;
		GUI.FocusControl("Message Receiver");
	}

	public override void Hide() {
		base.Hide();
		_showComposeMessage = false;
		_showReceiverDropdownList = false;
	}

	public void SelectReceiver(int cmid, string name) {
		_useFixedReceiver = true;
		_msgRcvCmid = cmid;
		_msgReceiver = name;
		GUI.FocusControl("Message Content");
	}
}
