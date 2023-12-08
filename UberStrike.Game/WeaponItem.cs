using UnityEngine;

public class WeaponItem : BaseUnityItem {
	[SerializeField]
	private WeaponItemConfiguration _config;

	public WeaponItemConfiguration Configuration {
		get { return _config; }
		set { _config = value; }
	}
}
