namespace Cmune.DataCenter.Common.Entities {
	public class EpinView {
		public int EpinId { get; private set; }
		public string Pin { get; private set; }
		public bool IsRedeemed { get; private set; }
		public int BatchId { get; private set; }
		public bool IsRetired { get; private set; }

		public EpinView(int epinId, string pin, bool isRedeemed, int batchId, bool isRetired) {
			EpinId = epinId;
			Pin = pin;
			IsRedeemed = isRedeemed;
			BatchId = batchId;
			IsRetired = isRetired;
		}
	}
}
