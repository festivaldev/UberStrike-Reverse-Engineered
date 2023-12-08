using System;
using UnityEngine;

public class TouchControl : TouchBaseControl {
	private bool _inside;
	protected float _rotationAngle;
	protected Vector2 _rotationPoint = Vector2.zero;
	private bool enabled;
	public TouchFinger finger;

	public override bool Enabled {
		get { return enabled; }
		set {
			if (value != enabled) {
				enabled = value;

				if (!enabled) {
					finger.Reset();
					_inside = false;
				}
			}
		}
	}

	public bool IsActive {
		get { return finger.FingerId != -1; }
	}

	public TouchControl() {
		finger = new TouchFinger();
	}

	public event Action<Vector2> OnTouchBegan;
	public event Action<Vector2, Vector2> OnTouchLeftBoundary;
	public event Action<Vector2, Vector2> OnTouchMoved;
	public event Action<Vector2, Vector2> OnTouchEnteredBoundary;
	public event Action<Vector2> OnTouchEnded;

	public void SetRotation(float angle, Vector2 point) {
		_rotationAngle = angle;
		_rotationPoint = point;
	}

	public override void UpdateTouches(Touch touch) {
		if (finger.FingerId != -1 && touch.fingerId != finger.FingerId) {
			return;
		}

		if (finger.FingerId == -1 && touch.phase != TouchPhase.Began) {
			return;
		}

		var vector = touch.position;

		if (_rotationAngle != 0f) {
			vector = Mathfx.RotateVector2AboutPoint(touch.position, new Vector2(_rotationPoint.x, Screen.height - _rotationPoint.y), -_rotationAngle);
		}

		switch (touch.phase) {
			case TouchPhase.Began:
				if (TouchInside(vector)) {
					finger.StartPos = vector;
					finger.LastPos = vector;
					finger.StartTouchTime = Time.time;
					finger.FingerId = touch.fingerId;
					_inside = true;

					if (OnTouchBegan != null) {
						OnTouchBegan(vector);
					}
				}

				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary: {
				var flag = TouchInside(vector);

				if (_inside && !flag) {
					_inside = false;

					if (OnTouchLeftBoundary != null) {
						OnTouchLeftBoundary(vector, touch.deltaPosition);
					}
				} else if (!_inside && flag) {
					_inside = true;

					if (OnTouchEnteredBoundary != null) {
						OnTouchEnteredBoundary(vector, touch.deltaPosition);
					}
				}

				if (OnTouchMoved != null) {
					OnTouchMoved(vector, touch.deltaPosition);
				}

				finger.LastPos = vector;

				break;
			}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				if (OnTouchEnded != null) {
					OnTouchEnded(vector);
				}

				ResetTouch();

				break;
		}
	}

	protected virtual void ResetTouch() {
		finger.Reset();
		_inside = false;
	}

	protected virtual bool TouchInside(Vector2 position) {
		return Boundary.ContainsTouch(position);
	}
}
