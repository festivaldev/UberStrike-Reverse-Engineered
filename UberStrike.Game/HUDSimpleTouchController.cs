using UnityEngine;

public class HUDSimpleTouchController : MonoBehaviour {
	[SerializeField]
	private UIEventReceiver fireButton;

	[SerializeField]
	private NGUITouchJoystick joystick;

	[SerializeField]
	private UIEventReceiver jumpButton;

	private void Start() {
		var nguitouchJoystick = joystick;
		var num = 0f;
		float num2 = Screen.height / 2;
		var rect = new Rect(0f, 0f, Screen.width * 0.4f, Screen.height);
		nguitouchJoystick.TouchBoundary = new Rect(num, num2, rect.width, Screen.height / 2);
		joystick.OnJoystickMoved = delegate(Vector2 el) { TouchInput.WishDirection = el; };
		joystick.OnJoystickStopped = delegate { TouchInput.WishDirection = Vector2.zero; };
		jumpButton.OnPressed = delegate(bool el) { TouchInput.WishJump = el; };
		fireButton.OnPressed = delegate(bool el) { EventHandler.Global.Fire(new GlobalEvents.InputChanged(GameInputKey.PrimaryFire, (!el) ? 0 : 1)); };
	}
}
