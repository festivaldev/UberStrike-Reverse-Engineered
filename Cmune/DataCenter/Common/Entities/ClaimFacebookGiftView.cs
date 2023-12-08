using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ClaimFacebookGiftView {
		public ClaimFacebookGiftResult ClaimResult { get; set; }
		public int? ItemId { get; set; }
		public ClaimFacebookGiftView() { }

		public ClaimFacebookGiftView(ClaimFacebookGiftResult _claimResult, int? _itemId) {
			ClaimResult = _claimResult;
			ItemId = _itemId;
		}
	}
}
