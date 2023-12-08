using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UnityEngine;

public class ShopPageGUI : PageGUI {
	private const int SlotHeight = 70;
	private const int LoadoutWidth = 190;
	private const int ShopWidth = 590;
	private List<Rect> _activeLoadoutUsedSpace = new List<Rect>();
	private CreditBundlesShopGui _creditsGui = new CreditBundlesShopGui();
	private bool _firstLogin;
	private SelectionGroup<UberstrikeItemClass> _gearClassSelection = new SelectionGroup<UberstrikeItemClass>();
	private LoadoutSlotType _highlightedSlot = LoadoutSlotType.None;
	private float _highlightedSlotAlpha = 0.2f;
	private ShopSorting.ItemComparer<IShopItemGUI> _inventoryComparer = new ShopSorting.ItemClassComparer();
	private List<IShopItemGUI> _inventoryItemGUIList = new List<IShopItemGUI>();
	private bool _isReloadingShop;
	private IShopItemFilter _itemFilter;
	private Vector2 _labScroll;
	private Rect _loadoutArea;
	private SelectionGroup<LoadoutArea> _loadoutAreaSelection = new SelectionGroup<LoadoutArea>();
	private Vector2 _loadoutGearScroll;
	private Vector2 _loadoutQuickUseFuncScroll;
	private Vector2 _loadoutWeaponScroll;
	private Rect _rectLabs;
	private Dictionary<LoadoutSlotType, bool> _renewItem = new Dictionary<LoadoutSlotType, bool>();
	private SearchBarGUI _searchBar;
	private Rect _shopArea;
	private SelectionGroup<ShopArea> _shopAreaSelection = new SelectionGroup<ShopArea>();
	private ShopSorting.ItemComparer<IShopItemGUI> _shopComparer = new ShopSorting.LevelComparer();
	private List<IShopItemGUI> _shopItemGUIList = new List<IShopItemGUI>();
	private bool _showRenewLoadoutButton;
	private int _skippedDefaultGearCount;
	private SelectionGroup<UberstrikeItemType> _typeSelection = new SelectionGroup<UberstrikeItemType>();
	private SelectionGroup<UberstrikeItemClass> _weaponClassSelection = new SelectionGroup<UberstrikeItemClass>();
	private float shopPositionX;
	public static ShopPageGUI Instance { get; private set; }

	private void Awake() {
		Instance = this;
		_itemFilter = new SpecialItemFilter();
		_firstLogin = true;
		IsOnGUIEnabled = true;
		_searchBar = new SearchBarGUI("SearchInShop");
		UpdateShopItems();
		Singleton<InventoryManager>.Instance.OnInventoryUpdated += UpdateShopItems;
	}

	private void Start() {
		_loadoutAreaSelection.Add(LoadoutArea.Weapons, new GUIContent(ShopIcons.LoadoutTabWeapons, LocalizedStrings.Weapons));
		_loadoutAreaSelection.Add(LoadoutArea.Gear, new GUIContent(ShopIcons.LoadoutTabGear, LocalizedStrings.Gear));
		_loadoutAreaSelection.Add(LoadoutArea.QuickItems, new GUIContent(ShopIcons.LoadoutTabItems, LocalizedStrings.Items));
		_loadoutAreaSelection.OnSelectionChange += SelectLoadoutArea;
		_shopAreaSelection.Add(ShopArea.Inventory, new GUIContent(LocalizedStrings.Inventory, ShopIcons.LabsInventory, LocalizedStrings.Inventory));
		_shopAreaSelection.Add(ShopArea.Shop, new GUIContent(LocalizedStrings.Shop, ShopIcons.LabsShop, LocalizedStrings.Shop));
		_shopAreaSelection.Add(ShopArea.Credits, new GUIContent(LocalizedStrings.Credits, ShopIcons.CreditsIcon32x32, LocalizedStrings.Credits));
		_shopAreaSelection.OnSelectionChange += delegate { UpdateItemFilter(); };
		_typeSelection.Add(UberstrikeItemType.Special, new GUIContent(ShopIcons.NewItems, LocalizedStrings.NewAndSaleItems));
		_typeSelection.Add(UberstrikeItemType.Weapon, new GUIContent(ShopIcons.WeaponItems, LocalizedStrings.Weapons));
		_typeSelection.Add(UberstrikeItemType.Gear, new GUIContent(ShopIcons.GearItems, LocalizedStrings.Gear));
		_typeSelection.Add(UberstrikeItemType.QuickUse, new GUIContent(ShopIcons.QuickItems, LocalizedStrings.QuickItems));
		_typeSelection.Add(UberstrikeItemType.Functional, new GUIContent(ShopIcons.FunctionalItems, LocalizedStrings.FunctionalItems));
		_typeSelection.OnSelectionChange += delegate { UpdateItemFilter(); };
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponMelee, new GUIContent(ShopIcons.StatsMostWeaponSplatsMelee, LocalizedStrings.MeleeWeapons));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponMachinegun, new GUIContent(ShopIcons.StatsMostWeaponSplatsMachinegun, LocalizedStrings.Machineguns));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponShotgun, new GUIContent(ShopIcons.StatsMostWeaponSplatsShotgun, LocalizedStrings.Shotguns));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponSniperRifle, new GUIContent(ShopIcons.StatsMostWeaponSplatsSniperRifle, LocalizedStrings.SniperRifles));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponCannon, new GUIContent(ShopIcons.StatsMostWeaponSplatsCannon, LocalizedStrings.Cannons));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponSplattergun, new GUIContent(ShopIcons.StatsMostWeaponSplatsSplattergun, LocalizedStrings.Splatterguns));
		_weaponClassSelection.Add(UberstrikeItemClass.WeaponLauncher, new GUIContent(ShopIcons.StatsMostWeaponSplatsLauncher, LocalizedStrings.Launchers));
		_weaponClassSelection.OnSelectionChange += delegate { UpdateItemFilter(); };
		_gearClassSelection.Add(UberstrikeItemClass.GearBoots, new GUIContent(ShopIcons.Boots, LocalizedStrings.Boots));
		_gearClassSelection.Add(UberstrikeItemClass.GearHead, new GUIContent(ShopIcons.Head, LocalizedStrings.Head));
		_gearClassSelection.Add(UberstrikeItemClass.GearFace, new GUIContent(ShopIcons.Face, LocalizedStrings.Face));
		_gearClassSelection.Add(UberstrikeItemClass.GearUpperBody, new GUIContent(ShopIcons.Upperbody, LocalizedStrings.UpperBody));
		_gearClassSelection.Add(UberstrikeItemClass.GearLowerBody, new GUIContent(ShopIcons.Lowerbody, LocalizedStrings.LowerBody));
		_gearClassSelection.Add(UberstrikeItemClass.GearGloves, new GUIContent(ShopIcons.Gloves, LocalizedStrings.Gloves));
		_gearClassSelection.Add(UberstrikeItemClass.GearHolo, new GUIContent(ShopIcons.Holos, LocalizedStrings.Holo));
		_gearClassSelection.OnSelectionChange += delegate { UpdateItemFilter(); };

		if (_showRenewLoadoutButton) {
			foreach (var loadoutSlotType in LoadoutManager.WeaponSlots) {
				InventoryItem inventoryItem;

				if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(loadoutSlotType, out inventoryItem)) {
					_renewItem[loadoutSlotType] = !Singleton<InventoryManager>.Instance.IsItemValidForDays(inventoryItem, 5);
				}
			}

			foreach (var loadoutSlotType2 in LoadoutManager.GearSlots) {
				InventoryItem inventoryItem2;

				if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(loadoutSlotType2, out inventoryItem2)) {
					_renewItem[loadoutSlotType2] = !Singleton<InventoryManager>.Instance.IsItemValidForDays(inventoryItem2, 5);
				}
			}
		}

		if (_shopAreaSelection.Index == 0) {
			_shopAreaSelection.Select(ShopArea.Shop);
		}

		_typeSelection.Select(UberstrikeItemType.Special);
		_gearClassSelection.SetIndex(-1);
		_weaponClassSelection.SetIndex(-1);
	}

	private void OnEnable() {
		EventHandler.Global.AddListener(new Action<ShopEvents.SelectShopArea>(OnSelectShopAreaEvent));
		EventHandler.Global.AddListener(new Action<ShopEvents.SelectLoadoutArea>(OnSelectLoadoutAreaEvent));
		EventHandler.Global.AddListener(new Action<ShopEvents.ShopHighlightSlot>(OnHighlightSlotEvent));
		EventHandler.Global.AddListener(new Action<ShopEvents.RefreshCurrentItemList>(OnRefreshCurrentItemListEvent));
		Singleton<DragAndDrop>.Instance.OnDragBegin += OnBeginDrag;
		Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout();
		StartCoroutine(StartNotifyLoadoutArea());

		if (MouseOrbit.Instance != null) {
			MouseOrbit.Instance.MaxX = Screen.width - 590;
		}

		if (IsOnGUIEnabled) {
			StartCoroutine(ScrollShopFromRight(0.25f));
		}

		_searchBar.ClearFilter();
		GameData.Instance.IsShopLoaded.Value = true;
	}

	private void OnDisable() {
		EventHandler.Global.RemoveListener(new Action<ShopEvents.SelectShopArea>(OnSelectShopAreaEvent));
		EventHandler.Global.RemoveListener(new Action<ShopEvents.SelectLoadoutArea>(OnSelectLoadoutAreaEvent));
		EventHandler.Global.RemoveListener(new Action<ShopEvents.RefreshCurrentItemList>(OnRefreshCurrentItemListEvent));
		EventHandler.Global.RemoveListener(new Action<ShopEvents.ShopHighlightSlot>(OnHighlightSlotEvent));
		Singleton<DragAndDrop>.Instance.OnDragBegin -= OnBeginDrag;

		if (MouseOrbit.Instance != null) {
			MouseOrbit.Instance.MaxX = Screen.width;
		}

		GameData.Instance.IsShopLoaded.Value = false;
	}

	private void OnHighlightSlotEvent(ShopEvents.ShopHighlightSlot ev) {
		HighlightingSlot(ev.SlotType);
	}

	private void OnSelectShopAreaEvent(ShopEvents.SelectShopArea ev) {
		_shopAreaSelection.Select(ev.ShopArea);

		if (ev.ItemType != 0) {
			_typeSelection.Select(ev.ItemType);
		}

		if (ev.ItemClass != 0) {
			switch (ev.ItemType) {
				case UberstrikeItemType.Weapon:
					_weaponClassSelection.Select(ev.ItemClass);

					break;
				case UberstrikeItemType.Gear:
					_gearClassSelection.Select(ev.ItemClass);

					break;
			}
		}
	}

	private void OnSelectLoadoutAreaEvent(ShopEvents.SelectLoadoutArea ev) {
		_loadoutAreaSelection.Select(ev.Area);
	}

	private IEnumerator ScrollShopFromRight(float time) {
		var t = 0f;

		while (t < time) {
			t += Time.deltaTime;
			shopPositionX = Mathf.Lerp(0f, 590f, t / time * (t / time));

			if (MenuPageManager.Instance != null) {
				MenuPageManager.Instance.LeftAreaGUIOffset = shopPositionX;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator StartNotifyLoadoutArea() {
		yield return new WaitForEndOfFrame();

		EventHandler.Global.Fire(new ShopEvents.LoadoutAreaChanged {
			Area = _loadoutAreaSelection.Current
		});
	}

	private void OnGUI() {
		if (IsOnGUIEnabled) {
			DrawGUI(new Rect(Screen.width - shopPositionX, GlobalUIRibbon.Instance.Height(), 590f, Screen.height - GlobalUIRibbon.Instance.Height() + 1));
		}
	}

	public override void DrawGUI(Rect rect) {
		GUI.depth = 11;
		GUI.skin = BlueStonez.Skin;

		if (_firstLogin) {
			_firstLogin = false;
		}

		_rectLabs = rect;
		_rectLabs.width = _rectLabs.width + 10f;
		GUITools.PushGUIState();
		GUI.enabled = !PopupSystem.IsAnyPopupOpen && !PanelManager.IsAnyPanelOpen;
		GUI.BeginGroup(_rectLabs);
		DrawLoadout(new Rect(0f, 0f, 190f, _rectLabs.height));
		DrawShop(new Rect(190f, 0f, _rectLabs.width - 190f - 10f, _rectLabs.height));
		GUI.EndGroup();
		Singleton<DragAndDrop>.Instance.DrawSlot(new Rect(0f, 55f, Screen.width - 580, Screen.height - 55), new Action<int, ShopDragSlot>(OnDropAvatar));
		Singleton<DragAndDrop>.Instance.DrawSlot(new Rect(Screen.width - _rectLabs.width + 200f, 55f, _rectLabs.width - 200f, Screen.height - 55), new Action<int, ShopDragSlot>(OnDropShop));
		GUITools.PopGUIState();

		if (!PopupSystem.IsAnyPopupOpen && !PanelManager.IsAnyPanelOpen) {
			GuiManager.DrawTooltip();
		}

		if (_highlightedSlotAlpha > 0f) {
			_highlightedSlotAlpha = Mathf.Max(_highlightedSlotAlpha - Time.deltaTime * 0.5f, 0f);
		}

		if (PlayerDataManager.AccessLevel == MemberAccessLevel.Admin) {
			GUI.enabled = !_isReloadingShop;

			if (GUI.Button(new Rect(_rectLabs.x + 10f, Screen.height - 50, 128f, 32f), "Reload Shop", BlueStonez.button_green)) {
				StartCoroutine(ReloadShop());
			}

			GUI.enabled = true;
		}
	}

	private IEnumerator ReloadShop() {
		_isReloadingShop = true;
		var _progress = new ProgressPopupDialog("Reload Shop", "I'm reloading the shop, please wait...");
		PopupSystem.Show(_progress);

		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetShop());

		PopupSystem.HideMessage(_progress);

		if (!Singleton<ItemManager>.Instance.ValidateItemMall()) {
			PopupSystem.ShowMessage(LocalizedStrings.ErrorGettingShopData, LocalizedStrings.ErrorGettingShopDataSupport, PopupSystem.AlertType.OK);
		}

		_isReloadingShop = false;
	}

	public void EquipItemFromArea(IUnityItem item, LoadoutSlotType slot, ShopArea area) {
		if (item != null && !Singleton<LoadoutManager>.Instance.IsItemEquipped(item.View.ID)) {
			if (Singleton<InventoryManager>.Instance.Contains(item.View.ID)) {
				Singleton<InventoryManager>.Instance.EquipItemOnSlot(item.View.ID, slot);
			} else if (item.View.LevelLock <= PlayerDataManager.PlayerLevel) {
				var buyPanelGUI = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;

				if (buyPanelGUI) {
					buyPanelGUI.SetItem(item, BuyingLocationType.Shop, BuyingRecommendationType.None, true);
				}
			}
		} else {
			Debug.LogError("Item is null or already equipped!");
		}
	}

	public void SelectLoadoutWeapon(LoadoutSlotType slot) {
		if (Singleton<InventoryManager>.Instance.CurrentWeaponSlot != slot) {
			Singleton<InventoryManager>.Instance.CurrentWeaponSlot = slot;
			GameState.Current.Avatar.ShowWeapon(slot);
			InventoryItem inventoryItem;

			if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(slot, out inventoryItem)) {
				GameState.Current.Avatar.Decorator.AnimationController.ChangeWeaponType(inventoryItem.Item.View.ItemClass);
			} else {
				GameState.Current.Avatar.Decorator.AnimationController.ChangeWeaponType(0);
			}
		}
	}

	public void UnequipItem(IUnityItem item) {
		LoadoutSlotType loadoutSlotType;

		if (item != null && Singleton<LoadoutManager>.Instance.TryGetSlotForItem(item, out loadoutSlotType)) {
			switch (item.View.ItemType) {
				case UberstrikeItemType.Weapon:
					GameState.Current.Avatar.UnassignWeapon(loadoutSlotType);
					GameState.Current.Avatar.Decorator.AnimationController.ChangeWeaponType(0);

					break;
				case UberstrikeItemType.Gear:
					ShowUnequipGearFx(item, loadoutSlotType);

					break;
			}

			Singleton<LoadoutManager>.Instance.ResetSlot(loadoutSlotType);
			HighlightingSlot(loadoutSlotType);
		}
	}

	private void ShowUnequipGearFx(IUnityItem item, LoadoutSlotType slot) {
		var itemClass = item.View.ItemClass;

		if (itemClass != UberstrikeItemClass.GearFace && itemClass != UberstrikeItemClass.GearHolo) {
			IUnityItem unityItem;

			if (Singleton<ItemManager>.Instance.TryGetDefaultItem(item.View.ItemClass, out unityItem)) {
				Singleton<InventoryManager>.Instance.EquipItemOnSlot(unityItem.View.ID, slot);
			}
		} else {
			Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout(slot);
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.EquipGear);

			if (GameState.Current.Avatar != null) {
				GameState.Current.Avatar.HideWeapons();
			}
		}
	}

	private void SetActiveLoadoutActiveSpaces(int slots, float width) {
		_activeLoadoutUsedSpace.Clear();

		for (var i = 0; i < slots; i++) {
			_activeLoadoutUsedSpace.Add(new Rect(0f, i * 70, width - 5f, 70f));
		}
	}

	private void SetActiveLoadoutActiveSpaces(params Rect[] rects) {
		_activeLoadoutUsedSpace.Clear();

		for (var i = 0; i < rects.Length; i++) {
			_activeLoadoutUsedSpace.Add(rects[i]);
		}
	}

	private bool IsMouseCursorInLoadout(Vector2 pos) {
		for (var i = 0; i < _activeLoadoutUsedSpace.Count; i++) {
			if (_activeLoadoutUsedSpace[i].Contains(pos)) {
				return true;
			}
		}

		return false;
	}

	public void HighlightingSlot(LoadoutSlotType slot) {
		_highlightedSlot = slot;
		_highlightedSlotAlpha = 0.5f;

		switch (slot) {
			case LoadoutSlotType.WeaponMelee:
			case LoadoutSlotType.WeaponPrimary:
			case LoadoutSlotType.WeaponSecondary:
			case LoadoutSlotType.WeaponTertiary:
				SelectLoadoutArea(LoadoutArea.Weapons);

				break;
			case LoadoutSlotType.QuickUseItem1:
			case LoadoutSlotType.QuickUseItem2:
			case LoadoutSlotType.QuickUseItem3:
			case LoadoutSlotType.FunctionalItem1:
			case LoadoutSlotType.FunctionalItem2:
			case LoadoutSlotType.FunctionalItem3:
				SelectLoadoutArea(LoadoutArea.QuickItems);

				break;
			default:
				SelectLoadoutArea(LoadoutArea.Gear);

				break;
		}
	}

	public void SelectLoadoutArea(LoadoutArea area) {
		switch (area) {
			case LoadoutArea.Weapons:
				SetActiveLoadoutActiveSpaces(4, 185f);

				break;
			case LoadoutArea.Gear:
			case LoadoutArea.QuickItems:
				SetActiveLoadoutActiveSpaces(6, 185f);

				break;
		}

		EventHandler.Global.Fire(new ShopEvents.LoadoutAreaChanged {
			Area = area
		});
	}

	private void UpdateShopItems() {
		_shopItemGUIList.Clear();
		_inventoryItemGUIList.Clear();

		foreach (var inventoryItem in Singleton<InventoryManager>.Instance.GetAllItems(false)) {
			_inventoryItemGUIList.Add(new InventoryItemGUI(inventoryItem, BuyingLocationType.Shop));
		}

		_inventoryItemGUIList.Sort(_inventoryComparer);

		foreach (var unityItem in Singleton<ItemManager>.Instance.GetAllShopItems()) {
			_shopItemGUIList.Add(new ShopItemGUI(unityItem, BuyingLocationType.Shop));
		}

		_shopItemGUIList.Sort(_shopComparer);
	}

	private void OnRefreshCurrentItemListEvent(ShopEvents.RefreshCurrentItemList ev) { }

	private void DrawLoadout(Rect rect) {
		_loadoutArea = rect;
		_loadoutArea.x = _loadoutArea.x + _rectLabs.x;
		_loadoutArea.y = _loadoutArea.y + _rectLabs.y;
		GUI.BeginGroup(rect, string.Empty, BlueStonez.window);
		GUI.Label(new Rect(0f, 0f, rect.width - 2f, 76f), LocalizedStrings.LoadoutCaps, BlueStonez.tab_strip_large);
		var num = UnityGUI.Toolbar(new Rect(4f, 32f, 120f, 44f), _loadoutAreaSelection.Index, _loadoutAreaSelection.GuiContent, _loadoutAreaSelection.Length, BlueStonez.tab_largeicon);

		if (num != _loadoutAreaSelection.Index) {
			_loadoutAreaSelection.SetIndex(num);
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
		}

		var rect2 = new Rect(0f, 76f, rect.width, rect.height - 76f);

		switch (_loadoutAreaSelection.Current) {
			case LoadoutArea.Weapons:
				DrawWeaponLoadout(rect2);

				break;
			case LoadoutArea.Gear:
				DrawGearLoadout(rect2);

				break;
			case LoadoutArea.QuickItems:
				DrawQuickItemLoadout(rect2);

				break;
		}

		GUI.EndGroup();
	}

	private void DrawShop(Rect labsRect) {
		_shopArea = labsRect;
		_shopArea.x = _shopArea.x + _rectLabs.x;
		_shopArea.y = _shopArea.y + _rectLabs.y;
		var flag = false;

		if (!Application.isWebPlayer || Application.isEditor) {
			flag = true;
		}

		GUI.BeginGroup(labsRect, BlueStonez.window);
		DrawShopTabs(labsRect);

		if (_shopAreaSelection.Current == ShopArea.Inventory || _shopAreaSelection.Current == ShopArea.Shop) {
			_searchBar.Draw(new Rect((!flag) ? (labsRect.width - 128f) : (labsRect.width - 200f), 8f, 123f, 20f));
		}

		switch (_shopAreaSelection.Current) {
			case ShopArea.Inventory:
				DrawItemGUIList(_inventoryItemGUIList, labsRect);
				DrawSortBar(new Rect(0f, 74f, labsRect.width, 22f), false, true);

				break;
			case ShopArea.Shop:
				DrawItemGUIList(_shopItemGUIList, labsRect);
				DrawShopSubTabs(labsRect, true);

				break;
			case ShopArea.Credits:
				_creditsGui.Draw(new Rect(0f, 74f, labsRect.width, labsRect.height - 74f));

				break;
		}

		if (flag) {
			var flag2 = PlayerDataManager.IsPlayerLoggedIn && GUITools.SaveClickIn(7f);
			GUI.enabled = flag2 || (PlayerDataManager.IsPlayerLoggedIn && GUITools.SaveClickIn(7f));

			if (GUITools.Button(new Rect(labsRect.width - 66f, 7f, 64f, 20f), new GUIContent(LocalizedStrings.Refresh), BlueStonez.buttondark_medium)) {
				GUITools.Clicked();
				ApplicationDataManager.RefreshWallet();
			}

			GUI.enabled = flag2;
		}

		GUI.EndGroup();
	}

	private void DrawShopTabs(Rect rect) {
		rect = new Rect(0f, 0f, rect.width, rect.height);
		GUI.Box(rect, string.Empty, BlueStonez.window);
		GUI.Label(new Rect(0f, 0f, rect.width, 76f), LocalizedStrings.ShopCaps, BlueStonez.tab_strip_large);
		var num = UnityGUI.Toolbar(new Rect(1f, 32f, rect.width - 2f, 44f), _shopAreaSelection.Index, _shopAreaSelection.GuiContent, BlueStonez.tab_largeicon);

		if (num != _shopAreaSelection.Index) {
			_shopAreaSelection.SetIndex(num);
			_searchBar.ClearFilter();
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
		}
	}

	private void DrawShopSubTabs(Rect position, bool showLevel) {
		var num = UnityGUI.Toolbar(new Rect(1f, 74f, position.width - 2f, 44f), _typeSelection.Index, _typeSelection.GuiContent, _typeSelection.Length, BlueStonez.tab_large);

		if (num != _typeSelection.Index) {
			_typeSelection.SetIndex(num);
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
		}

		if (_typeSelection.Current == UberstrikeItemType.Weapon) {
			DrawWeaponClassFilter(new Rect(0f, 114f, position.width, 30f));
			DrawSortBar(new Rect(0f, 149f, position.width + 1f, 22f), showLevel, false);
		} else if (_typeSelection.Current == UberstrikeItemType.Gear) {
			DrawGearClassFilter(new Rect(0f, 114f, position.width, 30f));
			DrawSortBar(new Rect(0f, 149f, position.width + 1f, 22f), showLevel, false);
		} else {
			DrawSortBar(new Rect(0f, 118f, position.width, 22f), showLevel, false);
		}
	}

	private void DrawWeaponClassFilter(Rect rect) {
		GUI.changed = false;
		var rect2 = new Rect(rect.x, rect.y + 5f, rect.width, rect.height);
		var num = UnityGUI.Toolbar(rect2, _weaponClassSelection.Index, _weaponClassSelection.GuiContent, _weaponClassSelection.Length, BlueStonez.tab_large);

		if (GUI.changed) {
			if (num == _weaponClassSelection.Index) {
				_weaponClassSelection.SetIndex(-1);
			} else {
				_weaponClassSelection.SetIndex(num);
			}

			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
		}
	}

	private void DrawGearClassFilter(Rect rect) {
		GUI.changed = false;
		var rect2 = new Rect(rect.x, rect.y + 5f, rect.width, rect.height);
		var num = UnityGUI.Toolbar(rect2, _gearClassSelection.Index, _gearClassSelection.GuiContent, _gearClassSelection.Length, BlueStonez.tab_large);

		if (GUI.changed) {
			if (num == _gearClassSelection.Index) {
				_gearClassSelection.SetIndex(-1);
			} else {
				_gearClassSelection.SetIndex(num);
			}

			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
		}
	}

	private void DrawSortBar(Rect sortRect, bool showLevel, bool showExpDay) {
		GUI.BeginGroup(sortRect);

		if (!showLevel && showExpDay) {
			GUI.Label(new Rect(0f, 0f, sortRect.width - 134f, sortRect.height), new GUIContent(LocalizedStrings.Name), BlueStonez.label_interparkmed_11pt);
			GUI.Label(new Rect(sortRect.width - 136f, 0f, 64f, sortRect.height), new GUIContent(LocalizedStrings.Duration), BlueStonez.label_interparkmed_11pt);
		} else if (showLevel && !showExpDay) {
			GUI.Label(new Rect(0f, 0f, sortRect.width - 179f, sortRect.height), new GUIContent(LocalizedStrings.Name), BlueStonez.label_interparkmed_11pt);
			GUI.Label(new Rect(sortRect.width - 173f, 0f, 48f, sortRect.height), new GUIContent(LocalizedStrings.Level), BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();
	}

	private void DrawItemGUIList<T>(List<T> list, Rect position) where T : IShopItemGUI {
		var num = ((_typeSelection.Current != UberstrikeItemType.Weapon && _typeSelection.Current != UberstrikeItemType.Gear) ? 29 : 58);
		var num2 = -1;
		var num3 = 0;
		var num4 = 60;
		var num5 = ((_shopAreaSelection.Current != ShopArea.Inventory) ? (116 + num) : 109);
		var rect = new Rect(0f, num5, position.width, position.height - (num5 + 1));
		var rect2 = new Rect(0f, 0f, position.width - 20f, (list.Count - _skippedDefaultGearCount) * num4 + 106);
		var flag = rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen;
		var list2 = new List<string>();
		_labScroll = GUITools.BeginScrollView(rect, _labScroll, rect2);
		var num6 = ((rect2.height <= rect.height) ? 5 : 20);
		_skippedDefaultGearCount = 0;
		var num7 = -num4;

		for (var i = 0; i < list.Count; i++) {
			var searchBar = _searchBar;
			var t = list[i];

			if (!searchBar.CheckIfPassFilter(t.Item.Name)) {
				_skippedDefaultGearCount++;
			} else {
				if (_itemFilter != null) {
					var itemFilter = _itemFilter;
					var t2 = list[i];

					if (!itemFilter.CanPass(t2.Item)) {
						_skippedDefaultGearCount++;

						goto IL_371;
					}
				}

				num7 += num4;

				if (num7 + num4 >= _labScroll.y && num7 <= _labScroll.y + rect.height) {
					var rect3 = new Rect(0f, num7 + ((num2 != -1) ? (num3 - 20) : 0), position.width - num6, num4);
					var rect4 = new Rect(rect3.x, rect3.y, rect3.width - 100f, rect3.height);
					var t3 = list[i];
					GUITools.PushGUIState();

					if (!Singleton<InventoryManager>.Instance.Contains(t3.Item.View.ID) && t3.Item.View.LevelLock > PlayerDataManager.PlayerLevel) {
						GUI.enabled = false;
					}

					t3.Draw(rect3, rect3.Contains(Event.current.mousePosition));
					GUITools.PopGUIState();
					list2.Add(t3.Item.View.PrefabName);

					if (flag && rect4.Contains(Event.current.mousePosition) && !Singleton<DragAndDrop>.Instance.IsDragging) {
						AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(t3.Item, rect3, PopupViewSide.Left);
					}

					var instance = Singleton<DragAndDrop>.Instance;
					var rect5 = rect3;
					var shopDragSlot = default(ShopDragSlot);
					var t4 = list[i];
					shopDragSlot.Item = t4.Item;
					shopDragSlot.Slot = LoadoutSlotType.Shop;
					instance.DrawSlot(rect5, shopDragSlot, OnDropShop, null, true);
				}
			}

			IL_371: ;
		}

		GUITools.EndScrollView();
	}

	private void UpdateItemFilter() {
		var shopArea = _shopAreaSelection.Current;

		if (shopArea != ShopArea.Inventory) {
			if (shopArea == ShopArea.Shop) {
				switch (_typeSelection.Current) {
					case UberstrikeItemType.Weapon:
						if (_weaponClassSelection.Current == 0) {
							_itemFilter = new ItemByTypeFilter(_typeSelection.Current);
						} else {
							_itemFilter = new ItemByClassFilter(_typeSelection.Current, _weaponClassSelection.Current);
						}

						break;
					case UberstrikeItemType.Gear:
						if (_gearClassSelection.Current == 0) {
							_itemFilter = new ItemByTypeFilter(_typeSelection.Current);
						} else {
							_itemFilter = new ItemByClassFilter(_typeSelection.Current, _gearClassSelection.Current);
						}

						break;
					case UberstrikeItemType.QuickUse:
					case UberstrikeItemType.Functional:
						_itemFilter = new ItemByTypeFilter(_typeSelection.Current);

						break;
					case UberstrikeItemType.Special:
						_itemFilter = new SpecialItemFilter();

						break;
				}
			}
		} else {
			_itemFilter = new InventoryItemFilter();
		}
	}

	private void DrawWeaponLoadout(Rect position) {
		_loadoutWeaponScroll = GUITools.BeginScrollView(position, _loadoutWeaponScroll, new Rect(0f, 0f, position.width - 20f, 285f));

		string[] array = {
			LocalizedStrings.Melee,
			LocalizedStrings.PrimaryWeapon,
			LocalizedStrings.SecondaryWeapon,
			LocalizedStrings.TertiaryWeapon
		};

		LoadoutSlotType[] array2 = {
			LoadoutSlotType.WeaponMelee,
			LoadoutSlotType.WeaponPrimary,
			LoadoutSlotType.WeaponSecondary,
			LoadoutSlotType.WeaponTertiary
		};

		var rect = default(Rect);

		for (var i = 0; i < array.Length; i++) {
			var rect2 = new Rect(0f, 70 * i, position.width - 5f, 70f);
			var item = Singleton<InventoryManager>.Instance.GetItem(Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(array2[i]));
			DrawLoadoutWeaponItem(array[i], item, rect2, array2[i]);

			if (array2[i] == Singleton<InventoryManager>.Instance.CurrentWeaponSlot) {
				rect.x = rect2.x + 5f;
				rect.y = rect2.y;
				rect.width = rect2.width - 16f;
				rect.height = rect2.height - 10f;
			}
		}

		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.Box(rect, GUIContent.none, BlueStonez.group_grey81);
		GUI.color = Color.white;

		if (_showRenewLoadoutButton) {
			Rect[] array3 = {
				new Rect(0f, 0f, 5f, 70f),
				new Rect(0f, 70f, 5f, 70f),
				new Rect(0f, 140f, 5f, 70f),
				new Rect(0f, 210f, 5f, 70f)
			};

			for (var j = 0; j < LoadoutManager.WeaponSlots.Length; j++) {
				var loadoutSlotType = LoadoutManager.WeaponSlots[j];
				_renewItem[loadoutSlotType] = GUI.Toggle(array3[j], _renewItem[loadoutSlotType], (!_renewItem[loadoutSlotType]) ? "<" : ">", BlueStonez.panelquad_toggle);
			}
		}

		GUI.color = Color.white;
		GUITools.EndScrollView();
	}

	private void DrawGearLoadout(Rect position) {
		Rect[] array = {
			new Rect(0f, 70f, position.width - 5f, 70f),
			new Rect(0f, 140f, position.width - 5f, 70f),
			new Rect(0f, 210f, position.width - 5f, 70f),
			new Rect(0f, 280f, position.width - 5f, 70f),
			new Rect(0f, 350f, position.width - 5f, 70f),
			new Rect(0f, 420f, position.width - 5f, 70f)
		};

		Rect[] array2 = {
			new Rect(0f, 0f, 5f, 60f),
			new Rect(0f, 60f, 5f, 70f),
			new Rect(0f, 130f, 5f, 70f),
			new Rect(0f, 200f, 5f, 70f),
			new Rect(0f, 270f, 5f, 70f),
			new Rect(0f, 340f, 5f, 70f)
		};

		_loadoutGearScroll = GUITools.BeginScrollView(position, _loadoutGearScroll, new Rect(0f, 0f, position.width - 20f, 490f));
		var inventoryItem = Singleton<InventoryManager>.Instance.GetItem(Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo));
		DrawLoadoutGearItem(LocalizedStrings.Holo, inventoryItem, LoadoutSlotType.GearHolo, new Rect(0f, 0f, position.width - 5f, 70f), UberstrikeItemClass.GearHolo);

		for (var i = 0; i < LoadoutManager.GearSlots.Length; i++) {
			var text = LoadoutManager.GearSlotNames[i];
			var loadoutSlotType = LoadoutManager.GearSlots[i];
			inventoryItem = Singleton<InventoryManager>.Instance.GetItem(Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(loadoutSlotType));
			DrawLoadoutGearItem(text, inventoryItem, loadoutSlotType, array[i], LoadoutManager.GearSlotClasses[i]);
		}

		if (_showRenewLoadoutButton) {
			for (var j = 0; j < LoadoutManager.GearSlots.Length; j++) {
				var rect = array2[j];
				var loadoutSlotType2 = LoadoutManager.GearSlots[j];
				_renewItem[loadoutSlotType2] = GUI.Toggle(rect, _renewItem[loadoutSlotType2], (!_renewItem[loadoutSlotType2]) ? "<" : ">", BlueStonez.panelquad_toggle);
			}
		}

		GUITools.EndScrollView();
	}

	private void DrawQuickItemLoadout(Rect position) {
		_loadoutQuickUseFuncScroll = GUITools.BeginScrollView(position, _loadoutQuickUseFuncScroll, new Rect(0f, 0f, position.width - 20f, 285f));
		var rect = default(Rect);

		for (var i = 0; i < 3; i++) {
			var loadoutSlotType = LoadoutSlotType.QuickUseItem1 + i;
			var itemIdOnSlot = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(loadoutSlotType);
			var item = Singleton<InventoryManager>.Instance.GetItem(itemIdOnSlot);
			DrawLoadoutQuickUseItem(LocalizedStrings.QuickItem + " " + (i + 1), item, loadoutSlotType, new Rect(0f, 70 * i, position.width - 5f, 70f), AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString(GameInputKey.QuickItem1 + i));

			if (Singleton<InventoryManager>.Instance.CurrentQuickItemSot == loadoutSlotType) {
				rect.x = 5f;
				rect.y = 70 * i;
				rect.width = position.width - 16f;
				rect.height = 60f;
			}
		}

		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.Box(rect, GUIContent.none, BlueStonez.group_grey81);
		GUI.color = Color.white;
		GUI.color = Color.white;
		GUITools.EndScrollView();
	}

	private void DrawLoadoutWeaponItem(string slotName, InventoryItem item, Rect rect, LoadoutSlotType slot) {
		var rect2 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
		GUI.BeginGroup(rect2);

		if (item.Item != null) {
			item.Item.DrawIcon(new Rect(rect2.width - 60f, 0f, 48f, 48f));
			GUI.Label(new Rect(0f, 5f, rect2.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
			GUI.Label(new Rect(0f, 30f, rect2.width - 65f, 12f), item.Item.Name, BlueStonez.label_interparkmed_10pt_right);
			GUI.Label(new Rect(0f, rect2.height - 1f, rect2.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
		} else {
			GUI.Label(new Rect(rect2.width - 60f, 0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
			GUI.Label(new Rect(0f, 5f, rect2.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
			GUI.Label(new Rect(0f, rect2.height - 1f, rect2.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
		}

		GUI.EndGroup();

		if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen) {
			if (Event.current.type == EventType.MouseDown) {
				if (Singleton<InventoryManager>.Instance.CurrentWeaponSlot != slot) {
					Singleton<InventoryManager>.Instance.CurrentWeaponSlot = slot;
					GameState.Current.Avatar.ShowWeapon(slot);
				}
			} else {
				AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(item.Item, rect2, PopupViewSide.Left, item.DaysRemaining);
			}
		}

		Color? color = null;

		if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemClass == UberstrikeItemClass.WeaponMelee && slot == LoadoutSlotType.WeaponMelee) {
			color = new Color(1f, 1f, 1f, 0.1f);
		} else if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemClass != UberstrikeItemClass.WeaponMelee && slot != LoadoutSlotType.WeaponMelee) {
			color = new Color(1f, 1f, 1f, 0.1f);
		} else if (slot == _highlightedSlot) {
			color = new Color(1f, 1f, 1f, _highlightedSlotAlpha);
		}

		var rect3 = new Rect(rect2.x, rect2.y - 5f, rect2.width - 6f, rect2.height);

		Singleton<DragAndDrop>.Instance.DrawSlot(rect3, new ShopDragSlot {
			Item = item.Item,
			Slot = slot
		}, OnDropLoadout, color);
	}

	private void DrawLoadoutGearItem(string slotName, InventoryItem item, LoadoutSlotType loadoutSlotType, Rect rect, UberstrikeItemClass itemClass) {
		var rect2 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
		GUI.BeginGroup(rect2);

		if (item.Item != null && !Singleton<ItemManager>.Instance.IsDefaultGearItem(item.Item.View.PrefabName)) {
			item.Item.DrawIcon(new Rect(rect2.width - 60f, 0f, 48f, 48f));
			GUI.Label(new Rect(0f, 5f, rect2.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
			GUI.Label(new Rect(0f, 30f, rect2.width - 65f, 12f), item.Item.Name, BlueStonez.label_interparkmed_10pt_right);
		} else {
			GUI.Label(new Rect(rect2.width - 60f, 0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
			GUI.Label(new Rect(0f, 5f, rect2.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
		}

		GUI.Label(new Rect(0f, rect2.height - 5f, rect2.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
		GUI.EndGroup();

		if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen) {
			if (Event.current.type == EventType.MouseDown) {
				if (item.Item != null && GameState.Current.Avatar.Decorator && GameState.Current.Avatar.Decorator.AnimationController) {
					GameState.Current.Avatar.Decorator.AnimationController.TriggerGearAnimation(item.Item.View.ItemClass);
				}
			} else {
				AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(item.Item, rect2, PopupViewSide.Left, item.DaysRemaining);
			}
		}

		Color? color = null;

		if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemClass == itemClass) {
			color = new Color(1f, 1f, 1f, 0.2f);
		} else if (loadoutSlotType == _highlightedSlot) {
			color = new Color(1f, 1f, 1f, _highlightedSlotAlpha);
		}

		Singleton<DragAndDrop>.Instance.DrawSlot(new Rect(rect2.x, rect2.y - 15f, rect2.width, rect2.height + 11f), new ShopDragSlot {
			Item = item.Item,
			Slot = loadoutSlotType
		}, OnDropLoadout, color);
	}

	private void DrawLoadoutQuickUseItem(string slotName, InventoryItem itemQuickUse, LoadoutSlotType loadoutSlotType, Rect rect, string slotTag) {
		var rect2 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
		GUI.BeginGroup(rect2);

		if (itemQuickUse != null && itemQuickUse.Item != null) {
			itemQuickUse.Item.DrawIcon(new Rect(rect2.width - 60f, 0f, 48f, 48f));
			GUI.Label(new Rect(3f, 5f, rect2.width - 65f, 26f), itemQuickUse.Item.Name, BlueStonez.label_interparkbold_13pt_left);

			if (itemQuickUse.AmountRemaining > 0) {
				GUI.color = Color.white.SetAlpha(0.5f);
				GUI.Label(new Rect(3f, 34f, rect2.width - 65f, 12f), string.Format("Uses: {0}", itemQuickUse.AmountRemaining), BlueStonez.label_interparkbold_11pt_left);
				GUI.color = Color.white;
			}

			GUI.Label(new Rect(0f, rect2.height - 1f, rect2.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
		} else {
			GUI.Label(new Rect(rect2.width - 60f, 0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
			GUI.Label(new Rect(0f, 5f, rect2.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
			GUI.Label(new Rect(0f, rect2.height - 1f, rect2.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
		}

		GUI.EndGroup();

		if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen) {
			if (Event.current.type == EventType.MouseDown) {
				Singleton<InventoryManager>.Instance.CurrentQuickItemSot = loadoutSlotType;
			} else {
				AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(itemQuickUse.Item, rect2, PopupViewSide.Left);
			}
		}

		Color? color = null;

		if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemType == UberstrikeItemType.QuickUse) {
			color = new Color(1f, 1f, 1f, 0.1f);
		} else if (loadoutSlotType == _highlightedSlot) {
			color = new Color(1f, 1f, 1f, _highlightedSlotAlpha);
		}

		Singleton<DragAndDrop>.Instance.DrawSlot(new Rect(rect2.x, rect2.y - 5f, rect2.width - 6f, rect2.height), new ShopDragSlot {
			Item = itemQuickUse.Item,
			Slot = loadoutSlotType
		}, OnDropLoadout, color);
	}

	private void OnDropAvatar(int slotId, ShopDragSlot item) {
		if (item.Slot == LoadoutSlotType.Shop) {
			EquipItemFromArea(item.Item, LoadoutSlotType.None, ShopArea.Shop);
		} else {
			UnequipItem(item.Item);
		}
	}

	private void OnDropShop(int slotId, ShopDragSlot item) {
		if (item.Slot != LoadoutSlotType.Shop) {
			UnequipItem(item.Item);
		}
	}

	private void OnDropLoadout(int slotId, ShopDragSlot item) {
		Singleton<InventoryManager>.Instance.CurrentWeaponSlot = (LoadoutSlotType)slotId;

		if (item.Slot == LoadoutSlotType.Shop) {
			EquipItemFromArea(item.Item, (LoadoutSlotType)slotId, ShopArea.Shop);
		} else {
			switch (slotId) {
				case 8:
				case 9:
				case 10:
					if (item.Slot >= LoadoutSlotType.WeaponPrimary && item.Slot <= LoadoutSlotType.WeaponTertiary) {
						SwapWeapons(item.Slot, (LoadoutSlotType)slotId);
					}

					break;
				case 11:
				case 12:
				case 13:
					SwapQuickItems(item.Slot, (LoadoutSlotType)slotId);

					break;
			}
		}
	}

	private void OnBeginDrag(IDragSlot item) {
		if (item != null) {
			switch (item.Item.View.ItemType) {
				case UberstrikeItemType.Weapon:
				case UberstrikeItemType.WeaponMod:
					_loadoutAreaSelection.SetIndex(0);
					SelectLoadoutArea(LoadoutArea.Weapons);

					break;
				case UberstrikeItemType.Gear:
					_loadoutAreaSelection.SetIndex(1);
					SelectLoadoutArea(LoadoutArea.Gear);

					break;
				case UberstrikeItemType.QuickUse:
				case UberstrikeItemType.Functional:
					_loadoutAreaSelection.SetIndex(2);
					SelectLoadoutArea(LoadoutArea.QuickItems);

					break;
			}
		}
	}

	private void SwapQuickItems(LoadoutSlotType slot, LoadoutSlotType newslot) {
		if (Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slot, newslot)) {
			Singleton<InventoryManager>.Instance.CurrentQuickItemSot = newslot;
			HighlightingSlot(newslot);
		}
	}

	private void SwapWeapons(LoadoutSlotType slot, LoadoutSlotType newslot) {
		if (Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slot, newslot)) {
			Singleton<InventoryManager>.Instance.CurrentWeaponSlot = newslot;
			HighlightingSlot(newslot);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ShopDragSlot : IDragSlot {
		public int Id {
			get { return (int)Slot; }
		}

		public IUnityItem Item { get; set; }
		public LoadoutSlotType Slot { get; set; }
	}
}
