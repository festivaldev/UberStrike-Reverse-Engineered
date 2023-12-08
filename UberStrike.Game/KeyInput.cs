using System.Collections.Generic;
using UnityEngine;

public static class KeyInput {
	private static Dictionary<KeyCode, bool> keys = new Dictionary<KeyCode, bool>();
	private static KeyCode LastKey;
	public static bool AltPressed { get; private set; }
	public static bool CtrlPressed { get; private set; }
	public static KeyCode KeyPressed { get; private set; }

	static KeyInput() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += Update;
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGUI;
	}

	public static bool GetKeyDown(KeyCode key) {
		return KeyPressed == key;
	}

	private static void Update() {
		if (keys.ContainsKey(LastKey) && keys[LastKey]) {
			keys[LastKey] = false;
			KeyPressed = LastKey;
		} else {
			KeyPressed = KeyCode.None;
		}

		LastKey = KeyCode.None;
	}

	private static void OnGUI() {
		AltPressed = Event.current.alt;
		CtrlPressed = Event.current.control;

		if (Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && !keys.ContainsKey(Event.current.keyCode)) {
			keys[Event.current.keyCode] = true;
			LastKey = Event.current.keyCode;
		} else if (Event.current.type == EventType.KeyUp) {
			keys.Remove(Event.current.keyCode);
			LastKey = KeyCode.None;
		}
	}
}
