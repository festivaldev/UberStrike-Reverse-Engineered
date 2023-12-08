using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class ItemQuickUseConfigView {
		public int ItemId { get; set; }
		public int LevelRequired { get; set; }
		public int UsesPerLife { get; set; }
		public int UsesPerRound { get; set; }
		public int UsesPerGame { get; set; }
		public int CoolDownTime { get; set; }
		public int WarmUpTime { get; set; }
		public QuickItemLogic BehaviourType { get; set; }
	}
}
