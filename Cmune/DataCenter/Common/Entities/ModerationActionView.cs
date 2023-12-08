using System;

namespace Cmune.DataCenter.Common.Entities {
	public class ModerationActionView {
		public int ModerationActionId { get; private set; }
		public ModerationActionType ActionType { get; private set; }
		public int SourceCmid { get; private set; }
		public string SourceName { get; private set; }
		public int TargetCmid { get; private set; }
		public string TargetName { get; private set; }
		public DateTime ActionDate { get; private set; }
		public int ApplicationId { get; private set; }
		public string Reason { get; private set; }
		public long SourceIp { get; private set; }

		public ModerationActionView(int moderationActionId, ModerationActionType actionType, int sourceCmid, string sourceName, int targetCmid, string targetName, DateTime actionDate, int applicationId, string reason, long sourceIp) {
			ModerationActionId = moderationActionId;
			ActionType = actionType;
			SourceCmid = sourceCmid;
			SourceName = sourceName;
			TargetCmid = targetCmid;
			TargetName = targetName;
			ActionDate = actionDate;
			ApplicationId = applicationId;
			Reason = reason;
			SourceIp = sourceIp;
		}
	}
}
