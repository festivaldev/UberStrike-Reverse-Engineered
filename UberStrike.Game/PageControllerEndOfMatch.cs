using UnityEngine;

public class PageControllerEndOfMatch : PageControllerBase {
	[SerializeField]
	private GameObject joinButtons;

	private void Start() {
		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) { }, this);
	}
}
