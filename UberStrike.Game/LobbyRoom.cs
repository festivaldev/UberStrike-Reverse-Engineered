using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Realtime.Client;
using UnityEngine;

public class LobbyRoom : BaseLobbyRoom {
	private Dictionary<int, CommActorInfo> _actors = new Dictionary<int, CommActorInfo>();
	public Property<bool> IsPlayerMuted = new Property<bool>(false);
	private CommActorInfo LocalPlayer;

	public IEnumerable<CommActorInfo> Players {
		get { return _actors.Values; }
	}

	public bool HasPlayer(int cmid) {
		return _actors.ContainsKey(cmid);
	}

	public bool TryGetPlayer(int cmid, out CommActorInfo player) {
		return _actors.TryGetValue(cmid, out player) && player != null;
	}

	protected override void OnClanChatMessage(int cmid, string name, string message) {
		var instantMessage = new InstantMessage(cmid, name, message, MemberAccessLevel.Default);
		Singleton<ChatManager>.Instance.AddClanMessage(cmid, instantMessage);
	}

	protected override void OnFullPlayerListUpdate(List<CommActorInfo> players) {
		_actors.Clear();

		foreach (var commActorInfo in players) {
			_actors[commActorInfo.Cmid] = commActorInfo;
		}

		Singleton<ChatManager>.Instance.RefreshAll(true);
	}

	protected override void OnInGameChatMessage(int cmid, string name, string message, MemberAccessLevel accessLevel, byte context) {
		if (ChatManager.CanShowMessage((ChatContext)context)) {
			GameData.Instance.OnHUDChatMessage.Fire(name, message, accessLevel);
		}

		Singleton<ChatManager>.Instance.InGameDialog.AddMessage(new InstantMessage(cmid, name, message, accessLevel, (ChatContext)context));
	}

	protected override void OnLobbyChatMessage(int cmid, string name, string message) {
		var memberAccessLevel = MemberAccessLevel.Default;
		CommActorInfo commActorInfo;

		if (_actors.TryGetValue(cmid, out commActorInfo)) {
			memberAccessLevel = commActorInfo.AccessLevel;

			if (!string.IsNullOrEmpty(name)) {
				name = PrependClanTagToPlayerName(commActorInfo);
			}
		}

		var instantMessage = new InstantMessage(cmid, name, message, memberAccessLevel, commActorInfo);
		Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(instantMessage);
	}

	private bool DoModChatCmd(string message) {
		var text = message.Substring(1);
		var array = (from Match m in Regex.Matches(text, "\\w+|\"[^\\r\\n]*\"") select m.Value).ToArray();

		if (text == "h" || text == "help") {
			Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", " Usage: /cmd user [duration]\nValid commands are [short | long]:\n\tm | mute\n\tg | ghost\n\tu | unmute\n\tk | kick\n\th | help", MemberAccessLevel.Admin));
			Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", "Duration defaults to 12 hours if none is given, and applies to mute/ghost only. Use Copy Name menu item to get the name", MemberAccessLevel.Admin));

			return true;
		}

		if (array.Length < 2) {
			Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", "Error! No player specified!", MemberAccessLevel.Admin));

			return false;
		}

		var text2 = array[0];
		var text3 = array[1];
		var text4 = "720";

		if (array.Length > 2) {
			text4 = array[2];
		}

		string text5;

		if (text3[0] == '"') {
			var array2 = text3.Substring(1).Split('"');
			text5 = array2[0];

			if (array2.Length > 2) {
				text4 = text.Split('"')[2].Trim();
			}
		} else {
			text5 = array[1];
		}

		var num = Convert.ToInt32(text4);
		int num2;
		string text6;

		if (!FindPlayerByName(text5, out num2, out text6)) {
			Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", "Error! Player not found, or too many players matched that pattern!", MemberAccessLevel.Admin));

			return true;
		}

		var text7 = text2;

		switch (text7) {
			case "m":
			case "mute":
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendModerationMutePlayer(num, num2, true);
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendModerationMutePlayer(num, num2, false);
				Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", string.Concat("User ", text6, " was muted for ", num.ToString(), " minutes!"), MemberAccessLevel.Admin));

				break;
			case "g":
			case "ghost":
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendModerationMutePlayer(num, num2, false);
				Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", string.Concat("User ", text6, " was ghosted for ", num.ToString(), " minutes!"), MemberAccessLevel.Admin));

				break;
			case "u":
			case "unmute":
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendModerationMutePlayer(0, num2, false);
				Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", "User " + text6 + " was unmuted!", MemberAccessLevel.Admin));

				break;
			case "k":
			case "kick":
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendModerationBanPlayer(num2);
				Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(0, "[MOD]", "User " + text6 + " was kicked!", MemberAccessLevel.Admin));

				break;
		}

		return true;
	}

	private bool FindPlayerByName(string name, out int cmid, out string uname) {
		var num = 0;
		uname = string.Empty;
		cmid = 0;

		foreach (var keyValuePair in _actors) {
			if (!string.IsNullOrEmpty(keyValuePair.Value.PlayerName)) {
				if (keyValuePair.Value.PlayerName.Contains(name)) {
					if (num > 0) {
						return false;
					}

					num = keyValuePair.Value.Cmid;
					uname = keyValuePair.Value.PlayerName;
				}
			}
		}

		if (num == 0) {
			return false;
		}

		cmid = num;

		return true;
	}

	protected override void OnModerationCustomMessage(string message) {
		PopupSystem.ShowMessage("Administrator Message", message, PopupSystem.AlertType.OK, delegate { });
		EventHandler.Global.Fire(new GameEvents.PlayerPause());
	}

	protected override void OnModerationKickGame() {
		Singleton<GameStateController>.Instance.LeaveGame();
		PopupSystem.ShowMessage("ADMIN MESSAGE", "You were kicked out of the game!", PopupSystem.AlertType.OK, delegate { });
	}

	protected override void OnModerationMutePlayer(bool isPlayerMuted) {
		IsPlayerMuted.Value = isPlayerMuted;

		if (isPlayerMuted) {
			PopupSystem.ShowMessage("ADMIN MESSAGE", "You have been muted!", PopupSystem.AlertType.OK, delegate { });
		}
	}

	protected override void OnPlayerHide(int cmid) {
		if (!PlayerDataManager.IsClanMember(cmid) && !PlayerDataManager.IsFriend(cmid) && !Singleton<ChatManager>.Instance.HasDialogWith(cmid)) {
			OnPlayerLeft(cmid, true);
		}
	}

	protected override void OnPlayerJoined(CommActorInfo data) {
		_actors.Clear();
		Debug.Log("OnPlayerJoined " + data.Cmid);
		_actors[data.Cmid] = data;
		Singleton<ChatManager>.Instance.RefreshAll(true);
	}

	protected override void OnPlayerLeft(int cmid, bool refreshComm) {
		CommActorInfo commActorInfo;

		if (_actors.TryGetValue(cmid, out commActorInfo)) {
			_actors.Remove(cmid);
			commActorInfo.CurrentRoom = null;
		}

		Singleton<ChatManager>.Instance.RefreshAll(refreshComm);
	}

	protected override void OnPlayerUpdate(CommActorInfo data) {
		_actors[data.Cmid] = data;
		Singleton<ChatManager>.Instance.RefreshAll();
	}

	protected override void OnPrivateChatMessage(int cmid, string name, string message) {
		var memberAccessLevel = MemberAccessLevel.Default;
		CommActorInfo commActorInfo;

		if (_actors.TryGetValue(cmid, out commActorInfo)) {
			memberAccessLevel = commActorInfo.AccessLevel;

			if (!string.IsNullOrEmpty(name)) {
				name = PrependClanTagToPlayerName(commActorInfo);
			}
		}

		var instantMessage = new InstantMessage(cmid, name, message, memberAccessLevel, commActorInfo);
		Singleton<ChatManager>.Instance.AddNewPrivateMessage(cmid, instantMessage);
	}

	protected override void OnUpdateActorsForModeration(List<CommActorInfo> naughtyList) {
		Singleton<ChatManager>.Instance.SetNaughtyList(naughtyList);
		SendContactList();
	}

	protected override void OnUpdateClanData() {
		Singleton<ClanDataManager>.Instance.CheckCompleteClanData();
	}

	protected override void OnUpdateClanMembers() {
		Singleton<ClanDataManager>.Instance.RefreshClanData(true);
	}

	protected override void OnUpdateContacts(List<CommActorInfo> updated, List<int> removed) {
		foreach (var commActorInfo in updated) {
			_actors[commActorInfo.Cmid] = commActorInfo;
		}

		foreach (var num in removed) {
			OnPlayerLeft(num, false);
		}

		Singleton<ChatManager>.Instance.RefreshAll(true);
	}

	protected override void OnUpdateFriendsList() {
		UnityRuntime.StartRoutine(Singleton<CommsManager>.Instance.GetContactsByGroups());
	}

	protected override void OnUpdateInboxMessages(int messageId) {
		Singleton<InboxManager>.Instance.GetMessageWithId(messageId);
	}

	protected override void OnUpdateInboxRequests() {
		Singleton<InboxManager>.Instance.RefreshAllRequests();
	}

	public void SendContactList() {
		var hashSet = new HashSet<int>();

		foreach (var commUser in Singleton<ChatManager>.Instance.FriendUsers) {
			hashSet.Add(commUser.Cmid);
		}

		foreach (var commUser2 in Singleton<ChatManager>.Instance.ClanUsers) {
			hashSet.Add(commUser2.Cmid);
		}

		foreach (var commUser3 in Singleton<ChatManager>.Instance.OtherUsers) {
			hashSet.Add(commUser3.Cmid);
		}

		foreach (var commUser4 in Singleton<ChatManager>.Instance.NaughtyUsers) {
			hashSet.Add(commUser4.Cmid);
		}

		if (hashSet.Count > 0) {
			Operations.SendSetContactList(hashSet.ToList());
		}
	}

	public void UpdatePlayerRoom(GameRoom room) {
		Operations.SendUpdatePlayerRoom(room);
	}

	public void SendUpdateClanMembers() {
		var list = new List<int>();

		foreach (var clanMemberView in Singleton<PlayerDataManager>.Instance.ClanMembers) {
			if (clanMemberView.Cmid != PlayerDataManager.Cmid) {
				list.Add(clanMemberView.Cmid);
			}
		}

		list.RemoveAll(id => id == PlayerDataManager.Cmid);
		Operations.SendUpdateClanMembers(list);
	}

	public void UpdateContacts() {
		if (Singleton<ChatManager>.Instance.TotalContacts > 0) {
			Operations.SendUpdateContacts();
		}
	}

	public void SendUpdateResetLobby() {
		_actors.Clear();
		Singleton<ChatManager>.Instance.RefreshAll();
		Operations.SendFullPlayerListUpdate();
	}

	public void SendClanChatMessage(string message) {
		message = ChatMessageFilter.Cleanup(message);

		if (!string.IsNullOrEmpty(message)) {
			var list = new List<int>();

			foreach (var commUser in Singleton<ChatManager>.Instance.ClanUsers) {
				if (commUser.Cmid != PlayerDataManager.Cmid) {
					list.Add(commUser.Cmid);
				}
			}

			OnClanChatMessage(PlayerDataManager.Cmid, PlayerDataManager.Name, message);
			Operations.SendChatMessageToClan(list, message);
		}
	}

	public bool SendLobbyChatMessage(string message) {
		message = ChatMessageFilter.Cleanup(message);

		if (string.IsNullOrEmpty(message)) {
			return false;
		}

		if (PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator && message[0] == '/' && DoModChatCmd(message)) {
			GUI.FocusControl("@CurrentChatMessage");

			return true;
		}

		if (ChatMessageFilter.IsSpamming(message)) {
			return false;
		}

		OnLobbyChatMessage(PlayerDataManager.Cmid, PlayerDataManager.Name, message);
		Operations.SendChatMessageToAll(message);

		return true;
	}

	public void SendPrivateChatMessage(int receiverCmid, string receiverName, string message) {
		message = ChatMessageFilter.Cleanup(message);

		if (!string.IsNullOrEmpty(message)) {
			Singleton<ChatManager>.Instance.AddNewPrivateMessage(receiverCmid, new InstantMessage(PlayerDataManager.Cmid, PlayerDataManager.Name, message, PlayerDataManager.AccessLevel));
			Operations.SendChatMessageToPlayer(receiverCmid, message);
		}
	}

	private string PrependClanTagToPlayerName(CommActorInfo actor) {
		if (!string.IsNullOrEmpty(actor.ClanTag)) {
			return string.Concat("[" + actor.ClanTag + "] " + actor.PlayerName);
		}

		return actor.PlayerName;
	}
}
