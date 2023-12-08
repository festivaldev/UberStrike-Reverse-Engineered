using System;
using System.Collections;
using UnityEngine;

internal class GUIController : MonoBehaviour {
	[SerializeField]
	private GUIPageBase home;

	[SerializeField]
	private GameObject xpbar;

	private IEnumerator Start() {
		while (!Singleton<AuthenticationManager>.Instance.IsAuthComplete) {
			yield return new WaitForEndOfFrame();
		}

		home.gameObject.SetActive(true);
		GameData.Instance.MainMenu.AddEventAndFire(OnMenuChanged, this);
		EventHandler.Global.AddListener(new Action<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged));
		OnCameraWidthChanged(null);
	}

	private void OnDestroy() {
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged));
	}

	private void OnCameraWidthChanged(GlobalEvents.CameraWidthChanged obj) {
		UICamera.eventHandler.cachedCamera.rect = new Rect(0f, 0f, AutoMonoBehaviour<CameraRectController>.Instance.NormalizedWidth, 1f);
	}

	private void OnMenuChanged(MainMenuState state) {
		SetPage(home, state == MainMenuState.Home);

		if (xpbar != null) {
			xpbar.SetActive(state != MainMenuState.Logout);
		}
	}

	private void SetPage(GUIPageBase page, bool enabled) {
		if (page == null) {
			return;
		}

		page.gameObject.SetActive(enabled);

		if (enabled) {
			page.BringIn();
		}
	}
}
