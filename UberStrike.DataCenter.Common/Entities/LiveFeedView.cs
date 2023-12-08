using System;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class LiveFeedView {
		public DateTime Date { get; set; }
		public int Priority { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public int LivedFeedId { get; set; }

		public LiveFeedView() {
			Date = DateTime.UtcNow;
			Priority = 0;
			Description = string.Empty;
			Url = string.Empty;
			LivedFeedId = 0;
		}

		public LiveFeedView(DateTime date, int priority, string description, string url, int liveFeedId) {
			Date = date;
			Priority = priority;
			Description = description;
			Url = url;
			LivedFeedId = liveFeedId;
		}
	}
}
