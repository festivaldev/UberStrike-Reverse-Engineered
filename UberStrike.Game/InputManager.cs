using System.Collections.Generic;
using UnityEngine;

public class InputManager : AutoMonoBehaviour<InputManager> {
	private const float _mouseScrollThreshold = 0.01f;
	private bool _inputEnabled;
	private bool _isGamepadEnabled;
	private Dictionary<int, UserInputMap> _keyMapping = new Dictionary<int, UserInputMap>();
	private bool _unassignedKeyMappings;
	public static int SkipFrame { get; set; }

	public bool IsGamepadEnabled {
		get { return _isGamepadEnabled; }
		set {
			_isGamepadEnabled = value;

			if (_isGamepadEnabled) {
				KeyMapping[1].Channel = new AxisInputChannel("RS X", 0f);
				KeyMapping[2].Channel = new AxisInputChannel("RS Y", 0f);
			} else {
				KeyMapping[1].Channel = new AxisInputChannel("Mouse X", 0f);
				KeyMapping[2].Channel = new AxisInputChannel("Mouse Y", 0f);
			}
		}
	}

	public Dictionary<int, UserInputMap> KeyMapping {
		get { return _keyMapping; }
	}

	public bool IsAnyDown {
		get {
			if (IsInputEnabled) {
				foreach (var userInputMap in _keyMapping.Values) {
					if (userInputMap.Value != 0f) {
						return true;
					}
				}

				return false;
			}

			return false;
		}
	}

	public bool IsInputEnabled {
		get { return _inputEnabled; }
		set {
			_inputEnabled = value;

			if (!_inputEnabled) {
				foreach (var userInputMap in _keyMapping.Values) {
					if (userInputMap != null && userInputMap.Channel != null) {
						userInputMap.Channel.Reset();

						if (userInputMap.IsEventSender && userInputMap.Channel.IsChanged) {
							EventHandler.Global.Fire(new GlobalEvents.InputChanged(userInputMap.Slot, userInputMap.Channel.Value));
						}
					}
				}
			}
		}
	}

	public bool IsSettingKeymap { get; private set; }

	public bool HasUnassignedKeyMappings {
		get { return _unassignedKeyMappings; }
	}

	private void Awake() {
		SetDefaultKeyMapping();
	}

	private void Update() {
		if (SkipFrame == Time.frameCount) {
			return;
		}

		if (GameData.Instance.HUDChatIsTyping) {
			return;
		}

		foreach (var userInputMap in _keyMapping.Values) {
			if (userInputMap != null && userInputMap.Channel != null) {
				userInputMap.Channel.Listen();

				if (userInputMap.IsEventSender && userInputMap.Channel.IsChanged) {
					EventHandler.Global.Fire(new GlobalEvents.InputChanged(userInputMap.Slot, userInputMap.Channel.Value));
				}
			}
		}

		if (RawValue(GameInputKey.Fullscreen) != 0f && GUITools.SaveClickIn(0.2f)) {
			GUITools.Clicked();
			ScreenResolutionManager.IsFullScreen = !Screen.fullScreen;
		}
	}

	private void OnGUI() {
		if (Event.current.shift && Event.current.type == EventType.ScrollWheel) {
			if (Event.current.delta.x > 0f) {
				EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.PrevWeapon, Event.current.delta.x));
			}

			if (Event.current.delta.x < 0f) {
				EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.NextWeapon, Event.current.delta.x));
			}
		}
	}

	public static bool GetMouseButtonDown(int button) {
		return (Event.current == null || Event.current.type == EventType.Layout) && Input.GetMouseButtonDown(button);
	}

	public bool ListenForNewKeyAssignment(UserInputMap map) {
		if (Event.current.keyCode == KeyCode.Escape) {
			IsSettingKeymap = false;

			return true;
		}

		if (Event.current.keyCode != KeyCode.None) {
			map.Channel = new KeyInputChannel(Event.current.keyCode);
		} else if (Event.current.shift) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				map.Channel = new KeyInputChannel(KeyCode.LeftShift);
			}

			if (Input.GetKey(KeyCode.RightShift)) {
				map.Channel = new KeyInputChannel(KeyCode.RightShift);
			}
		} else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3) || Input.GetMouseButtonDown(4)) {
			map.Channel = new MouseInputChannel(Event.current.button);
		} else if (Mathf.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) > 0.1f) {
			map.Channel = new AxisInputChannel("Mouse ScrollWheel", 0.1f, (Input.GetAxisRaw("Mouse ScrollWheel") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("LS X")) > 0.1f) {
			map.Channel = new AxisInputChannel("LS X", 0.1f, (Input.GetAxisRaw("LS X") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("LS Y")) > 0.1f) {
			map.Channel = new AxisInputChannel("LS Y", 0.1f, (Input.GetAxisRaw("LS Y") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("RS X")) > 0.1f) {
			map.Channel = new AxisInputChannel("RS X", 0.1f, (Input.GetAxisRaw("RS X") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("RS Y")) > 0.1f) {
			map.Channel = new AxisInputChannel("RS Y", 0.1f, (Input.GetAxisRaw("RS Y") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("DPad X")) > 0.1f) {
			map.Channel = new AxisInputChannel("DPad X", 0.1f, (Input.GetAxisRaw("DPad X") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Mathf.Abs(Input.GetAxisRaw("DPad Y")) > 0.1f) {
			map.Channel = new AxisInputChannel("DPad Y", 0.1f, (Input.GetAxisRaw("DPad Y") <= 0f) ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
		} else if (Input.GetAxisRaw("LT") > 0.1f) {
			map.Channel = new AxisInputChannel("LT", 0.1f);
		} else if (Input.GetAxisRaw("RT") > 0.1f) {
			map.Channel = new AxisInputChannel("RT", 0.1f);
		} else if (Input.GetButton("A")) {
			map.Channel = new ButtonInputChannel("A");
		} else if (Input.GetButton("B")) {
			map.Channel = new ButtonInputChannel("B");
		} else if (Input.GetButton("X")) {
			map.Channel = new ButtonInputChannel("X");
		} else if (Input.GetButton("Y")) {
			map.Channel = new ButtonInputChannel("Y");
		} else if (Input.GetButton("LB")) {
			map.Channel = new ButtonInputChannel("LB");
		} else if (Input.GetButton("RB")) {
			map.Channel = new ButtonInputChannel("RB");
		} else if (Input.GetButton("Start")) {
			map.Channel = new ButtonInputChannel("Start");
		} else {
			if (!Input.GetButton("Back")) {
				IsSettingKeymap = true;

				return false;
			}

			map.Channel = new ButtonInputChannel("Back");
		}

		EventHandler.Global.Fire(new GlobalEvents.InputAssignment());
		Event.current.Use();
		ResolveMultipleAssignment(map);
		WriteAllKeyMappings();
		IsSettingKeymap = false;

		return true;
	}

	public void Reset() {
		_keyMapping.Clear();
		SetDefaultKeyMapping();
		IsGamepadEnabled = false;
		WriteAllKeyMappings();
	}

	public float RawValue(GameInputKey slot) {
		UserInputMap userInputMap;

		if (!IsSettingKeymap && _keyMapping.TryGetValue((int)slot, out userInputMap)) {
			return userInputMap.RawValue();
		}

		return 0f;
	}

	public float GetValue(GameInputKey slot) {
		UserInputMap userInputMap;

		if (!IsSettingKeymap && IsInputEnabled && _keyMapping.TryGetValue((int)slot, out userInputMap)) {
			return userInputMap.Value;
		}

		return 0f;
	}

	public bool IsDown(GameInputKey slot) {
		UserInputMap userInputMap;

		return !IsSettingKeymap && _keyMapping.TryGetValue((int)slot, out userInputMap) && userInputMap.Value != 0f;
	}

	public string GetKeyAssignmentString(GameInputKey slot) {
		UserInputMap userInputMap;

		if (_keyMapping.TryGetValue((int)slot, out userInputMap) && userInputMap != null) {
			return userInputMap.Assignment;
		}

		return "Not set";
	}

	public string GetSlotName(GameInputKey slot) {
		switch (slot) {
			case GameInputKey.None:
				return "None";
			case GameInputKey.HorizontalLook:
				return "HorizontalLook";
			case GameInputKey.VerticalLook:
				return "VerticalLook";
			case GameInputKey.Forward:
				return "Forward";
			case GameInputKey.Backward:
				return "Backward";
			case GameInputKey.Left:
				return "Left";
			case GameInputKey.Right:
				return "Right";
			case GameInputKey.Jump:
				return "Jump";
			case GameInputKey.Crouch:
				return "Crouch";
			case GameInputKey.PrimaryFire:
				return "Primary Fire";
			case GameInputKey.SecondaryFire:
				return "Secondary Fire";
			case GameInputKey.Weapon1:
				return "Primary Weapon";
			case GameInputKey.Weapon2:
				return "Secondary Weapon";
			case GameInputKey.Weapon3:
				return "Tertiary Weapon";
			case GameInputKey.WeaponMelee:
				return "Melee Weapon";
			case GameInputKey.QuickItem1:
				return "Quick Item 1";
			case GameInputKey.QuickItem2:
				return "Quick Item 2";
			case GameInputKey.QuickItem3:
				return "Quick Item 3";
			case GameInputKey.NextWeapon:
				return "Next Weapon / Zoom In";
			case GameInputKey.PrevWeapon:
				return "Prev Weapon / Zoom Out";
			case GameInputKey.Pause:
				return "Pause";
			case GameInputKey.Fullscreen:
				return "Fullscreen";
			case GameInputKey.Tabscreen:
				return "Tabscreen";
			case GameInputKey.Chat:
				return "Chat";
			case GameInputKey.Loadout:
				return "Loadout";
			case GameInputKey.UseQuickItem:
				return "Use QuickItem";
			case GameInputKey.ChangeTeam:
				return "Change Team";
			case GameInputKey.NextQuickItem:
				return "Cycle QuickItems";
			case GameInputKey.SendScreenshotToFacebook:
				return "Send Screenshot to Facebook";
		}

		return "No Name";
	}

	private void ResolveMultipleAssignment(UserInputMap map) {
		foreach (var userInputMap in _keyMapping.Values) {
			if (userInputMap != map && userInputMap.Channel != null && userInputMap.Channel.ChannelType == map.Channel.ChannelType && map.Assignment == userInputMap.Assignment) {
				userInputMap.Channel = null;

				break;
			}
		}
	}

	private bool IsChannelTaken(IInputChannel channel) {
		var flag = false;

		foreach (var userInputMap in _keyMapping.Values) {
			if (userInputMap.Channel.Equals(channel)) {
				flag = true;

				break;
			}
		}

		return flag;
	}

	private void SetDefaultKeyMapping() {
		_keyMapping[1] = new UserInputMap(GetSlotName(GameInputKey.HorizontalLook), GameInputKey.HorizontalLook, new AxisInputChannel("Mouse X", 0f), false, false);
		_keyMapping[2] = new UserInputMap(GetSlotName(GameInputKey.VerticalLook), GameInputKey.VerticalLook, new AxisInputChannel("Mouse Y", 1f), false, false);
		_keyMapping[21] = new UserInputMap(GetSlotName(GameInputKey.Pause), GameInputKey.Pause, new KeyInputChannel(KeyCode.Backspace));
		_keyMapping[23] = new UserInputMap(GetSlotName(GameInputKey.Tabscreen), GameInputKey.Tabscreen, new KeyInputChannel(KeyCode.Tab));
		_keyMapping[22] = new UserInputMap(GetSlotName(GameInputKey.Fullscreen), GameInputKey.Fullscreen, new KeyInputChannel(KeyCode.F), true, true, KeyCode.LeftAlt);
		_keyMapping[30] = new UserInputMap(GetSlotName(GameInputKey.SendScreenshotToFacebook), GameInputKey.SendScreenshotToFacebook, new KeyInputChannel(KeyCode.B));
		_keyMapping[3] = new UserInputMap(GetSlotName(GameInputKey.Forward), GameInputKey.Forward, new KeyInputChannel(KeyCode.W));
		_keyMapping[5] = new UserInputMap(GetSlotName(GameInputKey.Left), GameInputKey.Left, new KeyInputChannel(KeyCode.A));
		_keyMapping[4] = new UserInputMap(GetSlotName(GameInputKey.Backward), GameInputKey.Backward, new KeyInputChannel(KeyCode.S));
		_keyMapping[6] = new UserInputMap(GetSlotName(GameInputKey.Right), GameInputKey.Right, new KeyInputChannel(KeyCode.D));
		_keyMapping[7] = new UserInputMap(GetSlotName(GameInputKey.Jump), GameInputKey.Jump, new KeyInputChannel(KeyCode.Space));
		_keyMapping[8] = new UserInputMap(GetSlotName(GameInputKey.Crouch), GameInputKey.Crouch, new KeyInputChannel(KeyCode.LeftShift));
		_keyMapping[9] = new UserInputMap(GetSlotName(GameInputKey.PrimaryFire), GameInputKey.PrimaryFire, new MouseInputChannel(0));
		_keyMapping[10] = new UserInputMap(GetSlotName(GameInputKey.SecondaryFire), GameInputKey.SecondaryFire, new MouseInputChannel(1));
		_keyMapping[19] = new UserInputMap(GetSlotName(GameInputKey.NextWeapon), GameInputKey.NextWeapon, new AxisInputChannel("Mouse ScrollWheel", 0.01f, AxisInputChannel.AxisReadingMethod.PositiveOnly));
		_keyMapping[20] = new UserInputMap(GetSlotName(GameInputKey.PrevWeapon), GameInputKey.PrevWeapon, new AxisInputChannel("Mouse ScrollWheel", 0.01f, AxisInputChannel.AxisReadingMethod.NegativeOnly));
		_keyMapping[15] = new UserInputMap(GetSlotName(GameInputKey.WeaponMelee), GameInputKey.WeaponMelee, new KeyInputChannel(KeyCode.Alpha1));
		_keyMapping[11] = new UserInputMap(GetSlotName(GameInputKey.Weapon1), GameInputKey.Weapon1, new KeyInputChannel(KeyCode.Alpha2));
		_keyMapping[12] = new UserInputMap(GetSlotName(GameInputKey.Weapon2), GameInputKey.Weapon2, new KeyInputChannel(KeyCode.Alpha3));
		_keyMapping[13] = new UserInputMap(GetSlotName(GameInputKey.Weapon3), GameInputKey.Weapon3, new KeyInputChannel(KeyCode.Alpha4));
		_keyMapping[16] = new UserInputMap(GetSlotName(GameInputKey.QuickItem1), GameInputKey.QuickItem1, new KeyInputChannel(KeyCode.Alpha6));
		_keyMapping[17] = new UserInputMap(GetSlotName(GameInputKey.QuickItem2), GameInputKey.QuickItem2, new KeyInputChannel(KeyCode.Alpha7));
		_keyMapping[18] = new UserInputMap(GetSlotName(GameInputKey.QuickItem3), GameInputKey.QuickItem3, new KeyInputChannel(KeyCode.Alpha8));
		_keyMapping[27] = new UserInputMap(GetSlotName(GameInputKey.ChangeTeam), GameInputKey.ChangeTeam, new KeyInputChannel(KeyCode.M), true, true, KeyCode.LeftAlt);
		_keyMapping[26] = new UserInputMap(GetSlotName(GameInputKey.UseQuickItem), GameInputKey.UseQuickItem, new KeyInputChannel(KeyCode.E));
		_keyMapping[28] = new UserInputMap(GetSlotName(GameInputKey.NextQuickItem), GameInputKey.NextQuickItem, new KeyInputChannel(KeyCode.R));
	}

	private static CmunePrefs.Key GetPrefsKeyForSlot(int slot) {
		switch (slot) {
			case 0:
				return CmunePrefs.Key.Keymap_None;
			case 1:
				return CmunePrefs.Key.Keymap_HorizontalLook;
			case 2:
				return CmunePrefs.Key.Keymap_VerticalLook;
			case 3:
				return CmunePrefs.Key.Keymap_Forward;
			case 4:
				return CmunePrefs.Key.Keymap_Backward;
			case 5:
				return CmunePrefs.Key.Keymap_Left;
			case 6:
				return CmunePrefs.Key.Keymap_Right;
			case 7:
				return CmunePrefs.Key.Keymap_Jump;
			case 8:
				return CmunePrefs.Key.Keymap_Crouch;
			case 9:
				return CmunePrefs.Key.Keymap_PrimaryFire;
			case 10:
				return CmunePrefs.Key.Keymap_SecondaryFire;
			case 11:
				return CmunePrefs.Key.Keymap_Weapon1;
			case 12:
				return CmunePrefs.Key.Keymap_Weapon2;
			case 13:
				return CmunePrefs.Key.Keymap_Weapon3;
			case 15:
				return CmunePrefs.Key.Keymap_WeaponMelee;
			case 16:
				return CmunePrefs.Key.Keymap_QuickItem1;
			case 17:
				return CmunePrefs.Key.Keymap_QuickItem2;
			case 18:
				return CmunePrefs.Key.Keymap_QuickItem3;
			case 19:
				return CmunePrefs.Key.Keymap_NextWeapon;
			case 20:
				return CmunePrefs.Key.Keymap_PrevWeapon;
			case 21:
				return CmunePrefs.Key.Keymap_Pause;
			case 22:
				return CmunePrefs.Key.Keymap_Fullscreen;
			case 23:
				return CmunePrefs.Key.Keymap_Tabscreen;
			case 24:
				return CmunePrefs.Key.Keymap_Chat;
			case 25:
				return CmunePrefs.Key.Keymap_Inventory;
			case 26:
				return CmunePrefs.Key.Keymap_UseQuickItem;
			case 27:
				return CmunePrefs.Key.Keymap_ChangeTeam;
			case 28:
				return CmunePrefs.Key.Keymap_NextQuickItem;
			case 30:
				return CmunePrefs.Key.Keymap_SendScreenshotToFacebook;
		}

		return CmunePrefs.Key.Keymap_None;
	}

	private void WriteAllKeyMappings() {
		_unassignedKeyMappings = false;

		foreach (var keyValuePair in _keyMapping) {
			if (keyValuePair.Value.IsConfigurable) {
				CmunePrefs.WriteKey(GetPrefsKeyForSlot(keyValuePair.Key), keyValuePair.Value.GetPlayerPrefs());

				if (keyValuePair.Value.Channel == null) {
					_unassignedKeyMappings = true;
				}
			}
		}
	}

	public void ReadAllKeyMappings() {
		_unassignedKeyMappings = false;

		foreach (var keyValuePair in _keyMapping) {
			if (keyValuePair.Value.IsConfigurable) {
				string text;

				if (CmunePrefs.TryGetKey(GetPrefsKeyForSlot(keyValuePair.Key), out text)) {
					keyValuePair.Value.ReadPlayerPrefs(text);

					if (keyValuePair.Value.Channel == null) {
						_unassignedKeyMappings = true;
					}
				}
			}
		}
	}

	public void SetKeyboardKeyMappingAndroid() {
		_keyMapping[1] = new UserInputMap(GetSlotName(GameInputKey.HorizontalLook), GameInputKey.HorizontalLook, new AxisInputChannel("GameStopLook X", 0f), false, false);
		_keyMapping[2] = new UserInputMap(GetSlotName(GameInputKey.VerticalLook), GameInputKey.VerticalLook, new AxisInputChannel("GameStopLook Y", 1f), false, false);
		_keyMapping[21] = new UserInputMap(GetSlotName(GameInputKey.Pause), GameInputKey.Pause, new KeyInputChannel(KeyCode.Escape));
		_keyMapping[3] = new UserInputMap(GetSlotName(GameInputKey.Forward), GameInputKey.Forward, new KeyInputChannel(KeyCode.UpArrow));
		_keyMapping[5] = new UserInputMap(GetSlotName(GameInputKey.Left), GameInputKey.Left, new KeyInputChannel(KeyCode.LeftArrow));
		_keyMapping[4] = new UserInputMap(GetSlotName(GameInputKey.Backward), GameInputKey.Backward, new KeyInputChannel(KeyCode.DownArrow));
		_keyMapping[6] = new UserInputMap(GetSlotName(GameInputKey.Right), GameInputKey.Right, new KeyInputChannel(KeyCode.RightArrow));
		_keyMapping[7] = new UserInputMap(GetSlotName(GameInputKey.Jump), GameInputKey.Jump, new KeyInputChannel(KeyCode.Alpha6));
		_keyMapping[8] = new UserInputMap(GetSlotName(GameInputKey.Crouch), GameInputKey.Crouch, new KeyInputChannel(KeyCode.Alpha8));
		_keyMapping[9] = new UserInputMap(GetSlotName(GameInputKey.PrimaryFire), GameInputKey.PrimaryFire, new KeyInputChannel(KeyCode.Alpha1));
		_keyMapping[10] = new UserInputMap(GetSlotName(GameInputKey.SecondaryFire), GameInputKey.SecondaryFire, new KeyInputChannel(KeyCode.Alpha2));
		_keyMapping[19] = new UserInputMap(GetSlotName(GameInputKey.NextWeapon), GameInputKey.NextWeapon, new KeyInputChannel(KeyCode.Alpha7));
		_keyMapping[20] = new UserInputMap(GetSlotName(GameInputKey.PrevWeapon), GameInputKey.PrevWeapon, new KeyInputChannel(KeyCode.Alpha5));
		_keyMapping[26] = new UserInputMap(GetSlotName(GameInputKey.UseQuickItem), GameInputKey.UseQuickItem, new KeyInputChannel(KeyCode.Alpha3));
		_keyMapping[28] = new UserInputMap(GetSlotName(GameInputKey.NextQuickItem), GameInputKey.NextQuickItem, new KeyInputChannel(KeyCode.Alpha4));
	}

	public string InputChannelForSlot(GameInputKey keySlot) {
		UserInputMap userInputMap;

		if (KeyMapping.TryGetValue((int)keySlot, out userInputMap)) {
			return userInputMap.Assignment;
		}

		return "None";
	}
}
