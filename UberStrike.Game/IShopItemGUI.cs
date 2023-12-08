using UnityEngine;

public interface IShopItemGUI {
	IUnityItem Item { get; }
	void Draw(Rect rect, bool selected);
}
