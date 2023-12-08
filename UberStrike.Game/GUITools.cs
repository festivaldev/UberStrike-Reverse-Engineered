using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUITools {
	private static int _screenX = 1;
	private static int _screenY = 1;
	private static int _screenHalfX = 1;
	private static int _screenHalfY = 1;
	private static float _aspectRatio = 1f;
	private static float _lastClick;
	private static float _lastRepeatClick;
	private static float _repeatButtonPressed;
	private static Stack<bool> _stateStack = new Stack<bool>();
	private static Color _lastGuiColor;
	private static int HoverButtonHash = "Button".GetHashCode();

	public static float AspectRatio {
		get { return _aspectRatio; }
	}

	public static float SinusPulse {
		get { return (Mathf.Sin(Time.time * 2f) + 1.3f) * 0.5f; }
	}

	public static float FastSinusPulse {
		get { return (Mathf.Sin(Time.time * 5f) + 1.3f) * 0.5f; }
	}

	public static int ScreenHalfWidth {
		get { return _screenHalfX; }
	}

	public static int ScreenHalfHeight {
		get { return _screenHalfY; }
	}

	public static int ScreenWidth {
		get { return _screenX; }
		private set {
			_screenX = Mathf.Max(value, 1);
			_screenHalfX = _screenX >> 1;
		}
	}

	public static int ScreenHeight {
		get { return _screenY; }
		private set {
			_screenY = Mathf.Max(value, 1);
			_screenHalfY = _screenY >> 1;
		}
	}

	public static float LastClick {
		get { return _lastClick; }
	}

	public static bool SaveClick {
		get { return _lastClick + 0.5f < Time.time; }
	}

	public static bool IsScrolling { get; private set; }

	public static IEnumerator StartScreenSizeListener(float s) {
		UpdateScreenSize();

		yield return new WaitForEndOfFrame();

		for (;;) {
			UpdateScreenSize();

			yield return new WaitForSeconds(s);
		}

		yield break;
	}

	public static void UpdateScreenSize() {
		if (ScreenWidth != Screen.width) {
			ScreenWidth = Screen.width;
			_aspectRatio = ScreenWidth / ScreenHeight;
		}

		if (ScreenHeight != Screen.height) {
			ScreenHeight = Screen.height;
			_aspectRatio = ScreenWidth / ScreenHeight;
		}
	}

	public static bool IsScreenResolutionChanged() {
		return Screen.width != Screen.width || Screen.height != Screen.height;
	}

	public static bool RepeatClick(float vel) {
		if (Mathf.Abs(Time.time - _lastRepeatClick - Time.deltaTime) < 0.0001f) {
			_repeatButtonPressed += Time.deltaTime;
		} else {
			_repeatButtonPressed = 0f;
		}

		_lastRepeatClick = Time.time;

		return _repeatButtonPressed == 0f || (_repeatButtonPressed > 0.5f && SaveClickIn(vel));
	}

	public static bool SaveClickIn(float t) {
		return _lastClick + t < Time.time;
	}

	public static void ClickAndUse() {
		if (Event.current != null) {
			Event.current.Use();
		}

		_lastClick = Time.time;
	}

	public static void Clicked() {
		_lastClick = Time.time;
	}

	public static int CheckGUIState() {
		return _stateStack.Count;
	}

	public static void PushGUIState() {
		if (_stateStack.Count < 100) {
			_stateStack.Push(GUI.enabled);
		} else {
			Debug.LogError("Check your calls of PushGUIState");
		}
	}

	public static void PopGUIState() {
		GUI.enabled = _stateStack.Pop();
	}

	public static void BeginGUIColor(Color color) {
		_lastGuiColor = GUI.color;
		GUI.color = color;
	}

	public static void EndGUIColor() {
		GUI.color = _lastGuiColor;
	}

	public static void LabelShadow(Rect rect, string text, GUIStyle style, Color color) {
		GUI.color = new Color(0f, 0f, 0f, color.a * 0.5f);
		GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), text, style);
		GUI.color = color;
		GUI.Label(rect, text, style);
		GUI.color = Color.white;
	}

	public static bool Button(Rect rect, GUIContent content, GUIStyle style) {
		return Button(rect, content, style, GameAudio.ButtonClick);
	}

	public static bool Button(Rect rect, GUIContent content, GUIStyle style, AudioClip soundEffect) {
		if (GUI.Button(rect, content, style) && AutoMonoBehaviour<SfxManager>.Instance != null) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(soundEffect);

			return true;
		}

		return false;
	}

	public static Rect ToGlobal(Rect rect) {
		var vector = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));

		return new Rect(vector.x, vector.y, rect.width, rect.height);
	}

	public static bool Label(Rect rect, Texture2D image, GUIStyle style) {
		GUI.Label(rect, image, style);

		return rect.Contains(Event.current.mousePosition);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect contentRect, bool showHorizontalScrollbar, bool showVerticalScrollbar, GUIStyle hStyle, GUIStyle vStyle, bool allowDrag = true) {
		return GUI.BeginScrollView(position, scrollPosition, contentRect);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect contentRect, bool useHorizontal = false, bool useVertical = false, bool allowDrag = true) {
		return BeginScrollView(position, scrollPosition, contentRect, useHorizontal, useVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, allowDrag);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect contentRect, GUIStyle hStyle, GUIStyle vStyle) {
		return BeginScrollView(position, scrollPosition, contentRect, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
	}

	public static void EndScrollView() {
		GUI.EndScrollView();
	}

	public static bool BeginList(ref bool showList, Rect listRect, Rect buttonRect) {
		if (!showList) {
			return false;
		}

		if (Input.GetMouseButtonUp(0) && !listRect.ContainsTouch(Input.mousePosition) && !buttonRect.ContainsTouch(Input.mousePosition)) {
			showList = false;
		}

		if (Input.touchCount > 0) {
			foreach (var touch in Input.touches) {
				if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && !listRect.ContainsTouch(touch.position) && !buttonRect.ContainsTouch(touch.position)) {
					showList = false;
				}
			}
		}

		return showList;
	}

	public static EventType HoverButton(Rect position, GUIContent content, GUIStyle style) {
		var controlID = GUIUtility.GetControlID(HoverButtonHash, FocusType.Native);

		switch (Event.current.GetTypeForControl(controlID)) {
			case EventType.MouseDown:
				if (position.Contains(Event.current.mousePosition)) {
					GUIUtility.hotControl = controlID;
					Event.current.Use();

					return EventType.MouseDown;
				}

				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID) {
					GUIUtility.hotControl = 0;

					if (position.Contains(Event.current.mousePosition)) {
						Event.current.Use();

						return EventType.MouseUp;
					}
				} else if (position.Contains(Event.current.mousePosition)) {
					return EventType.DragExited;
				}

				return EventType.Ignore;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID) {
					Event.current.Use();

					return EventType.MouseDrag;
				}

				return EventType.Ignore;
			case EventType.Repaint:
				style.Draw(position, content, controlID);

				if (position.Contains(Event.current.mousePosition)) {
					return EventType.MouseMove;
				}

				return EventType.Repaint;
		}

		if (position.Contains(Event.current.mousePosition)) {
			return EventType.MouseMove;
		}

		return EventType.Ignore;
	}

	public static string PasswordField(Rect mPosition, string mPassword) {
		string text;

		if (Event.current.type == EventType.Repaint || Event.current.type == EventType.MouseDown) {
			text = string.Empty;

			for (var i = 0; i < mPassword.Length; i++) {
				text += "*";
			}
		} else {
			text = mPassword;
		}

		GUI.changed = false;
		text = GUI.TextField(mPosition, text, 20);

		if (GUI.changed) {
			mPassword = text;
		}

		return mPassword;
	}

	public static string PasswordField(string mPassword) {
		string text;

		if (Event.current.type == EventType.Repaint || Event.current.type == EventType.MouseDown) {
			text = string.Empty;

			for (var i = 0; i < mPassword.Length; i++) {
				text += "*";
			}
		} else {
			text = mPassword;
		}

		GUI.changed = false;
		text = GUILayout.TextField(text, 24, GUILayout.Height(30f));

		if (GUI.changed) {
			mPassword = text;
		}

		return mPassword;
	}

	public static Vector2 DoScrollArea(Rect position, GUIContent[] buttons, int buttonHeight, Vector2 listScroller) {
		var num = 0f;

		if (buttons.Length > 0) {
			num = (buttons.Length - 1) * buttonHeight;
		}

		listScroller = BeginScrollView(position, listScroller, new Rect(0f, 0f, position.width - 20f, num + buttonHeight));
		int i;

		for (i = 0; i < buttons.Length; i++) {
			if ((i + 1) * buttonHeight > listScroller.y) {
				break;
			}
		}

		while (i < buttons.Length && i * buttonHeight < listScroller.y + position.height) {
			GUI.Button(new Rect(0f, i * buttonHeight, position.width - 16f, buttonHeight), buttons[i]);
			i++;
		}

		EndScrollView();

		return listScroller;
	}

	public static void OutlineLabel(Rect position, string text) {
		OutlineLabel(position, text, "SuperBigTitle", 1);
	}

	public static void OutlineLabel(Rect position, string text, GUIStyle style) {
		OutlineLabel(position, text, style, 1);
	}

	public static void OutlineLabel(Rect position, string text, GUIStyle style, Color c) {
		OutlineLabel(position, text, style, 1, Color.white, c);
	}

	public static void OutlineLabel(Rect position, string text, GUIStyle style, int Offset) {
		OutlineLabel(position, text, style, Offset, Color.white, Color.black);
	}

	public static void OutlineLabel(Rect position, string text, GUIStyle style, int Offset, Color textColor, Color outlineColor) {
		var textColor2 = style.normal.textColor;
		style.normal.textColor = outlineColor;

		if (Offset > 0) {
			GUI.Label(new Rect(position.x, position.y + Offset, position.width, position.height), text, style);
			GUI.Label(new Rect(position.x - Offset, position.y, position.width, position.height), text, style);
			GUI.Label(new Rect(position.x + Offset, position.y, position.width, position.height), text, style);
			GUI.Label(new Rect(position.x, position.y - Offset, position.width, position.height), text, style);

			if (Offset > 1) {
				GUI.Label(new Rect(position.x - Offset, position.y - Offset, position.width, position.height), text, style);
				GUI.Label(new Rect(position.x - Offset, position.y + Offset, position.width, position.height), text, style);
				GUI.Label(new Rect(position.x + Offset, position.y + Offset, position.width, position.height), text, style);
				GUI.Label(new Rect(position.x + Offset, position.y - Offset, position.width, position.height), text, style);
			}
		} else {
			GUI.Label(new Rect(position.x, position.y + 1f, position.width, position.height), text, style);
		}

		style.normal.textColor = textColor2;
		GUI.color = textColor;
		GUI.Label(position, text, style);
		GUI.color = Color.white;
	}

	public static void ProgressBar(Rect position, string text, float percentage, Color barColor, int barWidth) {
		GUI.BeginGroup(position);
		GUI.Label(new Rect(0f, 0f, position.width - (barWidth + 4) - 2f - 30f, 14f), text, BlueStonez.label_interparkbold_11pt_right);
		GUI.Label(new Rect(position.width - barWidth - 30f, 1f, barWidth, 12f), GUIContent.none, BlueStonez.progressbar_background);
		GUI.color = barColor;
		GUI.Label(new Rect(position.width - barWidth - 30f + 2f, 3f, (barWidth - 4) * Mathf.Clamp01(percentage), 8f), string.Empty, BlueStonez.progressbar_thumb);
		GUI.color = Color.white;
		GUI.EndGroup();
	}

	public static void DrawWarmupBar(Rect position, float value, float maxValue) {
		GUI.BeginGroup(position);
		GUI.Box(new Rect(0f, 0f, position.width, position.height), GUIContent.none, StormFront.ProgressBackground);
		var num = (position.width - 8f) * value / maxValue;
		GUI.Box(new Rect(4f, 4f, num, position.height - 8f), GUIContent.none, StormFront.ProgressForeground);
		var num2 = position.height * 0.5f;
		GUI.Box(new Rect(4f + num - num2 * 0.5f, 2f, num2, position.height - 4f), GUIContent.none, StormFront.ProgressThumb);
		GUI.EndGroup();
	}
}
