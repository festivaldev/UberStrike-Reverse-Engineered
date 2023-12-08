using UnityEngine;

public class HUDMultitouchController : MonoBehaviour {
	[SerializeField]
	private UIEventReceiver downButton;

	[SerializeField]
	private UIEventReceiver jumpButton;

	private Vector2 lastDirection;

	[SerializeField]
	private UIEventReceiver leftButton;

	private bool moveBack;
	private bool moveFwd;
	private Vector2 MoveInteriaRolloff = new Vector2(24f, 20f);
	private bool moveLeft;
	private bool moveRight;

	[SerializeField]
	private UIEventReceiver rightButton;

	[SerializeField]
	private UIEventReceiver upButton;

	public bool Moving { get; private set; }

	private void Start() {
		upButton.OnPressed = delegate(bool el) { moveFwd = el; };
		downButton.OnPressed = delegate(bool el) { moveBack = el; };
		rightButton.OnPressed = delegate(bool el) { moveRight = el; };
		leftButton.OnPressed = delegate(bool el) { moveLeft = el; };
		jumpButton.OnPressed = delegate(bool el) { TouchInput.WishJump = el; };
	}

	private void LateUpdate() {
		Moving = moveFwd || moveBack || moveLeft || moveRight;
		var vector = Vector2.zero;

		if (moveLeft) {
			vector += new Vector2(-1f, 0f);
		}

		if (moveRight) {
			vector += new Vector2(1f, 0f);
		}

		if (moveFwd) {
			vector += new Vector2(0f, 1f);
		}

		if (moveBack) {
			vector += new Vector2(0f, -1f);
		}

		if (vector.y == 0f) {
			vector.y = Mathf.Lerp(lastDirection.y, vector.y, Time.deltaTime * MoveInteriaRolloff.y);
		}

		if (vector.x == 0f) {
			vector.x = Mathf.Lerp(lastDirection.x, vector.x, Time.deltaTime * MoveInteriaRolloff.x);
		}

		lastDirection = TouchInput.WishDirection;
		TouchInput.WishDirection = vector;
	}
}
