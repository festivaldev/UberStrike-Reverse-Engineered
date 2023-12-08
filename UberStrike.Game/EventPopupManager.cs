using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPopupManager : Singleton<EventPopupManager> {
	private Queue<IPopupDialog> popups;

	private EventPopupManager() {
		popups = new Queue<IPopupDialog>();
	}

	public void AddEventPopup(IPopupDialog popup) {
		popups.Enqueue(popup);
	}

	public void ShowNextPopup(int delay = 0) {
		if (popups.Count > 0) {
			UnityRuntime.StartRoutine(ShowPopup(popups.Dequeue(), delay));
		}
	}

	private IEnumerator ShowPopup(IPopupDialog popup, int delay) {
		yield return new WaitForSeconds(delay);

		PopupSystem.Show(popup);
	}
}
