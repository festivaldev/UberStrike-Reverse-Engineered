using System.Collections;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

internal class GUIHomePage : GUIPageBase {
	[SerializeField]
	private UIEventReceiver chatButton;

	[SerializeField]
	private UITweener chatFlashTween;

	[SerializeField]
	private UIEventReceiver clansButton;

	[SerializeField]
	private UIEventReceiver inboxButton;

	[SerializeField]
	private UITweener inboxFlashTween;

	[SerializeField]
	private UIEventReceiver playButton;

	[SerializeField]
	private UIEventReceiver profileButton;

	[SerializeField]
	private UIEventReceiver quitButton;

	[SerializeField]
	private UIEventReceiver shopButton;

	private void OnEnable() {
		Singleton<InboxManager>.Instance.UnreadMessageCount.Fire();
		Singleton<InboxManager>.Instance.FriendRequests.Fire();
		Singleton<InboxManager>.Instance.IncomingClanRequests.Fire();
		Singleton<ChatManager>.Instance.HasUnreadClanMessage.Fire();
		Singleton<ChatManager>.Instance.HasUnreadPrivateMessage.Fire();
	}

	private void Start() {
		quitButton.gameObject.SetActive(ApplicationDataManager.IsDesktop);
		Singleton<InboxManager>.Instance.UnreadMessageCount.AddEventAndFire(HandlePendingInboxMessages, this);
		Singleton<InboxManager>.Instance.FriendRequests.AddEventAndFire(HandlePendingFriendRequests, this);
		Singleton<InboxManager>.Instance.IncomingClanRequests.AddEventAndFire(HandlePendingClanRequests, this);
		Singleton<ChatManager>.Instance.HasUnreadClanMessage.AddEventAndFire(HandlePendingChatMessages, this);
		Singleton<ChatManager>.Instance.HasUnreadPrivateMessage.AddEventAndFire(HandlePendingChatMessages, this);

		playButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Play);
			});
		};

		shopButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Shop);
			});
		};

		profileButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Stats);
			});
		};

		clansButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Clans);
			});
		};

		chatButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Chat);
			});
		};

		inboxButton.OnClicked = delegate {
			Dismiss(delegate {
				GameData.Instance.MainMenu.Value = MainMenuState.None;
				MenuPageManager.Instance.LoadPage(PageType.Inbox);
			});
		};

		quitButton.OnClicked = delegate { PopupSystem.ShowMessage("Quit", "Are you sure?", PopupSystem.AlertType.OKCancel, delegate { Application.Quit(); }, "OK", null, "Cancel"); };
	}

	private void HandlePendingInboxMessages(int unreadMessages) {
		FlashInbox(unreadMessages > 0 || Singleton<InboxManager>.Instance.FriendRequests.Value.Count > 0 || Singleton<InboxManager>.Instance.IncomingClanRequests.Value.Count > 0);
	}

	private void HandlePendingFriendRequests(List<ContactRequestView> friendRequests) {
		FlashInbox(friendRequests.Count > 0 || Singleton<InboxManager>.Instance.IncomingClanRequests.Value.Count > 0 || Singleton<InboxManager>.Instance.UnreadMessageCount > 0);
	}

	private void HandlePendingClanRequests(List<GroupInvitationView> clanRequests) {
		FlashInbox(clanRequests.Count > 0 || Singleton<InboxManager>.Instance.FriendRequests.Value.Count > 0 || Singleton<InboxManager>.Instance.UnreadMessageCount > 0);
	}

	private void HandlePendingChatMessages(bool hasUnreadMessages) {
		FlashChat(Singleton<ChatManager>.Instance.HasUnreadClanMessage || Singleton<ChatManager>.Instance.HasUnreadPrivateMessage);
	}

	private void FlashInbox(bool bFlash) {
		FlashMenuIcon(inboxFlashTween, bFlash);
	}

	private void FlashChat(bool bFlash) {
		FlashMenuIcon(chatFlashTween, bFlash);
	}

	private void FlashMenuIcon(UITweener buttonTween, bool bFlash) {
		if (buttonTween == null) {
			return;
		}

		if (!bFlash) {
			buttonTween.Reset();
			buttonTween.gameObject.GetComponent<UISprite>().alpha = 1f;
		}

		buttonTween.enabled = bFlash;
	}

	protected override IEnumerator OnBringIn() {
		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, playButton));
		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, shopButton));
		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, chatButton));

		if (ApplicationDataManager.IsDesktop) {
			yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, quitButton));
		}

		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, profileButton));
		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, inboxButton));
		yield return StartCoroutine(AnimateAlpha(1f, bringInDuration, clansButton));
	}

	protected override IEnumerator OnDismiss() {
		var duration = dismissDuration;

		if (ApplicationDataManager.IsDesktop) {
			yield return StartCoroutine(AnimateAlpha(0f, duration, profileButton, clansButton, chatButton, inboxButton, shopButton, playButton, quitButton));
		} else {
			yield return StartCoroutine(AnimateAlpha(0f, duration, profileButton, clansButton, chatButton, inboxButton, shopButton, playButton));
		}
	}
}
