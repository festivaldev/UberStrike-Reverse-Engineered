using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class CurrencyDepositsViewModel {
		public List<CurrencyDepositView> CurrencyDeposits { get; set; }
		public int TotalCount { get; set; }
	}
}
