using UnityEngine;

public class PageControllerPregameLoadout : PageControllerBase {
	[SerializeField]
	private GameObject joinButtons;

	private void Start() {
		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) { }, this);
	}
}
