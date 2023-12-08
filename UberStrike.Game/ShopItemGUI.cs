using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class ShopItemGUI : BaseItemGUI {
	private ItemPrice _creditsPrice;
	private ItemPrice _pointsPrice;

	public ShopItemGUI(IUnityItem item, BuyingLocationType location) : base(item, location, BuyingRecommendationType.None) {
		_pointsPrice = ShopUtils.GetLowestPrice(item, UberStrikeCurrencyType.Points);
		_creditsPrice = ShopUtils.GetLowestPrice(item, UberStrikeCurrencyType.Credits);
	}

	public override void Draw(Rect rect, bool selected) {
		GUI.BeginGroup(rect);
		DrawIcon(new Rect(4f, 4f, 48f, 48f));
		DrawArmorOverlay();
		DrawPromotionalTag();
		DrawName(new Rect(63f, 10f, 220f, 20f));
		DrawLevelRequirement();

		if (Singleton<LoadoutManager>.Instance.IsItemEquipped(Item.View.ID)) {
			DrawUnequipButton(new Rect(rect.width - 52f, 4f, 52f, 52f), LocalizedStrings.Unequip);
		} else if (Singleton<InventoryManager>.Instance.Contains(Item.View.ID)) {
			if (Item.Equippable) {
				DrawEquipButton(new Rect(rect.width - 52f, 4f, 52f, 52f), LocalizedStrings.Equip);
			} else {
				DrawBuyButton(new Rect(rect.width - 52f, 4f, 52f, 52f), LocalizedStrings.Buy);
			}
		} else if (PlayerDataManager.PlayerLevel < Item.View.LevelLock) {
			GUI.color = new Color(1f, 1f, 1f, 0.1f);
			GUI.DrawTexture(new Rect(rect.width - 52f, 4f, 52f, 52f), ShopIcons.BlankItemFrame);
			GUI.color = Color.white;
		} else {
			if (Item.View.ItemType == UberstrikeItemType.Gear && GameState.Current.MatchState.CurrentStateId == GameStateId.None) {
				DrawTryButton(new Rect(rect.width - 106f, 4f, 52f, 52f));
			}

			DrawPrice(new Rect(63f, 30f, 220f, 20f), _pointsPrice, _creditsPrice);
			DrawBuyButton(new Rect(rect.width - 52f, 4f, 52f, 52f), LocalizedStrings.Buy);
		}

		DrawGrayLine(rect);
		GUI.EndGroup();
	}
}
