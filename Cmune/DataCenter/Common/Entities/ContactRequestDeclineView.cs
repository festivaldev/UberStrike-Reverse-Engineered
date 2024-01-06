using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ContactRequestDeclineView {
		public int ActionResult { get; set; }
		public int RequestId { get; set; }
	}
}
