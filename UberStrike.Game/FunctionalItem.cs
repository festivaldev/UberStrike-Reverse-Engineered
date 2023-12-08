using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class FunctionalItem : IUnityItem {
	private Texture2D _icon;

	public UberstrikeItemClass ItemClass {
		get { return View.ItemClass; }
	}

	public string PrefabName {
		get { return string.Empty; }
	}

	public FunctionalItem(BaseUberStrikeItemView view) {
		View = view;
		_icon = UnityItemConfiguration.Instance.GetFunctionalItemIcon(view.ID);
	}

	public bool Equippable {
		get { return false; }
	}

	public string Name {
		get { return View.Name; }
		set { View.Name = value; }
	}

	public bool IsLoaded {
		get { return true; }
	}

	public GameObject Prefab {
		get { return null; }
	}

	public BaseUberStrikeItemView View { get; private set; }
	public void Unload() { }

	public GameObject Create(Vector3 position, Quaternion rotation) {
		return null;
	}

	public void DrawIcon(Rect position) {
		if (_icon != null) {
			GUI.DrawTexture(position, _icon);
		} else {
			Debug.LogError(string.Concat("Can't find icon for item:", View.ID, ", ", View.Name));
		}
	}
}
