using System;
using UberStrike.Core.Types;
using UnityEngine;

public class PageControllerMatch : PageControllerBase {
	[SerializeField]
	private GameObject ammoBar;

	[SerializeField]
	private GameObject armorBar;

	[SerializeField]
	private GameObject desktopChat;

	[SerializeField]
	private HUDDesktopEventStream eventStream;

	[SerializeField]
	private GameObject fps;

	[SerializeField]
	private GameObject healthBar;

	[SerializeField]
	private GameObject hudReticleController;

	[SerializeField]
	private GameObject hudStatusPanel;

	[SerializeField]
	private GameObject itemPickup;

	[SerializeField]
	private GameObject quickItems;

	[SerializeField]
	private GameObject weaponScroller;

	private void Start() {
		GameData.Instance.PlayerState.AddEventAndFire((Action<PlayerStateId>)(el => {
			HandleSharedViews(el, healthBar, ammoBar, armorBar, hudReticleController, hudStatusPanel, itemPickup);
			var flag1 = el == PlayerStateId.Playing;
			var flag2 = el == PlayerStateId.Spectating;
			var flag3 = el == PlayerStateId.Killed;
			var flag4 = el == PlayerStateId.Paused;
			var flag5 = el == PlayerStateId.PrepareForMatch;
			desktopChat.SetActive(flag1 || flag4 || flag5 || flag3 || flag2);
			eventStream.gameObject.SetActive(flag1 || flag4 || flag5 || flag3 || flag2);
			weaponScroller.SetActive(flag1 || flag5);
			quickItems.SetActive(flag1 || flag5);

			if (eventStream.gameObject.activeInHierarchy)
				eventStream.DoAnimateDown(flag4 || flag3);

			fps.SetActive(true);
		}), (MonoBehaviour)this);

		GameData.Instance.OnHUDStreamClear.AddEvent((Action)(() => Singleton<DamageFeedbackHud>.Instance.ClearAll()), (MonoBehaviour)this);
	}

	public static void HandleSharedViews(PlayerStateId state, GameObject healthBar, GameObject ammoBar, GameObject armorBar, GameObject hudReticleController, GameObject hudStatusPanel, GameObject itemPickup) {
		var flag = state == PlayerStateId.Playing;
		var flag2 = state == PlayerStateId.Spectating;
		var flag3 = state == PlayerStateId.Killed;
		var flag4 = state == PlayerStateId.Paused;
		var flag5 = state == PlayerStateId.PrepareForMatch;
		var flag6 = GameState.Current.GameMode == GameModeType.None;
		var flag7 = GameData.Instance.GameState.Value == GameStateId.WaitingForPlayers;
		flag2 |= flag4 && (!GameState.Current.Players.ContainsKey(PlayerDataManager.Cmid) || GameState.Current.PlayerData.IsSpectator);
		healthBar.SetActive((flag || flag4 || flag5) && !flag2);
		armorBar.SetActive((flag || flag4 || flag5) && !flag2);
		ammoBar.SetActive((flag || flag5) && !flag2);
		hudReticleController.SetActive(flag || flag5);
		hudStatusPanel.SetActive((flag || flag4 || flag5 || flag2 || flag3) && !flag6 && !flag7);

		if (hudStatusPanel.activeInHierarchy) {
			hudStatusPanel.GetComponent<HUDStatusPanel>().IsOnPaused(flag4 || flag3 || flag2);
		}

		itemPickup.SetActive(flag);
	}

	private void Update() {
		Singleton<DamageFeedbackHud>.Instance.Update();
	}

	private void OnGUI() {
		Singleton<DamageFeedbackHud>.Instance.Draw();
	}
}
