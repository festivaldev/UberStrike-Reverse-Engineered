using UnityEngine;

public class FloatAnim {
	public delegate void OnValueChange(float oldValue, float newValue);

	private float _animDest;
	private EaseType _animEaseType;
	private float _animSrc;
	private float _animStartTime;
	private float _animTime;
	private bool _isAnimating;
	private OnValueChange _onValueChange;
	private float _value;

	public float Value {
		get { return _value; }
		set {
			var value2 = _value;
			_value = value;

			if (_onValueChange != null) {
				_onValueChange(value2, _value);
			}
		}
	}

	public bool IsAnimating {
		get { return _isAnimating; }
	}

	public FloatAnim(OnValueChange onValueChange = null, float value = 0f) {
		_isAnimating = false;
		_value = value;

		if (onValueChange != null) {
			_onValueChange = onValueChange;
		}
	}

	public void Update() {
		if (_isAnimating) {
			var num = Time.time - _animStartTime;

			if (num <= _animTime) {
				var num2 = Mathf.Clamp01(num * (1f / _animTime));
				Value = Mathf.Lerp(_animSrc, _animDest, Mathfx.Ease(num2, _animEaseType));
			} else {
				Value = _animDest;
				_isAnimating = false;
			}
		}
	}

	public void AnimTo(float destValue, float time = 0f, EaseType easeType = EaseType.None) {
		if (time <= 0f) {
			Value = destValue;

			return;
		}

		_isAnimating = true;
		_animSrc = Value;
		_animDest = destValue;
		_animTime = time;
		_animEaseType = easeType;
		_animStartTime = Time.time;
	}

	public void AnimBy(float deltaValue, float time = 0f, EaseType easeType = EaseType.None) {
		var num = Value + deltaValue;
		AnimTo(num, time, easeType);
	}

	public void StopAnim() {
		_isAnimating = false;
	}
}
