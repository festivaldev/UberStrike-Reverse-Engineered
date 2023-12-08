using System;
using UnityEngine;

public abstract class BaseEventPopup : IPopupDialog {
	private const float BerpSpeed = 2.5f;
	protected Action _onCloseButtonClicked;
	private float _startTime;
	protected bool ClickAnywhereToExit = true;
	protected int Height = 330;
	protected int Width = 650;

	public float Scale {
		get {
			if (_startTime > Time.time - 1f) {
				return Mathfx.Berp(0.01f, 1f, (Time.time - _startTime) * 2.5f);
			}

			return 1f;
		}
	}

	public string Text { get; set; }
	public string Title { get; set; }

	public GuiDepth Depth {
		get { return GuiDepth.Event; }
	}

	public virtual void OnHide() { }

	public void OnGUI() {
		if (_startTime == 0f) {
			_startTime = Time.time;
		}

		GUI.color = Color.white.SetAlpha(Scale);
		var num = (Screen.width - Width) * 0.5f;
		var num2 = GlobalUIRibbon.Instance.Height() + (Screen.height - GlobalUIRibbon.Instance.Height() - Height) * 0.5f;
		var rect = new Rect(num, num2, Width, 64 + Height - 64f * Scale);
		GUI.Box(new Rect(num - 1f, num2 - 1f, rect.width + 2f, rect.height + 2f), GUIContent.none, BlueStonez.window);
		GUI.BeginGroup(rect);

		if (GUI.Button(new Rect(rect.width - 20f, 0f, 20f, 20f), "X", BlueStonez.friends_hidden_button)) {
			Close();
		}

		DrawGUI(rect);
		GUI.EndGroup();
		GUI.color = Color.white;

		if (ClickAnywhereToExit && Event.current.type == EventType.MouseDown && !rect.Contains(Event.current.mousePosition)) {
			Event.current.Use();
			Close();
		}

		OnAfterGUI();
	}

	protected abstract void DrawGUI(Rect rect);
	public virtual void OnAfterGUI() { }

	private void Close() {
		PopupSystem.HideMessage(this);

		if (_onCloseButtonClicked != null) {
			_onCloseButtonClicked();
		}
	}
}
