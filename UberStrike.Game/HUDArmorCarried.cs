using UnityEngine;

public class HUDArmorCarried : MonoBehaviour {
	[SerializeField]
	private UILabel armorCarriedValue;

	private void OnEnable() {
		GameState.Current.PlayerData.ArmorCarried.Fire();
	}

	private void Start() {
		GameState.Current.PlayerData.ArmorCarried.AddEventAndFire(delegate(int el) { armorCarriedValue.text = el.ToString(); }, this);
		GameData.Instance.IsShopLoaded.AddEventAndFire(delegate(bool el) { NGUITools.SetActiveChildren(gameObject, el); }, this);
	}
}
