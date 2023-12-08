using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InboxThread {
	public const int AdminCmid = 767;
	public const int NameWidth = 100;
	public const int ThreadHeight = 76;
	private int _curPageIndex;
	private SortedList<int, InboxMessage> _messages;
	private bool _messagesLoaded;
	private MessageThreadView _threadView;
	public Vector2 Scroll;
	public static InboxThread Current { get; set; }
	public bool IsLoading { get; set; }
	public DateTime LastServerUpdate { get; private set; }

	public int ThreadId {
		get { return _threadView.ThreadId; }
	}

	public string Name {
		get { return _threadView.ThreadName; }
	}

	public DateTime LastMessageDateTime {
		get { return _threadView.LastUpdate; }
	}

	public IEnumerable<InboxMessage> Messages {
		get { return _messages.Values; }
	}

	public bool HasUnreadMessage {
		get { return _threadView.HasNewMessages; }
	}

	private string Date {
		get { return string.Concat(_threadView.LastUpdate.ToString("yyyy MMM "), " ", _threadView.LastUpdate.Day.ToString(), " at ", _threadView.LastUpdate.ToShortTimeString()); }
	}

	public bool IsAdmin {
		get { return ThreadId == 767; }
	}

	public InboxThread(MessageThreadView threadView) {
		_threadView = threadView;
		_messages = new SortedList<int, InboxMessage>(threadView.MessageCount, new MessageSorter());
		LastServerUpdate = _threadView.LastUpdate;
	}

	public bool Contains(string keyword) {
		var flag = false;
		var text = keyword.ToLower();

		if (_threadView.ThreadName.ToLower().Contains(text)) {
			return true;
		}

		foreach (var inboxMessage in _messages.Values) {
			if (inboxMessage.Content.ToLower().Contains(text)) {
				flag = true;

				break;
			}
		}

		return flag;
	}

	public int DrawThread(int y, int width) {
		var rect = new Rect(8f, y + 8, width - 8, 68f);

		if (Current == this) {
			GUI.Box(new Rect(4f, y + 4, width, 76f), GUIContent.none, BlueStonez.box_grey50);
		}

		GUI.BeginGroup(rect);
		GUI.Label(new Rect(0f, 0f, width, 18f), string.Format("{0} ({1})", _threadView.ThreadName, _threadView.MessageCount), BlueStonez.label_interparkbold_13pt);
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.Label(new Rect(0f, 20f, width, 10f), Date, BlueStonez.label_interparkmed_10pt_left);
		GUI.color = Color.white;
		GUI.Label(new Rect(0f, 50f, width, 18f), _threadView.LastMessagePreview, BlueStonez.label_interparkmed_10pt_left);
		GUI.EndGroup();
		var rect2 = new Rect(width - 18, y + 9, 16f, 16f);

		if (GUI.enabled && rect.Contains(Event.current.mousePosition)) {
			GUI.Box(new Rect(4f, y + 4, width, 76f), GUIContent.none, BlueStonez.group_grey81);

			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !rect2.Contains(Event.current.mousePosition)) {
				Current = this;
				Scroll.y = float.MinValue;

				if (!_messagesLoaded) {
					_messagesLoaded = true;
					Singleton<InboxManager>.Instance.LoadMessagesForThread(this, 0);
				}

				if (_threadView.HasNewMessages) {
					_threadView.HasNewMessages = false;
					Singleton<InboxManager>.Instance.MarkThreadAsRead(_threadView.ThreadId);
				}

				Event.current.Use();
			}
		}

		if (_threadView.HasNewMessages) {
			GUI.Label(new Rect(width - 40, y + 5, 29f, 29f), CommunicatorIcons.NewInboxMessage);
		}

		return y + 76 + 8;
	}

	public int DrawMessageList(int y, int scrollRectWidth, float scrollRectHeight, float curScrollY) {
		for (var i = _messages.Values.Count - 1; i >= 0; i--) {
			var inboxMessage = _messages.Values[i];
			y += DrawContent(inboxMessage, y + 12, scrollRectWidth) + 16;
		}

		if (_messages.Count == 0) {
			GUI.Label(new Rect(0f, y, scrollRectWidth, 100f), "This thread is empty", BlueStonez.label_interparkbold_13pt);
		} else {
			var num = y - scrollRectHeight;
			num = Mathf.Clamp(num, 0f, num);

			if (curScrollY >= num && _threadView.MessageCount > _messages.Count && !IsLoading) {
				_curPageIndex++;
				Singleton<InboxManager>.Instance.LoadMessagesForThread(this, _curPageIndex);
			}
		}

		if (IsLoading) {
			GUI.Label(new Rect(0f, y, scrollRectWidth, 30f), "Loading messages...", BlueStonez.label_interparkbold_13pt);
			y += 30;
		}

		return y;
	}

	public int DrawContent(InboxMessage msg, int y, int width) {
		if (msg.IsMine) {
			return DrawMyMessage(msg, 100, y, width - 100);
		}

		return DrawOtherMessage(msg, 0, y, width - 100);
	}

	private int DrawOtherMessage(InboxMessage msg, int x, int y, int width) {
		var num = Mathf.RoundToInt(BlueStonez.speechbubble_left.CalcHeight(new GUIContent(msg.Content), width)) + 30;
		var rect = new Rect(x, y, width, num);
		GUI.color = new Color(0.5f, 0.5f, 0.5f);
		var num2 = (int)BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(msg.SenderName)).x;
		GUI.Label(new Rect(rect.x + 28f, rect.y - 16f, rect.width, 12f), msg.SenderName, BlueStonez.label_interparkbold_11pt_left);
		GUI.Label(new Rect(rect.x + num2 + 34f, rect.y - 15f, rect.width, 12f), msg.SentDateString, BlueStonez.label_interparkmed_10pt_left);
		GUI.color = Color.white;
		GUI.BeginGroup(rect);
		GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);

		if (ApplicationDataManager.IsMobile) {
			GUI.Label(new Rect(0f, 0f, rect.width, num), msg.Content, BlueStonez.speechbubble_left);
		} else {
			GUI.TextArea(new Rect(0f, 0f, rect.width, num), msg.Content, BlueStonez.speechbubble_left);
		}

		GUI.backgroundColor = Color.white;
		GUI.EndGroup();

		return num;
	}

	private int DrawMyMessage(InboxMessage msg, int x, int y, int width) {
		var num = Mathf.RoundToInt(BlueStonez.speechbubble_right.CalcHeight(new GUIContent(msg.Content), width)) + 30;
		var rect = new Rect(x, y, width, num);
		GUI.color = new Color(0.5f, 0.5f, 0.5f);
		var num2 = (int)BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(msg.SenderName)).x;
		var num3 = (int)BlueStonez.label_interparkmed_10pt_left.CalcSize(new GUIContent(msg.SentDateString)).x;
		GUI.Label(new Rect(rect.x + rect.width - (num3 + num2 + 40), rect.y - 16f, num2 + 2, 12f), msg.SenderName, BlueStonez.label_interparkbold_11pt_left);
		GUI.Label(new Rect(rect.x + rect.width - (num3 + 32), rect.y - 15f, num3 + 2, 12f), msg.SentDateString, BlueStonez.label_interparkmed_10pt_left);
		GUI.color = Color.white;
		GUI.BeginGroup(rect);
		GUI.backgroundColor = new Color(0.376f, 0.631f, 0.886f, 0.5f);

		if (ApplicationDataManager.IsMobile) {
			GUI.Label(new Rect(rect.width - rect.width, 0f, rect.width, num), msg.Content, BlueStonez.speechbubble_right);
		} else {
			GUI.TextArea(new Rect(rect.width - rect.width, 0f, rect.width, num), msg.Content, BlueStonez.speechbubble_right);
		}

		GUI.backgroundColor = Color.white;
		GUI.EndGroup();

		return num;
	}

	internal void UpdateThread(MessageThreadView newThreadView) {
		if (newThreadView.MessageCount != _threadView.MessageCount) {
			_messagesLoaded = false;
		}

		_threadView = newThreadView;
		LastServerUpdate = _threadView.LastUpdate;
	}

	internal void AddMessage(PrivateMessageView message) {
		if (!_messages.ContainsKey(message.PrivateMessageId)) {
			_messages.Add(message.PrivateMessageId, new InboxMessage(message, (message.FromCmid != PlayerDataManager.Cmid) ? _threadView.ThreadName : PlayerDataManager.Name));
			_threadView.MessageCount++;

			if (!message.IsRead && message.ToCmid == PlayerDataManager.Cmid) {
				_threadView.HasNewMessages = true;
			}

			if (message.DateSent > _threadView.LastUpdate) {
				_threadView.LastUpdate = message.DateSent;
				_threadView.LastMessagePreview = TextUtilities.ShortenText(message.ContentText, 25, true);
			}
		}

		Scroll.y = float.MinValue;
	}

	internal void AddMessages(List<PrivateMessageView> messages) {
		foreach (var privateMessageView in messages) {
			if (!_messages.ContainsKey(privateMessageView.PrivateMessageId)) {
				_messages.Add(privateMessageView.PrivateMessageId, new InboxMessage(privateMessageView, (privateMessageView.FromCmid != PlayerDataManager.Cmid) ? _threadView.ThreadName : PlayerDataManager.Name));
			}
		}
	}

	private class MessageSorter : IComparer<int> {
		public int Compare(int obj1, int obj2) {
			return obj1 - obj2;
		}
	}
}
