using System;
using UnityEngine;

public class FlickerAnim {
	private float _flickerEndTime;
	private float _flickerInterval;
	private float _flickerStartTime;
	private bool _isAnimating;
	private bool _isFlickerVisible;
	private float _lastFlickerTime;
	private Action<FlickerAnim> _onFlickerVisibleChange;

	public bool IsAnimating {
		get { return _isAnimating; }
	}

	public bool IsFlickerVisible {
		get { return _isFlickerVisible; }
		set {
			_isFlickerVisible = value;

			if (_onFlickerVisibleChange != null) {
				_onFlickerVisibleChange(this);
			}
		}
	}

	public FlickerAnim(Action<FlickerAnim> onFlickerVisibleChange = null) {
		_isAnimating = false;
		_onFlickerVisibleChange = onFlickerVisibleChange;
	}

	public void Update() {
		if (_isAnimating) {
			var time = Time.time;

			if (time > _flickerEndTime) {
				_isAnimating = false;
				IsFlickerVisible = true;
			} else if (time > _lastFlickerTime + _flickerInterval) {
				IsFlickerVisible = !_isFlickerVisible;
				_lastFlickerTime = time;
			}
		}
	}

	public void Flicker(float time, float flickerInterval = 0.02f) {
		if (time <= 0f || flickerInterval >= time) {
			return;
		}

		_isAnimating = true;
		_flickerInterval = 0.02f;
		_flickerStartTime = Time.time;
		_flickerEndTime = _flickerStartTime + time;
		_lastFlickerTime = _flickerStartTime;
	}

	public void StopAnim() {
		_isAnimating = false;
	}
}
