using System;

namespace Cmune.DataCenter.Common.Entities {
	public class ApplicationMilestoneView {
		public int MilestoneId { get; private set; }
		public int ApplicationId { get; private set; }
		public DateTime MilestoneDate { get; private set; }
		public string Description { get; private set; }

		public ApplicationMilestoneView(int milestoneId, int applicationId, DateTime milestoneDate, string description) {
			MilestoneId = milestoneId;
			ApplicationId = applicationId;
			MilestoneDate = milestoneDate;
			Description = description;
		}
	}
}
