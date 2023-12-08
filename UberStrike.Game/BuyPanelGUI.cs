using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.WebService.Unity;
using UnityEngine;

public class BuyPanelGUI : PanelGuiBase {
	private const int WIDTH = 300;
	private const int BORDER = 10;
	private const int TITLE_HEIGHT = 100;
	private static bool _isBuyingItem;
	private bool _autoEquip;
	private BuyingLocationType _buyingLocation;
	private BuyingRecommendationType _buyingRecommendation;
	private IUnityItem _item;
	private ItemPriceGUI _price;
	private Texture _priceIcon;
	private string _priceTag;
	private int Height;

	private void OnGUI() {
		GUI.skin = BlueStonez.Skin;
		GUI.depth = 3;
		Height = 100 + _price.Height + 100;
		DrawUnityItem(new Rect((Screen.width - 300) / 2, (Screen.height - Height) / 2, 300f, Height));
		GuiManager.DrawTooltip();
	}

	private void DrawUnityItem(Rect rect) {
		GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
		var num = 20;

		if (ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone) {
			num = 45;
		}

		if (GUI.Button(new Rect(rect.width - num, 0f, num, num), "X", BlueStonez.friends_hidden_button)) {
			Hide();
		}

		DrawTitle(new Rect(10f, 10f, rect.width - 20f, 100f));
		var rect2 = new Rect(30f, 110f, rect.width - 60f, rect.height - 100f);
		DrawPrice(rect2);
		DrawBuyButton(new Rect(0f, rect.height - 90f, rect.width, 90f));
		GUI.EndGroup();

		if (Event.current.type == EventType.MouseDown && !rect.Contains(Event.current.mousePosition)) {
			Hide();
			Event.current.Use();
		}
	}

	private void DrawTitle(Rect rect) {
		GUI.BeginGroup(rect);
		_item.DrawIcon(new Rect(0f, 0f, 48f, 48f));
		var num = rect.width - 48f - 20f - 32f;
		GUI.Label(new Rect(58f, 0f, num, 48f), _item.Name, BlueStonez.label_interparkmed_18pt_left_wrap);

		if (_item.View.LevelLock > PlayerDataManager.PlayerLevel) {
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			var num2 = 0;

			if (ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone) {
				num2 = 25;
			}

			GUI.Label(new Rect(rect.width - 10f - 32f - num2, 8f, 32f, 32f), ShopIcons.BlankItemFrame);
			GUI.Label(new Rect(rect.width - 10f - 31f - num2, 16f, 24f, 24f), _item.View.LevelLock.ToString(), BlueStonez.label_interparkmed_11pt);
			GUI.color = Color.white;
		}

		GUI.Label(new Rect(0f, 58f, rect.width, rect.height - 48f - 10f), _item.View.Description, BlueStonez.label_itemdescription);
		GUI.EndGroup();
	}

	private void DrawPrice(Rect rect) {
		_price.Draw(rect);
	}

	private void DrawBuyButton(Rect rect) {
		GUI.BeginGroup(rect);
		var rect2 = new Rect((rect.width - 120f) / 2f, (rect.height - 30f) / 2f, 120f, 30f);
		GUITools.PushGUIState();
		GUI.enabled = !_isBuyingItem && _price.SelectedPriceOption != null;

		if (GUI.Button(rect2, GUIContent.none, BlueStonez.buttongold_large) && !_isBuyingItem) {
			_isBuyingItem = true;
			BuyItem(_item, _price.SelectedPriceOption, _buyingLocation, _buyingRecommendation, _autoEquip);
		}

		GUITools.PopGUIState();
		var rect3 = new Rect((rect.width - 120f) / 2f, (rect.height - 20f) / 2f, 120f, 20f);
		GUI.Label(rect3, new GUIContent(_priceTag, _priceIcon), BlueStonez.label_interparkbold_13pt_black);
		GUI.EndGroup();
	}

	private void OnPriceOptionSelected(ItemPrice price) {
		_priceTag = ((price.Price != 0) ? string.Format("{0:N0}", price.Price) : "FREE");
		_priceIcon = ((price.Currency != UberStrikeCurrencyType.Points) ? ShopIcons.IconCredits20x20 : ShopIcons.IconPoints20x20);
	}

	public static void BuyItem(IUnityItem item, ItemPrice price, BuyingLocationType buyingLocation = BuyingLocationType.Shop, BuyingRecommendationType recommendation = BuyingRecommendationType.Manual, bool autoEquip = false) {
		if (item.View.IsConsumable) {
			var id = item.View.ID;

			ShopWebServiceClient.BuyPack(id, PlayerDataManager.AuthToken, price.PackType, price.Currency, item.View.ItemType, buyingLocation, recommendation, delegate(int result) { HandleBuyItem(item, (BuyItemResult)result, autoEquip); }, delegate {
				_isBuyingItem = false;
				PanelManager.Instance.ClosePanel(PanelType.BuyItem);
			});
		} else {
			var id2 = item.View.ID;

			ShopWebServiceClient.BuyItem(id2, PlayerDataManager.AuthToken, price.Currency, price.Duration, item.View.ItemType, buyingLocation, recommendation, delegate(int result) { HandleBuyItem(item, (BuyItemResult)result, autoEquip); }, delegate {
				_isBuyingItem = false;
				PanelManager.Instance.ClosePanel(PanelType.BuyItem);
			});
		}
	}

	private static void HandleBuyItem(IUnityItem item, BuyItemResult result, bool autoEquip) {
		_isBuyingItem = false;

		switch (result) {
			case BuyItemResult.OK:
				UnityRuntime.StartRoutine(Singleton<InventoryManager>.Instance.StartUpdateInventoryAndEquipNewItem(item, autoEquip));

				break;
			case BuyItemResult.DisableInShop:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBeRented, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			default:
				if (result != BuyItemResult.InvalidLevel) {
					PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.DataError, PopupSystem.AlertType.OK, HandleWebServiceError);
				} else {
					PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YourLevelIsTooLowToBuyThisItem, PopupSystem.AlertType.OK, HandleWebServiceError);
				}

				break;
			case BuyItemResult.DisableForRent:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBeRented, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.DisableForPermanent:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBePurchasedPermanently, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.DurationDisabled:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBePurchasedForDuration, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.PackDisabled:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisPackIsDisabled, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.IsNotForSale:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemIsNotForSale, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.NotEnoughCurrency:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouDontHaveEnoughPointsOrCreditsToPurchaseThisItem, PopupSystem.AlertType.OKCancel, HandleWebServiceError, LocalizedStrings.OkCaps, ApplicationDataManager.OpenBuyCredits, "GET CREDITS");

				break;
			case BuyItemResult.InvalidMember:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.AccountIsInvalid, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.InvalidExpirationDate:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, string.Format(LocalizedStrings.YouCannotPurchaseThisItemForMoreThanNDays, item.View.MaxDurationDays), PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.AlreadyInInventory:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouAlreadyOwnThisItem, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.InvalidAmount: {
				var maxOwnableAmount = (item.View as UberStrikeItemQuickView).MaxOwnableAmount;
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, string.Format(LocalizedStrings.TheAmountYouTriedToPurchaseIsInvalid, maxOwnableAmount), PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			}
			case BuyItemResult.NoStockRemaining:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemIsOutOfStock, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
			case BuyItemResult.InvalidData:
				PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.InvalidData, PopupSystem.AlertType.OK, HandleWebServiceError);

				break;
		}

		PanelManager.Instance.ClosePanel(PanelType.BuyItem);
	}

	private static void HandleWebServiceError() { }

	public void SetItem(IUnityItem item, BuyingLocationType location, BuyingRecommendationType recommendation, bool autoEquip = false) {
		_autoEquip = autoEquip;
		_item = item;
		_buyingLocation = location;
		_buyingRecommendation = recommendation;
		_isBuyingItem = false;

		if (item != null && item.View.Prices.Count > 0) {
			if (item.View.IsConsumable) {
				_price = new PackItemPriceGUI(item, OnPriceOptionSelected);
			} else {
				_price = new RentItemPriceGUI(item, OnPriceOptionSelected);
			}
		} else {
			Debug.LogError("Item is null or not for sale");
		}
	}
}
