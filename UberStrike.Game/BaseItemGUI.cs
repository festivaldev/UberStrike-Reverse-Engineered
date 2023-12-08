using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public abstract class BaseItemGUI : IShopItemGUI {
	protected const int BuyButtonSize = 52;
	private int _armorPoints;
	private string _description = "No description available.";
	private BuyingLocationType _location;
	private BuyingRecommendationType _recommendation;
	private protected IBaseItemDetailGUI DetailGUI { get; private set; }

	public BaseItemGUI(IUnityItem item, BuyingLocationType location, BuyingRecommendationType recommendation) {
		_location = location;
		_recommendation = recommendation;

		if (item != null) {
			Item = item;

			if (Item.View.ItemType == UberstrikeItemType.Weapon) {
				DetailGUI = new WeaponItemDetailGUI(item.View as UberStrikeItemWeaponView);
			} else if (Item.View.ItemClass == UberstrikeItemClass.GearUpperBody || Item.View.ItemClass == UberstrikeItemClass.GearLowerBody) {
				var uberStrikeItemGearView = item.View as UberStrikeItemGearView;
				_armorPoints = uberStrikeItemGearView.ArmorPoints;
				DetailGUI = new ArmorItemDetailGUI(uberStrikeItemGearView, ShopIcons.ItemarmorpointsIcon);
			}

			if (Item.View != null && !string.IsNullOrEmpty(Item.View.Description)) {
				_description = Item.View.Description;
			}
		} else {
			Item = new NullItem();
			Debug.LogError("BaseItemGUI creation failed because item is NULL");
		}
	}

	public IUnityItem Item { get; private set; }
	public abstract void Draw(Rect rect, bool selected);

	protected void DrawIcon(Rect rect) {
		Item.DrawIcon(rect);
	}

	protected void DrawName(Rect rect) {
		if (!string.IsNullOrEmpty(Item.Name)) {
			GUI.Label(rect, Item.Name, BlueStonez.label_interparkbold_11pt_left_wrap);
		}
	}

	protected void DrawHintArrow(Rect rect) {
		if (rect.Contains(Event.current.mousePosition)) {
			GUI.color = new Color(1f, 1f, 1f, 0.1f);
			GUI.Label(new Rect(rect.width / 2f - 16f, rect.yMin, ShopIcons.ArrowBigShop.width, ShopIcons.ArrowBigShop.height), ShopIcons.ArrowBigShop, GUIStyle.none);
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	protected void DrawArmorOverlay() {
		if (Item.View.ItemClass == UberstrikeItemClass.GearUpperBody || Item.View.ItemClass == UberstrikeItemClass.GearLowerBody) {
			if (_armorPoints > 0) {
				GUI.DrawTexture(new Rect(4f, 35f, 16f, 16f), ShopIcons.ItemarmorpointsIcon);
			}

			if (_armorPoints > 15) {
				GUI.DrawTexture(new Rect(8f, 35f, 16f, 16f), ShopIcons.ItemarmorpointsIcon);
			}

			if (_armorPoints > 30) {
				GUI.DrawTexture(new Rect(12f, 35f, 16f, 16f), ShopIcons.ItemarmorpointsIcon);
			}

			if (_armorPoints > 45) {
				GUI.DrawTexture(new Rect(16f, 35f, 16f, 16f), ShopIcons.ItemarmorpointsIcon);
			}
		}
	}

	protected void DrawPromotionalTag() {
		if (Item.View != null) {
			switch (Item.View.ShopHighlightType) {
				case ItemShopHighlightType.Featured:
					GUI.DrawTexture(new Rect(0f, -3f, 32f, 32f), ShopIcons.Sale);

					break;
				case ItemShopHighlightType.Popular:
					GUI.DrawTexture(new Rect(0f, -3f, 32f, 32f), ShopIcons.Hot);

					break;
				case ItemShopHighlightType.New:
					GUI.DrawTexture(new Rect(0f, -3f, 32f, 32f), ShopIcons.New);

					break;
			}
		}
	}

	protected void DrawClassIcon() {
		GUI.color = new Color(1f, 1f, 1f, 0.5f);

		if (Item.View.ItemType == UberstrikeItemType.Weapon || Item.View.ItemType == UberstrikeItemType.Gear) {
			GUI.DrawTexture(new Rect(54f, 4f, 24f, 24f), UberstrikeIconsHelper.GetIconForItemClass(Item.View.ItemClass));
		}
	}

	protected void DrawLevelRequirement() {
		if (Item.View != null) {
			GUI.Label(new Rect(240f, 26f, 60f, 24f), string.Format("Lv {0}", Item.View.LevelLock.ToString()), BlueStonez.label_interparkbold_11pt_left);
		}
	}

	protected void DrawPrice(Rect rect, ItemPrice points, ItemPrice credits) {
		var num = 0f;

		if (points != null) {
			var text = string.Format("{0}", (points.Price != 0) ? points.Price.ToString("N0") : "FREE");
			GUI.DrawTexture(new Rect(rect.x, rect.y, 16f, 16f), ShopUtils.CurrencyIcon(points.Currency));
			GUI.Label(new Rect(rect.x + 20f, rect.y + 3f, rect.width - 20f, 16f), text, BlueStonez.label_interparkmed_11pt_left);
			num += 40f + BlueStonez.label_interparkmed_11pt_left.CalcSize(new GUIContent(text)).x;
		}

		if (credits != null) {
			var text2 = string.Format("{0}", (credits.Price != 0) ? credits.Price.ToString("N0") : "FREE");

			if (num > 0f) {
				GUI.Label(new Rect(rect.x + num - 10f, rect.y + 3f, 10f, 16f), "/", BlueStonez.label_interparkmed_11pt_left);
			}

			GUI.DrawTexture(new Rect(rect.x + num, rect.y, 16f, 16f), ShopUtils.CurrencyIcon(credits.Currency));
			GUI.Label(new Rect(rect.x + num + 20f, rect.y + 3f, rect.width - 20f, 16f), text2, BlueStonez.label_interparkmed_11pt_left);
		}
	}

	protected void DrawEquipButton(Rect rect, string content) {
		if ((Item.View.ItemType == UberstrikeItemType.Weapon || Item.View.ItemType == UberstrikeItemType.Gear || Item.View.ItemType == UberstrikeItemType.QuickUse) && GUI.Button(rect, new GUIContent(content), BlueStonez.buttondark_medium) && Item != null) {
			switch (Item.View.ItemType) {
				case UberstrikeItemType.Weapon:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.Weapons
					});

					break;
				case UberstrikeItemType.Gear:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.Gear
					});

					break;
				case UberstrikeItemType.QuickUse:
					EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea {
						Area = LoadoutArea.QuickItems
					});

					break;
			}

			if (!Singleton<InventoryManager>.Instance.EquipItem(Item)) {
				var buyPanelGUI = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;

				if (buyPanelGUI) {
					buyPanelGUI.SetItem(Item, _location, _recommendation);
				}
			}
		}
	}

	protected void DrawUnequipButton(Rect rect, string content) {
		if (GUI.Button(rect, new GUIContent(content), BlueStonez.buttondark_medium) && Item != null) {
			ShopPageGUI.Instance.UnequipItem(Item);
		}
	}

	protected void DrawTryButton(Rect position) {
		if (GUI.Button(position, new GUIContent(LocalizedStrings.Try), BlueStonez.buttondark_medium)) {
			Singleton<TemporaryLoadoutManager>.Instance.TryGear(Item);
		}
	}

	protected void DrawBuyButton(Rect position, string text, ShopArea area = ShopArea.Shop) {
		GUI.contentColor = ColorScheme.UberStrikeYellow;

		if (GUITools.Button(position, new GUIContent(text), BlueStonez.buttondark_medium)) {
			var buyPanelGUI = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;

			if (buyPanelGUI) {
				buyPanelGUI.SetItem(Item, _location, _recommendation);
			}
		}

		GUI.contentColor = Color.white;
	}

	protected void DrawGrayLine(Rect position) {
		GUI.Label(new Rect(4f, position.height - 1f, position.width - 4f, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
	}

	protected void DrawDescription(Rect position) {
		GUI.Label(position, _description, BlueStonez.label_itemdescription);
	}

	protected void DrawUseButton(Rect position) {
		if (GUITools.Button(position, new GUIContent("Use"), BlueStonez.buttondark_medium)) {
			PanelManager.Instance.OpenPanel(PanelType.NameChange);
		}
	}

	private class NullItem : IUnityItem {
		public int ItemId { get; set; }

		public UberstrikeItemType ItemType {
			get { return 0; }
		}

		public UberstrikeItemClass ItemClass {
			get { return 0; }
		}

		public string PrefabName {
			get { return string.Empty; }
		}

		public bool Equippable {
			get { return false; }
		}

		public bool IsLoaded {
			get { return true; }
		}

		public GameObject Prefab {
			get { return null; }
		}

		public string Name {
			get { return "Unsupported Item"; }
		}

		public BaseUberStrikeItemView View { get; private set; }
		public void Unload() { }

		public GameObject Create(Vector3 position, Quaternion rotation) {
			return null;
		}

		public void DrawIcon(Rect position) { }
	}
}
