using System.Collections.Generic;
using UnityEngine;

public class GamePageManager : MonoBehaviour {
	private static IDictionary<IngamePageType, SceneGuiController> _pageByPageType;
	private static IngamePageType _currentPageType;
	public static GamePageManager Instance { get; private set; }

	public static bool Exists {
		get { return Instance != null; }
	}

	public static bool HasPage {
		get { return _currentPageType != IngamePageType.None; }
	}

	private void Awake() {
		Instance = this;
		_pageByPageType = new Dictionary<IngamePageType, SceneGuiController>();
	}

	private void Start() {
		foreach (var sceneGuiController in GetComponentsInChildren<SceneGuiController>(true)) {
			_pageByPageType[sceneGuiController.PageType] = sceneGuiController;
		}
	}

	public static bool IsCurrentPage(IngamePageType type) {
		return _currentPageType == type;
	}

	public SceneGuiController GetCurrentPage() {
		SceneGuiController sceneGuiController;
		_pageByPageType.TryGetValue(_currentPageType, out sceneGuiController);

		return sceneGuiController;
	}

	public void UnloadCurrentPage() {
		var currentPage = GetCurrentPage();

		if (currentPage) {
			currentPage.gameObject.SetActive(false);
			_currentPageType = IngamePageType.None;
		}

		EventHandler.Global.Fire(new GlobalEvents.GamePageChanged());
	}

	public void LoadPage(IngamePageType pageType) {
		if (pageType == _currentPageType) {
			return;
		}

		SceneGuiController sceneGuiController = null;

		if (_pageByPageType.TryGetValue(pageType, out sceneGuiController)) {
			SceneGuiController sceneGuiController2 = null;
			_pageByPageType.TryGetValue(_currentPageType, out sceneGuiController2);

			if (sceneGuiController2) {
				sceneGuiController2.gameObject.SetActive(false);
			}

			_currentPageType = pageType;
			sceneGuiController.gameObject.SetActive(true);
			EventHandler.Global.Fire(new GlobalEvents.GamePageChanged());
		}
	}
}
