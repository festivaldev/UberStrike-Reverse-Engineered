// # LEGACY # //
using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class WeeklySpecialView {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public string ImageUrl { get; set; }
		public int ItemId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public WeeklySpecialView() { }

		public WeeklySpecialView(string title, string text, string imageUrl, int itemId) {
			Title = title;
			Text = text;
			ImageUrl = imageUrl;
			ItemId = itemId;
		}

		public WeeklySpecialView(string title, string text, string imageUrl, int itemId, int id, DateTime startDate, DateTime? endDate) : this(title, text, imageUrl, itemId) {
			Id = id;
			StartDate = startDate;
			EndDate = endDate;
		}
	}
}
