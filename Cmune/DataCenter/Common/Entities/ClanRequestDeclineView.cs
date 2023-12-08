using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ClanRequestDeclineView {
		public int ActionResult { get; set; }
		public int ClanRequestId { get; set; }
	}
}
