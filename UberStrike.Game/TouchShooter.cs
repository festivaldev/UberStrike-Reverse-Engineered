using System;
using System.Collections;
using UnityEngine;

public class TouchShooter : TouchBaseControl {
	private ArrayList _ignoreTouches;
	private Vector2 _lastFirePosition = Vector2.zero;
	private float _lastFireTouch;
	private TouchFinger _primaryFinger;
	private TouchFinger _secondaryFinger;
	private bool enabled;
	public float SecondaryFireTapDelay = 0.4f;
	public float SecondaryFireTapMaxDistanceSqr = 10000f;
	public Vector2 Aim { get; private set; }

	public override bool Enabled {
		get { return enabled; }
		set {
			if (value != enabled) {
				enabled = value;

				if (!enabled) {
					_primaryFinger = new TouchFinger();
					_secondaryFinger = new TouchFinger();
					Aim = Vector2.zero;
				}
			}
		}
	}

	public TouchShooter() {
		_primaryFinger = new TouchFinger();
		_secondaryFinger = new TouchFinger();
		_ignoreTouches = new ArrayList();
	}

	public event Action<Vector2> OnDoubleTap;
	public event Action OnFireStart;
	public event Action OnFireEnd;

	public override void UpdateTouches(Touch touch) {
		if (touch.phase == TouchPhase.Began && Boundary.ContainsTouch(touch.position) && ValidArea(touch.position)) {
			if (_primaryFinger.FingerId == -1) {
				_primaryFinger = new TouchFinger {
					StartPos = touch.position,
					StartTouchTime = Time.time,
					LastPos = touch.position,
					FingerId = touch.fingerId
				};

				if (_lastFireTouch + SecondaryFireTapDelay > Time.time && (_lastFirePosition - touch.position).sqrMagnitude < SecondaryFireTapMaxDistanceSqr) {
					if (OnDoubleTap != null) {
						OnDoubleTap(touch.position);
					}
				} else {
					_lastFireTouch = Time.time;
					_lastFirePosition = touch.position;
				}
			} else if (_primaryFinger.FingerId != touch.fingerId && _secondaryFinger.FingerId == -1) {
				_secondaryFinger = new TouchFinger {
					StartPos = touch.position,
					StartTouchTime = Time.time,
					LastPos = touch.position,
					FingerId = touch.fingerId
				};

				if (OnFireStart != null) {
					OnFireStart();
				}
			}
		} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
			if (_primaryFinger.FingerId == touch.fingerId) {
				Aim = touch.deltaPosition * 500f / Screen.width;
			}
		} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
			if (_primaryFinger.FingerId == touch.fingerId) {
				_primaryFinger.Reset();
				Aim = Vector2.zero;
			} else if (_secondaryFinger.FingerId == touch.fingerId) {
				if (OnFireEnd != null) {
					OnFireEnd();
				}

				_secondaryFinger.Reset();
			}
		}
	}

	public void IgnoreRect(Rect r) {
		if (!_ignoreTouches.Contains(r)) {
			_ignoreTouches.Add(r);
		}
	}

	public void UnignoreRect(Rect r) {
		if (_ignoreTouches.Contains(r)) {
			_ignoreTouches.Remove(r);
		}
	}

	private bool ValidArea(Vector2 pos) {
		if (_ignoreTouches.Count == 0) {
			return true;
		}

		foreach (var obj in _ignoreTouches) {
			var rect = (Rect)obj;

			if (rect.ContainsTouch(pos)) {
				return false;
			}
		}

		return true;
	}
}
