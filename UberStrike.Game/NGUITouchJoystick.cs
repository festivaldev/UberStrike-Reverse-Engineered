using System;
using UnityEngine;

public class NGUITouchJoystick : MonoBehaviour {
	[SerializeField]
	private GameObject backgroundContainer;

	private Rect boundary = default(Rect);
	private TouchFinger finger = new TouchFinger();
	private Rect joystickBoundary = default(Rect);

	[SerializeField]
	private Vector2 joystickLimits = new Vector2(128f, 128f);

	private Vector2 joystickPosition = Vector2.zero;

	[SerializeField]
	private UISprite movingStick;

	public Action<Vector2> OnJoystickMoved;
	public Action OnJoystickStopped;

	[SerializeField]
	private Rect touchBoundary = new Rect(0f, 0f, Screen.width, Screen.height);

	public Rect TouchBoundary {
		set {
			touchBoundary = value;
			boundary = touchBoundary;
		}
	}

	private void Awake() {
		boundary = touchBoundary;
	}

	private void Update() {
		foreach (var touch in Input.touches) {
			if (touch.phase == TouchPhase.Began && boundary.ContainsTouch(touch.position) && finger.FingerId == -1) {
				finger = new TouchFinger {
					StartPos = new Vector2(touch.position.x, touch.position.y),
					StartTouchTime = Time.time,
					FingerId = touch.fingerId
				};

				joystickBoundary = new Rect(touch.position.x - joystickLimits.x / 2f, touch.position.y - joystickLimits.y / 2f, joystickLimits.x, joystickLimits.y);
				Vector3 vector = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, UICamera.currentCamera.nearClipPlane));
				vector = backgroundContainer.transform.parent.InverseTransformPoint(new Vector3(vector.x, vector.y, 0f));
				backgroundContainer.transform.localPosition = vector;
				movingStick.transform.localPosition = vector;
				ShowJoystick(true);
			} else if (finger.FingerId == touch.fingerId) {
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					joystickPosition.x = Mathf.Clamp(touch.position.x, joystickBoundary.x, joystickBoundary.x + joystickBoundary.width);
					joystickPosition.y = Mathf.Clamp(touch.position.y, joystickBoundary.y, joystickBoundary.y + joystickBoundary.height);
					var vector2 = new Vector3(joystickPosition.x, joystickPosition.y, 0f);
					vector2 = UICamera.currentCamera.ScreenToWorldPoint(vector2);
					movingStick.transform.localPosition = backgroundContainer.transform.parent.InverseTransformPoint(vector2);
					var vector3 = Vector2.zero;
					vector3.x = (joystickPosition.x - finger.StartPos.x) * 2f / joystickBoundary.width;
					vector3.y = (joystickPosition.y - finger.StartPos.y) * 2f / joystickBoundary.height;
					vector3 *= ApplicationDataManager.ApplicationOptions.TouchMoveSensitivity;

					if (touch.phase == TouchPhase.Moved && OnJoystickMoved != null) {
						OnJoystickMoved(vector3);
					}
				} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
					ShowJoystick(false);
					boundary = touchBoundary;

					if (OnJoystickStopped != null) {
						OnJoystickStopped();
					}

					finger.Reset();
				}
			}
		}
	}

	private void ShowJoystick(bool show) {
		movingStick.enabled = show;
		NGUITools.SetActiveChildren(backgroundContainer, show);
		NGUITools.SetActiveChildren(movingStick.gameObject, show);
	}
}
