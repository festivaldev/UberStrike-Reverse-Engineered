using System;
using UberStrike.Core.Models.Views;
using UberStrike.WebService.Unity;
using UnityEngine;

public class QuickItemController : Singleton<QuickItemController> {
	private const float CooldownTime = 0.5f;
	private bool _isEnabled;
	private QuickItem[] _quickItems;
	public bool IsQuickItemMobilePushed;
	private float[] recharges = new float[3];

	public bool IsEnabled {
		get { return _isEnabled && GameState.Current.PlayerData.IsAlive; }
		set { _isEnabled = value; }
	}

	public bool IsConsumptionEnabled { get; set; }
	public int CurrentSlotIndex { get; private set; }
	public float NextCooldownFinishTime { get; set; }
	public QuickItemRestriction Restriction { get; private set; }

	public QuickItem[] QuickItems {
		get { return _quickItems; }
	}

	private QuickItemController() {
		_quickItems = new QuickItem[LoadoutManager.QuickSlots.Length];
		Restriction = new QuickItemRestriction();
		EventHandler.Global.AddListener(new Action<GlobalEvents.InputChanged>(OnInputChanged));
	}

	public void Initialize() {
		Clear();

		for (var i = 0; i < LoadoutManager.QuickSlots.Length; i++) {
			var loadoutSlotType = LoadoutManager.QuickSlots[i];
			InventoryItem inventoryItem;

			if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(loadoutSlotType, out inventoryItem)) {
				var gameObject = inventoryItem.Item.Create(Vector3.zero, Quaternion.identity);
				InitializeQuickItem(gameObject, loadoutSlotType, inventoryItem);
			} else {
				Restriction.InitializeSlot(i);
			}
		}

		CurrentSlotIndex = ((!(_quickItems[0] == null)) ? 0 : GetNextAvailableSlotIndex(0));
		GameData.Instance.OnQuickItemsChanged.Fire();
	}

	private void InitializeQuickItem(GameObject quickItemObject, LoadoutSlotType slot, InventoryItem inventoryItem) {
		var slotIndex = GetSlotIndex(slot);
		var component = quickItemObject.GetComponent<QuickItem>();

		if (component) {
			component.gameObject.SetActive(true);

			for (var i = 0; i < component.gameObject.transform.childCount; i++) {
				component.gameObject.transform.GetChild(i).gameObject.SetActive(false);
			}

			component.gameObject.name = inventoryItem.Item.Name;
			component.transform.parent = GameState.Current.Player.WeaponAttachPoint;

			if (component.rigidbody) {
				component.rigidbody.isKinematic = true;
			}

			ItemConfigurationUtil.CopyProperties<UberStrikeItemQuickView>(component.Configuration, inventoryItem.Item.View);
			ItemConfigurationUtil.CopyCustomProperties(inventoryItem.Item.View, component.Configuration);

			if (component.Configuration.RechargeTime <= 0) {
				var index = slotIndex;
				var behaviour = component.Behaviour;

				behaviour.OnActivated = (Action)Delegate.Combine(behaviour.OnActivated, new Action(delegate {
					UseConsumableItem(inventoryItem);
					Restriction.DecreaseUse(index);
					NextCooldownFinishTime = Time.time + 0.5f;
				}));

				Restriction.InitializeSlot(slotIndex, component, inventoryItem.AmountRemaining);
			} else {
				component.Behaviour.CurrentAmount = component.Configuration.AmountRemaining;
			}

			component.Behaviour.FocusKey = GetFocusKey(slot);
			var grenadeProjectile = component as IGrenadeProjectile;

			if (grenadeProjectile != null) {
				grenadeProjectile.OnProjectileEmitted += delegate(IGrenadeProjectile p) {
					Singleton<ProjectileManager>.Instance.AddProjectile(p, Singleton<WeaponController>.Instance.NextProjectileId());
					GameState.Current.Actions.EmitQuickItem(p.Position, p.Velocity, inventoryItem.Item.View.ID, GameState.Current.PlayerData.Player.PlayerId, p.ID);
				};
			}

			if (_quickItems[slotIndex]) {
				UnityEngine.Object.Destroy(_quickItems[slotIndex].gameObject);
			}

			_quickItems[slotIndex] = component;
		} else {
			Debug.LogError("Failed to initialize QuickItem");
		}

		GameData.Instance.OnQuickItemsChanged.Fire();
	}

	private bool IsCastingOrCooldown() {
		foreach (var quickItem in _quickItems) {
			if (quickItem != null && quickItem.Behaviour.IsCoolingDown) {
				return true;
			}
		}

		return false;
	}

	private void UseQuickItem(int index) {
		if (!IsEnabled || IsCastingOrCooldown() || Time.time < NextCooldownFinishTime) {
			return;
		}

		if (_quickItems != null && index >= 0 && _quickItems[index] != null) {
			if (_quickItems[index].Behaviour.Run() && GameState.Current.Player.Character != null) {
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.WeaponSwitch);
			}
		} else {
			Debug.LogError("The QuickItem has no Behaviour: " + index);
		}
	}

	public void Update() {
		if (_quickItems == null) {
			return;
		}

		for (var i = 0; i < _quickItems.Length; i++) {
			if (!(_quickItems[i] == null)) {
				var chargingTimeRemaining = _quickItems[i].Behaviour.ChargingTimeRemaining;

				if (recharges[i] != chargingTimeRemaining) {
					recharges[i] = chargingTimeRemaining;
				}
			}
		}
	}

	private void OnInputChanged(GlobalEvents.InputChanged ev) {
		if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled && ev.IsDown && !LevelCamera.IsZoomedIn && IsEnabled) {
			var key = ev.Key;

			switch (key) {
				case GameInputKey.UseQuickItem:
					UseQuickItem(CurrentSlotIndex);

					break;
				default:
					switch (key) {
						case GameInputKey.QuickItem1:
							UseQuickItem(0);

							break;
						case GameInputKey.QuickItem2:
							UseQuickItem(1);

							break;
						case GameInputKey.QuickItem3:
							UseQuickItem(2);

							break;
					}

					break;
				case GameInputKey.NextQuickItem:
					if (_quickItems.Length > 0) {
						CurrentSlotIndex = GetNextAvailableSlotIndex(CurrentSlotIndex);
						GameData.Instance.OnQuickItemsChanged.Fire();
					}

					break;
				case GameInputKey.PrevQuickItem:
					if (_quickItems.Length > 0) {
						CurrentSlotIndex = GetPrevAvailableSlotIndex(CurrentSlotIndex);
						GameData.Instance.OnQuickItemsChanged.Fire();
					}

					break;
			}
		}

		if (ev.Key == GameInputKey.UseQuickItem && !LevelCamera.IsZoomedIn && IsEnabled) {
			IsQuickItemMobilePushed = ev.IsDown;
		}
	}

	private int GetNextAvailableSlotIndex(int currentSlot) {
		for (var num = (currentSlot + 1) % _quickItems.Length; num != currentSlot; num = (num + 1) % _quickItems.Length) {
			if (_quickItems[num] != null) {
				return num;
			}
		}

		return currentSlot;
	}

	private int GetPrevAvailableSlotIndex(int currentSlot) {
		for (var num = (currentSlot - 1) % _quickItems.Length; num != currentSlot; num = (num - 1) % _quickItems.Length) {
			if (num < 0) {
				num = _quickItems.Length - 1;
			}

			if (_quickItems[num] != null) {
				return num;
			}
		}

		return currentSlot;
	}

	private void UseConsumableItem(InventoryItem inventoryItem) {
		if (IsConsumptionEnabled) {
			ShopWebServiceClient.UseConsumableItem(PlayerDataManager.AuthToken, inventoryItem.Item.View.ID, null, null);
			inventoryItem.AmountRemaining--;

			if (inventoryItem.AmountRemaining == 0) {
				UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(false));
			}
		}
	}

	private GameInputKey GetFocusKey(LoadoutSlotType slot) {
		switch (slot) {
			case LoadoutSlotType.QuickUseItem1:
				return GameInputKey.QuickItem1;
			case LoadoutSlotType.QuickUseItem2:
				return GameInputKey.QuickItem2;
			case LoadoutSlotType.QuickUseItem3:
				return GameInputKey.QuickItem3;
			default:
				return GameInputKey.None;
		}
	}

	private int GetSlotIndex(LoadoutSlotType slot) {
		switch (slot) {
			case LoadoutSlotType.QuickUseItem1:
				return 0;
			case LoadoutSlotType.QuickUseItem2:
				return 1;
			case LoadoutSlotType.QuickUseItem3:
				return 2;
			default:
				return -1;
		}
	}

	internal void Reset() { }

	internal void Clear() {
		for (var i = 0; i < _quickItems.Length; i++) {
			if (_quickItems[i] != null) {
				UnityEngine.Object.Destroy(_quickItems[i].gameObject);
			}

			_quickItems[i] = null;
		}
	}
}
