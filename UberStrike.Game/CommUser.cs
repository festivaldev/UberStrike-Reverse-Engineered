using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;

public class CommUser {
	private string _name = string.Empty;
	public int Cmid { get; private set; }

	public string Name {
		get { return _name; }
		set {
			ShortName = value;
			_name = value;
			var num = _name.IndexOf("]");

			if (num > 0 && num + 1 < _name.Length) {
				ShortName = _name.Substring(num + 1);
			}
		}
	}

	public int ActorId { get; private set; }
	public string ShortName { get; private set; }
	public MemberAccessLevel AccessLevel { get; private set; }

	public PresenceType PresenceIndex {
		get {
			if (IsOnline) {
				return (!IsInGame) ? PresenceType.Online : PresenceType.InGame;
			}

			return PresenceType.Offline;
		}
	}

	public int ModerationFlag { get; private set; }
	public string ModerationInfo { get; private set; }
	public ChannelType Channel { get; private set; }
	public GameRoom CurrentGame { get; set; }
	public bool IsFriend { get; set; }
	public bool IsFacebookFriend { get; set; }
	public bool IsClanMember { get; set; }

	public bool IsInGame {
		get { return CurrentGame != null && CurrentGame.Number > 0 && CurrentGame.Server != null; }
	}

	public bool IsOnline { get; set; }

	public CommUser(CommActorInfo user) {
		SetActor(user);
	}

	public CommUser(GameActorInfo user) {
		Cmid = user.Cmid;
		Name = user.PlayerName;
		ActorId = user.Cmid;
	}

	public CommUser(PublicProfileView profile) {
		if (profile != null) {
			IsFriend = PlayerDataManager.IsFriend(profile.Cmid);
			IsFacebookFriend = PlayerDataManager.IsFacebookFriend(profile.Cmid);
			Cmid = profile.Cmid;
			AccessLevel = profile.AccessLevel;
			Name = ((!string.IsNullOrEmpty(profile.GroupTag)) ? ("[" + profile.GroupTag + "] " + profile.Name) : profile.Name);
		}
	}

	public CommUser(ClanMemberView member) {
		if (member != null) {
			IsClanMember = true;
			Cmid = member.Cmid;
			AccessLevel = MemberAccessLevel.Default;
			Name = ((!string.IsNullOrEmpty(PlayerDataManager.ClanTag)) ? ("[" + PlayerDataManager.ClanTag + "] " + member.Name) : member.Name);
		}
	}

	public override int GetHashCode() {
		return Cmid;
	}

	public void SetActor(CommActorInfo actor) {
		if (actor != null) {
			Cmid = actor.Cmid;
			AccessLevel = actor.AccessLevel;
			Name = ((!string.IsNullOrEmpty(actor.ClanTag)) ? ("[" + actor.ClanTag + "] " + actor.PlayerName) : actor.PlayerName);
			Channel = actor.Channel;
			ModerationFlag = actor.ModerationFlag;
			ModerationInfo = actor.ModInformation;
			CurrentGame = actor.CurrentRoom;
			IsOnline = true;
		} else {
			ActorId = 0;
			CurrentGame = null;
			IsOnline = false;
		}
	}
}
