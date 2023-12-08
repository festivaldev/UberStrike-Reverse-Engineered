using System;
using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Event Receiver")]
[ExecuteInEditMode]
public class UIEventReceiver : MonoBehaviour {
	public Action OnClicked;
	public Action<Vector2> OnDragging;
	public Action<bool> OnHovered;
	public Action<string> OnInputEntered;
	public Action<KeyCode> OnKeyEntered;
	public Action<bool> OnPressed;
	public Action OnReleased;
	public Action<bool> OnSelected;
	public Action<bool> OnTooltipActive;

	private void OnHover(bool isOver) {
		if (OnHovered != null && enabled) {
			OnHovered(isOver);
		}
	}

	private void OnPress(bool isPressed) {
		if (enabled) {
			if (OnPressed != null) {
				OnPressed(isPressed);
			}

			if (!isPressed && OnReleased != null) {
				OnReleased();
			}
		}
	}

	private void OnSelect(bool selected) {
		if (OnSelected != null && enabled) {
			OnSelected(selected);
		}
	}

	private void OnClick() {
		if (OnClicked != null && enabled) {
			OnClicked();
		}
	}

	private void OnDrag(Vector2 delta) {
		if (OnDragging != null && enabled) {
			OnDragging(delta);
		}
	}

	private void OnInput(string text) {
		if (OnInputEntered != null && enabled) {
			OnInputEntered(text);
		}
	}

	private void OnKey(KeyCode key) {
		if (OnKeyEntered != null && enabled) {
			OnKeyEntered(key);
		}
	}

	private void OnTooltip(bool show) { }
}
