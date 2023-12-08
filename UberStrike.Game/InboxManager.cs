using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class InboxManager : Singleton<InboxManager> {
	private Dictionary<int, InboxThread> _allThreads = new Dictionary<int, InboxThread>();
	private int _curThreadsPageIndex;
	public List<GroupInvitationView> _outgoingClanRequests = new List<GroupInvitationView>();
	private List<InboxThread> _sortedAllThreads = new List<InboxThread>();
	public Property<List<ContactRequestView>> FriendRequests = new Property<List<ContactRequestView>>(new List<ContactRequestView>());
	public Property<List<GroupInvitationView>> IncomingClanRequests = new Property<List<GroupInvitationView>>(new List<GroupInvitationView>());
	public Property<int> UnreadMessageCount = new Property<int>(0);
	public bool IsInitialized { get; private set; }

	public IList<InboxThread> AllThreads {
		get { return this._sortedAllThreads; }
	}

	public int ThreadCount {
		get { return this._sortedAllThreads.Count; }
	}

	public bool IsLoadingThreads { get; private set; }
	public bool IsNoMoreThreads { get; private set; }
	public float NextInboxRefresh { get; private set; }
	public float NextRequestRefresh { get; private set; }
	private InboxManager() { }

	public void Initialize() {
		if (!this.IsInitialized) {
			this.IsInitialized = true;
			this.LoadNextPageThreads();
			this.RefreshAllRequests();
		}
	}

	public void SendPrivateMessage(int cmidId, string name, string rawMessage) {
		string text = TextUtilities.ShortenText(TextUtilities.Trim(rawMessage), 140, false);

		if (!string.IsNullOrEmpty(text)) {
			if (!this._allThreads.ContainsKey(cmidId)) {
				InboxThread inboxThread = new InboxThread(new MessageThreadView {
					HasNewMessages = false,
					ThreadName = name,
					LastMessagePreview = string.Empty,
					ThreadId = cmidId,
					LastUpdate = DateTime.Now,
					MessageCount = 0
				});

				this._allThreads.Add(inboxThread.ThreadId, inboxThread);
				this._sortedAllThreads.Add(inboxThread);
			}

			PrivateMessageWebServiceClient.SendMessage(PlayerDataManager.AuthToken, cmidId, text, delegate(PrivateMessageView pm) { this.OnPrivateMessageSent(cmidId, pm); }, delegate(Exception ex) { });
		}
	}

	public void UpdateNewMessageCount() {
		this._sortedAllThreads.Sort((InboxThread t1, InboxThread t2) => t2.LastMessageDateTime.CompareTo(t1.LastMessageDateTime));
		this.UnreadMessageCount.Value = this._sortedAllThreads.Reduce((InboxThread el, int acc) => (!el.HasUnreadMessage) ? acc : (acc + 1), 0);
	}

	public void RemoveFriend(int friendCmid) {
		Singleton<PlayerDataManager>.Instance.RemoveFriend(friendCmid);

		RelationshipWebServiceClient.DeleteContact(PlayerDataManager.AuthToken, friendCmid, delegate(MemberOperationResult ev) {
			if (ev == MemberOperationResult.Ok) {
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUpdateFriendsList(friendCmid);
				Singleton<CommsManager>.Instance.UpdateCommunicator();
			} else {
				Debug.LogError("DeleteContact failed with: " + ev);
			}
		}, delegate(Exception ex) { });
	}

	public void AcceptContactRequest(int requestId) {
		this.FriendRequests.Value.RemoveAll((ContactRequestView r) => r.RequestId == requestId);
		this.FriendRequests.Fire();

		RelationshipWebServiceClient.AcceptContactRequest(PlayerDataManager.AuthToken, requestId, delegate(PublicProfileView view) {
			if (view != null) {
				Singleton<PlayerDataManager>.Instance.AddFriend(view);
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUpdateFriendsList(view.Cmid);
				Singleton<CommsManager>.Instance.UpdateCommunicator();
			} else {
				PopupSystem.ShowMessage(LocalizedStrings.Clan, "Failed accepting friend request", PopupSystem.AlertType.OK);
			}
		}, delegate(Exception ex) { });
	}

	public void DeclineContactRequest(int requestId) {
		this.FriendRequests.Value.RemoveAll((ContactRequestView r) => r.RequestId == requestId);
		this.FriendRequests.Fire();
		RelationshipWebServiceClient.DeclineContactRequest(PlayerDataManager.AuthToken, requestId, delegate(bool ev) { }, delegate(Exception ex) { });
	}

	public void AcceptClanRequest(int clanInvitationId) {
		this.IncomingClanRequests.Value.RemoveAll((GroupInvitationView r) => r.GroupInvitationId == clanInvitationId);
		this.IncomingClanRequests.Fire();

		ClanWebServiceClient.AcceptClanInvitation(clanInvitationId, PlayerDataManager.AuthToken, delegate(ClanRequestAcceptView ev) {
			if (ev != null && ev.ActionResult == 0) {
				PopupSystem.ShowMessage(LocalizedStrings.Clan, LocalizedStrings.JoinClanSuccessMsg, PopupSystem.AlertType.OKCancel, delegate { MenuPageManager.Instance.LoadPage(PageType.Clans, false); }, "Go to Clans", null, "Not now", PopupSystem.ActionType.Positive);
				Singleton<ClanDataManager>.Instance.SetClanData(ev.ClanView);
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.SendUpdateClanMembers();
			} else {
				PopupSystem.ShowMessage(LocalizedStrings.Clan, LocalizedStrings.JoinClanErrorMsg, PopupSystem.AlertType.OK);
			}
		}, delegate(Exception ex) { });
	}

	public void DeclineClanRequest(int requestId) {
		this.IncomingClanRequests.Value.RemoveAll((GroupInvitationView r) => r.GroupInvitationId == requestId);
		this.IncomingClanRequests.Fire();
		ClanWebServiceClient.DeclineClanInvitation(requestId, PlayerDataManager.AuthToken, delegate(ClanRequestDeclineView ev) { }, delegate(Exception ex) { });
	}

	internal void LoadNextPageThreads() {
		if (!this.IsNoMoreThreads || this.NextInboxRefresh - Time.time < 0f) {
			this.IsLoadingThreads = true;
			this.NextInboxRefresh = Time.time + 30f;
			PrivateMessageWebServiceClient.GetAllMessageThreadsForUser(PlayerDataManager.AuthToken, this._curThreadsPageIndex, new Action<List<MessageThreadView>>(this.OnFinishLoadingNextPageThreads), delegate(Exception ex) { });
		}
	}

	private void OnFinishLoadingNextPageThreads(List<MessageThreadView> listView) {
		this.IsLoadingThreads = false;

		if (listView.Count > 0) {
			this._curThreadsPageIndex++;
			this.OnGetThreads(listView);
			this.IsNoMoreThreads = false;
		} else {
			this.IsNoMoreThreads = true;
		}
	}

	internal void LoadMessagesForThread(InboxThread inboxThread, int pageIndex) {
		inboxThread.IsLoading = true;

		PrivateMessageWebServiceClient.GetThreadMessages(PlayerDataManager.AuthToken, inboxThread.ThreadId, pageIndex, delegate(List<PrivateMessageView> list) {
			inboxThread.IsLoading = false;
			this.OnGetMessages(inboxThread.ThreadId, list);
		}, delegate(Exception ex) { });
	}

	private void OnGetThreads(List<MessageThreadView> threadView) {
		foreach (MessageThreadView messageThreadView in threadView) {
			InboxThread inboxThread;

			if (this._allThreads.TryGetValue(messageThreadView.ThreadId, out inboxThread)) {
				inboxThread.UpdateThread(messageThreadView);
			} else {
				inboxThread = new InboxThread(messageThreadView);
				this._allThreads.Add(inboxThread.ThreadId, inboxThread);
				this._sortedAllThreads.Add(inboxThread);
			}
		}

		this.UpdateNewMessageCount();
	}

	private void OnGetMessages(int threadId, List<PrivateMessageView> messages) {
		InboxThread inboxThread;

		if (this._allThreads.TryGetValue(threadId, out inboxThread)) {
			inboxThread.AddMessages(messages);
		} else {
			Debug.LogError("Getting messages of non existing thread " + threadId);
		}
	}

	private void OnPrivateMessageSent(int threadId, PrivateMessageView privateMessage) {
		if (privateMessage != null) {
			AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUpdateInboxMessages(privateMessage.ToCmid, privateMessage.PrivateMessageId);
			privateMessage.IsRead = true;
			this.AddMessageToThread(threadId, privateMessage);
		} else {
			Debug.LogError("PrivateMessage sending failed");
			PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.YourMessageHasNotBeenSent);
		}
	}

	private void AddMessage(PrivateMessageView privateMessage) {
		if (privateMessage != null) {
			this.AddMessageToThread(privateMessage.FromCmid, privateMessage);
		} else {
			Debug.LogError("AddMessage called with NULL message");
		}
	}

	private void AddMessageToThread(int threadId, PrivateMessageView privateMessage) {
		InboxThread inboxThread;

		if (!this._allThreads.TryGetValue(threadId, out inboxThread)) {
			inboxThread = new InboxThread(new MessageThreadView {
				ThreadName = privateMessage.FromName,
				ThreadId = threadId
			});

			this._allThreads.Add(inboxThread.ThreadId, inboxThread);
			this._sortedAllThreads.Add(inboxThread);
		}

		inboxThread.AddMessage(privateMessage);
		this.UpdateNewMessageCount();
	}

	internal void MarkThreadAsRead(int threadId) {
		PrivateMessageWebServiceClient.MarkThreadAsRead(PlayerDataManager.AuthToken, threadId, delegate { }, delegate(Exception ex) { });
		this.UpdateNewMessageCount();
	}

	internal void DeleteThread(int threadId) {
		PrivateMessageWebServiceClient.DeleteThread(PlayerDataManager.AuthToken, threadId, delegate { this.OnDeleteThread(threadId); }, delegate(Exception ex) { });
	}

	private void OnDeleteThread(int threadId) {
		this._allThreads.Remove(threadId);
		this._sortedAllThreads.RemoveAll((InboxThread t) => t.ThreadId == threadId);
		this.UpdateNewMessageCount();
	}

	internal void GetMessageWithId(int messageId) {
		PrivateMessageWebServiceClient.GetMessageWithIdForCmid(PlayerDataManager.AuthToken, messageId, new Action<PrivateMessageView>(this.AddMessage), delegate(Exception ex) { });
	}

	internal void RefreshAllRequests() {
		this.NextRequestRefresh = Time.time + 30f;
		RelationshipWebServiceClient.GetContactRequests(PlayerDataManager.AuthToken, new Action<List<ContactRequestView>>(this.OnGetContactRequests), delegate(Exception ex) { });
		ClanWebServiceClient.GetAllGroupInvitations(PlayerDataManager.AuthToken, new Action<List<GroupInvitationView>>(this.OnGetAllGroupInvitations), delegate(Exception ex) { });

		if (Singleton<PlayerDataManager>.Instance.RankInClan != GroupPosition.Member) {
			ClanWebServiceClient.GetPendingGroupInvitations(PlayerDataManager.ClanID, PlayerDataManager.AuthToken, new Action<List<GroupInvitationView>>(this.OnGetPendingGroupInvitations), delegate(Exception ex) { });
		}
	}

	private void OnGetContactRequests(List<ContactRequestView> requests) {
		this.FriendRequests.Value = requests;
		this.FriendRequests.Fire();

		if (this.FriendRequests.Value.Count > 0) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewRequest, 0UL, 1f, 1f);
		}
	}

	private void OnGetAllGroupInvitations(List<GroupInvitationView> requests) {
		this.IncomingClanRequests.Value = requests;
		this.IncomingClanRequests.Fire();

		if (this.IncomingClanRequests.Value.Count > 0) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewRequest, 0UL, 1f, 1f);
		}
	}

	private void OnGetPendingGroupInvitations(List<GroupInvitationView> requests) {
		this._outgoingClanRequests = requests;
	}
}
