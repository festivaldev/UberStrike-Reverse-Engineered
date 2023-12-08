using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class DefaultItem : IUnityItem {
	private Texture2D _icon;
	private GameObject _prefab;

	public DefaultItem(BaseUberStrikeItemView view) {
		View = view;

		switch (view.ItemType) {
			case UberstrikeItemType.Weapon:
				_prefab = Singleton<ItemManager>.Instance.GetDefaultWeaponItem(view.ItemClass);

				break;
			case UberstrikeItemType.Gear:
				_prefab = Singleton<ItemManager>.Instance.GetDefaultGearItem(view.ItemClass);

				break;
		}

		_icon = UnityItemConfiguration.Instance.GetDefaultIcon(view.ItemClass);
	}

	public DefaultItem(GameObject prefab) {
		_prefab = prefab;
	}

	public bool Equippable {
		get { return true; }
	}

	public bool IsLoaded {
		get { return true; }
	}

	public GameObject Prefab {
		get { return _prefab; }
	}

	public string Name {
		get { return View.Name; }
	}

	public BaseUberStrikeItemView View { get; private set; }
	public void Unload() { }

	public GameObject Create(Vector3 position, Quaternion rotation) {
		if (_prefab) {
			return UnityEngine.Object.Instantiate(_prefab.gameObject, position, rotation) as GameObject;
		}

		Debug.LogError("Failed to create default item: " + View.Name);

		return null;
	}

	public void DrawIcon(Rect position) {
		var color = GUI.color;
		GUI.color = color.SetAlpha((!GUI.enabled) ? 0.5f : 1f);
		GUI.DrawTexture(position, _icon);
		GUI.color = color;
	}
}
