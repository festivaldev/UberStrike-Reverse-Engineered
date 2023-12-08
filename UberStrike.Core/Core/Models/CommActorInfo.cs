using System;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Models {
	[Serializable]
	[Synchronizable]
	public class CommActorInfo {
		public int Cmid { get; set; }
		public string PlayerName { get; set; }
		public MemberAccessLevel AccessLevel { get; set; }
		public ChannelType Channel { get; set; }
		public string ClanTag { get; set; }
		public GameRoom CurrentRoom { get; set; }
		public byte ModerationFlag { get; set; }
		public string ModInformation { get; set; }
	}
}
