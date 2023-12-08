using System;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class UberstrikeMemberView {
		public PlayerCardView PlayerCardView { get; set; }
		public PlayerStatisticsView PlayerStatisticsView { get; set; }
		public UberstrikeMemberView() { }

		public UberstrikeMemberView(PlayerCardView playerCardView, PlayerStatisticsView playerStatisticsView) {
			PlayerCardView = playerCardView;
			PlayerStatisticsView = playerStatisticsView;
		}

		public override string ToString() {
			var text = "[Uberstrike member view: ";

			if (PlayerCardView != null) {
				text += PlayerCardView.ToString();
			} else {
				text += "null";
			}

			if (PlayerStatisticsView != null) {
				text += PlayerStatisticsView.ToString();
			} else {
				text += "null";
			}

			return text + "]";
		}
	}
}
