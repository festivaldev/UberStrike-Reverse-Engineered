using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameChatPageGUI : PageGUI {
	private const float TitleHeight = 24f;
	private const int TAB_WIDTH = 150;
	private const int CHAT_USER_HEIGHT = 24;
	private string _currentChatMessage = string.Empty;
	private Vector2 _dialogScroll;
	private float _keyboardOffset;
	private float _lastMessageSentTimer = 0.3f;
	private Rect _mainRect;
	private float _nextNaughtyListUpdate;
	private PopupMenu _playerMenu;
	private int _selectedCmid;
	private float _spammingNotificationTime;
	private float _yPosition;

	private void Awake() {
		_playerMenu = new PopupMenu();
		IsOnGUIEnabled = true;
	}

	private void Start() {
		_playerMenu.AddMenuItem("Add Friend", MenuCmdAddFriend, MenuChkAddFriend);
		_playerMenu.AddMenuItem("Unfriend", MenuCmdRemoveFriend, MenuChkRemoveFriend);
		_playerMenu.AddMenuItem(LocalizedStrings.InviteToClan, MenuCmdInviteClan, MenuChkInviteClan);
		_playerMenu.AddMenuItem(LocalizedStrings.Report + " Cheater", MenuCmdReportPlayer, MenuChkReportPlayer);

		if (PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator) {
			_playerMenu.AddMenuItem("- - - - - - - - - - - - -", null, A_0 => true);
			_playerMenu.AddMenuItem("Copy Data", MenuCmdCopyData, A_0 => true);
			_playerMenu.AddMenuItem("Moderate Player", MenuCmdModeratePlayer, A_0 => true);
		}
	}

	private void MenuCmdCopyData(CommUser user) {
		if (user != null) {
			var textEditor = new TextEditor();
			textEditor.content = new GUIContent(string.Concat("<Cmid:", user.Cmid, "> <Name:", user.Name, ">"));
			textEditor.SelectAll();
			textEditor.Copy();
		}
	}

	private void MenuCmdModeratePlayer(CommUser user) {
		if (user != null) {
			var moderationPanelGUI = PanelManager.Instance.OpenPanel(PanelType.Moderation) as ModerationPanelGUI;

			if (moderationPanelGUI) {
				moderationPanelGUI.SetSelectedUser(user);
			}
		}
	}

	private void Update() {
		if (_lastMessageSentTimer < 0.3f) {
			_lastMessageSentTimer += Time.deltaTime;
		}

		if (_yPosition < 0f) {
			_yPosition = Mathf.Lerp(_yPosition, 0.1f, Time.deltaTime * 8f);
		} else {
			_yPosition = 0f;
		}
	}

	private void OnGUI() {
		if (IsOnGUIEnabled) {
			GUI.skin = BlueStonez.Skin;
			GUI.depth = 9;
			_mainRect = new Rect(0f, GlobalUIRibbon.Instance.Height(), Screen.width, Screen.height - GlobalUIRibbon.Instance.Height());
			DrawGUI(_mainRect);
		}
	}

	public override void DrawGUI(Rect rect) {
		GUI.BeginGroup(rect, BlueStonez.window);

		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) {
			GUIUtility.keyboardControl = 0;
		}

		var rect2 = new Rect(0f, 21f, 150f, rect.height - 21f);
		var rect3 = new Rect(149f, 0f, rect.width - 150f, 22f);
		var rect4 = new Rect(150f, 22f, rect.width - 150f, rect.height - 22f - 36f - _keyboardOffset);
		var rect5 = new Rect(149f, rect.height - 37f, rect.width - 150f + 1f, 37f);
		GUITools.PushGUIState();
		GUI.enabled &= !PopupMenu.IsEnabled;
		var chatGroupPanel = Singleton<ChatManager>.Instance._commPanes[3];
		DoDialogFooter(rect5, chatGroupPanel, Singleton<ChatManager>.Instance.InGameDialog);
		DrawCommPane(rect2, chatGroupPanel);
		DoDialogHeader(rect3, Singleton<ChatManager>.Instance.InGameDialog);
		DoDialog(rect4, chatGroupPanel, Singleton<ChatManager>.Instance.InGameDialog);
		GUITools.PopGUIState();

		if (PopupMenu.Current != null) {
			PopupMenu.Current.Draw();
		}

		GUI.EndGroup();
		GuiManager.DrawTooltip();
	}

	private bool IsMobileChannel(ChannelType channel) {
		return channel == ChannelType.Android || channel == ChannelType.IPad || channel == ChannelType.IPhone;
	}

	public void DrawCommPane(Rect rect, ChatGroupPanel pane) {
		GUI.BeginGroup(rect);
		pane.WindowHeight = rect.height;
		var num = Mathf.Max(pane.WindowHeight, pane.ContentHeight);
		var num2 = 0f;
		pane.Scroll = GUITools.BeginScrollView(new Rect(0f, 0f, rect.width, pane.WindowHeight), pane.Scroll, new Rect(0f, 0f, rect.width - 17f, num), false, true);
		GUI.BeginGroup(new Rect(0f, 0f, rect.width, pane.WindowHeight + pane.Scroll.y));
		var num3 = 0;
		var text = pane.SearchText.ToLower();
		GUI.BeginGroup(new Rect(0f, num2, rect.width - 17f, GameState.Current.Players.Count * 24));

		foreach (var gameActorInfo in GameState.Current.Players.Values) {
			if (string.IsNullOrEmpty(text) || gameActorInfo.PlayerName.ToLower().Contains(text)) {
				GroupDrawUser(num3++ * 24, rect.width - 17f, gameActorInfo, true);
			}
		}

		GUI.EndGroup();
		num2 += 24f + GameState.Current.Players.Count * 24;
		GUI.EndGroup();
		GUITools.EndScrollView();
		pane.ContentHeight = num2;
		GUI.EndGroup();
	}

	private void DoDialog(Rect rect, ChatGroupPanel pane, ChatDialog dialog) {
		if (dialog == null) {
			return;
		}

		if (dialog.CheckSize(rect) && !Input.GetMouseButton(0)) {
			_dialogScroll.y = float.MaxValue;
		}

		GUI.BeginGroup(new Rect(rect.x, rect.y + Mathf.Clamp(rect.height - dialog._heightCache, 0f, rect.height), rect.width, rect.height));
		var num = 0;
		var num2 = 0f;
		_dialogScroll = GUITools.BeginScrollView(new Rect(0f, 0f, dialog._frameSize.x, dialog._frameSize.y), _dialogScroll, new Rect(0f, 0f, dialog._contentSize.x, dialog._contentSize.y));

		foreach (var instantMessage in dialog._msgQueue) {
			if (dialog.CanShow == null || dialog.CanShow(instantMessage.Context)) {
				if (num % 2 == 0) {
					GUI.Label(new Rect(0f, num2, dialog._contentSize.x - 1f, instantMessage.Height), GUIContent.none, BlueStonez.box_grey38);
				}

				if (GUI.Button(new Rect(0f, num2, dialog._contentSize.x - 1f, instantMessage.Height), GUIContent.none, BlueStonez.dropdown_list)) {
					_selectedCmid = instantMessage.Cmid;
				}

				if (string.IsNullOrEmpty(instantMessage.PlayerName)) {
					GUI.color = new Color(0.6f, 0.6f, 0.6f);
					GUI.Label(new Rect(4f, num2, dialog._contentSize.x - 8f, 20f), instantMessage.Text, BlueStonez.label_interparkbold_11pt_left);
				} else {
					GUI.color = GetNameColor(instantMessage);
					GUI.Label(new Rect(4f, num2, dialog._contentSize.x - 8f, 20f), instantMessage.PlayerName + ":", BlueStonez.label_interparkbold_11pt_left);
					GUI.color = new Color(0.9f, 0.9f, 0.9f);
					GUI.Label(new Rect(4f, num2 + 20f, dialog._contentSize.x - 8f, instantMessage.Height - 20f), instantMessage.Text, BlueStonez.label_interparkmed_11pt_left);
				}

				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				GUI.Label(new Rect(4f, num2, dialog._contentSize.x - 8f, 20f), instantMessage.TimeString, BlueStonez.label_interparkmed_10pt_right);
				GUI.color = Color.white;
				num2 += instantMessage.Height;
				num++;
			}
		}

		GUITools.EndScrollView();
		dialog._heightCache = num2;
		GUI.EndGroup();
	}

	private void DoDialogHeader(Rect rect, ChatDialog d) {
		GUI.Label(rect, GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.Label(rect, d.Title, BlueStonez.label_interparkbold_11pt);
	}

	private void DoDialogFooter(Rect rect, ChatGroupPanel pane, ChatDialog dialog) {
		GUI.BeginGroup(rect, BlueStonez.window_standard_grey38);
		var enabled = GUI.enabled;
		GUI.enabled &= !AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted && dialog != null;
		GUI.SetNextControlName("@CurrentChatMessage");
		_currentChatMessage = GUI.TextField(new Rect(6f, 6f, rect.width - 60f, rect.height - 12f), _currentChatMessage, 140, BlueStonez.textField);
		_currentChatMessage = _currentChatMessage.Trim('\n');

		if (_spammingNotificationTime > Time.time) {
			GUI.color = Color.red;
			GUI.Label(new Rect(15f, 6f, rect.width - 66f, rect.height - 12f), LocalizedStrings.DontSpamTheLobbyChat, BlueStonez.label_interparkmed_10pt_left);
			GUI.color = Color.white;
		} else {
			var text = string.Empty;

			if (dialog != null && dialog.UserCmid > 0) {
				if (dialog.CanChat) {
					text = LocalizedStrings.EnterAMessageHere;
				} else {
					text = dialog.UserName + LocalizedStrings.Offline;
				}
			} else {
				text = LocalizedStrings.EnterAMessageHere;
			}

			if (string.IsNullOrEmpty(_currentChatMessage) && GUI.GetNameOfFocusedControl() != "@CurrentChatMessage") {
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
				GUI.Label(new Rect(10f, 6f, rect.width - 66f, rect.height - 12f), text, BlueStonez.label_interparkmed_10pt_left);
				GUI.color = Color.white;
			}
		}

		if ((GUITools.Button(new Rect(rect.width - 51f, 6f, 45f, rect.height - 12f), new GUIContent(LocalizedStrings.Send), BlueStonez.buttondark_small) || Event.current.keyCode == KeyCode.Return) && !AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted && _lastMessageSentTimer > 0.29f) {
			SendChatMessage();
			GUI.FocusControl("@CurrentChatMessage");
		}

		GUI.enabled = enabled;
		GUI.EndGroup();
	}

	private Texture2D GetIcon(GameActorInfo info) {
		if (info.IsSpectator) {
			return CommunicatorIcons.PresenceOnline;
		}

		if (!info.IsAlive) {
			return CommunicatorIcons.SkullCrossbonesIcon;
		}

		return CommunicatorIcons.PresencePlaying;
	}

	private void GroupDrawUser(float vOffset, float width, GameActorInfo user, bool allowSelfSelection = false) {
		var cmid = PlayerDataManager.Cmid;
		var rect = new Rect(3f, vOffset, width - 3f, 24f);

		if (_selectedCmid == user.Cmid) {
			GUI.color = new Color(ColorScheme.UberStrikeBlue.r, ColorScheme.UberStrikeBlue.g, ColorScheme.UberStrikeBlue.b, 0.5f);
			GUI.Label(rect, GUIContent.none, BlueStonez.box_white);
			GUI.color = Color.white;
		}

		var enabled = GUI.enabled;
		GUI.Label(new Rect(10f, vOffset + 3f, 16f, 16f), GetIcon(user), GUIStyle.none);
		GUI.Label(new Rect(23f, vOffset + 3f, 16f, 16f), UberstrikeIconsHelper.GetIconForChannel(user.Channel), GUIStyle.none);
		var teamID = user.TeamID;

		if (teamID != TeamID.BLUE) {
			if (teamID != TeamID.RED) {
				GUI.color = Color.white;
			} else {
				GUI.color = ColorScheme.GuiTeamRed;
			}
		} else {
			GUI.color = ColorScheme.GuiTeamBlue;
		}

		GUI.Label(new Rect(44f, vOffset, width - 66f, 24f), user.PlayerName, BlueStonez.label_interparkmed_10pt_left);
		GUI.color = Color.white;

		if (user.Cmid != cmid && GUI.Button(new Rect(rect.width - 17f, vOffset + 1f, 18f, 18f), GUIContent.none, BlueStonez.button_context)) {
			_selectedCmid = user.Cmid;
			_playerMenu.Show(Event.current.mousePosition, new CommUser(user));
		}

		GUI.Box(rect.Expand(0, -1), GUIContent.none, BlueStonez.dropdown_list);

		if (MouseInput.IsMouseClickIn(rect)) {
			if (_selectedCmid != user.Cmid && (allowSelfSelection || user.Cmid != cmid)) {
				_selectedCmid = user.Cmid;
			}
		} else if (MouseInput.IsMouseClickIn(rect, 1)) {
			_playerMenu.Show(Event.current.mousePosition, new CommUser(user));
		}

		GUI.enabled = enabled;
	}

	private void SendChatMessage() {
		if (string.IsNullOrEmpty(_currentChatMessage)) {
			return;
		}

		_dialogScroll.y = float.MaxValue;
		_currentChatMessage = TextUtilities.ShortenText(TextUtilities.Trim(_currentChatMessage), 140, false);
		GameState.Current.SendChatMessage(_currentChatMessage, ChatContext.Player);
		_lastMessageSentTimer = 0f;
		_currentChatMessage = string.Empty;
	}

	private Color GetNameColor(InstantMessage msg) {
		Color color;

		if (msg.Cmid == PlayerDataManager.Cmid) {
			color = ColorScheme.ChatNameCurrentUser;
		} else if (msg.IsFriend || msg.IsClan) {
			color = ColorScheme.ChatNameFriendsUser;
		} else {
			color = ColorScheme.GetNameColorByAccessLevel(msg.AccessLevel);
		}

		return color;
	}

	private void MenuCmdRemoveFriend(CommUser user) {
		if (user != null) {
			var friendCmid = user.Cmid;
			PopupSystem.ShowMessage(LocalizedStrings.RemoveFriendCaps, string.Format(LocalizedStrings.DoYouReallyWantToRemoveNFromYourFriendsList, user.Name), PopupSystem.AlertType.OKCancel, delegate { Singleton<InboxManager>.Instance.RemoveFriend(friendCmid); }, LocalizedStrings.Remove, null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
		}
	}

	private void MenuCmdAddFriend(CommUser user) {
		if (user != null) {
			var friendRequestPanelGUI = PanelManager.Instance.OpenPanel(PanelType.FriendRequest) as FriendRequestPanelGUI;

			if (friendRequestPanelGUI) {
				friendRequestPanelGUI.SelectReceiver(user.Cmid, user.Name);
			}
		}
	}

	private void MenuCmdInviteClan(CommUser user) {
		if (user != null) {
			var inviteToClanPanelGUI = PanelManager.Instance.OpenPanel(PanelType.ClanRequest) as InviteToClanPanelGUI;

			if (inviteToClanPanelGUI) {
				inviteToClanPanelGUI.SelectReceiver(user.Cmid, user.ShortName);
			}
		}
	}

	private void MenuCmdReportPlayer(CommUser user) {
		if (user != null && Singleton<GameStateController>.Instance.Client.IsInsideRoom) {
			PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, "Are you sure you want to report\n" + user.Name + "\nfor cheating?", PopupSystem.AlertType.OKCancel, delegate { Singleton<GameStateController>.Instance.Client.Operations.SendReportPlayer(user.Cmid, PlayerDataManager.AuthToken); }, "Report", null, "Cancel", PopupSystem.ActionType.Negative);
		}
	}

	private bool MenuChkReportPlayer(CommUser user) {
		return user != null && user.Cmid != PlayerDataManager.Cmid && user.AccessLevel == MemberAccessLevel.Default;
	}

	private bool MenuChkAddFriend(CommUser user) {
		return user != null && user.Cmid != PlayerDataManager.Cmid && user.AccessLevel <= PlayerDataManager.AccessLevel && !PlayerDataManager.IsFriend(user.Cmid);
	}

	private bool MenuChkRemoveFriend(CommUser user) {
		return user != null && user.Cmid != PlayerDataManager.Cmid && PlayerDataManager.IsFriend(user.Cmid);
	}

	private bool MenuChkInviteClan(CommUser user) {
		return user != null && user.Cmid != PlayerDataManager.Cmid && (user.AccessLevel <= PlayerDataManager.AccessLevel || PlayerDataManager.IsFriend(user.Cmid)) && PlayerDataManager.IsPlayerInClan && PlayerDataManager.CanInviteToClan && !PlayerDataManager.IsClanMember(user.Cmid);
	}
}
