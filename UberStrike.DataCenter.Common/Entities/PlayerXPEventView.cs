using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class PlayerXPEventView {
		public int PlayerXPEventId { get; set; }
		public string Name { get; set; }
		public decimal XPMultiplier { get; set; }
		public PlayerXPEventView() { }

		public PlayerXPEventView(string name, decimal xpMultiplier) {
			Name = name;
			XPMultiplier = xpMultiplier;
		}

		public PlayerXPEventView(int playerXPEventId, string name, decimal xpMultiplier) : this(name, xpMultiplier) {
			PlayerXPEventId = playerXPEventId;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[PlayerXPEventView: ");
			stringBuilder.Append("[PlayerXPEventId: ");
			stringBuilder.Append(PlayerXPEventId);
			stringBuilder.Append("][Name: ");
			stringBuilder.Append(Name);
			stringBuilder.Append("][XPMultiplier: ");
			stringBuilder.Append(XPMultiplier);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
