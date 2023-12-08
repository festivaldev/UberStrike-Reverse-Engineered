using System;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UnityEngine;

public class InstantMessage {
	public int Cmid { get; private set; }
	public float Height { get; private set; }
	public string PlayerName { get; private set; }
	public string Text { get; private set; }
	public string TimeString { get; private set; }
	public DateTime ArrivalTime { get; private set; }
	public MemberAccessLevel AccessLevel { get; private set; }
	public bool IsFriend { get; private set; }
	public bool IsFacebookFriend { get; private set; }
	public bool IsClan { get; private set; }
	public ChatContext Context { get; private set; }

	public bool IsNotification {
		get { return string.IsNullOrEmpty(PlayerName); }
	}

	public CommActorInfo Actor { get; private set; }

	public InstantMessage(int cmid, string playerName, string messageText, MemberAccessLevel level, ChatContext context = ChatContext.None) {
		Cmid = cmid;
		PlayerName = playerName;
		Text = messageText;
		AccessLevel = level;
		ArrivalTime = DateTime.Now;
		TimeString = ArrivalTime.ToString("t");
		Context = context;
		IsFriend = PlayerDataManager.IsFriend(Cmid);
		IsFacebookFriend = PlayerDataManager.IsFacebookFriend(Cmid);
		IsClan = PlayerDataManager.IsClanMember(Cmid);
	}

	public InstantMessage(int cmid, string playerName, string messageText, MemberAccessLevel level, CommActorInfo actor, ChatContext context = ChatContext.None) {
		Cmid = cmid;
		PlayerName = playerName;
		Text = messageText;
		AccessLevel = level;
		ArrivalTime = DateTime.Now;
		TimeString = ArrivalTime.ToString("t");
		Context = context;
		IsFriend = PlayerDataManager.IsFriend(Cmid);
		IsFacebookFriend = PlayerDataManager.IsFacebookFriend(Cmid);
		IsClan = PlayerDataManager.IsClanMember(Cmid);
		Actor = actor;
	}

	public InstantMessage(InstantMessage instantMessage) {
		Cmid = instantMessage.Cmid;
		PlayerName = instantMessage.PlayerName;
		Text = instantMessage.Text;
		TimeString = instantMessage.TimeString;
		AccessLevel = instantMessage.AccessLevel;
		Context = instantMessage.Context;
		IsFriend = PlayerDataManager.IsFriend(Cmid);
		IsFacebookFriend = PlayerDataManager.IsFacebookFriend(Cmid);
		IsClan = PlayerDataManager.IsClanMember(Cmid);
	}

	public void UpdateHeight(GUIStyle style, float width, int offset = 0, bool isMuted = false) {
		Height = ((!isMuted) ? (style.CalcHeight(new GUIContent(Text), width) + offset) : 0f);
	}

	public void Append(string message) {
		Text = Text + "\n" + message;
	}
}
