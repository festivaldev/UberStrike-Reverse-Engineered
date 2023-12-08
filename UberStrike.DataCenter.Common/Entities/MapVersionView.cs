using System;

namespace UberStrike.DataCenter.Common.Entities {
	public class MapVersionView {
		public string FileName { get; private set; }
		public DateTime LastUpdatedDate { get; set; }

		public MapVersionView(string fileName, DateTime lastUpdatedDate) {
			FileName = fileName;
			LastUpdatedDate = lastUpdatedDate;
		}
	}
}
