using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPageManager : MonoBehaviour {
	private static PageType _currentPageType;
	private int _lastScreenHeight;
	private int _lastScreenWidth;
	private IDictionary<PageType, PageScene> _pageByPageType;
	private EaseType _transitionType = EaseType.InOut;
	public static MenuPageManager Instance { get; private set; }
	public float LeftAreaGUIOffset { get; set; }

	private void Awake() {
		_pageByPageType = new Dictionary<PageType, PageScene>();
	}

	private void OnEnable() {
		Instance = this;
		EventHandler.Global.AddListener(new Action<GlobalEvents.ScreenResolutionChanged>(OnScreenResolutionEvent));
	}

	private void OnDisable() {
		Instance = null;
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.ScreenResolutionChanged>(OnScreenResolutionEvent));
	}

	private void Start() {
		foreach (var pageScene in GetComponentsInChildren<PageScene>(true)) {
			_pageByPageType.Add(pageScene.PageType, pageScene);
		}

		if (GlobalUIRibbon.Instance) {
			GlobalUIRibbon.Instance.Show();
		}
	}

	private void OnScreenResolutionEvent(GlobalEvents.ScreenResolutionChanged ev) {
		var pagePanelWidth = GetPagePanelWidth(_currentPageType);
		AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width - pagePanelWidth);
	}

	private IEnumerator StartPageTransition(PageScene newPage, float time) {
		newPage.Load();

		if (newPage.HaveMouseOrbitCamera) {
			MouseOrbit.Instance.enabled = true;
			var offset = MouseOrbit.Instance.OrbitOffset;
			var config = MouseOrbit.Instance.OrbitConfig;
			var t = 0f;

			while (t < time && newPage.PageType == _currentPageType) {
				t += Time.deltaTime;
				MouseOrbit.Instance.OrbitConfig = Vector3.Lerp(config, newPage.MouseOrbitConfig, Mathfx.Ease(t / time, _transitionType));
				MouseOrbit.Instance.OrbitOffset = Vector3.Lerp(offset, newPage.MouseOrbitPivot, Mathfx.Ease(t / time, _transitionType));
				MouseOrbit.Instance.yPanningOffset = Mathf.Lerp(MouseOrbit.Instance.yPanningOffset, 0f, Mathfx.Ease(t / time, _transitionType));

				yield return new WaitForEndOfFrame();
			}

			if (newPage.PageType == _currentPageType) {
				MouseOrbit.Instance.OrbitOffset = newPage.MouseOrbitPivot;
				MouseOrbit.Instance.OrbitConfig = newPage.MouseOrbitConfig;
			}
		} else {
			MouseOrbit.Instance.enabled = false;
		}
	}

	private int GetPagePanelWidth(PageType type) {
		PageScene pageScene;

		if (_pageByPageType.TryGetValue(type, out pageScene)) {
			return pageScene.GuiWidth;
		}

		return 0;
	}

	private IEnumerator AnimateCameraPixelRect(PageType type, float time, bool immediate) {
		if (immediate) {
			AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width - GetPagePanelWidth(_currentPageType));

			yield return new WaitForEndOfFrame();
		} else {
			var t = time * 0.1f;
			var oldCameraWidth = AutoMonoBehaviour<CameraRectController>.Instance.PixelWidth;
			var panelWidth = GetPagePanelWidth(type);
			RenderSettingsController.Instance.DisableImageEffects();

			while (t < time && type == _currentPageType) {
				AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Mathf.Lerp(oldCameraWidth, Screen.width - panelWidth, t / time * (t / time)));

				yield return new WaitForEndOfFrame();

				t += Time.deltaTime;
			}

			AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width - GetPagePanelWidth(_currentPageType));
			RenderSettingsController.Instance.EnableImageEffects();
		}
	}

	public bool IsCurrentPage(PageType type) {
		return _currentPageType == type;
	}

	public PageType GetCurrentPage() {
		return _currentPageType;
	}

	public void UnloadCurrentPage() {
		PageScene pageScene;

		if (_pageByPageType.TryGetValue(_currentPageType, out pageScene) && pageScene) {
			pageScene.Unload();
			_currentPageType = PageType.None;
			MouseOrbit.Instance.enabled = false;
			AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width);
		}
	}

	public void LoadPage(PageType pageType, bool forceReload = false) {
		LeftAreaGUIOffset = 0f;

		if (PanelManager.Instance) {
			PanelManager.Instance.CloseAllPanels();
		}

		if (pageType == PageType.Home) {
			GameData.Instance.MainMenu.Value = MainMenuState.Home;
		}

		if (pageType == _currentPageType && !forceReload) {
			return;
		}

		PageScene pageScene = null;

		if (_pageByPageType.TryGetValue(pageType, out pageScene)) {
			PageScene pageScene2 = null;
			_pageByPageType.TryGetValue(_currentPageType, out pageScene2);

			if (pageScene2 && !forceReload) {
				pageScene2.Unload();
			}

			var flag = (_currentPageType == PageType.Home && pageType == PageType.Shop) || (_currentPageType == PageType.Home && pageType == PageType.Stats);
			_currentPageType = pageType;
			StartCoroutine(AnimateCameraPixelRect(pageScene.PageType, 0.25f, !flag));
			MouseOrbit.Instance.enabled = false;
			Instance.StartCoroutine(StartPageTransition(pageScene, 1f));
		}
	}

	private bool IsScreenResolutionChanged() {
		if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight) {
			_lastScreenWidth = Screen.width;
			_lastScreenHeight = Screen.height;

			return true;
		}

		return false;
	}
}
