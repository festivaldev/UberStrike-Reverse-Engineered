using UnityEngine;

public class HoloGearItem : BaseUnityItem {
	[SerializeField]
	private HoloGearItemConfiguration _config;

	public HoloGearItemConfiguration Configuration {
		get { return _config; }
		set { _config = value; }
	}
}
