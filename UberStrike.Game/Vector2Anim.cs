using System;
using UnityEngine;

public class Vector2Anim {
	private Vector2 _animDest;
	private EaseType _animEaseType;
	private Vector2 _animSrc;
	private float _animStartTime;
	private float _animTime;
	private bool _isAnimating;
	private Action<Vector2, Vector2> _onVec2Change;
	private Vector2 _vec2;

	public Vector2 Vec2 {
		get { return _vec2; }
		set {
			var vec = _vec2;
			_vec2 = value;

			if (_onVec2Change != null) {
				_onVec2Change(vec, _vec2);
			}
		}
	}

	public bool IsAnimating {
		get { return _isAnimating; }
	}

	public Vector2Anim(Action<Vector2, Vector2> onVec2Change = null) {
		_isAnimating = false;

		if (onVec2Change != null) {
			_onVec2Change = onVec2Change;
		}
	}

	public void Update() {
		if (_isAnimating) {
			var num = Time.time - _animStartTime;

			if (num <= _animTime) {
				var num2 = Mathf.Clamp01(num * (1f / _animTime));
				Vec2 = Vector2.Lerp(_animSrc, _animDest, Mathfx.Ease(num2, _animEaseType));
			} else {
				Vec2 = _animDest;
				_isAnimating = false;
			}
		}
	}

	public void AnimTo(Vector2 destPosition, float time = 0f, EaseType easeType = EaseType.None, float startDelay = 0f) {
		if (time <= 0f) {
			Vec2 = destPosition;

			return;
		}

		_isAnimating = true;
		_animSrc = Vec2;
		_animDest = destPosition;
		_animTime = time;
		_animEaseType = easeType;
		_animStartTime = Time.time + startDelay;
	}

	public void AnimBy(Vector2 deltaPosition, float time = 0f, EaseType easeType = EaseType.None) {
		var vector = Vec2 + deltaPosition;
		AnimTo(vector, time, easeType);
	}

	public void StopAnim() {
		_isAnimating = false;
	}
}
