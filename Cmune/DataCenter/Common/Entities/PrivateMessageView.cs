using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class PrivateMessageView {
		public int PrivateMessageId { get; set; }
		public int FromCmid { get; set; }
		public string FromName { get; set; }
		public int ToCmid { get; set; }
		public DateTime DateSent { get; set; }
		public string ContentText { get; set; }
		public bool IsRead { get; set; }
		public bool HasAttachment { get; set; }
		public bool IsDeletedBySender { get; set; }
		public bool IsDeletedByReceiver { get; set; }

		public override string ToString() {
			var text = "[Private Message: ";
			var text2 = text;
			text = string.Concat(text2, "[ID:", PrivateMessageId, "][From:", FromCmid, "][To:", ToCmid, "][Date:", DateSent, "][");
			text2 = text;
			text = string.Concat(text2, "[Content:", ContentText, "][Is Read:", IsRead, "][Has attachment:", HasAttachment, "][Is deleted by sender:", IsDeletedBySender, "][Is deleted by receiver:", IsDeletedByReceiver, "]");

			return text + "]";
		}
	}
}
