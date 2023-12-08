using UnityEngine;

public class ColorAnim {
	public delegate void OnValueChange(Color oldValue, Color newValue);

	private Color _animDest;
	private EaseType _animEaseType;
	private Color _animSrc;
	private float _animStartTime;
	private float _animTime;
	private Color _color;
	private bool _isAnimating;
	private OnValueChange _onColorChange;

	public Color Color {
		get { return _color; }
		set {
			var color = _color;
			_color = value;

			if (_onColorChange != null) {
				_onColorChange(color, _color);
			}
		}
	}

	public bool IsAnimating {
		get { return _isAnimating; }
	}

	public float Alpha {
		get { return _color.a; }
		set {
			var color = _color;
			_color.a = value;

			if (_onColorChange != null) {
				_onColorChange(color, _color);
			}
		}
	}

	public ColorAnim(OnValueChange onColorChange = null) {
		_isAnimating = false;

		if (onColorChange != null) {
			_onColorChange = onColorChange;
		}
	}

	public void Update() {
		if (_isAnimating) {
			var num = Time.time - _animStartTime;

			if (num <= _animTime) {
				var num2 = Mathf.Clamp01(num * (1f / _animTime));
				Color = Color.Lerp(_animSrc, _animDest, Mathfx.Ease(num2, _animEaseType));
				Alpha = Color.a;
			} else {
				Color = _animDest;
				Alpha = Color.a;
				_isAnimating = false;
			}
		}
	}

	public void FadeAlphaTo(float destAlpha, float time = 0f, EaseType easeType = EaseType.None) {
		if (time <= 0f) {
			Alpha = destAlpha;

			return;
		}

		_isAnimating = true;
		_animSrc = Color;
		_animDest = Color;
		_animDest.a = destAlpha;
		_animTime = time;
		_animEaseType = easeType;
		_animStartTime = Time.time;
	}

	public void FadeAlpha(float deltaAlpha, float time = 0f, EaseType easeType = EaseType.None) {
		var num = Color.a + deltaAlpha;
		FadeAlphaTo(num, time, easeType);
	}

	public void FadeColorTo(Color destColor, float time = 0f, EaseType easeType = EaseType.None) {
		if (time <= 0f) {
			Color = destColor;

			return;
		}

		_isAnimating = true;
		_animSrc = Color;
		_animDest = destColor;
		_animTime = time;
		_animEaseType = easeType;
		_animStartTime = Time.time;
	}

	public void FadeColor(Color deltaColor, float time = 0f, EaseType easeType = EaseType.None) {
		var color = Color + deltaColor;
		FadeColorTo(color, time, easeType);
	}

	public void StopFading() {
		_isAnimating = false;
	}
}
