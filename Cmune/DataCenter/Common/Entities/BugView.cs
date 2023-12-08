using System;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class BugView {
		public string Content { get; set; }
		public string Subject { get; set; }
		public BugView() { }

		public BugView(string subject, string content) {
			Subject = subject.Trim();
			Content = content.Trim();
		}

		public override string ToString() {
			return string.Concat("[Bug: [Subject: ", Subject, "][Content :", Content, "]]");
		}
	}
}
