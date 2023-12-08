//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------
using System;
using UnityEngine;

/// <summary>
/// Similar to UIButtonColor, but adds a 'disabled' state based on whether the collider is enabled or not.
/// </summary>
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor {
	/// <summary>
	/// Color that will be applied when the button is disabled.
	/// </summary>
	public Color disabledColor = Color.grey;

	public Action OnHovered;
	public Action OnPressed;
	public Action OnRelease;

	/// <summary>
	/// Whether the button should be enabled.
	/// </summary>
	public bool isEnabled {
		get {
			var col = collider;

			return col && col.enabled;
		}
		set {
			var col = collider;

			if (!col) return;

			if (col.enabled != value) {
				col.enabled = value;
				UpdateColor(value, false);
			}
		}
	}

	/// <summary>
	/// If the collider is disabled, assume the disabled color.
	/// </summary>
	protected override void OnEnable() {
		if (isEnabled) base.OnEnable();
		else UpdateColor(false, true);
	}

	public override void OnHover(bool isOver) {
		if (!isEnabled)
			return;

		if (isOver && OnHovered != null)
			OnHovered();

		base.OnHover(isOver);
	}

	public override void OnPress(bool isPressed) {
		if (!isEnabled)
			return;

		if (isPressed) {
			if (OnPressed != null)
				OnPressed();
		} else if (OnRelease != null)
			OnRelease();

		base.OnPress(isPressed);
	}

	/// <summary>
	/// Update the button's color to either enabled or disabled state.
	/// </summary>
	public void UpdateColor(bool shouldBeEnabled, bool immediate) {
		if (tweenTarget) {
			if (!mStarted) {
				mStarted = true;
				Init();
			}

			var c = shouldBeEnabled ? defaultColor : disabledColor;
			var tc = TweenColor.Begin(tweenTarget, 0.15f, c);

			if (immediate) {
				tc.color = c;
				tc.enabled = false;
			}
		}
	}
}
