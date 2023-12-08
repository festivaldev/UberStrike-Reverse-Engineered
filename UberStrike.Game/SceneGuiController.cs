using System;
using UnityEngine;

public class SceneGuiController : MonoBehaviour {
	private int _currentGuiPageIndex;
	private FloatAnim _currentWidth;

	[SerializeField]
	private PageGUI[] _guiPages;

	private GUIContent[] _guiPageTabs;

	[SerializeField]
	private IngamePageType _pageType;

	private Rect _rect;

	[SerializeField]
	private string _title;

	[SerializeField]
	private float _width;

	public IngamePageType PageType {
		get { return _pageType; }
	}

	private void Awake() {
		_currentWidth = new FloatAnim(delegate { AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width - _currentWidth.Value); });
		_guiPageTabs = new GUIContent[_guiPages.Length];

		for (var i = 0; i < _guiPages.Length; i++) {
			_guiPageTabs[i] = new GUIContent(_guiPages[i].Title);
		}
	}

	private void OnEnable() {
		if (_guiPages.Length > 0) {
			SetCurrentPage(0);
			_currentWidth.Value = _width;
		}

		EventHandler.Global.AddListener(new Action<GlobalEvents.ScreenResolutionChanged>(OnScreenResolutionChange));
	}

	private void OnDisable() {
		if (_guiPages[_currentGuiPageIndex] != null) {
			_guiPages[_currentGuiPageIndex].enabled = false;
		}

		_currentWidth.Value = 0f;
		_currentGuiPageIndex = -1;
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.ScreenResolutionChanged>(OnScreenResolutionChange));
	}

	private void OnGUI() {
		GUI.depth = 11;
		_rect.x = Screen.width - _width;
		_rect.y = GlobalUIRibbon.Instance.Height();
		_rect.width = _width;
		_rect.height = Screen.height - _rect.y;
		GUI.skin = BlueStonez.Skin;
		GUI.BeginGroup(_rect, GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.Label(new Rect(0f, 0f, _rect.width, 56f), _title, BlueStonez.tab_strip);
		GUI.changed = false;
		_currentGuiPageIndex = UnityGUI.Toolbar(new Rect(0f, 34f, 140 * _guiPageTabs.Length, 22f), _currentGuiPageIndex, _guiPageTabs, _guiPageTabs.Length, BlueStonez.tab_medium);

		if (GUI.changed) {
			SetCurrentPage(_currentGuiPageIndex);

			return;
		}

		GUI.EndGroup();
		_guiPages[_currentGuiPageIndex].DrawGUI(new Rect(_rect.x, _rect.y + 57f, _rect.width, _rect.height - 56f));
		GuiManager.DrawTooltip();
	}

	private void SetCurrentPage(int index) {
		for (var i = 0; i < _guiPages.Length; i++) {
			_guiPages[i].IsOnGUIEnabled = false;
			_guiPages[i].enabled = false;
		}

		if (index >= 0 && index < _guiPages.Length) {
			_currentGuiPageIndex = index;
			_guiPages[_currentGuiPageIndex].enabled = true;
		}
	}

	private void OnScreenResolutionChange(GlobalEvents.ScreenResolutionChanged ev) {
		AutoMonoBehaviour<CameraRectController>.Instance.SetAbsoluteWidth(Screen.width - _currentWidth.Value);
	}
}
