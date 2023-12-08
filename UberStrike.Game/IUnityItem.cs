using UberStrike.Core.Models.Views;
using UnityEngine;

public interface IUnityItem {
	string Name { get; }
	bool Equippable { get; }
	BaseUberStrikeItemView View { get; }
	bool IsLoaded { get; }
	GameObject Prefab { get; }
	GameObject Create(Vector3 position, Quaternion rotation);
	void Unload();
	void DrawIcon(Rect position);
}
