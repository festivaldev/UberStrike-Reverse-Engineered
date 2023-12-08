using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ContactRequestView {
		public int RequestId { get; set; }
		public int InitiatorCmid { get; set; }
		public string InitiatorName { get; set; }
		public int ReceiverCmid { get; set; }
		public string InitiatorMessage { get; set; }
		public ContactRequestStatus Status { get; set; }
		public DateTime SentDate { get; set; }
		public ContactRequestView() { }

		public ContactRequestView(int initiatorCmid, int receiverCmid, string initiatorMessage) {
			InitiatorCmid = initiatorCmid;
			ReceiverCmid = receiverCmid;
			InitiatorMessage = initiatorMessage;
		}

		public ContactRequestView(int requestID, int initiatorCmid, string initiatorName, int receiverCmid, string initiatorMessage, ContactRequestStatus status, DateTime sentDate) {
			SetContactRequest(requestID, initiatorCmid, initiatorName, receiverCmid, initiatorMessage, status, sentDate);
		}

		public void SetContactRequest(int requestID, int initiatorCmid, string initiatorName, int receiverCmid, string initiatorMessage, ContactRequestStatus status, DateTime sentDate) {
			RequestId = requestID;
			InitiatorCmid = initiatorCmid;
			InitiatorName = initiatorName;
			ReceiverCmid = receiverCmid;
			InitiatorMessage = initiatorMessage;
			Status = status;
			SentDate = sentDate;
		}

		public override string ToString() {
			var text = string.Concat("[Request contact: [Request ID: ", RequestId, "][Initiator Cmid :", InitiatorCmid, "][Initiator Name:", InitiatorName, "][Receiver Cmid: ", ReceiverCmid, "]");
			var text2 = text;

			return string.Concat(text2, "[Initiator Message: ", InitiatorMessage, "][Status: ", Status, "][Sent Date: ", SentDate, "]]");
		}
	}
}
