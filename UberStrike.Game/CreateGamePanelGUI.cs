using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CreateGamePanelGUI : MonoBehaviour, IPanelGui {
	private const int LevelMax = 80;
	private const int LevelMin = 1;
	private const int OFFSET = 6;
	private const int BUTTON_HEIGHT = 50;
	private const int MAP_WIDTH = 200;
	private const int MODE_WIDTH = 160;
	private const int DESC_WIDTH = 255;
	private const int MODS_WIDTH = 360;
	private const int MIN_WIDTH = 640;
	private const int MAX_WIDTH = 960;
	private const int MIN_NAME_FIELD_WIDTH = 115;
	private const int MAX_NAME_FIELD_WIDTH = 150;
	private const int LEFT_X = 0;
	private const int RIGHT_X = 370;
	private bool _animatingIndex;
	private bool _animatingWidth;
	private string _dmDescMsg = string.Empty;
	private string _elmDescMsg = string.Empty;
	private GameFlags.GAME_FLAGS _gameFlags;
	private string _gameName = string.Empty;
	private UberstrikeMap _mapSelected;
	private int _maxLevelCurrent = 80;
	private int _minLevelCurrent = 1;
	private SelectionGroup<GameModeType> _modeSelection = new SelectionGroup<GameModeType>();
	private string _password = string.Empty;
	private Vector2 _scroll = Vector2.zero;
	private int _selectedGrid;
	private float _sliderWidth = 130f;
	private string _tdmDescMsg = string.Empty;
	private float _textFieldWidth = 170f;
	private bool _viewingLeft = true;
	private Rect _windowRect;
	private float _xOffset;

	private bool IsModeSupported {
		get { return _mapSelected != null && _mapSelected.IsGameModeSupported(_modeSelection.Current); }
	}

	public bool IsEnabled {
		get { return enabled; }
	}

	public void Show() {
		enabled = true;
		_viewingLeft = true;
		_gameName = PlayerDataManager.Name;

		if (_gameName.Length > 18) {
			_gameName = _gameName.Remove(18);
		}
	}

	public void Hide() {
		enabled = false;
	}

	private void Awake() {
		_gameName = string.Empty;
	}

	private void Start() {
		_dmDescMsg = LocalizedStrings.DMModeDescriptionMsg;
		_tdmDescMsg = LocalizedStrings.TDMModeDescriptionMsg;
		_elmDescMsg = LocalizedStrings.ELMModeDescriptionMsg;
		_modeSelection.Add(GameModeType.EliminationMode, new GUIContent(LocalizedStrings.TeamElimination));
		_modeSelection.Add(GameModeType.TeamDeathMatch, new GUIContent(LocalizedStrings.TeamDeathMatch));
		_modeSelection.Add(GameModeType.DeathMatch, new GUIContent(LocalizedStrings.DeathMatch));
		_modeSelection.OnSelectionChange += delegate { };
	}

	private void Update() {
		if ((_windowRect.width != 960f && Screen.width >= 989) || (_windowRect.width != 640f && Screen.width < 989)) {
			_animatingWidth = true;
		}

		if (_animatingWidth) {
			if (Screen.width < 989) {
				_sliderWidth = Mathf.Lerp(_sliderWidth, 160f, Time.deltaTime * 8f);
				_textFieldWidth = Mathf.Lerp(_textFieldWidth, 150f, Time.deltaTime * 8f);
				_windowRect.width = Mathf.Lerp(_windowRect.width, 640f, Time.deltaTime * 8f);

				if (Mathf.Approximately(_windowRect.width, 640f)) {
					_animatingWidth = false;
					_sliderWidth = 160f;
					_textFieldWidth = 150f;
					_windowRect.width = 640f;
				}
			} else {
				_sliderWidth = Mathf.Lerp(_sliderWidth, 130f, Time.deltaTime * 8f);
				_textFieldWidth = Mathf.Lerp(_textFieldWidth, 115f, Time.deltaTime * 8f);
				_windowRect.width = Mathf.Lerp(_windowRect.width, 960f, Time.deltaTime * 8f);

				if (Mathf.Approximately(_windowRect.width, 960f)) {
					_animatingWidth = false;
					_sliderWidth = 130f;
					_textFieldWidth = 115f;
					_windowRect.width = 960f;
				}
			}
		}

		if (_animatingIndex) {
			if (_viewingLeft) {
				_xOffset = Mathf.Lerp(_xOffset, 0f, Time.deltaTime * 8f);

				if (Mathf.Abs(_xOffset) < 2f) {
					_xOffset = 0f;
					_animatingIndex = false;
				}
			} else {
				_xOffset = Mathf.Lerp(_xOffset, 370f, Time.deltaTime * 8f);

				if (Mathf.Abs(370f - _xOffset) < 2f) {
					_xOffset = 370f;
					_animatingIndex = false;
				}
			}
		}

		_windowRect.x = (Screen.width - _windowRect.width) * 0.5f;
		_windowRect.y = (Screen.height - _windowRect.height) * 0.5f + 25f;
	}

	private void OnGUI() {
		GUI.skin = BlueStonez.Skin;
		GUI.depth = 3;
		GUI.BeginGroup(_windowRect, GUIContent.none, BlueStonez.window);
		GUI.Label(new Rect(0f, 0f, _windowRect.width, 56f), LocalizedStrings.CreateGameCaps, BlueStonez.tab_strip);
		var rect = new Rect(0f, 60f, _windowRect.width, _windowRect.height - 60f);

		if (Screen.width < 989) {
			DrawRestrictedPanel(rect);
		} else {
			DrawFullPanel(rect);
		}

		GUI.EndGroup();
		GuiManager.DrawTooltip();
	}

	private void OnEnable() {
		_windowRect.width = (Screen.width >= 989) ? 960 : 640;
		_windowRect.height = 420f;
		_password = string.Empty;

		if (Screen.width < 989) {
			_sliderWidth = 160f;
			_windowRect.width = 640f;
			_textFieldWidth = 150f;
		} else {
			_sliderWidth = 130f;
			_windowRect.width = 960f;
			_textFieldWidth = 115f;
		}
	}

	private void SelectMap(UberstrikeMap map) {
		_mapSelected = map;

		foreach (var gameModeType in _modeSelection.Items) {
			if (_mapSelected.IsGameModeSupported(gameModeType)) {
				_modeSelection.Select(gameModeType);

				break;
			}
		}
	}

	private void DrawMapSelection(Rect rect) {
		var num = ((Singleton<MapManager>.Instance.Count <= 8) ? rect.width : (rect.width - 18f));
		var num2 = 0;

		foreach (var uberstrikeMap in Singleton<MapManager>.Instance.AllMaps) {
			if (uberstrikeMap.IsVisible) {
				num2++;
			}
		}

		_scroll = GUITools.BeginScrollView(rect, _scroll, new Rect(0f, 0f, rect.width - 18f, 10 + num2 * 35));
		var num3 = 0;

		foreach (var uberstrikeMap2 in Singleton<MapManager>.Instance.AllMaps) {
			if (uberstrikeMap2.IsVisible) {
				if (_mapSelected == null) {
					SelectMap(uberstrikeMap2);
				}

				var guicontent = new GUIContent(uberstrikeMap2.Name);

				if (GUI.Toggle(new Rect(0f, num3 * 35, num, 35f), uberstrikeMap2 == _mapSelected, guicontent, BlueStonez.tab_large_left) && _mapSelected != uberstrikeMap2) {
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.CreateGame);
					SelectMap(uberstrikeMap2);
				}

				num3++;
			}
		}

		GUITools.EndScrollView();
	}

	private void DrawGameModeSelection(Rect rect) {
		GUI.BeginGroup(rect);

		for (var i = 0; i < _modeSelection.Items.Length; i++) {
			GUITools.PushGUIState();

			if (_mapSelected != null && !_mapSelected.IsGameModeSupported(_modeSelection.Items[i])) {
				GUI.enabled = false;
			}

			if (GUI.Toggle(new Rect(0f, i * 20, rect.width, 20f), i == _modeSelection.Index, _modeSelection.GuiContent[i], BlueStonez.tab_medium) && _modeSelection.Index != i) {
				_modeSelection.SetIndex(i);

				if (GUI.changed) {
					GUI.changed = false;
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.CreateGame);
				}
			}

			GUI.enabled = true;
			GUITools.PopGUIState();
		}

		GUI.EndGroup();
	}

	private void DrawGameDescription(Rect rect) {
		var text = string.Empty;

		switch (_modeSelection.Current) {
			case GameModeType.DeathMatch:
				text = _dmDescMsg;

				break;
			case GameModeType.TeamDeathMatch:
				text = _tdmDescMsg;

				break;
			case GameModeType.EliminationMode:
				text = _elmDescMsg;

				break;
		}

		GUI.BeginGroup(rect);

		if (_mapSelected != null) {
			var num = 0;
			_mapSelected.Icon.Draw(new Rect(0f, 6f, rect.width, rect.width * _mapSelected.Icon.Aspect));
			num += 6 + Mathf.RoundToInt(rect.width * _mapSelected.Icon.Aspect);
			GUI.Label(new Rect(6f, num, rect.width - 12f, 20f), "Mission", BlueStonez.label_interparkbold_11pt_left);
			num += 20;
			GUI.Label(new Rect(6f, num, rect.width - 12f, 60f), text, BlueStonez.label_itemdescription);
			num += 36;
			GUI.Label(new Rect(6f, num, rect.width - 12f, 20f), "Location", BlueStonez.label_interparkbold_11pt_left);
			num += 20;
			GUI.Label(new Rect(6f, num, rect.width - 12f, 100f), _mapSelected.Description, BlueStonez.label_itemdescription);
		} else {
			GUI.Label(new Rect(6f, 100f, rect.width - 12f, 100f), "Please select a map", BlueStonez.label_interparkbold_16pt);
		}

		GUI.EndGroup();
	}

	private void DrawGameConfiguration(Rect rect) {
		if (IsModeSupported) {
			var mapSettings = _mapSelected.View.Settings[_modeSelection.Current];

			if (ApplicationDataManager.IsMobile) {
				mapSettings.PlayersMax = Mathf.Min(mapSettings.PlayersMax, 6);
			}

			GUI.BeginGroup(rect);
			GUI.Label(new Rect(6f, 0f, 100f, 25f), LocalizedStrings.GameName, BlueStonez.label_interparkbold_18pt_left);

			if (PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator) {
				GUI.SetNextControlName("GameName");
				_gameName = GUI.TextField(new Rect(130f, 5f, _textFieldWidth, 19f), _gameName, 18, BlueStonez.textField);

				if (string.IsNullOrEmpty(_gameName) && !GUI.GetNameOfFocusedControl().Equals("GameName")) {
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
					GUI.Label(new Rect(128f, 12f, 200f, 19f), LocalizedStrings.EnterGameName, BlueStonez.label_interparkmed_11pt_left);
					GUI.color = Color.white;
				}

				if (_gameName.Length > 18) {
					_gameName = _gameName.Remove(18);
				}
			} else {
				GUI.Label(new Rect(130f, 5f, _textFieldWidth, 19f), _gameName, BlueStonez.label);
			}

			GUI.Label(new Rect(130f + _textFieldWidth + 16f, 5f, 100f, 19f), "(" + _gameName.Length + "/18)", BlueStonez.label_interparkbold_11pt_left);
			GUI.Label(new Rect(6f, 25f, 100f, 25f), LocalizedStrings.Password, BlueStonez.label_interparkbold_18pt_left);
			GUI.SetNextControlName("GamePasswd");
			_password = GUI.PasswordField(new Rect(130f, 28f, _textFieldWidth, 19f), _password, '*', 8);
			_password = _password.Trim('\n');

			if (string.IsNullOrEmpty(_password) && !GUI.GetNameOfFocusedControl().Equals("GamePasswd")) {
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
				GUI.Label(new Rect(138f, 33f, 200f, 19f), "No password", BlueStonez.label_interparkmed_11pt_left);
				GUI.color = Color.white;
			}

			if (_password.Length > 8) {
				_password = _password.Remove(8);
			}

			GUI.Label(new Rect(130f + _textFieldWidth + 16f, 28f, 100f, 19f), "(" + _password.Length + "/8)", BlueStonez.label_interparkbold_11pt_left);
			GUI.Label(new Rect(6f, 55f, 110f, 25f), LocalizedStrings.MaxPlayers, BlueStonez.label_interparkbold_18pt_left);
			GUI.Label(new Rect(130f, 60f, 33f, 15f), Mathf.RoundToInt(mapSettings.PlayersCurrent).ToString(), BlueStonez.label_dropdown);
			mapSettings.PlayersCurrent = ((!ApplicationDataManager.IsMobile) ? mapSettings.PlayersCurrent : Mathf.Clamp(mapSettings.PlayersCurrent, 0, 6));
			mapSettings.PlayersCurrent = (int)GUI.HorizontalSlider(new Rect(170f, 60f, _sliderWidth, 15f), mapSettings.PlayersCurrent, mapSettings.PlayersMin, mapSettings.PlayersMax);
			var num = Mathf.RoundToInt(mapSettings.TimeCurrent / 60);
			GUI.Label(new Rect(6f, 83f, 100f, 25f), LocalizedStrings.TimeLimit, BlueStonez.label_interparkbold_18pt_left);
			GUI.Label(new Rect(130f, 83f, 33f, 15f), num.ToString(), BlueStonez.label_dropdown);
			mapSettings.TimeCurrent = 60 * (int)GUI.HorizontalSlider(new Rect(170f, 86f, _sliderWidth, 15f), num, mapSettings.TimeMin / 60, 10f);
			GUI.Label(new Rect(6f, 106f, 100f, 25f), LocalizedStrings.MaxKills, BlueStonez.label_interparkbold_18pt_left);
			GUI.Label(new Rect(130f, 106f, 33f, 15f), mapSettings.KillsCurrent.ToString(), BlueStonez.label_dropdown);
			mapSettings.KillsCurrent = (int)GUI.HorizontalSlider(new Rect(170f, 109f, _sliderWidth, 15f), mapSettings.KillsCurrent, mapSettings.KillsMin, 200f);
			GUI.Label(new Rect(6f, 150f, 100f, 25f), "Min Level", BlueStonez.label_interparkbold_18pt_left);
			GUI.Label(new Rect(130f, 150f, 33f, 15f), (_minLevelCurrent != 1) ? _minLevelCurrent.ToString() : "All", BlueStonez.label_dropdown);
			var num2 = (int)GUI.HorizontalSlider(new Rect(170f, 153f, _sliderWidth, 15f), _minLevelCurrent, 1f, 80f);

			if (num2 != _minLevelCurrent) {
				_minLevelCurrent = num2;
				_maxLevelCurrent = Mathf.Clamp(_maxLevelCurrent, _minLevelCurrent, 80);
			}

			GUI.Label(new Rect(6f, 172f, 100f, 25f), "Max Level", BlueStonez.label_interparkbold_18pt_left);
			GUI.Label(new Rect(130f, 172f, 33f, 15f), (_maxLevelCurrent != 80) ? _maxLevelCurrent.ToString() : "All", BlueStonez.label_dropdown);
			var num3 = (int)GUI.HorizontalSlider(new Rect(170f, 175f, _sliderWidth, 15f), _maxLevelCurrent, 1f, 80f);

			if (num3 != _maxLevelCurrent) {
				_maxLevelCurrent = num3;
				_minLevelCurrent = Mathf.Clamp(_minLevelCurrent, 1, _maxLevelCurrent);
			}

			if (!GameRoomHelper.IsLevelAllowed(_minLevelCurrent, _maxLevelCurrent, PlayerDataManager.PlayerLevel) && _minLevelCurrent > PlayerDataManager.PlayerLevel) {
				GUI.contentColor = Color.red;
				GUI.Label(new Rect(170f, 190f, _sliderWidth, 25f), "MinLevel is too high for you!", BlueStonez.label_interparkbold_11pt);
				GUI.contentColor = Color.white;
			} else if (!GameRoomHelper.IsLevelAllowed(_minLevelCurrent, _maxLevelCurrent, PlayerDataManager.PlayerLevel) && _maxLevelCurrent < PlayerDataManager.PlayerLevel) {
				GUI.contentColor = Color.red;
				GUI.Label(new Rect(170f, 190f, _sliderWidth, 25f), "MaxLevel is too low for you!", BlueStonez.label_interparkbold_11pt);
				GUI.contentColor = Color.white;
			}

			GUI.EndGroup();
		} else {
			GUI.Label(rect, "Unsupported Game Mode!", BlueStonez.label_interparkbold_18pt);
		}
	}

	private void ToggleGameFlag(GameFlags.GAME_FLAGS flag, int y, string content) {
		var flag2 = GUI.Toggle(new Rect(6f, y, 160f, 16f), _gameFlags == flag, content, BlueStonez.toggle);

		if (flag2) {
			_gameFlags = flag;
		} else if (_gameFlags == flag) {
			_gameFlags = GameFlags.GAME_FLAGS.None;
		}
	}

	private void DrawFullPanel(Rect rect) {
		var num = 6;
		var num2 = (int)rect.height - 50;
		GUI.BeginGroup(rect);
		GUI.Box(new Rect(6f, 0f, rect.width - 12f, num2), GUIContent.none, BlueStonez.window_standard_grey38);
		DrawMapSelection(new Rect(num, 0f, 200f, num2));
		num += 206;
		DrawVerticalLine(num - 3, 2f, num2);
		DrawGameModeSelection(new Rect(num, 0f, 160f, num2));
		num += 166;
		DrawVerticalLine(num - 3, 2f, num2);
		DrawGameDescription(new Rect(num, 0f, 255f, num2));
		num += 261;
		DrawVerticalLine(num - 3, 2f, num2);
		DrawGameConfiguration(new Rect(num, 0f, 360f, num2));

		if (GUITools.Button(new Rect(rect.width - 138f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button)) {
			PanelManager.Instance.ClosePanel(PanelType.CreateGame);
		}

		GUITools.PushGUIState();
		var text = string.Empty;

		if (_mapSelected == null) {
			text = "No map selected";
		} else if (_modeSelection == null) {
			text = "No mode selected";
		} else if (!IsModeSupported) {
			text = "Unsupported game mode: " + _modeSelection.Current;
		} else if (!Singleton<GameServerController>.Instance.SelectedServer.IsValid) {
			text = "Game server not valid: " + Singleton<GameServerController>.Instance.SelectedServer.ConnectionString;
		} else if (!LocalizationHelper.ValidateMemberName(_gameName, ApplicationDataManager.CurrentLocale)) {
			text = "Game name not valid: " + _gameName;
		}

		GUI.enabled = IsModeSupported && Singleton<GameServerController>.Instance.SelectedServer.IsValid && LocalizationHelper.ValidateMemberName(_gameName, ApplicationDataManager.CurrentLocale) && (string.IsNullOrEmpty(_password) || ValidateGamePassword(_password));

		if (GUITools.Button(new Rect(rect.width - 138f - 125f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CreateCaps, text), BlueStonez.button_green)) {
			PanelManager.Instance.ClosePanel(PanelType.CreateGame);
			_gameName = TextUtilities.Trim(_gameName);
			var mapSettings = _mapSelected.View.Settings[_modeSelection.Current];
			var num3 = Mathf.RoundToInt(mapSettings.TimeCurrent / 60) * 60;
			var connectionString = Singleton<GameServerController>.Instance.SelectedServer.ConnectionString;
			Singleton<GameStateController>.Instance.CreateNetworkGame(connectionString, _mapSelected.Id, _modeSelection.Current, _gameName, _password, num3, mapSettings.KillsCurrent, mapSettings.PlayersCurrent, _minLevelCurrent, _maxLevelCurrent, _gameFlags);
		}

		GUITools.PopGUIState();
		GUI.EndGroup();
	}

	private void DrawRestrictedPanel(Rect rect) {
		var num = 6f - _xOffset;
		var num2 = (int)rect.height - 50;
		GUI.BeginGroup(rect);
		GUI.Box(new Rect(6f, 0f, rect.width - 12f, num2), GUIContent.none, BlueStonez.window_standard_grey38);

		if (_animatingIndex || _viewingLeft) {
			DrawMapSelection(new Rect(num, 0f, 200f, num2));
		}

		num += 206f;

		if (_animatingIndex || _viewingLeft) {
			DrawVerticalLine(num - 3f, 2f, 300f);
			DrawGameModeSelection(new Rect(num, 0f, 160f, num2));
		}

		num += 166f;

		if (_animatingIndex || _viewingLeft) {
			DrawVerticalLine(num - 3f, 2f, 300f);
		}

		DrawGameDescription(new Rect(num, 0f, 255f, num2));
		num += 261f;

		if (_animatingIndex || !_viewingLeft) {
			DrawVerticalLine(num - 3f, 2f, 300f);
		}

		DrawGameConfiguration(new Rect(num, 0f, 360f, num2));

		if (GUITools.Button(new Rect(rect.width - 138f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button)) {
			PanelManager.Instance.ClosePanel(PanelType.CreateGame);
		}

		GUITools.PushGUIState();
		GUI.enabled = !_animatingIndex && !_animatingWidth;
		var text = ((!_viewingLeft) ? "Back" : "Customize");

		if (GUITools.Button(new Rect(rect.width - 138f - 125f, rect.height - 40f, 120f, 32f), new GUIContent(text), BlueStonez.button)) {
			_animatingIndex = true;
			_viewingLeft = !_viewingLeft;
		}

		GUITools.PopGUIState();
		var text2 = string.Empty;

		if (_mapSelected == null) {
			text2 = "No map selected";
		} else if (_modeSelection == null) {
			text2 = "No mode selected";
		} else if (!IsModeSupported) {
			text2 = "Unsupported game mode: " + _modeSelection.Current;
		} else if (!Singleton<GameServerController>.Instance.SelectedServer.IsValid) {
			text2 = "Game server not valid: " + Singleton<GameServerController>.Instance.SelectedServer.ConnectionString;
		} else if (!LocalizationHelper.ValidateMemberName(_gameName, ApplicationDataManager.CurrentLocale)) {
			text2 = "Game name not valid: " + _gameName;
		}

		GUITools.PushGUIState();
		GUI.enabled = IsModeSupported && Singleton<GameServerController>.Instance.SelectedServer.IsValid && LocalizationHelper.ValidateMemberName(_gameName, ApplicationDataManager.CurrentLocale) && (string.IsNullOrEmpty(_password) || ValidateGamePassword(_password));

		if (GUITools.Button(new Rect(rect.width - 138f - 250f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CreateCaps, text2), BlueStonez.button_green)) {
			PanelManager.Instance.ClosePanel(PanelType.CreateGame);
			var mapSettings = _mapSelected.View.Settings[_modeSelection.Current];
			var connectionString = Singleton<GameServerController>.Instance.SelectedServer.ConnectionString;
			Singleton<GameStateController>.Instance.CreateNetworkGame(connectionString, _mapSelected.Id, _modeSelection.Current, _gameName, _password, mapSettings.TimeCurrent, mapSettings.KillsCurrent, mapSettings.PlayersCurrent, _minLevelCurrent, _maxLevelCurrent, _gameFlags);
		}

		GUITools.PopGUIState();
		GUI.EndGroup();
	}

	private void DrawVerticalLine(float x, float y, float height) {
		GUI.Label(new Rect(x, y, 1f, height), GUIContent.none, BlueStonez.vertical_line_grey95);
	}

	private bool ValidateGamePassword(string psv) {
		var flag = false;

		if (!string.IsNullOrEmpty(psv) && psv.Length <= 8) {
			flag = true;
		}

		return flag;
	}
}
