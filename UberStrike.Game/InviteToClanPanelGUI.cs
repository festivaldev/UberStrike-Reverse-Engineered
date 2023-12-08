using Cmune.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class InviteToClanPanelGUI : PanelGuiBase {
	private int _cmid;
	private bool _fixReceiver;
	private Vector2 _friendListScroll;
	private string _message = string.Empty;
	private string _name = string.Empty;
	private float _receiverDropdownHeight;
	private bool _showReceiverDropdownList;

	private void OnGUI() {
		DrawInvitePlayerMessage(new Rect(0f, GlobalUIRibbon.Instance.Height(), Screen.width, Screen.height - GlobalUIRibbon.Instance.Height()));
	}

	private void DrawInvitePlayerMessage(Rect rect) {
		GUI.depth = 3;
		GUI.enabled = true;
		var rect2 = new Rect(rect.x + (rect.width - 480f) / 2f, rect.y + (rect.height - 320f) / 2f, 480f, 320f);
		GUI.BeginGroup(rect2, BlueStonez.window);
		var num = 25;
		var num2 = 120;
		var num3 = 320;
		var num4 = 70;
		var num5 = 100;
		var num6 = 132;
		GUI.Label(new Rect(0f, 0f, rect2.width, 0f), LocalizedStrings.InvitePlayer, BlueStonez.tab_strip);
		GUI.Label(new Rect(12f, 55f, rect2.width - 24f, 208f), GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.Label(new Rect(num, num4, 400f, 20f), LocalizedStrings.UseThisFormToSendClanInvitations, BlueStonez.label_interparkbold_11pt);
		GUI.Label(new Rect(num, num5, 90f, 20f), LocalizedStrings.PlayerCaps, BlueStonez.label_interparkbold_18pt_right);
		GUI.Label(new Rect(num, num6, 90f, 20f), LocalizedStrings.MessageCaps, BlueStonez.label_interparkbold_18pt_right);
		GUI.SetNextControlName("Message Receiver");
		GUI.enabled = !_fixReceiver;
		_name = GUI.TextField(new Rect(num2, num5, num3, 24f), _name, BlueStonez.textField);

		if (string.IsNullOrEmpty(_name) && !GUI.GetNameOfFocusedControl().Equals("Message Receiver")) {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(num2, num5, num3, 24f), " " + LocalizedStrings.StartTypingTheNameOfAFriend, BlueStonez.label_interparkbold_11pt_left);
			GUI.color = Color.white;
		}

		GUI.enabled = !_showReceiverDropdownList;
		GUI.SetNextControlName("Description");
		_message = GUI.TextArea(new Rect(num2, num6, num3, 108f), _message, BlueStonez.textArea);
		_message = _message.Trim('\n', '\t');
		GUI.enabled = _cmid != 0;

		if (GUITools.Button(new Rect(rect2.width - 155f - 155f, rect2.height - 44f, 150f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_green)) {
			ClanWebServiceClient.InviteMemberToJoinAGroup(PlayerDataManager.ClanID, PlayerDataManager.AuthToken, _cmid, _message, delegate { }, delegate { });
			PanelManager.Instance.ClosePanel(PanelType.ClanRequest);
		}

		GUI.enabled = true;

		if (GUITools.Button(new Rect(rect2.width - 155f, rect2.height - 44f, 150f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button)) {
			_message = string.Empty;
			PanelManager.Instance.ClosePanel(PanelType.ClanRequest);
		}

		if (!_fixReceiver) {
			if (!_showReceiverDropdownList && GUI.GetNameOfFocusedControl().Equals("Message Receiver")) {
				_cmid = 0;
				_showReceiverDropdownList = true;
			}

			if (_showReceiverDropdownList) {
				DoReceiverDropdownList(new Rect(num2, num5 + 24, num3, _receiverDropdownHeight));
			}
		}

		GUI.EndGroup();
	}

	private void Update() {
		_receiverDropdownHeight = Mathf.Lerp(_receiverDropdownHeight, (!_showReceiverDropdownList) ? 0 : 146, Time.deltaTime * 9f);

		if (!_showReceiverDropdownList && Mathf.Approximately(_receiverDropdownHeight, 0f)) {
			_receiverDropdownHeight = 0f;
		}
	}

	private void DoReceiverDropdownList(Rect rect) {
		GUI.BeginGroup(rect, BlueStonez.window);
		var num = -1;

		if (Singleton<PlayerDataManager>.Instance.FriendsCount > 0) {
			var num2 = 0;
			var num3 = 0;
			_friendListScroll = GUITools.BeginScrollView(new Rect(0f, 0f, rect.width, rect.height), _friendListScroll, new Rect(0f, 0f, rect.width - 20f, Singleton<PlayerDataManager>.Instance.FriendsCount * 24));

			foreach (var publicProfileView in Singleton<PlayerDataManager>.Instance.MergedFriends) {
				if (_name.Length <= 0 || publicProfileView.Name.ToLower().Contains(_name.ToLower())) {
					if (num == -1) {
						num = num3;
					}

					var flag = PlayerDataManager.IsClanMember(publicProfileView.Cmid);
					var rect2 = new Rect(0f, num2 * 24, rect.width, 24f);

					if (GUI.enabled && rect2.Contains(Event.current.mousePosition) && GUI.Button(rect2, GUIContent.none, BlueStonez.box_grey50) && !flag) {
						_cmid = publicProfileView.Cmid;
						_name = publicProfileView.Name;
						_showReceiverDropdownList = false;
						GUI.FocusControl("Description");
					}

					var text = ((!string.IsNullOrEmpty(publicProfileView.GroupTag)) ? string.Format("[{0}] {1}", publicProfileView.GroupTag, publicProfileView.Name) : publicProfileView.Name);
					GUI.Label(new Rect(8f, num2 * 24 + 4, rect.width, rect.height), text, BlueStonez.label_interparkmed_11pt_left);

					if (flag) {
						GUI.contentColor = Color.gray;
						GUI.Label(new Rect(rect.width - 100f, num2 * 24 + 4, 100f, rect.height), LocalizedStrings.InMyClan, BlueStonez.label_interparkmed_11pt_left);
						GUI.contentColor = Color.white;
					}

					num2++;
				}
			}

			GUITools.EndScrollView();
		} else {
			GUI.Label(new Rect(0f, 0f, rect.width, rect.height), LocalizedStrings.YouHaveNoFriends, BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();

		if (Event.current.type == EventType.MouseDown && !rect.Contains(Event.current.mousePosition)) {
			GUI.FocusControl("Description");
			_showReceiverDropdownList = false;
			PublicProfileView publicProfileView2;

			if (PlayerDataManager.TryGetFriend(_cmid, out publicProfileView2)) {
				_name = publicProfileView2.Name;
			} else {
				_name = string.Empty;
				_cmid = 0;
			}
		}
	}

	public override void Show() {
		base.Show();
		_message = string.Format(LocalizedStrings.HiYoureInvitedToJoinMyClanN, PlayerDataManager.ClanName);
	}

	public override void Hide() {
		base.Hide();
		_name = string.Empty;
		_fixReceiver = false;
		_cmid = 0;
	}

	public void SelectReceiver(int cmid, string name) {
		_cmid = cmid;
		_name = name;
		_fixReceiver = _cmid != 0;
	}
}
