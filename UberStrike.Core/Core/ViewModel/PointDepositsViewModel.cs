using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class PointDepositsViewModel {
		public List<PointDepositView> PointDeposits { get; set; }
		public int TotalCount { get; set; }
	}
}
