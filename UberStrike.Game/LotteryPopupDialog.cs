using UnityEngine;

public abstract class LotteryPopupDialog : IPopupDialog {
	public const int IMG_WIDTH = 282;
	public const int IMG_HEIGHT = 317;
	private const float BerpSpeed = 2.5f;
	protected bool _showExitButton = true;
	protected State _state;
	public bool ClickAnywhereToExit = true;
	protected int Height = 330;
	protected int Width = 650;
	public bool IsVisible { get; set; }
	public bool IsUIDisabled { get; set; }
	public bool IsWaiting { get; set; }
	public string Text { get; set; }
	public string Title { get; set; }

	public GuiDepth Depth {
		get { return GuiDepth.Event; }
	}

	public void OnGUI() {
		var position = GetPosition();
		GUI.Box(position, GUIContent.none, BlueStonez.window);
		GUITools.PushGUIState();
		GUI.enabled = !IsUIDisabled;
		GUI.BeginGroup(position);

		if (_showExitButton && GUI.Button(new Rect(position.width - 20f, 0f, 20f, 20f), "X", BlueStonez.friends_hidden_button)) {
			PopupSystem.HideMessage(this);
		}

		DrawPlayGUI(position);
		GUI.EndGroup();
		GUITools.PopGUIState();

		if (IsWaiting) {
			WaitingTexture.Draw(position.center);
		}

		if (ClickAnywhereToExit && Event.current.type == EventType.MouseDown && !position.Contains(Event.current.mousePosition)) {
			ClosePopup();
			Event.current.Use();
		}

		OnAfterGUI();
	}

	public void OnHide() { }
	public virtual void OnAfterGUI() { }
	protected abstract void DrawPlayGUI(Rect rect);

	protected void ClosePopup() {
		PopupSystem.HideMessage(this);
	}

	private Rect GetPosition() {
		var num = (Screen.width - Width) * 0.5f;
		var num2 = GlobalUIRibbon.Instance.Height() + (Screen.height - GlobalUIRibbon.Instance.Height() - Height) * 0.5f;

		return new Rect(num, num2, Width, Height);
	}

	protected enum State {
		Normal,
		Rolled
	}
}
