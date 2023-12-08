using System;
using System.Collections.Generic;
using System.Text;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class ChatManager : Singleton<ChatManager> {
	private Dictionary<int, CommUser> _allTimePlayers;
	private Dictionary<int, CommUser> _clanUsers;
	public ChatGroupPanel[] _commPanes;
	public Dictionary<int, ChatDialog> _dialogsByCmid;
	private List<CommUser> _friendUsers;
	private List<CommUser> _ingameUsers;
	private List<CommUser> _lastgameUsers;
	private List<CommUser> _lobbyUsers;
	public HashSet<int> _mutedCmids;
	public Dictionary<int, CommUser> _naughtyUsers;
	private float _nextRefreshTime;
	private List<CommUser> _otherUsers;
	private HashSet<TabArea> _tabAreas;
	public Property<bool> HasUnreadClanMessage = new Property<bool>(false);
	public Property<bool> HasUnreadPrivateMessage = new Property<bool>(false);
	public int SelectedCmid;
	public ChatDialog SelectedDialog;

	public int TotalContacts {
		get { return _friendUsers.Count + _otherUsers.Count + _clanUsers.Count; }
	}

	public ChatDialog ClanDialog { get; private set; }
	public ChatDialog LobbyDialog { get; private set; }
	public ChatDialog InGameDialog { get; private set; }
	public ChatDialog ModerationDialog { get; private set; }

	public ICollection<CommUser> OtherUsers {
		get { return _otherUsers; }
	}

	public ICollection<CommUser> FriendUsers {
		get { return _friendUsers; }
	}

	public ICollection<CommUser> LobbyUsers {
		get { return _lobbyUsers; }
	}

	public ICollection<CommUser> ClanUsers {
		get { return _clanUsers.Values; }
	}

	public ICollection<CommUser> NaughtyUsers {
		get { return _naughtyUsers.Values; }
	}

	public ICollection<CommUser> GameUsers {
		get { return _ingameUsers; }
	}

	public ICollection<CommUser> GameHistoryUsers {
		get { return _lastgameUsers; }
	}

	public int TabCounter {
		get { return _tabAreas.Count + ((!ShowTab(TabArea.InGame)) ? 0 : 1) + ((!ShowTab(TabArea.Clan)) ? 0 : 1) + ((!ShowTab(TabArea.Moderation)) ? 0 : 1); }
	}

	public static ChatContext CurrentChatContext {
		get { return (!GameState.Current.PlayerData.IsSpectator) ? ChatContext.Player : ChatContext.Spectator; }
	}

	private ChatManager() {
		_otherUsers = new List<CommUser>();
		_friendUsers = new List<CommUser>();
		_lobbyUsers = new List<CommUser>();
		_clanUsers = new Dictionary<int, CommUser>();
		_naughtyUsers = new Dictionary<int, CommUser>();
		_ingameUsers = new List<CommUser>();
		_lastgameUsers = new List<CommUser>();
		_allTimePlayers = new Dictionary<int, CommUser>();
		_dialogsByCmid = new Dictionary<int, ChatDialog>();
		_mutedCmids = new HashSet<int>();
		ClanDialog = new ChatDialog(LocalizedStrings.ChatInClan);
		LobbyDialog = new ChatDialog(LocalizedStrings.ChatInLobby);
		ModerationDialog = new ChatDialog(LocalizedStrings.Moderate);
		InGameDialog = new ChatDialog(string.Empty);
		InGameDialog.CanShow = CanShowMessage;
		_commPanes = new ChatGroupPanel[5];
		_commPanes[0] = new ChatGroupPanel();
		_commPanes[1] = new ChatGroupPanel();
		_commPanes[2] = new ChatGroupPanel();
		_commPanes[3] = new ChatGroupPanel();
		_commPanes[4] = new ChatGroupPanel();

		_tabAreas = new HashSet<TabArea> {
			TabArea.Lobby,
			TabArea.Private
		};

		_commPanes[0].AddGroup(UserGroups.None, LocalizedStrings.Lobby, LobbyUsers);
		_commPanes[1].AddGroup(UserGroups.Friend, LocalizedStrings.Friends, FriendUsers);
		_commPanes[1].AddGroup(UserGroups.Other, LocalizedStrings.Others, OtherUsers);
		_commPanes[2].AddGroup(UserGroups.None, LocalizedStrings.Clan, ClanUsers);
		_commPanes[3].AddGroup(UserGroups.None, LocalizedStrings.Game, GameUsers);
		_commPanes[3].AddGroup(UserGroups.Other, "History", GameHistoryUsers);
		_commPanes[4].AddGroup(UserGroups.None, "Naughty List", NaughtyUsers);
		EventHandler.Global.AddListener(new Action<GlobalEvents.Login>(OnLoginEvent));
	}

	protected override void OnDispose() {
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.Login>(OnLoginEvent));
	}

	private void OnLoginEvent(GlobalEvents.Login ev) {
		if (ev.AccessLevel >= MemberAccessLevel.Moderator) {
			_tabAreas.Add(TabArea.Moderation);
		}
	}

	public bool IsMuted(int cmid) {
		return _mutedCmids.Contains(cmid);
	}

	public void HideConversations(int cmid) {
		_mutedCmids.Add(cmid);
		LobbyDialog.RecalulateBounds();
		ChatDialog chatDialog;

		if (_dialogsByCmid.TryGetValue(cmid, out chatDialog)) {
			chatDialog.RecalulateBounds();
		}
	}

	public void ShowConversations(int cmid) {
		_mutedCmids.Remove(cmid);
		LobbyDialog.RecalulateBounds();
		ChatDialog chatDialog;

		if (_dialogsByCmid.TryGetValue(cmid, out chatDialog)) {
			chatDialog.RecalulateBounds();
		}
	}

	public bool ShowTab(TabArea tab) {
		switch (tab) {
			case TabArea.Clan:
				return PlayerDataManager.IsPlayerInClan;
			case TabArea.InGame:
				return GameState.Current.HasJoinedGame || Instance.GameHistoryUsers.Count > 0;
			case TabArea.Moderation:
				return PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator;
			default:
				return _tabAreas.Contains(tab);
		}
	}

	public static bool CanShowMessage(ChatContext ctx) {
		if (GameState.Current.HasJoinedGame && GameState.Current.GameMode == GameModeType.EliminationMode && GameState.Current.IsInGame) {
			var chatContext = ((!GameState.Current.PlayerData.IsSpectator) ? ChatContext.Player : ChatContext.Spectator);

			return ctx == chatContext;
		}

		return true;
	}

	public bool HasDialogWith(int cmid) {
		return _dialogsByCmid.ContainsKey(cmid);
	}

	public void UpdateClanSection() {
		Instance._clanUsers.Clear();

		foreach (var clanMemberView in Singleton<PlayerDataManager>.Instance.ClanMembers) {
			Instance._clanUsers[clanMemberView.Cmid] = new CommUser(clanMemberView);
		}

		RefreshAll(true);
	}

	public void RefreshAll(bool forceRefresh = false) {
		if (forceRefresh || _nextRefreshTime < Time.time) {
			_nextRefreshTime = Time.time + 5f;
			_lobbyUsers.Clear();

			foreach (var commActorInfo in AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Players) {
				if (commActorInfo.Cmid > 0) {
					var commUser = new CommUser(commActorInfo) {
						IsClanMember = PlayerDataManager.IsClanMember(commActorInfo.Cmid),
						IsFriend = PlayerDataManager.IsFriend(commActorInfo.Cmid),
						IsFacebookFriend = PlayerDataManager.IsFacebookFriend(commActorInfo.Cmid),
						IsOnline = true
					};

					_lobbyUsers.Add(commUser);
				}
			}

			_lobbyUsers.Sort(new CommUserNameComparer());
			_lobbyUsers.Sort(new CommUserFriendsComparer());

			foreach (var commUser2 in Instance._lastgameUsers) {
				commUser2.IsClanMember = PlayerDataManager.IsClanMember(commUser2.Cmid);
				commUser2.IsFriend = PlayerDataManager.IsFriend(commUser2.Cmid);
				commUser2.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(commUser2.Cmid);
				CommActorInfo commActorInfo2;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(commUser2.Cmid, out commActorInfo2)) {
					commUser2.SetActor(commActorInfo2);
				} else {
					commUser2.SetActor(null);
				}
			}

			Instance._lastgameUsers.Sort(new CommUserPresenceComparer());

			foreach (var commUser3 in Instance._friendUsers) {
				CommActorInfo commActorInfo2;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(commUser3.Cmid, out commActorInfo2)) {
					commUser3.SetActor(commActorInfo2);
				} else {
					commUser3.SetActor(null);
				}
			}

			Instance._friendUsers.Sort(new CommUserPresenceComparer());

			foreach (var commUser4 in Instance._clanUsers.Values) {
				CommActorInfo commActorInfo2;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(commUser4.Cmid, out commActorInfo2)) {
					commUser4.SetActor(commActorInfo2);
				} else {
					commUser4.SetActor(null);
				}
			}

			foreach (var commUser5 in Instance._otherUsers) {
				CommActorInfo commActorInfo2;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(commUser5.Cmid, out commActorInfo2)) {
					commUser5.SetActor(commActorInfo2);
				} else {
					commUser5.SetActor(null);
				}
			}

			Instance._otherUsers.Sort(new CommUserNameComparer());

			foreach (var keyValuePair in Instance._naughtyUsers) {
				CommActorInfo commActorInfo2;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(keyValuePair.Key, out commActorInfo2)) {
					keyValuePair.Value.SetActor(commActorInfo2);
				} else {
					keyValuePair.Value.SetActor(null);
				}
			}
		}
	}

	public void UpdateFriendSection() {
		var list = new List<CommUser>(Instance._friendUsers);
		Instance._friendUsers.Clear();

		foreach (var publicProfileView in Singleton<PlayerDataManager>.Instance.FriendList) {
			Instance._friendUsers.Add(new CommUser(publicProfileView));
		}

		foreach (var publicProfileView2 in Singleton<PlayerDataManager>.Instance.FacebookFriends) {
			Instance._friendUsers.Add(new CommUser(publicProfileView2));
		}

		CommUser f2;

		foreach (var commUser in Instance._friendUsers) {
			f2 = commUser;
			ChatDialog chatDialog;

			if (Instance._otherUsers.RemoveAll(u => u.Cmid == f2.Cmid) > 0 && Instance._dialogsByCmid.TryGetValue(f2.Cmid, out chatDialog)) {
				chatDialog.Group = UserGroups.Friend;
			}
		}

		CommUser f;

		foreach (var commUser2 in list) {
			f = commUser2;
			ChatDialog chatDialog2;

			if (Instance._dialogsByCmid.TryGetValue(f.Cmid, out chatDialog2) && !Instance._friendUsers.Exists(u => u.Cmid == f.Cmid) && !Instance._otherUsers.Exists(u => u.Cmid == f.Cmid)) {
				Instance._otherUsers.Add(f);
				chatDialog2.Group = UserGroups.Other;
			}
		}

		Instance.RefreshAll();
	}

	public static Texture GetPresenceIcon(CommActorInfo user) {
		if (user != null) {
			return GetPresenceIcon((user.CurrentRoom == null) ? PresenceType.Online : PresenceType.InGame);
		}

		return GetPresenceIcon(PresenceType.Offline);
	}

	public static Texture GetPresenceIcon(PresenceType index) {
		switch (index) {
			case PresenceType.Offline:
				return CommunicatorIcons.PresenceOffline;
			case PresenceType.Online:
				return CommunicatorIcons.PresenceOnline;
			case PresenceType.InGame:
				return CommunicatorIcons.PresencePlaying;
			default:
				return CommunicatorIcons.PresenceOffline;
		}
	}

	public void SetGameSection(string server, int roomId, int mapId, IEnumerable<GameActorInfo> actors) {
		_ingameUsers.Clear();
		_lastgameUsers.Clear();
		_lastgameUsers.AddRange(_allTimePlayers.Values);
		GameActorInfo v;

		foreach (var gameActorInfo in actors) {
			v = gameActorInfo;
			var commUser = new CommUser(v);

			commUser.CurrentGame = new GameRoom {
				Server = new ConnectionAddress(server),
				Number = roomId,
				MapId = mapId
			};

			commUser.IsClanMember = PlayerDataManager.IsClanMember(commUser.Cmid);
			commUser.IsFriend = PlayerDataManager.IsFriend(commUser.Cmid);
			commUser.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(commUser.Cmid);
			_ingameUsers.Add(commUser);
			_lastgameUsers.RemoveAll(p => p.Cmid == v.Cmid);

			if (v.Cmid != PlayerDataManager.Cmid && !_allTimePlayers.ContainsKey(v.Cmid)) {
				var commUser2 = new CommUser(v);

				commUser2.CurrentGame = new GameRoom {
					Server = new ConnectionAddress(server),
					Number = roomId,
					MapId = mapId
				};

				_allTimePlayers[v.Cmid] = commUser2;
			}
		}

		_ingameUsers.Sort(new CommUserNameComparer());
	}

	public List<CommUser> GetCommUsersToReport() {
		var num = _ingameUsers.Count + _lobbyUsers.Count + _otherUsers.Count;
		var dictionary = new Dictionary<int, CommUser>(num);

		foreach (var commUser in _ingameUsers) {
			dictionary[commUser.Cmid] = commUser;
		}

		foreach (var commUser2 in _otherUsers) {
			dictionary[commUser2.Cmid] = commUser2;
		}

		foreach (var commUser3 in _lobbyUsers) {
			dictionary[commUser3.Cmid] = commUser3;
		}

		return new List<CommUser>(dictionary.Values);
	}

	public bool TryGetClanUsers(int cmid, out CommUser user) {
		return _clanUsers.TryGetValue(cmid, out user) && user != null;
	}

	public bool TryGetGameUser(int cmid, out CommUser user) {
		user = null;

		foreach (var commUser in _ingameUsers) {
			if (commUser.Cmid == cmid) {
				user = commUser;

				return true;
			}
		}

		return false;
	}

	public bool TryGetLobbyCommUser(int cmid, out CommUser user) {
		user = _lobbyUsers.Find(u => u.Cmid == cmid);

		return user != null;
	}

	public bool TryGetFriend(int cmid, out CommUser user) {
		foreach (var commUser in _friendUsers) {
			if (commUser.Cmid == cmid) {
				user = commUser;

				return true;
			}
		}

		user = null;

		return false;
	}

	public void CreatePrivateChat(int cmid) {
		ChatDialog chatDialog = null;
		ChatDialog chatDialog2;

		if (_dialogsByCmid.TryGetValue(cmid, out chatDialog2) && chatDialog2 != null) {
			chatDialog = chatDialog2;
		} else {
			CommActorInfo commActorInfo = null;

			if (PlayerDataManager.IsFriend(cmid) || PlayerDataManager.IsFacebookFriend(cmid)) {
				var commUser = _friendUsers.Find(u => u.Cmid == cmid);

				if (commUser != null) {
					chatDialog = new ChatDialog(commUser, UserGroups.Friend);
				}
			} else if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(cmid, out commActorInfo)) {
				CommUser commUser;
				ClanMemberView clanMemberView;

				if (PlayerDataManager.TryGetClanMember(cmid, out clanMemberView)) {
					commUser = new CommUser(clanMemberView);
					commUser.SetActor(commActorInfo);
				} else {
					commUser = new CommUser(commActorInfo);
				}

				_otherUsers.Add(commUser);
				chatDialog = new ChatDialog(commUser, UserGroups.Other);
			}

			if (chatDialog != null) {
				_dialogsByCmid.Add(cmid, chatDialog);
			}
		}

		if (chatDialog != null) {
			ChatPageGUI.SelectedTab = TabArea.Private;
			SelectedDialog = chatDialog;
			SelectedCmid = cmid;
		} else {
			Debug.LogError(string.Format("Player with cmuneID {0} not found in communicator!", cmid));
		}
	}

	public string GetAllChatMessagesForPlayerReport() {
		var stringBuilder = new StringBuilder();
		var collection = Instance.InGameDialog.AllMessages;

		if (collection.Count > 0) {
			stringBuilder.AppendLine("In Game Chat:");

			foreach (var instantMessage in collection) {
				stringBuilder.AppendLine(instantMessage.PlayerName + " : " + instantMessage.Text);
			}

			stringBuilder.AppendLine();
		}

		foreach (var chatDialog in Instance._dialogsByCmid.Values) {
			collection = chatDialog.AllMessages;

			if (collection.Count > 0) {
				stringBuilder.AppendLine("Private Chat:");

				foreach (var instantMessage2 in collection) {
					stringBuilder.AppendLine(instantMessage2.PlayerName + " : " + instantMessage2.Text);
				}

				stringBuilder.AppendLine();
			}
		}

		collection = Instance.ClanDialog.AllMessages;

		if (collection.Count > 0) {
			stringBuilder.AppendLine("Clan Chat:");

			foreach (var instantMessage3 in collection) {
				stringBuilder.AppendLine(instantMessage3.PlayerName + " : " + instantMessage3.Text);
			}

			stringBuilder.AppendLine();
		}

		collection = Instance.LobbyDialog.AllMessages;

		if (collection.Count > 0) {
			stringBuilder.AppendLine("Lobby Chat:");

			foreach (var instantMessage4 in collection) {
				stringBuilder.AppendLine(instantMessage4.PlayerName + " : " + instantMessage4.Text);
			}

			stringBuilder.AppendLine();
		}

		return stringBuilder.ToString();
	}

	public void UpdateLastgamePlayers() {
		Instance._lastgameUsers.Clear();

		foreach (var commUser in Instance._allTimePlayers.Values) {
			commUser.IsClanMember = PlayerDataManager.IsClanMember(commUser.Cmid);
			commUser.IsFriend = PlayerDataManager.IsFriend(commUser.Cmid);
			commUser.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(commUser.Cmid);
			CommActorInfo commActorInfo;

			if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(commUser.Cmid, out commActorInfo)) {
				commUser.SetActor(commActorInfo);
			} else {
				commUser.SetActor(null);
			}

			Instance._lastgameUsers.Add(commUser);
		}

		Instance._lastgameUsers.Sort(new CommUserPresenceComparer());
	}

	public void SetNaughtyList(List<CommActorInfo> hackers) {
		foreach (var commActorInfo in hackers) {
			_naughtyUsers[commActorInfo.Cmid] = new CommUser(commActorInfo);
		}
	}

	public void AddClanMessage(int cmid, InstantMessage msg) {
		ClanDialog.AddMessage(msg);

		if (cmid != PlayerDataManager.Cmid && ChatPageGUI.SelectedTab != TabArea.Clan) {
			HasUnreadClanMessage.Value = true;
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewMessage);
		}
	}

	public void AddNewPrivateMessage(int cmid, InstantMessage msg) {
		try {
			ChatDialog chatDialog;

			if (!_dialogsByCmid.TryGetValue(cmid, out chatDialog) && !msg.IsNotification) {
				CommActorInfo commActorInfo;

				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(cmid, out commActorInfo)) {
					var commUser = new CommUser(commActorInfo);
					chatDialog = AddNewDialog(commUser);

					if (!_friendUsers.Exists(p => p.Cmid == cmid)) {
						_otherUsers.Add(commUser);
					}
				} else {
					var commUser2 = new CommUser(new CommActorInfo {
						Cmid = cmid,
						PlayerName = msg.PlayerName,
						Channel = ChannelType.WebPortal,
						AccessLevel = msg.AccessLevel
					});

					chatDialog = AddNewDialog(commUser2);

					if (!_friendUsers.Exists(p => p.Cmid == cmid)) {
						_otherUsers.Add(commUser2);
					}
				}
			}

			if (chatDialog != null) {
				chatDialog.AddMessage(msg);

				if (chatDialog != SelectedDialog) {
					chatDialog.HasUnreadMessage = true;
				}

				if (ChatPageGUI.SelectedTab != TabArea.Private) {
					HasUnreadPrivateMessage.Value = true;
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewMessage);
				}
			}
		} catch {
			Debug.LogError(string.Format("AddNewPrivateMessage from cmid={0}", cmid));

			throw;
		}
	}

	public ChatDialog AddNewDialog(CommUser user) {
		ChatDialog chatDialog = null;

		if (user != null && !_dialogsByCmid.TryGetValue(user.Cmid, out chatDialog)) {
			if (PlayerDataManager.IsFriend(user.Cmid) || PlayerDataManager.IsFacebookFriend(user.Cmid)) {
				chatDialog = new ChatDialog(user, UserGroups.Friend);
			} else {
				chatDialog = new ChatDialog(user, UserGroups.Other);
			}

			_dialogsByCmid.Add(user.Cmid, chatDialog);
		}

		return chatDialog;
	}

	internal void RemoveDialog(ChatDialog d) {
		_dialogsByCmid.Remove(d.UserCmid);
		_otherUsers.RemoveAll(u => u.Cmid == d.UserCmid);
		SelectedDialog = null;
	}
}
