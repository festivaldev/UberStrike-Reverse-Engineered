using System.Collections.Generic;
using UnityEngine;

public class ChatGroupPanel {
	private readonly List<ChatGroup> chatGroups;
	public Vector2 Scroll { get; set; }
	public string SearchText { get; set; }
	public float ContentHeight { get; set; }
	public float WindowHeight { get; set; }

	public IEnumerable<ChatGroup> Groups {
		get { return chatGroups; }
	}

	public ChatGroupPanel() {
		SearchText = string.Empty;
		chatGroups = new List<ChatGroup>();
	}

	public void AddGroup(UserGroups group, string name, ICollection<CommUser> users) {
		chatGroups.Add(new ChatGroup(group, name, users));
	}

	public void ScrollToUser(int cmid) {
		var total = 0f;
		var num = 0;

		foreach (var chatGroup in chatGroups) {
			foreach (var commUser in chatGroup.Players) {
				num++;

				if (commUser.Cmid == cmid) {
					break;
				}
			}
		}

		chatGroups.ForEach(delegate(ChatGroup g) { total += g.Players.Count; });
		var num2 = ContentHeight * num / total;
		Scroll = new Vector2(Scroll.x, num2);
	}
}
