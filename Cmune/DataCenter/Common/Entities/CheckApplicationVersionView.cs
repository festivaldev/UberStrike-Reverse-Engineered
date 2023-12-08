using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class CheckApplicationVersionView {
		public ApplicationView ClientVersion { get; set; }
		public ApplicationView CurrentVersion { get; set; }
		public CheckApplicationVersionView() { }

		public CheckApplicationVersionView(ApplicationView clienVersion, ApplicationView currentVersion) {
			ClientVersion = clienVersion;
			CurrentVersion = currentVersion;
		}

		public override string ToString() {
			return string.Concat("[CheckApplicationVersionView: [ClientVersion: ", ClientVersion, "][CurrentVersion: ", CurrentVersion, "]]");
		}
	}
}
