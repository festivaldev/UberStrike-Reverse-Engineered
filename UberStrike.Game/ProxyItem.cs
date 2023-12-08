using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class ProxyItem : IUnityItem {
	private Texture2D m_Icon;

	public int CriticalStrikeBonus {
		get {
			if (View.ItemProperties.ContainsKey(ItemPropertyType.CritDamageBonus)) {
				return View.ItemProperties[ItemPropertyType.CritDamageBonus];
			}

			return 0;
		}
	}

	public ProxyItem(BaseUberStrikeItemView view) {
		View = view;

		if (UnityItemConfiguration.Instance.IsPrefabAvailable(View.PrefabName)) {
			var text = UnityItemConfiguration.Instance.GetPrefabPath(view.PrefabName);
			text += "-Icon";
			m_Icon = Resources.Load<Texture2D>(text);
		} else if (View.ItemClass == UberstrikeItemClass.FunctionalGeneral) {
			m_Icon = UnityItemConfiguration.Instance.GetFunctionalItemIcon(View.ID);
		} else {
			m_Icon = UnityItemConfiguration.Instance.GetDefaultIcon(view.ItemClass);
		}
	}

	public bool Equippable {
		get { return true; }
	}

	public string Name {
		get { return View.Name; }
	}

	public bool IsLoaded { get; private set; }
	public GameObject Prefab { get; private set; }
	public BaseUberStrikeItemView View { get; private set; }
	public void Unload() { }

	public GameObject Create(Vector3 position, Quaternion rotation) {
		if (UnityItemConfiguration.Instance.IsPrefabAvailable(View.PrefabName)) {
			var prefabPath = UnityItemConfiguration.Instance.GetPrefabPath(View.PrefabName);
			Debug.Log(string.Concat("Create Item:", View.ID, ", ", View.Name, ", ", prefabPath));
			UnityEngine.Object @object = Resources.Load<GameObject>(prefabPath);
			Prefab = (GameObject)@object;
		} else {
			Debug.Log(string.Concat("Create DEFAULT Item:", View.ID, ", ", View.Name, ", ", View.PrefabName));
			Prefab = UnityItemConfiguration.Instance.GetDefaultItem(View.ItemClass);
		}

		if (View.ItemType == UberstrikeItemType.QuickUse) {
			var component = Prefab.GetComponent<QuickItem>();

			if (component != null && component.Sfx) {
				Singleton<QuickItemSfxController>.Instance.RegisterQuickItemEffect(component.Logic, component.Sfx);
			}
		}

		GameObject gameObject = null;

		if (Prefab != null) {
			if (View.ItemClass == UberstrikeItemClass.GearHolo) {
				var component2 = Prefab.GetComponent<HoloGearItem>();

				if (component2 && component2.Configuration.Avatar) {
					gameObject = UnityEngine.Object.Instantiate(component2.Configuration.Avatar.gameObject) as GameObject;
				}
			} else {
				gameObject = UnityEngine.Object.Instantiate(Prefab, position, rotation) as GameObject;
			}

			if (gameObject && View.ItemType == UberstrikeItemType.Weapon) {
				var component3 = gameObject.GetComponent<WeaponItem>();

				if (component3) {
					ItemConfigurationUtil.CopyCustomProperties(View, component3.Configuration);

					if (View.ItemProperties.ContainsKey(ItemPropertyType.CritDamageBonus)) {
						component3.Configuration.CriticalStrikeBonus = View.ItemProperties[ItemPropertyType.CritDamageBonus];
					} else {
						component3.Configuration.CriticalStrikeBonus = 0;
					}
				}
			}
		} else {
			Debug.LogError("Trying to create item prefab, but it was null. Item Name:" + View.Name);
		}

		IsLoaded = true;

		return gameObject;
	}

	public void DrawIcon(Rect position) {
		GUI.DrawTexture(position, m_Icon);
	}

	public void UpdateProxyItem(BaseUberStrikeItemView view) {
		View = view;
	}
}
