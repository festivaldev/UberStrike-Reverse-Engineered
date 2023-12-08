using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class PointDepositView {
		public int PointDepositId { get; set; }
		public DateTime DepositDate { get; set; }
		public int Points { get; set; }
		public int Cmid { get; set; }
		public bool IsAdminAction { get; set; }
		public PointsDepositType DepositType { get; set; }
		public PointDepositView() { }

		public PointDepositView(int pointDepositId, DateTime depositDate, int points, int cmid, bool isAdminAction, PointsDepositType despositType) {
			PointDepositId = pointDepositId;
			DepositDate = depositDate;
			Points = points;
			Cmid = cmid;
			IsAdminAction = isAdminAction;
			DepositType = despositType;
		}
	}
}
