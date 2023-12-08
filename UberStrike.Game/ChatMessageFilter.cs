using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class ChatMessageFilter {
	private static LimitedQueue<Message> _lastMessages = new LimitedQueue<Message>(5);

	public static bool IsSpamming(string message) {
		var flag = false;
		var flag2 = false;
		var num = 0f;
		var num2 = 0f;
		var num3 = 0;
		var text = string.Empty;

		foreach (var message2 in _lastMessages) {
			if (message2.Time + 5f > Time.time) {
				if (message.StartsWith(message2.Text, StringComparison.InvariantCultureIgnoreCase)) {
					message2.Time = Time.time;
					message2.Count++;
					flag = message2.Count > 1;
					flag2 = true;
				}

				if (num2 != 0f) {
					num += Mathf.Clamp(1f - (message2.Time - num2), 0f, 1f);
					num3++;
				}
			}

			num2 = message2.Time;
			text = message2.Text;
		}

		if (!flag2) {
			_lastMessages.Enqueue(new Message(message));
		}

		if (message.Equals(text, StringComparison.InvariantCultureIgnoreCase) && num2 + 10f > Time.time) {
			flag = true;
		}

		if (num3 > 0) {
			num /= num3;
		}

		flag |= num > 0.3f;

		return flag;
	}

	public static string Cleanup(string msg) {
		return TextUtilities.ShortenText(TextUtilities.Trim(msg), 140, false);
	}

	private class Message {
		public int Count;
		public string Text;
		public float Time;

		public Message(string text) {
			Text = text;
			Time = UnityEngine.Time.time;
		}
	}
}
