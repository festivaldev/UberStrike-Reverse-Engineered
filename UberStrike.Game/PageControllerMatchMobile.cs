using UnityEngine;

public class PageControllerMatchMobile : PageControllerBase {
	[SerializeField]
	private GameObject ammoBar;

	[SerializeField]
	private GameObject armorBar;

	[SerializeField]
	private GameObject healthBar;

	[SerializeField]
	private GameObject hudMobileChatEventStream;

	[SerializeField]
	private GameObject hudMobileQuickItems;

	[SerializeField]
	private HUDMobileWeaponSelector hudMobileWeaponSelector;

	[SerializeField]
	private GameObject hudReticleController;

	[SerializeField]
	private GameObject hudStatusPanel;

	[SerializeField]
	private GameObject itemPickup;

	[SerializeField]
	private GameObject weaponFeedback;

	private void Start() {
		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) {
			PageControllerMatch.HandleSharedViews(el, healthBar, ammoBar, armorBar, hudReticleController, hudStatusPanel, itemPickup);
			var flag = el == PlayerStateId.Playing;
			var flag2 = el == PlayerStateId.PrepareForMatch;
			hudMobileWeaponSelector.gameObject.SetActive(flag || flag2);
			hudMobileQuickItems.SetActive(flag || flag2);
			weaponFeedback.SetActive(flag || flag2);
		}, this);

		TouchInput.OnSecondaryFire.AddEvent(delegate(bool el) {
			hudMobileWeaponSelector.Show(!el);
			hudMobileQuickItems.SetActive(!el);
		}, this);
	}
}
