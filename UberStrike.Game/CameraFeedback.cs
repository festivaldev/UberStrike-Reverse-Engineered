using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFeedback : MonoBehaviour {
	public enum FeedbackType {
		Land,
		Damage,
		Weapon
	}

	private static CameraFeedback _instance;
	private float _angle;
	private Feedback _currentFeedback;
	private Vector3 _rotationAxis;
	private float _testAngle;
	private float _timer;
	private Quaternion _tmpRotation;
	private Transform _transformCache;
	public bool DEBUG = true;
	public Feedback[] Feedbacks;

	public static CameraFeedback Instance {
		get { return _instance; }
	}

	private void Awake() {
		_instance = this;
		_transformCache = transform;
		_currentFeedback = null;
		_tmpRotation = Quaternion.identity;
	}

	private void Update() {
		if (_currentFeedback == null) {
			if (_transformCache.localPosition.sqrMagnitude > 0.001f) {
				_transformCache.localPosition = Vector3.Lerp(_transformCache.localPosition, Vector3.zero, Time.deltaTime);
				_transformCache.localRotation = Quaternion.Lerp(_transformCache.localRotation, Quaternion.identity, Time.deltaTime);
			}
		} else {
			var direction = _currentFeedback.GetDirection();
			var num = _currentFeedback.Peak;
			var num2 = UnityEngine.Random.Range(-_currentFeedback.MaxNoise, _currentFeedback.MaxNoise);

			if (_timer < _currentFeedback.TimeToEnd + _currentFeedback.TimeToPeak) {
				if (_timer < _currentFeedback.TimeToPeak) {
					num *= Mathf.Sin(_timer * 3.1415927f * 0.5f / _currentFeedback.TimeToPeak);
					num2 = Mathf.Lerp(num2, 0f, _timer / _currentFeedback.TimeToPeak);
					_angle = Mathf.Lerp(0f, _currentFeedback.MaxAngle, _timer / _currentFeedback.TimeToPeak);
				} else {
					var num3 = (_timer - _currentFeedback.TimeToPeak) / _currentFeedback.TimeToEnd;
					num = Mathf.Lerp(num, 0f, num3);
					_angle = Mathf.Lerp(_angle, 0f, num3);
					num2 = 0f;
				}

				_timer += Time.deltaTime;
				_transformCache.localPosition = num * direction + _transformCache.right * num2 + _transformCache.up * num2;
				_tmpRotation = Quaternion.AngleAxis(_angle, _rotationAxis);
				_testAngle = _angle;
			} else {
				_timer = 0f;
				_tmpRotation = Quaternion.identity;
				_currentFeedback = null;
			}
		}
	}

	private void DoApplyFeedback() {
		GUI.Label(new Rect(10f, 50f, 300f, 20f), "Camera local position = " + _transformCache.localPosition);
		GUI.Label(new Rect(10f, 60f, 300f, 20f), "Camera world position = " + _transformCache.position);
		GUI.Label(new Rect(10f, 70f, 300f, 20f), "Rotation Axis = " + _rotationAxis);
		GUI.Label(new Rect(10f, 80f, 300f, 20f), "Rotation Angle = " + _testAngle);

		if (GUI.Button(new Rect(10f, 100f, 60f, 25f), "Land")) {
			onPlayerLand(new PlayerLandEvent());
		}

		if (GUI.Button(new Rect(80f, 100f, 60f, 25f), "Damage")) {
			onDamage(new GetDamageEvent(Vector3.back));
		}

		if (GUI.Button(new Rect(150f, 100f, 60f, 25f), "Weapon")) {
			onWeaponShoot(new WeaponShootEvent(Vector3.back, Feedbacks[2].MaxNoise, Feedbacks[2].MaxAngle));
		}

		Vector3[] array = {
			new Vector3(-0.8f, -0.3f, 0.6f),
			new Vector3(-0.8f, -0.1f, 0.6f),
			new Vector3(0.5f, -0.7f, 0.5f)
		};

		for (var i = 0; i < array.Length; i++) {
			if (GUI.Button(new Rect(10f, 125 + 25 * i, 100f, 25f), "Damage " + i)) {
				onDamage(new GetDamageEvent(array[i]));
			}
		}
	}

	private void OnDrawGizmos() {
		if (!_transformCache) {
			_transformCache = transform;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawRay(_transformCache.position, Feedbacks[1].GetDirection());
	}

	private void onPlayerLand(PlayerLandEvent ev) {
		ApplyFeedback(FeedbackType.Land, Vector3.down, Vector3.right);
	}

	private void onDamage(GetDamageEvent ev) {
		ApplyFeedback(FeedbackType.Damage, ev.Force, Vector3.zero);
	}

	private void onWeaponShoot(WeaponShootEvent ev) {
		Feedbacks[2].Peak = 1f;
		Feedbacks[2].MaxNoise = ev.Noise;
		Feedbacks[2].MaxAngle = ev.Angle;
		ApplyFeedback(FeedbackType.Weapon, ev.Force, Vector3.left);
	}

	public void ApplyFeedback(FeedbackType t, Vector3 dir, Vector3 rotAxis) {
		_timer = 0f;
		_currentFeedback = Feedbacks[(int)t];
		_currentFeedback.SetDirection(dir);
		_rotationAxis = ((!(rotAxis == Vector3.zero)) ? rotAxis : _transformCache.InverseTransformDirection(Vector3.Cross(Vector3.up, dir)));
	}

	public void ApplyFeedback(Vector3 dir, float noise, float angle) {
		Feedbacks[2].Peak = 1f;
		Feedbacks[2].MaxNoise = noise;
		Feedbacks[2].MaxAngle = angle;
		ApplyFeedback(FeedbackType.Weapon, dir, Vector3.left);
	}

	public Quaternion GetFeedbackRoation() {
		return _tmpRotation;
	}

	[Serializable]
	public class Feedback {
		private Vector3 _dir;
		public float MaxAngle;
		public float MaxNoise;
		public float Peak;
		public float TimeToEnd;
		public float TimeToPeak;
		public FeedbackType Type;

		public Feedback(Feedback f) {
			_dir = Vector3.zero;
			Type = f.Type;
			Peak = f.Peak;
			TimeToPeak = f.TimeToPeak;
			TimeToEnd = f.TimeToEnd;
		}

		public void SetDirection(Vector3 dir) {
			_dir = dir;
		}

		public Vector3 GetDirection() {
			return _dir;
		}
	}
}
