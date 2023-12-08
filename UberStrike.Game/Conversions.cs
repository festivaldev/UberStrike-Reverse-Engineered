using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public static class Conversions {
	public static GameModeType GetGameModeType(this GameMode mode) {
		if (mode == GameMode.TeamDeathMatch) {
			return GameModeType.TeamDeathMatch;
		}

		if (mode == GameMode.DeathMatch) {
			return GameModeType.DeathMatch;
		}

		if (mode != GameMode.TeamElimination) {
			return GameModeType.None;
		}

		return GameModeType.EliminationMode;
	}

	public static GUIContent PriceTag(this ItemPrice price, bool printCurrency = false, string tooltip = "") {
		var currency = price.Currency;

		if (currency == UberStrikeCurrencyType.Credits) {
			return new GUIContent(price.Price.ToString("N0") + ((!printCurrency) ? string.Empty : "Credits"), ShopIcons.IconCredits20x20, tooltip);
		}

		if (currency != UberStrikeCurrencyType.Points) {
			return new GUIContent("N/A");
		}

		return new GUIContent(price.Price.ToString("N0") + ((!printCurrency) ? string.Empty : "Points"), ShopIcons.IconPoints20x20, tooltip);
	}
}
