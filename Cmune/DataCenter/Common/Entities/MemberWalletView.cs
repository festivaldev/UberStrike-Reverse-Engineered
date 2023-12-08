using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class MemberWalletView {
		public int Cmid { get; set; }
		public int Credits { get; set; }
		public int Points { get; set; }
		public DateTime CreditsExpiration { get; set; }
		public DateTime PointsExpiration { get; set; }

		public MemberWalletView() {
			CreditsExpiration = DateTime.Today;
			PointsExpiration = DateTime.Today;
		}

		public MemberWalletView(int cmid, int? credits, int? points, DateTime? creditsExpiration, DateTime? pointsExpiration) {
			if (credits == null) {
				credits = 0;
			}

			if (points == null) {
				points = 0;
			}

			if (creditsExpiration == null) {
				creditsExpiration = DateTime.MinValue;
			}

			if (pointsExpiration == null) {
				pointsExpiration = DateTime.MinValue;
			}

			SetMemberWallet(cmid, credits.Value, points.Value, creditsExpiration.Value, pointsExpiration.Value);
		}

		public MemberWalletView(int cmid, int credits, int points, DateTime creditsExpiration, DateTime pointsExpiration) {
			SetMemberWallet(cmid, credits, points, creditsExpiration, pointsExpiration);
		}

		private void SetMemberWallet(int cmid, int credits, int points, DateTime creditsExpiration, DateTime pointsExpiration) {
			Cmid = cmid;
			Credits = credits;
			Points = points;
			CreditsExpiration = creditsExpiration;
			PointsExpiration = pointsExpiration;
		}

		public override string ToString() {
			var text = "[Wallet: ";
			var text2 = text;
			text = string.Concat(text2, "[CMID:", Cmid, "][Credits:", Credits, "][Credits Expiration:", CreditsExpiration, "][Points:", Points, "][Points Expiration:", PointsExpiration, "]");

			return text + "]";
		}
	}
}
