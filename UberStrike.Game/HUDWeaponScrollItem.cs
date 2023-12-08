using UnityEngine;

public class HUDWeaponScrollItem : MonoBehaviour {
	[SerializeField]
	private UILabel weaponName;

	public string WeaponName {
		set { weaponName.text = value; }
	}
}
