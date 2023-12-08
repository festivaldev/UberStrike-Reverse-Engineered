using System;
using Cmune.DataCenter.Common.Entities;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class MemberAuthenticationResultView {
		public MemberAuthenticationResult MemberAuthenticationResult { get; set; }
		public MemberView MemberView { get; set; }
		public PlayerStatisticsView PlayerStatisticsView { get; set; }
		public DateTime ServerTime { get; set; }
		public bool IsAccountComplete { get; set; }
		public LuckyDrawUnityView LuckyDraw { get; set; }
		public string AuthToken { get; set; }
		public bool IsTutorialComplete { get; set; } // # LEGACY # //
		public WeeklySpecialView WeeklySpecial { get; set; } // # LEGACY # //
	}
}
