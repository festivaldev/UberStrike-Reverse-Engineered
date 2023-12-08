using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Models {
	public class CommActorInfoDelta {
		public enum Keys {
			AccessLevel,
			Channel,
			ClanTag,
			Cmid,
			CurrentRoom,
			ModerationFlag,
			ModInformation,
			PlayerName
		}

		public readonly Dictionary<Keys, object> Changes = new Dictionary<Keys, object>();
		public int DeltaMask { get; set; }
		public byte Id { get; set; }

		public void Apply(CommActorInfo instance) {
			foreach (var keyValuePair in Changes) {
				switch (keyValuePair.Key) {
					case Keys.AccessLevel:
						instance.AccessLevel = (MemberAccessLevel)((int)keyValuePair.Value);

						break;
					case Keys.Channel:
						instance.Channel = (ChannelType)((int)keyValuePair.Value);

						break;
					case Keys.ClanTag:
						instance.ClanTag = (string)keyValuePair.Value;

						break;
					case Keys.Cmid:
						instance.Cmid = (int)keyValuePair.Value;

						break;
					case Keys.CurrentRoom:
						instance.CurrentRoom = (GameRoom)keyValuePair.Value;

						break;
					case Keys.ModerationFlag:
						instance.ModerationFlag = (byte)keyValuePair.Value;

						break;
					case Keys.ModInformation:
						instance.ModInformation = (string)keyValuePair.Value;

						break;
					case Keys.PlayerName:
						instance.PlayerName = (string)keyValuePair.Value;

						break;
				}
			}
		}
	}
}
