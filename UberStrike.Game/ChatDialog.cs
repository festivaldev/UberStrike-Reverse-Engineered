using System;
using System.Collections.Generic;
using System.Text;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class ChatDialog {
	public delegate bool CanShowMessage(ChatContext c);

	public Vector2 _contentSize;
	public Vector2 _frameSize;
	public float _heightCache;
	private InstantMessage _lastMessage;
	public Queue<InstantMessage> _msgQueue;
	private bool _reset;
	private string _title;
	private float _totalHeight;
	public CanShowMessage CanShow;

	public bool CanChat {
		get { return UserCmid == 0 || AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.HasPlayer(UserCmid); }
	}

	public string Title { get; private set; }
	public string UserName { get; private set; }
	public int UserCmid { get; private set; }
	public UserGroups Group { get; set; }
	public bool HasUnreadMessage { get; set; }

	public ICollection<InstantMessage> AllMessages {
		get { return new List<InstantMessage>(_msgQueue.ToArray()); }
	}

	public ChatDialog(string title = "") {
		UserName = string.Empty;
		Title = title;
		_msgQueue = new Queue<InstantMessage>();
		AddMessage(new InstantMessage(0, "Disclaimer", "Do not share your password or any other confidential information with anybody. The members of Cmune and the Uberstrike Moderators will never ask you to provide such information.", MemberAccessLevel.Admin));
	}

	public ChatDialog(CommUser user, UserGroups group) : this(string.Empty) {
		Group = group;

		if (user != null) {
			UserName = user.ShortName;
			UserCmid = user.Cmid;
			Title = "Chat with " + UserName;
		}
	}

	public void AddMessage(InstantMessage msg) {
		_reset = true;

		while (_msgQueue.Count > 200) {
			_msgQueue.Dequeue();
		}

		if (_lastMessage != null && _lastMessage.Cmid == msg.Cmid && _lastMessage.ArrivalTime.AddMinutes(1.0) > DateTime.Now && !msg.IsNotification && !_lastMessage.IsNotification) {
			_lastMessage.Append(msg.Text);
		} else {
			_msgQueue.Enqueue(msg);
			_lastMessage = msg;
		}
	}

	public void Clear() {
		_msgQueue.Clear();
		_lastMessage = null;
	}

	public void RecalulateBounds() {
		_reset = true;
	}

	public bool CheckSize(Rect rect) {
		if (_reset || rect.width != _frameSize.x || rect.height != _frameSize.y) {
			_reset = false;
			_frameSize.x = rect.width;
			_frameSize.y = rect.height;
			_contentSize.y = rect.height;

			if (_totalHeight < rect.height) {
				_totalHeight = 0f;
				_contentSize.x = rect.width;

				foreach (var instantMessage in _msgQueue) {
					instantMessage.UpdateHeight(BlueStonez.label_interparkbold_11pt_left_wrap, _contentSize.x - 8f, 24, Singleton<ChatManager>.Instance.IsMuted(instantMessage.Cmid));
					_totalHeight += instantMessage.Height;
				}
			} else {
				_totalHeight = 0f;
				_contentSize.x = rect.width - 17f;

				foreach (var instantMessage2 in _msgQueue) {
					instantMessage2.UpdateHeight(BlueStonez.label_interparkbold_11pt_left_wrap, _contentSize.x - 8f, 24, Singleton<ChatManager>.Instance.IsMuted(instantMessage2.Cmid));
					_totalHeight += instantMessage2.Height;
				}
			}

			_contentSize.y = _totalHeight;

			return true;
		}

		return false;
	}

	public override string ToString() {
		var stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Title: " + Title);
		stringBuilder.AppendLine("Group: " + Group);
		stringBuilder.AppendLine(string.Concat("User: ", UserName, " ", UserCmid));
		stringBuilder.AppendLine("CanChat: " + CanChat);

		return stringBuilder.ToString();
	}
}
