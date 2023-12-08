using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UberStrike.Core.Types;
using UnityEngine;

public class Loadout {
	private Dictionary<LoadoutSlotType, IUnityItem> _items;

	public int ItemCount {
		get { return _items.Count; }
	}

	public static Loadout Empty {
		get { return new Loadout(new Dictionary<LoadoutSlotType, IUnityItem>()); }
	}

	public Loadout(Loadout gearLoadout) : this(gearLoadout._items) { }

	public Loadout(Dictionary<LoadoutSlotType, IUnityItem> items) {
		foreach (var keyValuePair in items)
			SetSlot(keyValuePair.Key, keyValuePair.Value);
	}

	public Loadout(List<int> gearItemIds, List<int> weaponItemIds) {
		if (gearItemIds.Count < 7 || weaponItemIds.Count < 4)
			Debug.LogError("Invalid parameters: gear count = " + gearItemIds.Count + " weapon count = " + weaponItemIds.Count);

		SetSlot(LoadoutSlotType.GearHead, gearItemIds[1]);
		SetSlot(LoadoutSlotType.GearFace, gearItemIds[2]);
		SetSlot(LoadoutSlotType.GearGloves, gearItemIds[3]);
		SetSlot(LoadoutSlotType.GearUpperBody, gearItemIds[4]);
		SetSlot(LoadoutSlotType.GearLowerBody, gearItemIds[5]);
		SetSlot(LoadoutSlotType.GearBoots, gearItemIds[6]);

		if (gearItemIds[0] > 0)
			SetSlot(LoadoutSlotType.GearHolo, gearItemIds[0]);

		SetSlot(LoadoutSlotType.WeaponMelee, weaponItemIds[0]);
		SetSlot(LoadoutSlotType.WeaponPrimary, weaponItemIds[1]);
		SetSlot(LoadoutSlotType.WeaponSecondary, weaponItemIds[2]);
		SetSlot(LoadoutSlotType.WeaponTertiary, weaponItemIds[3]);
	}

	public event Action OnGearChanged;
	public event Action<LoadoutSlotType> OnWeaponChanged;

	public bool TryGetItem(LoadoutSlotType slot, out IUnityItem item) {
		return _items.TryGetValue(slot, out item);
	}

	public void SetSlot(LoadoutSlotType slot, int itemId) {
		SetSlot(slot, Singleton<ItemManager>.Instance.GetItemInShop(itemId));
	}

	public void SetSlot(LoadoutSlotType slot, IUnityItem item) {
		if (item != null && CanGoInSlot(slot, item.View.ItemType)) {
			_items[slot] = item;

			switch (slot) {
				case LoadoutSlotType.GearHead:
				case LoadoutSlotType.GearFace:
				case LoadoutSlotType.GearGloves:
				case LoadoutSlotType.GearUpperBody:
				case LoadoutSlotType.GearLowerBody:
				case LoadoutSlotType.GearBoots:
				case LoadoutSlotType.GearHolo:
					OnGearChanged();

					break;
				case LoadoutSlotType.WeaponMelee:
				case LoadoutSlotType.WeaponPrimary:
				case LoadoutSlotType.WeaponSecondary:
				case LoadoutSlotType.WeaponTertiary:
					OnWeaponChanged(slot);

					break;
			}
		}
	}

	public bool CanGoInSlot(LoadoutSlotType slot, UberstrikeItemType type) {
		switch (type) {
			case UberstrikeItemType.Weapon:
				return slot >= LoadoutSlotType.WeaponMelee && slot <= LoadoutSlotType.WeaponTertiary;
			case UberstrikeItemType.Gear:
				return slot >= LoadoutSlotType.GearHead && slot <= LoadoutSlotType.GearHolo;
			case UberstrikeItemType.QuickUse:
				return slot >= LoadoutSlotType.QuickUseItem1 && slot <= LoadoutSlotType.QuickUseItem3;
			case UberstrikeItemType.Functional:
				return slot >= LoadoutSlotType.FunctionalItem1 && slot <= LoadoutSlotType.FunctionalItem3;
		}

		Debug.LogError("Item attempted to be equipped into a slot that isn't supported.");

		return false;
	}

	public void ClearSlot(LoadoutSlotType slot) {
		IUnityItem unityItem;

		if (_items.TryGetValue(slot, out unityItem)) {
			_items.Remove(slot);
			OnGearChanged();
		}
	}

	public void ClearAllSlots() { }

	public bool Compare(Loadout a) {
		var flag = ItemCount == a.ItemCount;

		if (flag) {
			foreach (var keyValuePair in _items) {
				IUnityItem unityItem;

				if (!a.TryGetItem(keyValuePair.Key, out unityItem)) {
					return false;
				}

				if (unityItem != keyValuePair.Value) {
					return false;
				}
			}

			return flag;
		}

		return flag;
	}

	public LoadoutSlotType GetItemClassSlotType(UberstrikeItemClass itemClass) {
		foreach (var keyValuePair in _items) {
			if (keyValuePair.Value.View.ItemClass == itemClass) {
				return keyValuePair.Key;
			}
		}

		return LoadoutSlotType.None;
	}

	public LoadoutSlotType GetFirstEmptyWeaponSlot() {
		if (!_items.ContainsKey(LoadoutSlotType.WeaponPrimary)) {
			return LoadoutSlotType.WeaponPrimary;
		}

		if (!_items.ContainsKey(LoadoutSlotType.WeaponSecondary)) {
			return LoadoutSlotType.WeaponSecondary;
		}

		if (!_items.ContainsKey(LoadoutSlotType.WeaponTertiary)) {
			return LoadoutSlotType.WeaponTertiary;
		}

		return LoadoutSlotType.None;
	}

	public bool Contains(string prefabName) {
		var flag = false;

		foreach (var unityItem in _items.Values) {
			if (unityItem.View.PrefabName.Equals(prefabName)) {
				flag = true;

				break;
			}
		}

		return flag;
	}

	public bool Contains(int itemId) {
		var flag = false;

		foreach (var unityItem in _items.Values) {
			if (unityItem.View.ID == itemId) {
				flag = true;

				break;
			}
		}

		return flag;
	}

	public override string ToString() {
		var stringBuilder = new StringBuilder();

		foreach (var keyValuePair in _items) {
			stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Key, keyValuePair.Value.Name));
		}

		return stringBuilder.ToString();
	}

	public Dictionary<LoadoutSlotType, IUnityItem>.Enumerator GetEnumerator() {
		return _items.GetEnumerator();
	}

	private void OnItemPrefabUpdated(IUnityItem item) {
		var keyValuePair = _items.FirstOrDefault(a => a.Value.View.ID == item.View.ID);

		if (keyValuePair.Value != null) {
			var key = keyValuePair.Key;
			IUnityItem unityItem;

			if (_items.TryGetValue(key, out unityItem) && unityItem == item) {
				switch (item.View.ItemType) {
					case UberstrikeItemType.Weapon:
						OnWeaponChanged(key);

						break;
					case UberstrikeItemType.Gear:
						CheckAllGear();

						break;
				}
			}
		} else {
			Debug.LogError("OnItemPrefabUpdated failed because slot not found");
		}
	}

	private void CheckAllGear() {
		IUnityItem unityItem;
		bool flag;

		if (_items.TryGetValue(LoadoutSlotType.GearHolo, out unityItem)) {
			flag = unityItem.IsLoaded;
		} else {
			var flag2 = true;

			foreach (var loadoutSlotType in LoadoutManager.GearSlots) {
				if (_items.TryGetValue(loadoutSlotType, out unityItem)) {
					flag2 &= unityItem.IsLoaded;
				}
			}

			flag = flag2;
		}

		if (flag) {
			OnGearChanged();
		}
	}

	public AvatarGearParts GetAvatarGear() {
		var flag = false;
		var avatarGearParts = new AvatarGearParts();
		IUnityItem unityItem;

		if (_items.TryGetValue(LoadoutSlotType.GearHolo, out unityItem)) {
			avatarGearParts.Base = unityItem.Create(Vector3.zero, Quaternion.identity);
		}

		if (!avatarGearParts.Base) {
			flag = true;
			avatarGearParts.Base = UnityEngine.Object.Instantiate(PrefabManager.Instance.DefaultAvatar.gameObject) as GameObject;
		}

		if (flag) {
			foreach (var loadoutSlotType in LoadoutManager.GearSlots) {
				if (_items.TryGetValue(loadoutSlotType, out unityItem)) {
					var gameObject = unityItem.Create(Vector3.zero, Quaternion.identity);

					if (gameObject) {
						avatarGearParts.Parts.Add(gameObject);
					}
				} else {
					var defaultGearItem = Singleton<ItemManager>.Instance.GetDefaultGearItem(ItemUtil.ItemClassFromSlot(loadoutSlotType));

					if (defaultGearItem) {
						var gameObject2 = UnityEngine.Object.Instantiate(defaultGearItem) as GameObject;

						if (gameObject2) {
							avatarGearParts.Parts.Add(gameObject2);
						}
					}
				}
			}
		}

		return avatarGearParts;
	}

	public AvatarGearParts GetRagdollGear() {
		var avatarGearParts = new AvatarGearParts();

		try {
			IUnityItem unityItem;
			bool flag;

			if (_items.TryGetValue(LoadoutSlotType.GearHolo, out unityItem)) {
				flag = !unityItem.IsLoaded;

				if (unityItem.Prefab) {
					var component = unityItem.Prefab.GetComponent<HoloGearItem>();

					if (component && component.Configuration.Ragdoll) {
						avatarGearParts.Base = UnityEngine.Object.Instantiate(component.Configuration.Ragdoll.gameObject) as GameObject;
					} else {
						avatarGearParts.Base = UnityEngine.Object.Instantiate(PrefabManager.Instance.DefaultRagdoll.gameObject) as GameObject;
					}
				} else {
					avatarGearParts.Base = UnityEngine.Object.Instantiate(PrefabManager.Instance.DefaultRagdoll.gameObject) as GameObject;
				}
			} else {
				flag = true;
				avatarGearParts.Base = UnityEngine.Object.Instantiate(PrefabManager.Instance.DefaultRagdoll.gameObject) as GameObject;
			}

			if (flag) {
				foreach (var loadoutSlotType in LoadoutManager.GearSlots) {
					if (_items.TryGetValue(loadoutSlotType, out unityItem)) {
						var gameObject = unityItem.Create(Vector3.zero, Quaternion.identity);

						if (gameObject) {
							avatarGearParts.Parts.Add(gameObject);
						}
					} else if (Singleton<ItemManager>.Instance.TryGetDefaultItem(ItemUtil.ItemClassFromSlot(loadoutSlotType), out unityItem)) {
						var gameObject2 = unityItem.Create(Vector3.zero, Quaternion.identity);

						if (gameObject2) {
							avatarGearParts.Parts.Add(gameObject2);
						}
					}
				}
			}
		} catch (Exception ex) {
			Debug.LogException(ex);
		}

		return avatarGearParts;
	}

	internal void UpdateWeaponSlots(List<int> weaponItemIds) {
		SetSlot(LoadoutSlotType.WeaponMelee, weaponItemIds[0]);
		SetSlot(LoadoutSlotType.WeaponPrimary, weaponItemIds[1]);
		SetSlot(LoadoutSlotType.WeaponSecondary, weaponItemIds[2]);
		SetSlot(LoadoutSlotType.WeaponTertiary, weaponItemIds[3]);
	}

	internal void UpdateGearSlots(List<int> gearItemIds) {
		SetSlot(LoadoutSlotType.GearHead, gearItemIds[1]);
		SetSlot(LoadoutSlotType.GearFace, gearItemIds[2]);
		SetSlot(LoadoutSlotType.GearGloves, gearItemIds[3]);
		SetSlot(LoadoutSlotType.GearUpperBody, gearItemIds[4]);
		SetSlot(LoadoutSlotType.GearLowerBody, gearItemIds[5]);
		SetSlot(LoadoutSlotType.GearBoots, gearItemIds[6]);

		if (gearItemIds[0] > 0) {
			SetSlot(LoadoutSlotType.GearHolo, gearItemIds[0]);
		}
	}
}
