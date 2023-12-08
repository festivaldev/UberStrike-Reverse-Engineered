using System;
using System.Collections.Generic;
using UnityEngine;

public class HUDMobileQuickItems : MonoBehaviour {
	private List<GameObject> availableSlots = new List<GameObject>();

	[SerializeField]
	private NGUIScrollList scrollList;

	[SerializeField]
	private GameObject selectorBackground;

	[SerializeField]
	private HUDQuickItem slot1;

	[SerializeField]
	private HUDQuickItem slot2;

	[SerializeField]
	private HUDQuickItem slot3;

	private void OnEnable() {
		GameData.Instance.OnQuickItemsChanged.Fire();
		UpdateActiveItemsInView();
	}

	private void Start() {
		AutoMonoBehaviour<TouchInput>.Instance.Shooter.IgnoreRect(new Rect(Screen.width - 240f, 200f, 240f, 100f));
		slot1.actionButton.OnClicked = (Action)(() => FireActiveQuickItem(slot1));
		slot2.actionButton.OnClicked = (Action)(() => FireActiveQuickItem(slot2));
		slot3.actionButton.OnClicked = (Action)(() => FireActiveQuickItem(slot3));
		availableSlots.Add(slot1.gameObject);
		availableSlots.Add(slot2.gameObject);
		availableSlots.Add(slot3.gameObject);

		GameData.Instance.OnQuickItemsChanged.AddEventAndFire((Action)(() => {
			var quickItems = Singleton<QuickItemController>.Instance.QuickItems;
			var currentSlotIndex = Singleton<QuickItemController>.Instance.CurrentSlotIndex;
			slot1.SetQuickItem(quickItems.Length <= 0 ? null : quickItems[0], 0 == currentSlotIndex);
			slot2.SetQuickItem(quickItems.Length <= 1 ? null : quickItems[1], 1 == currentSlotIndex);
			slot3.SetQuickItem(quickItems.Length <= 2 ? null : quickItems[2], 2 == currentSlotIndex);
		}), (MonoBehaviour)this);

		UpdateActiveItemsInView();

		GameData.Instance.OnQuickItemsCooldown.AddEventAndFire<int, float>((Action<int, float>)((index, progress) => {
			var currentSlotIndex = Singleton<QuickItemController>.Instance.CurrentSlotIndex;

			switch (index) {
				case 0:
					slot1.SetCooldown(progress, 0 == currentSlotIndex);

					break;
				case 1:
					slot2.SetCooldown(progress, 1 == currentSlotIndex);

					break;
				case 2:
					slot3.SetCooldown(progress, 2 == currentSlotIndex);

					break;
			}
		}), (MonoBehaviour)this);
	}

	private void UpdateActiveItemsInView() {
		var activeSlots = new List<GameObject>();

		availableSlots.ForEach(delegate(GameObject el) {
			if (el.activeInHierarchy) {
				activeSlots.Add(el);
			}
		});

		scrollList.SetActiveElements(activeSlots);
		selectorBackground.SetActive(activeSlots.Count > 1);
	}

	private void FireActiveQuickItem(HUDQuickItem item) {
		switch (GetSlotIndex(item)) {
			case 0:
				EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.QuickItem1, 1f));

				break;
			case 1:
				EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.QuickItem2, 1f));

				break;
			case 2:
				EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.QuickItem3, 1f));

				break;
		}
	}

	private int GetSlotIndex(HUDQuickItem item) {
		if (availableSlots.Contains(item.gameObject)) {
			return availableSlots.IndexOf(item.gameObject);
		}

		return -1;
	}
}
