using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFeedbackManager : MonoBehaviour {
	public enum WeaponMode {
		Primary,
		Second,
		PutDown
	}

	private float _angleX;
	private float _angleY;
	private WeaponBobManager _bobManager;
	private WeaponMode _currentWeaponMode;
	protected Feedback _dip;
	protected Feedback _fire;
	private bool _isIronSight;
	private bool _isIronSightPosDone;
	private bool _needLerp;
	private WeaponState _pickupWeaponState;

	[SerializeField]
	private Transform _pivotPoint;

	private WeaponState _putDownWeaponState;
	private float _sign;
	private float _time;
	public WeaponAnimData WeaponAnimation;
	public FeedbackData WeaponDip;
	public FeedbackData WeaponFire;
	public static WeaponFeedbackManager Instance { get; private set; }

	public static bool IsBobbing {
		get { return Instance && Instance._bobManager.Mode != LevelCamera.BobMode.None; }
	}

	public bool IsIronSighted {
		get { return _isIronSight; }
		private set {
			_isIronSight = value;
			GameState.Current.PlayerData.IsIronSighted.Value = value;
		}
	}

	public WeaponMode CurrentWeaponMode {
		get { return _currentWeaponMode; }
		private set { _currentWeaponMode = value; }
	}

	public bool _isWeaponInIronSightPosition {
		get { return _isIronSight && _isIronSightPosDone; }
	}

	private void Awake() {
		Instance = this;
		_bobManager = new WeaponBobManager();
	}

	private void OnEnable() {
		_dip.Reset();
		_fire.Reset();
		CurrentWeaponMode = WeaponMode.PutDown;
	}

	private void Update() {
		if (_putDownWeaponState != null) {
			_putDownWeaponState.Update();
		}

		if (_pickupWeaponState != null) {
			_pickupWeaponState.Update();
		}
	}

	private Quaternion CalculateBobDip() {
		if (_dip.time <= _dip.Duration) {
			_dip.HandleFeedback();
		} else if (_needLerp) {
			_angleX = Mathf.Lerp(_angleX, 0f, Time.deltaTime * 9f);
			_angleY = Mathf.Lerp(_angleY, 0f, Time.deltaTime * 9f);

			if (_angleX < 0.01f && _angleY < 0.01f) {
				_time = 0f;
				_needLerp = false;
			}
		} else {
			var num = Mathf.Sin(_bobManager.Data.Frequency * _time);
			_angleX = Mathf.Abs(_bobManager.Data.XAmplitude * num);
			_angleY = _bobManager.Data.YAmplitude * num * _sign;
			_time += Time.deltaTime;
		}

		return Quaternion.Euler(_angleX, _angleY, 0f);
	}

	public static void SetBobMode(LevelCamera.BobMode mode) {
		if (Instance && Instance._bobManager.Mode != mode) {
			Instance._bobManager.Mode = mode;

			if (mode == LevelCamera.BobMode.Run) {
				Instance._needLerp = false;
				Instance._sign = (!AutoMonoBehaviour<InputManager>.Instance.IsDown(GameInputKey.Right)) ? 1 : (-1);
				Instance._time = Mathf.Asin(Instance._angleX / Instance._bobManager.Data.XAmplitude) / Instance._bobManager.Data.Frequency;
			} else {
				Instance._needLerp = true;
			}
		}
	}

	public void LandingDip() {
		if (_fire.time > 0f && _fire.time < _fire.Duration) {
			return;
		}

		if (CurrentWeaponMode != WeaponMode.PutDown) {
			_dip.time = 0f;
			_dip.angle = WeaponDip.angle;
			_dip.noise = WeaponDip.noise;
			_dip.strength = WeaponDip.strength;
			_dip.timeToPeak = WeaponDip.timeToPeak;
			_dip.timeToEnd = WeaponDip.timeToEnd;
			_dip.direction = Vector3.down;
			_dip.rotationAxis = Vector3.right;
		}
	}

	public void Fire() {
		if (CurrentWeaponMode != WeaponMode.PutDown) {
			_fire.noise = WeaponFire.noise;
			_fire.strength = WeaponFire.strength;
			_fire.timeToPeak = WeaponFire.timeToPeak;
			_fire.timeToEnd = WeaponFire.timeToEnd;
			_fire.direction = Vector3.back;
			_fire.rotationAxis = Vector3.left;
			_fire.recoilTime = WeaponFire.recoilTime;

			if (_dip.time < _dip.Duration) {
				_dip.Reset();
			}

			if (_fire.time > _fire.recoilTime && _fire.time < _fire.Duration) {
				_fire.time = WeaponFire.timeToPeak / 3f;
				_fire.angle = WeaponFire.angle / 3f;
			} else if (_fire.time >= _fire.Duration) {
				_fire.time = 0f;
				_fire.angle = WeaponFire.angle;
			}
		}
	}

	public void PutDown(bool destroy = false) {
		if (_pickupWeaponState != null && _pickupWeaponState.IsValid) {
			PutDownWeapon(_pickupWeaponState.Weapon, _pickupWeaponState.Decorator, destroy);
			_pickupWeaponState = null;
		}
	}

	public void PickUp(WeaponSlot slot) {
		if (_pickupWeaponState != null && _pickupWeaponState.IsValid) {
			if (_pickupWeaponState.Weapon == slot.Logic) {
				return;
			}

			PutDownWeapon(_pickupWeaponState.Weapon, _pickupWeaponState.Decorator);
		} else if (_pickupWeaponState == null && _putDownWeaponState != null && _putDownWeaponState.Weapon == slot.Logic) {
			_putDownWeaponState.Finish();
		}

		_pickupWeaponState = new PickUpState(slot.Logic, slot.Decorator);
		WeaponFire.recoilTime = WeaponConfigurationHelper.GetRateOfFire(slot.View);
		WeaponFire.strength = WeaponConfigurationHelper.GetRecoilMovement(slot.View);
		WeaponFire.angle = WeaponConfigurationHelper.GetRecoilKickback(slot.View);
	}

	public void BeginIronSight() {
		if (!_isIronSight) {
			IsIronSighted = true;
		}
	}

	public void EndIronSight() {
		IsIronSighted = false;
	}

	public void ResetIronSight() {
		IsIronSighted = false;

		if (_pickupWeaponState != null) {
			_pickupWeaponState.Reset();
		}

		if (_putDownWeaponState != null) {
			_putDownWeaponState.Reset();
		}
	}

	private void PutDownWeapon(BaseWeaponLogic weapon, BaseWeaponDecorator decorator, bool destroy = false) {
		if (_putDownWeaponState != null) {
			_putDownWeaponState.Finish();
		}

		_putDownWeaponState = new PutDownState(weapon, decorator, destroy);
	}

	public void SetFireFeedback(FeedbackData data) {
		WeaponFire.angle = data.angle;
		WeaponFire.noise = data.noise;
		WeaponFire.strength = data.strength;
		WeaponFire.timeToEnd = data.timeToEnd;
		WeaponFire.timeToPeak = data.timeToPeak;
		WeaponFire.recoilTime = data.recoilTime;
	}

	private class WeaponBobManager {
		private readonly Dictionary<LevelCamera.BobMode, BobData> _bobData;
		private LevelCamera.BobMode _bobMode;
		private BobData _data;

		public BobData Data {
			get { return _data; }
		}

		public LevelCamera.BobMode Mode {
			get { return _bobMode; }
			set {
				if (_bobMode != value) {
					_bobMode = value;
					_data = _bobData[value];
				}
			}
		}

		public WeaponBobManager() {
			_bobData = new Dictionary<LevelCamera.BobMode, BobData>();

			foreach (var obj in Enum.GetValues(typeof(LevelCamera.BobMode))) {
				var bobMode = (LevelCamera.BobMode)((int)obj);

				switch (bobMode) {
					case LevelCamera.BobMode.Walk:
						_bobData[bobMode] = new BobData(0.5f, 3f, 6f);

						continue;
					case LevelCamera.BobMode.Run:
						_bobData[bobMode] = new BobData(1f, 3f, 8f);

						continue;
					case LevelCamera.BobMode.Crouch:
						_bobData[bobMode] = new BobData(0.5f, 3f, 12f);

						continue;
				}

				_bobData[bobMode] = new BobData(0f, 0f, 0f);
			}

			_data = _bobData[LevelCamera.BobMode.Idle];
		}

		public struct BobData {
			private float _xAmplitude;
			private float _yAmplitude;
			private float _frequency;

			public float XAmplitude {
				get { return _xAmplitude; }
			}

			public float YAmplitude {
				get { return _yAmplitude; }
			}

			public float Frequency {
				get { return _frequency; }
			}

			public BobData(float xamp, float yamp, float freq) {
				_xAmplitude = xamp;
				_yAmplitude = yamp;
				_frequency = freq;
			}
		}
	}

	private abstract class WeaponState {
		protected float _currentRotation;
		private BaseWeaponDecorator _decorator;
		protected bool _isRunning;
		protected Vector3 _pivotOffset;
		protected Vector3 _targetPosition;
		protected Quaternion _targetRotation;
		protected float _time;
		protected float _transitionTime;
		private BaseWeaponLogic _weapon;

		public Vector3 PivotVector {
			get { return _pivotOffset + ((!Instance._isIronSight) ? Decorator.DefaultPosition : Decorator.IronSightPosition); }
		}

		public bool IsRunning {
			get { return _isRunning; }
		}

		public bool IsValid {
			get { return _weapon != null && _decorator != null; }
		}

		public BaseWeaponDecorator Decorator {
			get { return _decorator; }
		}

		public BaseWeaponLogic Weapon {
			get { return _weapon; }
		}

		public Vector3 TargetPosition {
			get { return _targetPosition; }
		}

		public Quaternion TargetRotation {
			get { return _targetRotation; }
		}

		protected WeaponState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator) {
			_time = 0f;
			_weapon = weapon;
			_decorator = decorator;
			_isRunning = _weapon != null;
		}

		public abstract void Update();
		public abstract void Finish();

		public void Reset() {
			_pivotOffset = new Vector3(0f, 0f, 0.2f);
		}

		public virtual bool CanTransit(WeaponMode mode) {
			return Instance.CurrentWeaponMode != mode;
		}
	}

	private class PickUpState : WeaponState {
		private bool _isFiring;

		public PickUpState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator) : base(weapon, decorator) {
			_transitionTime = Mathf.Max(Instance.WeaponAnimation.PickUpDuration, weapon.Config.SwitchDelayMilliSeconds / 1000);

			if (decorator.IsMelee) {
				_currentRotation = -90f;

				if (Decorator) {
					Decorator.CurrentRotation = Quaternion.Euler(0f, 0f, _currentRotation);
					Decorator.CurrentPosition = decorator.DefaultPosition;
					Decorator.IsEnabled = true;
				}
			} else {
				_currentRotation = Instance.WeaponAnimation.PutDownAngles;
				_pivotOffset = -Instance._pivotPoint.localPosition;

				if (Decorator) {
					Decorator.CurrentRotation = Quaternion.Euler(Instance.WeaponAnimation.PutDownAngles, 0f, 0f);
					Decorator.CurrentPosition = Quaternion.AngleAxis(_currentRotation, Vector3.right) * PivotVector;
					Decorator.IsEnabled = true;
				}
			}

			LevelCamera.ResetZoom();
		}

		public override void Update() {
			if (IsValid) {
				if (IsRunning) {
					if (_time <= _transitionTime) {
						_currentRotation = Mathf.Lerp(_currentRotation, Instance.WeaponAnimation.PickUpAngles, _time / _transitionTime);

						if (Decorator.IsMelee) {
							_targetPosition = Decorator.DefaultPosition;
							_targetRotation = Quaternion.Euler(0f, 0f, _currentRotation);
						} else {
							_targetPosition = Quaternion.AngleAxis(_currentRotation, Vector3.right) * PivotVector;
							_targetRotation = Quaternion.Euler(_currentRotation + Decorator.DefaultAngles.x, Decorator.DefaultAngles.y, Decorator.DefaultAngles.z);
						}

						if (!Instance._isIronSight) {
							Decorator.CurrentPosition = _targetPosition;
							Decorator.CurrentRotation = _targetRotation;
						}

						_time += Time.deltaTime;
					}

					if (_time > _transitionTime * 0.25f) {
						Weapon.IsWeaponActive = true;
					}

					if (_time > _transitionTime) {
						Finish();
					}
				}

				if (_time > _transitionTime * 0.25f) {
					if (Instance._isIronSight) {
						_pivotOffset = Vector3.Lerp(_pivotOffset, Vector2.zero, Time.deltaTime * 20f);

						if (Decorator.CurrentPosition == Decorator.IronSightPosition) {
							Instance._isIronSightPosDone = true;
						} else {
							Instance._isIronSightPosDone = false;
						}
					} else {
						_pivotOffset = Vector3.Lerp(_pivotOffset, new Vector3(0f, 0f, 0.2f), Time.deltaTime * 10f);
					}

					if (Instance._fire.time < Instance._fire.Duration) {
						if (!IsRunning) {
							if (!Instance._isIronSight && _pivotOffset == new Vector3(0f, 0f, 0.2f)) {
								Instance._fire.HandleFeedback();
								Decorator.CurrentPosition = _targetPosition + Instance._fire.PositionOffset;
								Decorator.CurrentRotation = _targetRotation * Instance._fire.RotationOffset;
							} else {
								Decorator.CurrentPosition = PivotVector + Instance._dip.PositionOffset;
								Decorator.CurrentRotation = _targetRotation * Instance._dip.RotationOffset;
							}

							_isFiring = true;
						}
					} else {
						if (_isFiring) {
							_isFiring = false;
							Instance._time = 0f;
							Instance._angleX = 0f;
							Instance._angleY = 0f;
						}

						var quaternion = Quaternion.identity;

						if (Instance._isIronSight && Instance._dip.PositionOffset == Vector3.zero) {
							quaternion = Quaternion.identity;
						} else {
							quaternion = Instance.CalculateBobDip();
						}

						if (!Decorator.IsMelee) {
							Decorator.CurrentPosition = quaternion * PivotVector + Instance._dip.PositionOffset;
							Decorator.CurrentRotation = _targetRotation * Instance._dip.RotationOffset * quaternion;
						} else {
							Decorator.CurrentRotation = _targetRotation * Instance._dip.RotationOffset * quaternion;
						}
					}
				}
			}
		}

		public override void Finish() {
			if (_isRunning) {
				_isRunning = false;

				if (Weapon != null) {
					Weapon.IsWeaponActive = true;
					Instance._currentWeaponMode = WeaponMode.Primary;
				}

				if (Decorator.IsMelee) {
					_targetRotation = Quaternion.Euler(0f, 0f, Instance.WeaponAnimation.PickUpAngles);
					_targetPosition = Decorator.DefaultPosition;
				} else {
					_targetRotation = Quaternion.Euler(Instance.WeaponAnimation.PickUpAngles + Decorator.DefaultAngles.x, Decorator.DefaultAngles.y, Decorator.DefaultAngles.z);
					_targetPosition = Quaternion.AngleAxis(Instance.WeaponAnimation.PickUpAngles, Vector3.right) * PivotVector;
				}
			}
		}

		public override string ToString() {
			return "Pick Up State";
		}
	}

	private class PutDownState : WeaponState {
		private bool _destroy;

		public PutDownState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator, bool destroy = false) : base(weapon, decorator) {
			_destroy = destroy;
			_currentRotation = decorator.CurrentRotation.eulerAngles.x;

			if (_currentRotation > 300f) {
				_currentRotation = 360f - _currentRotation;
			}

			if (!decorator.IsMelee) {
				_pivotOffset = -Instance._pivotPoint.localPosition;
			}

			_transitionTime = Instance.WeaponAnimation.PutDownDuration;

			if (Weapon != null) {
				Weapon.IsWeaponActive = false;
			}
		}

		public override void Update() {
			if (IsRunning && IsValid) {
				if (_time > _transitionTime) {
					return;
				}

				if (Decorator.IsMelee) {
					_currentRotation = Mathf.Lerp(_currentRotation, -90f, _time / _transitionTime);
					_targetPosition = Decorator.DefaultPosition;
					_targetRotation = Quaternion.Euler(0f, 0f, _currentRotation);
				} else {
					_currentRotation = Mathf.Lerp(_currentRotation, Instance.WeaponAnimation.PutDownAngles, _time / _transitionTime);
					_targetPosition = Quaternion.AngleAxis(_currentRotation, Vector3.right) * PivotVector;
					_targetRotation = Quaternion.Euler(_currentRotation, 0f, 0f);
				}

				Decorator.CurrentPosition = _targetPosition;
				Decorator.CurrentRotation = _targetRotation;
				_time += Time.deltaTime;

				if (_time > _transitionTime) {
					Finish();
				}
			}
		}

		public override void Finish() {
			if (_isRunning) {
				_isRunning = false;

				if (Decorator) {
					Decorator.IsEnabled = false;
					Decorator.CurrentPosition = Decorator.DefaultPosition;
					Decorator.CurrentRotation = _targetRotation;

					if (_destroy) {
						Destroy(Decorator.gameObject);
					}
				}
			}
		}

		public override string ToString() {
			return "Put down";
		}
	}

	[Serializable]
	public class FeedbackData {
		public float angle;
		public float noise;
		public float recoilTime;
		public float strength;
		public float timeToEnd;
		public float timeToPeak;
	}

	protected struct Feedback {
		public float time;
		public float noise;
		public float angle;
		public float timeToPeak;
		public float timeToEnd;
		public float strength;
		public float recoilTime;
		public Vector3 direction;
		public Vector3 rotationAxis;
		private float _maxAngle;
		private float _angle;
		private Vector3 _positionOffset;
		private Quaternion _rotationOffset;

		public float DebugAngle {
			get { return _angle; }
		}

		public float Duration {
			get { return timeToPeak + timeToEnd; }
		}

		public Vector3 PositionOffset {
			get { return _positionOffset; }
		}

		public Quaternion RotationOffset {
			get { return _rotationOffset; }
		}

		public void HandleFeedback() {
			var num = UnityEngine.Random.Range(-noise, noise);
			_maxAngle = Mathf.Lerp(_maxAngle, angle, Time.deltaTime * 10f);

			if (time < Duration) {
				time += Time.deltaTime;

				if (time < Duration) {
					float num2;

					if (time < timeToPeak) {
						num2 = strength * Mathf.Sin(time * 3.1415927f * 0.5f / timeToPeak);
						noise = Mathf.Lerp(noise, 0f, time / timeToPeak);
						_angle = Mathf.Lerp(0f, _maxAngle, Mathf.Pow(time / timeToPeak, 2f));
					} else {
						var num3 = (time - timeToPeak) / timeToEnd;
						num2 = strength * Mathf.Cos((time - timeToPeak) * 3.1415927f * 0.5f / timeToEnd);
						_angle = Mathf.Lerp(_maxAngle, 0f, num3);

						if (time != 0f) {
							num = 0f;
						}
					}

					if (Singleton<WeaponController>.Instance.CurrentWeapon) {
						_positionOffset = num2 * direction + Singleton<WeaponController>.Instance.CurrentWeapon.transform.right * num + Singleton<WeaponController>.Instance.CurrentWeapon.transform.up * num;
						_rotationOffset = Quaternion.AngleAxis(_angle, rotationAxis);
					}
				} else {
					_angle = 0f;
					_positionOffset = Vector3.zero;
					_rotationOffset = Quaternion.identity;
				}
			} else {
				time = 0f;
				_angle = 0f;
				_positionOffset = Vector3.zero;
				_rotationOffset = Quaternion.identity;
			}
		}

		public void Reset() {
			time = 0f;
			timeToEnd = 0f;
			timeToPeak = -1f;
			angle = 0f;
			direction = Vector3.zero;
			_angle = 0f;
			_positionOffset = Vector3.zero;
			_rotationOffset = Quaternion.identity;
		}
	}

	[Serializable]
	public class WeaponAnimData {
		public float PickUpAngles;
		public float PickUpDuration;
		public float PutDownAngles = 30f;
		public float PutDownDuration;
	}
}
