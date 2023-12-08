using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour {
	public const int DefaultFOV = 75;
	public const int ZoomSpeed = 10;

	public enum BobMode {
		None,
		Idle,
		Walk,
		Run,
		Fly,
		Swim,
		Crouch
	}

	public enum CameraMode {
		Disabled,
		FirstPerson,
		OrbitAround,
		Paused,
		FreeSpectator,
		SmoothFollow,
		Ragdoll
	}

	public enum FeedbackType {
		JumpLand,
		GetDamage,
		ShootWeapon
	}

	private static LevelCamera _instance;
	private AudioListener _audioListener;
	private CameraConfiguration _cameraConfiguration;
	private CameraCollisionDetector _ccd;
	private CameraState _currentState;
	private CameraFeedbackData _jumpFeedback;
	private AudioLowPassFilter _lowpassFilter;
	private Quaternion _userInputCache;
	private Quaternion _userInputRotation;

	public static float FieldOfView {
		get { return (!_instance || !(_instance.MainCamera != null)) ? 65f : _instance.MainCamera.fieldOfView; }
	}

	public static CameraMode CurrentMode { get; private set; }

	public static bool IsLowpassFilterEnabled {
		get { return _instance && Application.isWebPlayer && _instance._lowpassFilter.enabled; }
	}

	public CameraFeedback Feedback { get; private set; }
	public CameraZoomData ZoomData { get; private set; }
	public static bool IsZoomedIn { get; private set; }
	public Camera MainCamera { get; private set; }
	public Transform Transform { get; private set; }
	public Transform TargetTransform { get; private set; }
	public float RotationOffset { get; private set; }

	private void Awake() {
		Transform = transform;
		Feedback = new CameraFeedback(transform);
		ZoomData = new CameraZoomData(this);
		_cameraConfiguration = new CameraConfiguration();
		_jumpFeedback = new CameraFeedbackData();
		_currentState = new DisabledState(this, Vector3.zero);
		_ccd = new CameraCollisionDetector();
		_ccd.Offset = 1f;
		_ccd.LayerMask = 1;

		if (Application.isWebPlayer) {
			_lowpassFilter = gameObject.AddComponent<AudioLowPassFilter>();
			_lowpassFilter.cutoffFrequency = 755f;
		}

		_audioListener = GetComponent<AudioListener>();
	}

	private void LateUpdate() {
		_currentState.Update();
	}

	private void OnDrawGizmos() {
		if (TargetTransform != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(TargetTransform.position, 0.1f);
			Gizmos.color = Color.white;
		}
	}

	private void OnDestroy() {
		_currentState.Finish();
		_instance = null;
	}

	private void InitUserInput() {
		var eulerAngles = UserInput.Rotation.eulerAngles;
		_userInputCache = UserInput.Rotation;
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, 0f, 60f);
		_userInputRotation = Quaternion.Euler(eulerAngles);
	}

	private void UpdateUserInput() {
		if (Input.GetMouseButton(0)) {
			UserInput.UpdateMouse();
		}

		var eulerAngles = UserInput.Rotation.eulerAngles;
		var num = _userInputCache.eulerAngles.x;
		var num2 = UserInput.Rotation.eulerAngles.x;

		if (num > 180f) {
			num -= 360f;
		}

		if (num2 > 180f) {
			num2 -= 360f;
		}

		eulerAngles.x = Mathf.Clamp(_userInputRotation.eulerAngles.x + (num2 - num), 0f, 60f);
		_userInputCache = UserInput.Rotation;
		_userInputRotation = Quaternion.Euler(eulerAngles);
	}

	private void TransformFollowCamera(Vector3 targetPosition, Quaternion targetRotation, float distance, ref float collideDistance) {
		var vector = _userInputRotation * Vector3.back * collideDistance;
		var matrix4x = Matrix4x4.TRS(targetPosition, targetRotation, Vector3.one);
		var vector2 = matrix4x.MultiplyPoint3x4(vector);
		var quaternion = Quaternion.LookRotation(targetPosition - vector2);
		var vector3 = matrix4x.MultiplyPoint3x4(_userInputRotation * Vector3.back * distance);

		if (_ccd.Detect(targetPosition, vector3, quaternion * Vector3.right)) {
			var distance2 = _ccd.Distance;

			if (distance2 < collideDistance) {
				collideDistance = Mathf.Clamp(distance2, 1f, distance);
			} else {
				collideDistance = Mathf.Lerp(collideDistance, distance2, Time.deltaTime * 3f);
			}
		} else if (!Mathf.Approximately(collideDistance, distance)) {
			collideDistance = Mathf.Lerp(collideDistance, distance, Time.deltaTime * 5f);
		} else {
			collideDistance = distance;
		}

		Transform.position = vector2;
		Transform.rotation = quaternion;
	}

	private void TransformDeathCamera(Vector3 targetPosition, Quaternion targetRotation, float distance, ref float collideDistance) {
		var vector = Vector3.back * collideDistance;
		var matrix4x = Matrix4x4.TRS(targetPosition, targetRotation, Vector3.one);
		var vector2 = matrix4x.MultiplyPoint3x4(vector);
		var quaternion = Quaternion.LookRotation(targetPosition - vector2);
		var vector3 = matrix4x.MultiplyPoint3x4(Vector3.back * distance);

		if (_ccd.Detect(targetPosition, vector3, quaternion * Vector3.right)) {
			var distance2 = _ccd.Distance;

			if (distance2 < collideDistance) {
				collideDistance = Mathf.Clamp(distance2, 1f, distance);
			} else {
				collideDistance = Mathf.Lerp(collideDistance, distance2, Time.deltaTime * 3f);
			}
		} else if (!Mathf.Approximately(collideDistance, distance)) {
			collideDistance = Mathf.Lerp(collideDistance, distance, Time.deltaTime * 5f);
		} else {
			collideDistance = distance;
		}

		Transform.position = vector2;
		Transform.rotation = quaternion;
	}

	private void SetCamera(Camera camera, Vector3 position, Quaternion rotation) {
		if (camera != MainCamera && camera != null) {
			ReleaseCamera();
			_cameraConfiguration.Parent = camera.transform.parent;
			_cameraConfiguration.Fov = camera.fieldOfView;
			_cameraConfiguration.CullingMask = camera.cullingMask;
			MainCamera = camera;
			MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalProjectile);
			MainCamera.transform.parent = transform;
			MainCamera.transform.localPosition = Vector3.zero;
			MainCamera.transform.localRotation = Quaternion.identity;
			ZoomData.TargetFOV = MainCamera.fieldOfView;
			Transform.position = position;
			Transform.rotation = rotation;
			_audioListener.enabled = true;
		}
	}

	private void SetCameraMode(CameraMode mode, Transform target) {
		Feedback.timeToEnd = 0f;
		CurrentMode = mode;
		_currentState.Finish();

		if (IsZoomedIn) {
			DoZoomOut(75f, 10f);
		}

		if (MainCamera != null) {
			MainCamera.transform.localRotation = Quaternion.identity;
			MainCamera.transform.localPosition = Vector3.zero;

			switch (mode) {
				case CameraMode.Disabled:
					TargetTransform = null;
					_currentState = new DisabledState(this, Vector3.zero);

					if (GameState.Current.Avatar.Decorator != null) {
						GameState.Current.Avatar.Decorator.gameObject.SetActive(true);
					}

					if (GameState.Current.Player.WeaponCamera != null) {
						GameState.Current.Player.WeaponCamera.IsEnabled = false;
					}

					break;
				case CameraMode.FirstPerson:
					MainCamera.cullingMask = LayerUtil.RemoveFromLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer, UberstrikeLayer.Weapons);
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.RemoteProjectile);
					TargetTransform = GameState.Current.Player.CameraTarget;
					_currentState = new FirstPersonState(this, GameState.Current.Player.EyePosition);

					if (GameState.Current.Avatar.Decorator != null) {
						GameState.Current.Avatar.Decorator.gameObject.SetActive(false);
					}

					if (GameState.Current.Player.WeaponCamera) {
						GameState.Current.Player.WeaponCamera.IsEnabled = true;
					}

					break;
				case CameraMode.OrbitAround:
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
					TargetTransform = GameState.Current.Player.CameraTarget;
					RotationOffset = 180f;
					_currentState = new OrbitAroundState(this, new Vector3(0f, -0.5f, 0f));

					if (GameState.Current.Avatar.Decorator != null && GameState.Current.Avatar.Ragdoll == null) {
						GameState.Current.Avatar.Decorator.gameObject.SetActive(true);
					}

					if (GameState.Current.Player.WeaponCamera) {
						GameState.Current.Player.WeaponCamera.IsEnabled = false;
					}

					break;
				case CameraMode.Paused:
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
					TargetTransform = GameState.Current.Player.CameraTarget;
					RotationOffset = 0f;
					_currentState = new OrbitAroundState(this, new Vector3(0f, -0.5f, 0f));

					if (GameState.Current.Avatar.Decorator != null && GameState.Current.Avatar.Ragdoll == null) {
						GameState.Current.Avatar.Decorator.gameObject.SetActive(true);
					}

					if (GameState.Current.Player.WeaponCamera) {
						GameState.Current.Player.WeaponCamera.IsEnabled = false;
					}

					break;
				case CameraMode.FreeSpectator:
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
					TargetTransform = null;
					_currentState = new SpectatorState(this, Vector3.zero);

					if (GameState.Current.Avatar.Decorator != null) {
						GameState.Current.Avatar.Decorator.gameObject.SetActive(false);
					}

					if (GameState.Current.Player.WeaponCamera) {
						GameState.Current.Player.WeaponCamera.IsEnabled = false;
					}

					break;
				case CameraMode.SmoothFollow:
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
					TargetTransform = target;
					_currentState = new SmoothFollowState(this, new Vector3(0f, 1.3f, 0f));

					break;
				case CameraMode.Ragdoll:
					MainCamera.cullingMask = LayerUtil.AddToLayerMask(MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
					TargetTransform = GameState.Current.Avatar.Ragdoll.GetBone(BoneIndex.Hips);
					_currentState = new RagdollState(this, new Vector3(0f, 1f, 0f));

					if (GameState.Current.Player.WeaponCamera) {
						GameState.Current.Player.WeaponCamera.IsEnabled = false;
					}

					break;
				default:
					Debug.LogError("Camera does not support " + mode);

					break;
			}
		}
	}

	private void ReleaseCamera() {
		if (MainCamera != null) {
			_audioListener.enabled = false;
			Destroy(MainCamera.gameObject);
			MainCamera = null;
		}
	}

	public static void SetLevelCamera(Camera camera, Vector3 position, Quaternion rotation) {
		if (_instance == null) {
			var gameObject = new GameObject("LevelCamera", typeof(AudioListener));
			gameObject.layer = 18;
			var sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = 0.01f;
			var rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
			_instance = gameObject.AddComponent<LevelCamera>();
		}

		_instance.SetCamera(camera, position, rotation);
	}

	public static void SetMode(CameraMode mode, Transform target = null) {
		if (_instance) {
			_instance.SetCameraMode(mode, target);
		}
	}

	public static void DoFeedback(FeedbackType type, Vector3 direction, float strength, float noise, float timeToPeak, float timeToEnd, float angle, Vector3 axis) {
		if (_instance) {
			_instance.Feedback.time = 0f;
			_instance.Feedback.noise = noise / 4f;
			_instance.Feedback.strength = strength;
			_instance.Feedback.timeToPeak = timeToPeak;
			_instance.Feedback.timeToEnd = timeToEnd;
			_instance.Feedback.direction = direction;
			_instance.Feedback.angle = angle;
			_instance.Feedback.rotationAxis = axis;
		}
	}

	public static bool DoLandFeedback(bool shake) {
		if (_instance && CurrentMode == CameraMode.FirstPerson && (_instance.Feedback.time == 0f || _instance.Feedback.time >= _instance.Feedback.Duration)) {
			_instance.Feedback.time = 0f;
			_instance.Feedback.angle = _instance._jumpFeedback.angle;
			_instance.Feedback.noise = ((!shake) ? 0f : _instance._jumpFeedback.noise);
			_instance.Feedback.strength = _instance._jumpFeedback.strength;
			_instance.Feedback.timeToPeak = _instance._jumpFeedback.timeToPeak;
			_instance.Feedback.timeToEnd = _instance._jumpFeedback.timeToEnd;
			_instance.Feedback.direction = Vector3.down;
			_instance.Feedback.rotationAxis = Vector3.right;
			WeaponFeedbackManager.Instance.LandingDip();

			return true;
		}

		return false;
	}

	public static void DoZoomIn(float fov, float speed, bool hideWeapon) {
		if (_instance) {
			if (fov < 1f || fov > 100f || speed < 0.001f || speed > 100f) {
				Debug.LogError(string.Concat("Invalid parameters specified!\n FOV should be >1 & <100, Speed should be >0.001 & <100.\nFOV = ", fov, " Speed = ", speed));

				return;
			}

			if (IsZoomedIn && fov == FieldOfView) {
				return;
			}

			if (CurrentMode == CameraMode.FirstPerson && hideWeapon) {
				GameState.Current.Player.WeaponCamera.IsEnabled = false;
			}

			_instance.ZoomData.Speed = speed;
			_instance.ZoomData.TargetFOV = fov;
			_instance.ZoomData.TargetAlpha = 1f;
			IsZoomedIn = true;
		}
	}

	public static void DoZoomOut(float fov, float speed) {
		if (_instance) {
			if (fov < 1f || fov > 100f || speed < 0.001f || speed > 100f) {
				Debug.LogError(string.Concat("Invalid parameters specified!\n FOV should be >1 & <100, Speed should be >0.001 & <100.\nFOV = ", fov, " Speed = ", speed));

				return;
			}

			if (!IsZoomedIn) {
				return;
			}

			_instance.ZoomData.Speed = speed;
			_instance.ZoomData.TargetFOV = fov;
			_instance.ZoomData.TargetAlpha = 0f;
			IsZoomedIn = false;

			if (CurrentMode == CameraMode.FirstPerson) {
				GameState.Current.Player.WeaponCamera.IsEnabled = true;
			}

			EventHandler.Global.Fire(new GameEvents.PlayerZoomOut());
		}
	}

	public static void ResetZoom() {
		IsZoomedIn = false;

		if (_instance) {
			_instance.ZoomData.ResetZoom();
		}

		if (CurrentMode == CameraMode.FirstPerson) {
			GameState.Current.Player.WeaponCamera.IsEnabled = true;
		}
	}

	public static void EnableLowPassFilter(bool enabled) {
		if (_instance && Application.isWebPlayer) {
			_instance._lowpassFilter.enabled = enabled;
		}
	}

	public static void ResetFeedback() {
		if (_instance) {
			_instance.Feedback.Reset();
		}
	}

	public static void SetPosition(Vector3 position) {
		if (_instance) {
			_instance.transform.position = position;
		}
	}

	public class CameraConfiguration {
		public int CullingMask;
		public float Fov;
		public Transform Parent;
	}

	public class CameraBobManager {
		private readonly Dictionary<BobMode, BobData> bobData;
		private BobMode bobMode;
		private LevelCamera camera;
		private BobData data;
		private float strength;

		public BobMode Mode {
			get { return bobMode; }
			set {
				if (bobMode != value) {
					strength = 0f;
					bobMode = value;
					data = bobData[value];
				}
			}
		}

		public CameraBobManager(LevelCamera camera) {
			this.camera = camera;
			bobData = new Dictionary<BobMode, BobData>();

			foreach (var obj in Enum.GetValues(typeof(BobMode))) {
				var bobMode = (BobMode)((int)obj);

				switch (bobMode) {
					case BobMode.Idle:
						bobData[bobMode] = new BobData(0.2f, 0f, 2f);

						continue;
					case BobMode.Walk:
						bobData[bobMode] = new BobData(0.3f, 0.3f, 6f);

						continue;
					case BobMode.Run:
						bobData[bobMode] = new BobData(0.5f, 0.3f, 8f);

						continue;
					case BobMode.Crouch:
						bobData[bobMode] = new BobData(0.8f, 0.8f, 12f);

						continue;
				}

				bobData[bobMode] = new BobData(0f, 0f, 0f);
			}

			data = bobData[BobMode.Idle];
		}

		public void Update() {
			switch (bobMode) {
				case BobMode.Idle: {
					var num = Mathf.Sin(Time.time * data.Frequency);
					camera.Transform.rotation = Quaternion.AngleAxis(num * data.XAmplitude * strength, camera.Transform.right) * Quaternion.AngleAxis(num * data.ZAmplitude, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
				case BobMode.Walk: {
					var num2 = Mathf.Sin(Time.time * data.Frequency);
					camera.Transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num2 * data.XAmplitude), camera.Transform.right) * Quaternion.AngleAxis(num2 * data.ZAmplitude, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
				case BobMode.Run: {
					var num3 = Mathf.Sin(Time.time * data.Frequency);
					camera.Transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num3 * data.XAmplitude * strength), camera.Transform.right) * Quaternion.AngleAxis(num3 * data.ZAmplitude, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
				case BobMode.Fly: {
					var num4 = Mathf.Sin(Time.time * data.Frequency) * data.ZAmplitude;
					camera.Transform.rotation = Quaternion.AngleAxis(num4, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
				case BobMode.Swim: {
					var num5 = Mathf.Sin(Time.time * data.Frequency) * data.ZAmplitude;
					camera.Transform.rotation = Quaternion.AngleAxis(num5, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
				case BobMode.Crouch: {
					var num6 = Mathf.Sin(Time.time * data.Frequency);
					camera.Transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num6 * data.XAmplitude), camera.Transform.right) * Quaternion.AngleAxis(num6 * data.ZAmplitude, camera.Transform.forward) * camera.Transform.rotation;

					break;
				}
			}

			strength = Mathf.Clamp01(strength + Time.deltaTime);
		}

		private struct BobData {
			private float _xAmplitude;
			private float _zAmplitude;
			private float _frequency;

			public float XAmplitude {
				get { return _xAmplitude; }
			}

			public float ZAmplitude {
				get { return _zAmplitude; }
			}

			public float Frequency {
				get { return _frequency; }
			}

			public BobData(float xamp, float zamp, float freq) {
				_xAmplitude = xamp;
				_zAmplitude = zamp;
				_frequency = freq;
			}
		}
	}

	public class CameraFeedbackData {
		public float angle;
		public float noise = 0.5f;
		public float strength = 0.3f;
		public float timeToEnd = 0.15f;
		public float timeToPeak = 0.2f;
	}

	public class CameraFeedback {
		private float _angle;
		private float _currentNoise;
		public float angle;
		public Vector3 direction;
		public float noise;
		public Vector3 rotationAxis;
		private Vector3 shakePos;
		public float strength;
		public float time;
		public float timeToEnd;
		public float timeToPeak;
		private Transform transform;

		public float DebugAngle {
			get { return _angle; }
		}

		public float Duration {
			get { return timeToPeak + timeToEnd; }
		}

		public CameraFeedback(Transform transform) {
			this.transform = transform;
		}

		public void HandleFeedback() {
			if (Duration == 0f) {
				return;
			}

			var num = UnityEngine.Random.Range(-noise, noise);

			if (time < Duration) {
				float num2;

				if (time < timeToPeak) {
					num2 = strength * Mathf.Sin(time * 3.1415927f * 0.5f / timeToPeak);
					_angle = Mathf.Lerp(0f, angle, time / timeToPeak);
				} else {
					var num3 = (time - timeToPeak) / timeToEnd;
					num2 = strength * Mathf.Cos((time - timeToPeak) * 3.1415927f * 0.5f / timeToEnd);
					_angle = Mathf.Lerp(_angle, 0f, num3);

					if (time != 0f) {
						num = 0f;
					}
				}

				_currentNoise = Mathf.Lerp(noise, 0f, time / Duration);
				shakePos = Vector3.Lerp(shakePos, UnityEngine.Random.insideUnitSphere * _currentNoise, Time.deltaTime * 30f);
				time += Time.deltaTime;
				transform.position += num2 * direction;
				transform.rotation = transform.rotation * Quaternion.AngleAxis(_angle, rotationAxis) * Quaternion.AngleAxis(num, transform.forward);
			} else {
				time = 0f;
				timeToEnd = 0f;
				timeToPeak = 0f;
				_angle = 0f;
			}
		}

		public void Reset() {
			_angle = 0f;
			time = 0f;
			timeToEnd = 0f;
			timeToPeak = 0f;
		}
	}

	public class CameraZoomData {
		private float alpha;
		private LevelCamera levelCamera;
		public float Speed;
		public float TargetAlpha;
		public float TargetFOV;

		public bool IsFovChanged {
			get { return TargetFOV != FieldOfView; }
		}

		public CameraZoomData(LevelCamera levelCamera) {
			this.levelCamera = levelCamera;
		}

		public void Update() {
			alpha = Mathf.Lerp(alpha, TargetAlpha, Time.deltaTime * Speed);

			if (levelCamera.MainCamera) {
				levelCamera.MainCamera.fieldOfView = Mathf.Lerp(levelCamera.MainCamera.fieldOfView, TargetFOV, Time.deltaTime * Speed);
			}
		}

		public void ResetZoom() {
			if (levelCamera.MainCamera) {
				TargetFOV = 75f;
				levelCamera.MainCamera.fieldOfView = 75f;
			}
		}
	}

	public abstract class CameraState {
		protected LevelCamera camera;
		protected Vector3 offset;

		protected CameraState(LevelCamera camera, Vector3 offset) {
			this.camera = camera;
			this.offset = offset;
		}

		protected Vector3 LookAtPosition(Transform target, Quaternion lookRot, float distance) {
			var vector = lookRot * Vector3.back * distance;

			return target.TransformPoint(vector) + offset;
		}

		public abstract void Update();
		public virtual void Finish() { }
		public virtual void OnDrawGizmos() { }
	}

	private class DisabledState : CameraState {
		public DisabledState(LevelCamera camera, Vector3 offset) : base(camera, offset) { }
		public override void Update() { }
	}

	private class FirstPersonState : CameraState {
		private const float _duration = 1f;
		private bool _handleFeedback = true;

		public FirstPersonState(LevelCamera camera, Vector3 offset) : base(camera, offset) {
			if (camera.TargetTransform != null) {
				camera.TargetTransform.localRotation = UserInput.Rotation;
			}
		}

		public override void Update() {
			if (camera.TargetTransform == null) {
				return;
			}

			var vector = camera.TargetTransform.position + offset;
			camera.Transform.position = vector;
			camera.Transform.rotation = camera.TargetTransform.rotation;

			if (_handleFeedback) {
				camera.Feedback.HandleFeedback();
			}

			if (camera.ZoomData.IsFovChanged) {
				camera.ZoomData.Update();
			}
		}

		public override void Finish() {
			camera.ZoomData.ResetZoom();
		}

		public override void OnDrawGizmos() {
			Gizmos.DrawRay(camera.Transform.position, camera.Transform.TransformDirection(camera.Feedback.rotationAxis));
		}

		public override string ToString() {
			return "FPS state";
		}
	}

	private class SmoothFollowState : CameraState {
		private const float _zoomSpeed = 40f;
		private float _collideDistance;
		private float _distance = 1.5f;
		private Quaternion _targetRotationY = Quaternion.identity;

		public SmoothFollowState(LevelCamera camera, Vector3 offset) : base(camera, offset) {
			_collideDistance = _distance / 2f;
			camera.InitUserInput();
		}

		public override void Update() {
			if (camera.TargetTransform != null) {
				var num = AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.NextWeapon);

				if (num != 0f) {
					_distance -= Mathf.Sign(num) * 40f * Time.deltaTime;
					_distance = Mathf.Clamp(_distance, 1f, 4f);
				}

				var eulerAngles = camera.TargetTransform.eulerAngles;
				_targetRotationY = Quaternion.Lerp(_targetRotationY, Quaternion.Euler(0f, eulerAngles.y, 0f), Time.deltaTime * 2f);
				var vector = camera.TargetTransform.position + offset;
				camera.UpdateUserInput();
				camera.TransformFollowCamera(vector, _targetRotationY, _distance, ref _collideDistance);
			}
		}

		public override string ToString() {
			return "Smooth follow state";
		}
	}

	private class OrbitAroundState : CameraState {
		private Vector2 _angle;
		private CameraCollisionDetector _ccd;
		private float _collideDistance;
		private float _distance = 2.5f;

		public OrbitAroundState(LevelCamera camera, Vector3 offset) : base(camera, offset) {
			if (camera.TargetTransform == null) {
				throw new NullReferenceException("The OrbitAroundState required a valid _targetTransform. Call LevelCamera.camera.SetTarget() before!");
			}

			_collideDistance = _distance / 2f;
			_ccd = new CameraCollisionDetector();
			_ccd.Offset = 1f;
			_angle.x = (camera.TargetTransform.rotation.eulerAngles.x + 360f) % 360f;
			_angle.y = (camera.TargetTransform.rotation.eulerAngles.y + camera.RotationOffset + 360f) % 360f;
			camera.TargetTransform.rotation = Quaternion.identity;

			if (_angle.x > 70f && _angle.x < 91f) {
				_angle.x = 70f;
			}

			if (_angle.x > 269f && _angle.x < 290f) {
				_angle.x = 290f;
			}
		}

		public override void Update() {
			var flag = true;

			if (Input.GetMouseButton(0)) {
				flag = false;
				_angle.x = (_angle.x - AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.VerticalLook) + 360f) % 360f;
				_angle.y = (_angle.y + AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.HorizontalLook)) % 360f;

				if (_angle.x > 70f && _angle.x < 91f) {
					_angle.x = 70f;
				}

				if (_angle.x > 269f && _angle.x < 290f) {
					_angle.x = 290f;
				}
			}

			var quaternion = Quaternion.Euler(_angle.x, _angle.y, 0f);
			var vector = camera.TargetTransform.position + offset;
			var vector2 = LookAtPosition(camera.TargetTransform, quaternion, 1f);

			if (_ccd.Detect(vector, vector2, camera.TargetTransform.right)) {
				if (_ccd.Distance < _collideDistance) {
					_collideDistance = Mathf.Clamp(_ccd.Distance, 0.5f, _distance);
				} else {
					_collideDistance = Mathf.Lerp(_collideDistance, _ccd.Distance, Time.deltaTime);
				}
			} else {
				_collideDistance = Mathf.Lerp(_collideDistance, _distance, Time.deltaTime);
			}

			if (flag) {
				camera.Transform.position = Vector3.Lerp(camera.Transform.position, LookAtPosition(camera.TargetTransform, quaternion, _collideDistance), Time.deltaTime * 5f);
				camera.Transform.rotation = Quaternion.Slerp(camera.Transform.rotation, Quaternion.LookRotation(vector - camera.Transform.position), Time.deltaTime * 5f);
			} else {
				camera.Transform.position = LookAtPosition(camera.TargetTransform, quaternion, _collideDistance);
				camera.Transform.rotation = Quaternion.LookRotation(vector - camera.Transform.position);
			}
		}
	}

	private class RagdollState : CameraState {
		private const float extraHeight = 1.5f;
		private Vector3 targetPosition;

		public RagdollState(LevelCamera camera, Vector3 offset) : base(camera, offset) {
			targetPosition = GameState.Current.Avatar.Ragdoll.GetBone(BoneIndex.Hips).position + new Vector3(0f, 1.5f, 0f);
		}

		public override void Update() {
			camera.Transform.position = Vector3.Lerp(camera.Transform.position, targetPosition, Time.deltaTime);
			camera.Transform.LookAt(camera.TargetTransform);
		}
	}

	private class SpectatorState : CameraState {
		private const int MaxSpeed = 22;
		private const float verticalSpeed = 0.8f;
		private float _speed = 11f;
		private Vector3 _targetPosition;

		public SpectatorState(LevelCamera camera, Vector3 offset) : base(camera, offset) {
			_targetPosition = camera.Transform.position;
		}

		public override void Update() {
			if (!GameData.Instance.HUDChatIsTyping && Screen.lockCursor) {
				var num = ((!UserInput.IsWalking) ? 4 : 6);
				_speed = Mathf.Lerp(_speed, (!UserInput.IsWalking) ? 11 : 22, Time.deltaTime);
				_targetPosition += (UserInput.Rotation * UserInput.HorizontalDirection + UserInput.VerticalDirection * 0.8f) * _speed * Time.deltaTime;
				camera.Transform.position = Vector3.Lerp(camera.Transform.position, _targetPosition, Time.deltaTime * num);
				camera.Transform.rotation = UserInput.Rotation;
			}
		}
	}
}
