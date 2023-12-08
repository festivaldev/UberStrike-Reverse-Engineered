using System;
using UnityEngine;

public class HUDManager : MonoBehaviour {
	[SerializeField]
	private PageControllerBase endOfMatchPage;

	[SerializeField]
	private PageControllerBase matchRunningPage;

	[SerializeField]
	private PageControllerBase pregameLoadoutPage;

	private void Start() {
		GameData.Instance.GameState.AddEventAndFire(delegate(GameStateId el) {
			var flag = el == GameStateId.MatchRunning;
			var flag2 = el == GameStateId.PregameLoadout;
			var flag3 = el == GameStateId.WaitingForPlayers;
			var flag4 = el == GameStateId.EndOfMatch;
			var flag5 = el == GameStateId.PrepareNextRound;
			TrySetActive(pregameLoadoutPage, flag2);
			TrySetActive(matchRunningPage, flag || flag3 || flag5);
			TrySetActive(endOfMatchPage, flag4);
			GameData.Instance.PlayerState.Fire();
		}, this);

		EventHandler.Global.AddListener(new Action<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged));
		OnCameraWidthChanged(null);
	}

	private void OnDestroy() {
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged));
	}

	private void OnCameraWidthChanged(GlobalEvents.CameraWidthChanged obj) {
		UICamera.eventHandler.cachedCamera.rect = new Rect(0f, 0f, AutoMonoBehaviour<CameraRectController>.Instance.NormalizedWidth, 1f);
	}

	private void TrySetActive(MonoBehaviour page, bool active) {
		if (page != null) {
			page.gameObject.SetActive(active);
		}
	}
}
