using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ContactRequestAcceptView {
		public int ActionResult { get; set; }
		public PublicProfileView Contact { get; set; }
		public int RequestId { get; set; }
	}
}
