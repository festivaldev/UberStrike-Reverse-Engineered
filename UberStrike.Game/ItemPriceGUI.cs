using System;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UnityEngine;

public abstract class ItemPriceGUI {
	protected bool _levelLocked;
	protected Action<ItemPrice> _onPriceSelected;
	protected ItemPrice _selectedPrice;
	protected string _tooltip = string.Empty;
	public int Height { get; protected set; }

	public ItemPrice SelectedPriceOption {
		get { return _selectedPrice; }
	}

	public ItemPriceGUI(int levelLock, Action<ItemPrice> onPriceSelected) {
		if (levelLock > PlayerDataManager.PlayerLevel) {
			_levelLocked = true;
			_tooltip = string.Format("Not so fast, squirt!\n\nYou need to be Level {0} to buy this item using points.\n\nGet fragging!", levelLock);
		}

		_onPriceSelected = delegate(ItemPrice price) { _selectedPrice = price; };
		_onPriceSelected = (Action<ItemPrice>)Delegate.Combine(_onPriceSelected, onPriceSelected);
	}

	public abstract void Draw(Rect rect);

	protected int DrawPrice(ItemPrice price, float width, int y) {
		var text = ((price.Price <= 0) ? " FREE" : string.Format(" {0:N0}", price.Price));
		Texture texture = ((price.Currency != UberStrikeCurrencyType.Points) ? ShopIcons.IconCredits20x20 : ShopIcons.IconPoints20x20);
		var guicontent = new GUIContent(text, texture);
		GUI.Label(new Rect(width, y, width, 20f), guicontent, BlueStonez.label_itemdescription);

		if (price.Price > 0 && price.Discount > 0) {
			var text2 = string.Format(LocalizedStrings.DiscountPercentOff, price.Discount);
			GUI.color = ColorScheme.UberStrikeYellow;
			GUI.Label(new Rect(width + 80f, y + 5, width, 20f), text2, BlueStonez.label_itemdescription);
			GUI.color = Color.white;
		}

		return y += 24;
	}
}
