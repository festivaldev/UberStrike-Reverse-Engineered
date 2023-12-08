using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WeaponCamera : MonoBehaviour {
	private const float RESET_VELOCITY = 5f;
	private const float LERP_DURATION = 0.1f;
	public Vector2 _currentAngle = Vector2.zero;

	[SerializeField]
	private float _maxDisplacement = 0.8f;

	[SerializeField]
	private float _maxDisplacementDelta = 0.4f;

	private Transform _transform;

	public bool IsEnabled {
		get { return camera.enabled; }
		set { camera.enabled = value; }
	}

	private void Awake() {
		_transform = transform;
	}

	public void SetCameraEnabled(bool enabled) {
		camera.enabled = enabled;
	}

	private void LateUpdate() {
		if (WeaponFeedbackManager.Instance) {
			if (WeaponFeedbackManager.Instance.IsIronSighted) {
				_currentAngle = Vector2.Lerp(_currentAngle, Vector2.zero, Time.deltaTime * 5f);
				MoveWeapon();
			} else {
				var value = AutoMonoBehaviour<InputManager>.Instance.GetValue(GameInputKey.HorizontalLook);
				var value2 = AutoMonoBehaviour<InputManager>.Instance.GetValue(GameInputKey.VerticalLook);
				AddDeltaAngle(value, value2);
				MoveWeapon();
				_currentAngle = Vector2.Lerp(_currentAngle, Vector2.zero, Time.deltaTime * 5f);
			}
		}
	}

	private void AddDeltaAngle(float x, float y) {
		var vector = Vector2.ClampMagnitude(new Vector2(x, y), _maxDisplacementDelta);
		_currentAngle = Vector2.ClampMagnitude(_currentAngle + vector, _maxDisplacement);
	}

	private void MoveWeapon() {
		_transform.localRotation = Quaternion.AngleAxis(_currentAngle.x, Vector3.up) * Quaternion.AngleAxis(-_currentAngle.y, Vector3.right);
	}
}
