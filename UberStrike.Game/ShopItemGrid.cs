using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class ShopItemGrid {
	protected const int MAX_COLUMN = 6;
	protected List<ShopItemView> _items;
	private float offset = -300f;
	public List<bool> HighlightState { get; set; }
	public bool Show { get; set; }

	public List<ShopItemView> Items {
		get { return _items; }
	}

	public ShopItemGrid(List<BundleItemView> items, int credits = 0, int points = 0) {
		_items = new List<ShopItemView>(items.Count + 2);

		foreach (var bundleItemView in items) {
			_items.Add(new ShopItemView(bundleItemView));
		}

		if (credits > 0) {
			_items.Add(new ShopItemView(UberStrikeCurrencyType.Credits, credits));
		}

		if (points > 0) {
			_items.Add(new ShopItemView(UberStrikeCurrencyType.Points, points));
		}
	}

	public ShopItemGrid(List<ShopItemView> items, int credits = 0, int points = 0) {
		_items = items;

		if (credits > 0) {
			_items.Add(new ShopItemView(UberStrikeCurrencyType.Credits, credits));
		}

		if (points > 0) {
			_items.Add(new ShopItemView(UberStrikeCurrencyType.Points, points));
		}
	}

	public void Draw(Rect rect) {
		var num = rect.width / 6f;
		var num2 = _items.Count / 6 + ((_items.Count % 6 <= 0) ? 0 : 1);
		var list = new List<string>(_items.Count);
		offset = ((!Show) ? Mathf.Lerp(offset, -(float)num2 * num, Time.deltaTime * 5f) : Mathf.Lerp(offset, 0f, Time.deltaTime * 5f));
		GUI.BeginGroup(rect);

		for (var i = 0; i < num2; i++) {
			for (var j = 0; j < 6; j++) {
				var num3 = i * 6 + j;
				var rect2 = new Rect(j * num, rect.height - (i + 1) * num - offset, num, num);

				if (num3 < _items.Count) {
					if (HighlightState != null) {
						if (HighlightState[num3]) {
							DrawIcon(rect2, _items[num3].UnityItem, BlueStonez.item_slot_small);
							GUI.color = GUI.color.SetAlpha(GUITools.FastSinusPulse);
							GUI.DrawTexture(rect2, ShopIcons.ItemSlotSelected);
							GUI.color = Color.white;

							if (AutoMonoBehaviour<ItemToolTip>.Instance != null && Show && (rect2.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile) && offset < num) {
								AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(_items[num3].UnityItem, rect2, PopupViewSide.Top, -1, _items[num3].Duration);
							}
						} else {
							GUITools.PushGUIState();
							GUI.enabled = false;
							DrawIcon(rect2, _items[num3].UnityItem, BlueStonez.item_slot_alpha);
							GUITools.PopGUIState();
							list.Add(_items[num3].UnityItem.View.PrefabName);
						}
					} else {
						DrawIcon(rect2, _items[num3].UnityItem, BlueStonez.item_slot_alpha);

						if (AutoMonoBehaviour<ItemToolTip>.Instance != null && Show && rect2.Contains(Event.current.mousePosition) && offset < num) {
							AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(_items[num3].UnityItem, rect2, PopupViewSide.Top, -1, _items[num3].Duration);
						}
					}
				} else {
					GUITools.PushGUIState();
					GUI.enabled = false;
					GUI.Label(rect2, GUIContent.none, BlueStonez.item_slot_alpha);
					GUITools.PopGUIState();
				}
			}
		}

		GUI.EndGroup();
	}

	private void DrawIcon(Rect position, IUnityItem item, GUIStyle style) {
		if (item != null) {
			GUI.Label(position, GUIContent.none, style);
			item.DrawIcon(position);
		} else {
			GUI.Label(position, UberstrikeIconsHelper.White, style);
		}
	}
}
