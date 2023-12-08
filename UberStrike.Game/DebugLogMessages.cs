using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugLogMessages : IDebugPage {
	public static readonly ConsoleDebug Console = new ConsoleDebug();

	public string Title {
		get { return "Logs"; }
	}

	public void Draw() {
		GUILayout.TextArea(Console.DebugOut);
	}

	public static void Log(int type, string msg) {
		Console.Log(type, msg);
	}

	public class ConsoleDebug {
		private string _debugOut = string.Empty;
		private LimitedQueue<string> _queue = new LimitedQueue<string>(300);

		public string DebugOut {
			get { return _debugOut; }
		}

		public void Log(int level, string s) {
			_queue.Enqueue(s);
			var stringBuilder = new StringBuilder();

			foreach (var text in _queue) {
				stringBuilder.AppendLine(text);
			}

			_debugOut = stringBuilder.ToString();
		}

		public string ToHTML() {
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<h3>DEBUG LOG</h3>");

			foreach (var text in _queue) {
				stringBuilder.AppendLine(text + "<br/>");
			}

			return stringBuilder.ToString();
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();

			foreach (var text in _queue) {
				stringBuilder.AppendLine(text);
			}

			return stringBuilder.ToString();
		}
	}
}
