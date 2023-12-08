using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class PackItemPriceGUI : ItemPriceGUI {
	private List<ItemPrice> _prices;

	public PackItemPriceGUI(IUnityItem item, Action<ItemPrice> onPriceSelected) : base(item.View.LevelLock, onPriceSelected) {
		_prices = new List<ItemPrice>(item.View.Prices);

		if (_prices.Count > 1) {
			_onPriceSelected(_prices[1]);
		} else {
			_onPriceSelected(_prices[0]);
		}
	}

	public override void Draw(Rect rect) {
		GUI.BeginGroup(rect);
		var num = 30;
		GUI.Label(new Rect(0f, 0f, rect.width, 16f), "Purchase Options", BlueStonez.label_interparkbold_16pt_left);

		foreach (var itemPrice in _prices) {
			var guicontent = new GUIContent(itemPrice.Amount + " Uses");

			if (_levelLocked && itemPrice.Currency == UberStrikeCurrencyType.Points) {
				GUI.enabled = false;
				guicontent.tooltip = _tooltip;
			}

			if (GUI.Toggle(new Rect(0f, num, rect.width, 20f), _selectedPrice == itemPrice, guicontent, BlueStonez.toggle) && itemPrice != _selectedPrice) {
				_onPriceSelected(itemPrice);
			}

			num = DrawPrice(itemPrice, rect.width * 0.5f, num);
			GUI.enabled = true;
		}

		Height = num;
		GUI.EndGroup();
	}
}
