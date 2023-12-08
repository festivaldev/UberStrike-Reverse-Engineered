using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class RentItemPriceGUI : ItemPriceGUI {
	private List<ItemPrice> _prices;

	public RentItemPriceGUI(IUnityItem item, Action<ItemPrice> onPriceSelected) : base(item.View.LevelLock, onPriceSelected) {
		_prices = new List<ItemPrice>(item.View.Prices);

		if (_prices.Count > 0) {
			_onPriceSelected(_prices[_prices.Count - 1]);
		}
	}

	public override void Draw(Rect rect) {
		GUI.BeginGroup(rect);
		var num = 30;

		if (_prices.Exists(p => p.Duration != BuyingDurationType.Permanent)) {
			GUI.Label(new Rect(0f, 0f, rect.width, 16f), "Limited Use", BlueStonez.label_interparkbold_16pt_left);

			foreach (var itemPrice in _prices) {
				if (itemPrice.Duration != BuyingDurationType.Permanent) {
					var guicontent = new GUIContent(ShopUtils.PrintDuration(itemPrice.Duration));

					if (itemPrice.Currency == UberStrikeCurrencyType.Points && _levelLocked) {
						GUI.enabled = false;
						guicontent.tooltip = _tooltip;
					}

					if (GUI.Toggle(new Rect(0f, num, rect.width, 20f), _selectedPrice == itemPrice, guicontent, BlueStonez.toggle) && itemPrice != _selectedPrice) {
						_onPriceSelected(itemPrice);
					}

					num = DrawPrice(itemPrice, rect.width * 0.5f, num);
					GUI.enabled = true;
				}
			}

			num += 20;
		}

		if (_prices.Exists(p => p.Duration == BuyingDurationType.Permanent)) {
			GUI.Label(new Rect(0f, num, rect.width, 16f), "Unlimited Use", BlueStonez.label_interparkbold_16pt_left);
			num += 30;

			foreach (var itemPrice2 in _prices) {
				if (itemPrice2.Duration == BuyingDurationType.Permanent) {
					var empty = string.Empty;

					if (GUI.Toggle(new Rect(0f, num, rect.width, 20f), _selectedPrice == itemPrice2, new GUIContent(LocalizedStrings.Permanent, empty), BlueStonez.toggle) && itemPrice2 != _selectedPrice) {
						_onPriceSelected(itemPrice2);
					}

					num = DrawPrice(itemPrice2, rect.width * 0.5f, num);
				}
			}
		}

		Height = num;
		GUI.EndGroup();
	}

	private string GetRentDuration(BuyingDurationType duration) {
		var text = string.Empty;

		if (duration != BuyingDurationType.OneDay) {
			if (duration == BuyingDurationType.SevenDays) {
				text = LocalizedStrings.SevenDays;
			}
		} else {
			text = LocalizedStrings.OneDay;
		}

		return text;
	}
}
