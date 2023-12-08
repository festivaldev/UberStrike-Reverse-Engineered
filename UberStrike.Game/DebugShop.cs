using UnityEngine;

public class DebugShop : IDebugPage {
	private Vector2 scroll1;
	private Vector2 scroll2;
	private Vector2 scroll3;

	public string Title {
		get { return "Shop"; }
	}

	public void Draw() {
		GUILayout.BeginHorizontal();
		scroll1 = GUILayout.BeginScrollView(scroll1, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("SHOP");

		foreach (var unityItem in Singleton<ItemManager>.Instance.ShopItems) {
			GUILayout.Label(unityItem.View.ID + ": " + unityItem.Name);
		}

		GUILayout.EndScrollView();
		scroll2 = GUILayout.BeginScrollView(scroll2, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("INVENTORY");

		foreach (var inventoryItem in Singleton<InventoryManager>.Instance.InventoryItems) {
			GUILayout.Label(string.Concat(inventoryItem.Item.View.ID, ": ", inventoryItem.Item.Name, ", Amount: ", inventoryItem.AmountRemaining, ", Days: ", inventoryItem.DaysRemaining));
		}

		GUILayout.EndScrollView();
		scroll3 = GUILayout.BeginScrollView(scroll3, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("LOADOUT");
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
	}
}
