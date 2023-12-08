using System;
using UberStrike.Core.Models;
using UnityEngine;

public class CharacterMoveController {
	public const float HEIGHT_NORMAL = 2f;
	public const float CENTER_OFFSET_NORMAL = 0f;
	public const float HEIGHT_DUCKED = 1f;
	public const float CENTER_OFFSET_DUCKED = -0.5f;
	public const float POWERUP_HASTE_SCALE = 1.3f;
	public const float PLAYER_WADE_SCALE = 0.2f;
	public const float PLAYER_SWIM_SCALE = 0.4f;
	public const float PLAYER_DUCK_SCALE = 0.23f;
	public const float PLAYER_TERMINAL_GRAVITY = -100f;
	public const float PLAYER_INITIAL_GRAVITY = -1f;
	public const float PLAYER_ZOOM_SLOWDOWN = 1.8f;
	public const float PLAYER_MIN_SCALE = 0.5f;
	public const float JumpPadModifier = 0.035f;
	public const float PlayerWalkSpeed = 7.6f;
	public const float PlayerJumpSpeed = 15f;
	public const float StrafeJumpMultiplier = 3f;

	public enum ForceType {
		None,
		Additive,
		Exclusive
	}

	private readonly CharacterController _controller;
	private readonly Transform _transform;
	private Vector3 _acceleration;
	private bool _canJump = true;
	private CollisionFlags _collisionFlag;
	private EnviromentSettings _currentEnviroment;
	private Vector3 _externalForce;
	private float _externalForceTime;
	private ForceType _externalForceType = ForceType.Additive;
	private bool _hasExternalForce;
	private bool _isOnLatter;
	private int _ungroundedCount;
	public Vector3 _velocity;
	private float _waterEnclosure;
	private float dynamicDamp = 0.976f;
	private float initialDamp = 0.035f;
	public bool IsLowGravity;
	private bool useNewMethod;
	public bool IsJumpDisabled { get; set; }

	public float PlayerHeight {
		get { return (!GameState.Current.PlayerData.Is(MoveStates.Ducked)) ? 2f : 1f; }
	}

	public MovingPlatform Platform { get; set; }

	public Vector3 Velocity {
		get { return _velocity; }
	}

	public int WaterLevel { get; private set; }
	public bool IsGrounded { get; private set; }

	public Bounds DebugEnvBounds {
		get { return _currentEnviroment.EnviromentBounds; }
	}

	private float Gravity {
		get { return ((!IsLowGravity) ? 1f : 0.4f) * _currentEnviroment.Gravity * Time.deltaTime; }
	}

	public CharacterMoveController(CharacterController controller) {
		_controller = controller;
		_transform = _controller.transform;
		EventHandler.Global.AddListener(new Action<GlobalEvents.InputChanged>(OnInputChanged));
	}

	public event Action<float> CharacterLanded;

	public void Init() {
		if (LevelEnviroment.Instance != null) {
			_currentEnviroment = LevelEnviroment.Instance.Settings;
		} else {
			Debug.LogWarning("You are trying to access the LevelEnvironment Instance that has not had Awake called.");
		}
	}

	public void Start() {
		Reset();
	}

	public void UpdatePlayerMovement() {
		UpdateMovementStates();
		UpdateMovement();
	}

	public void ResetDuckMode() {
		_controller.height = 2f;
		_controller.center = new Vector3(0f, 0f, 0f);
	}

	public static bool HasCollision(Vector3 pos, float height) {
		return Physics.CheckSphere(pos + Vector3.up * (height - 0.5f), 0.6f, UberstrikeLayerMasks.CrouchMask);
	}

	public void ResetEnviroment() {
		_currentEnviroment = LevelEnviroment.Instance.Settings;
		_currentEnviroment.EnviromentBounds = default(Bounds);
		_isOnLatter = false;
	}

	public void SetEnviroment(EnviromentSettings settings, Bounds bounds) {
		_currentEnviroment = settings;
		_currentEnviroment.EnviromentBounds = new Bounds(bounds.center, bounds.size);
		_isOnLatter = _currentEnviroment.Type == EnviromentSettings.TYPE.LATTER;
	}

	public float GetSpeedModifier() {
		var num = 0f;
		var num2 = ((!Singleton<WeaponController>.Instance.IsSecondaryAction) ? 0f : 1.8f);
		var num3 = Singleton<PlayerDataManager>.Instance.GearWeight * 1.2f;
		var num4 = ((!(GameState.Current.Player != null)) ? 0f : (GameState.Current.Player.DamageFactor * 7.6f));
		num += num2;
		num += num3;
		num += num4;

		if (WaterLevel > 0) {
			if (WaterLevel == 3) {
				num += 3.04f;
			} else {
				num += 1.52f;
			}
		} else if (IsGrounded && GameState.Current.PlayerData.Is(MoveStates.Ducked)) {
			num += 1.748f;
		}

		return num;
	}

	private void UpdateMovementStates() {
		if (_currentEnviroment.Type == EnviromentSettings.TYPE.LATTER && !_currentEnviroment.EnviromentBounds.Intersects(_controller.bounds)) {
			ResetEnviroment();
		}

		if (_currentEnviroment.Type == EnviromentSettings.TYPE.WATER) {
			_currentEnviroment.CheckPlayerEnclosure(GetFeetPosition(), PlayerHeight, out _waterEnclosure);
			var num = 1;

			if (_waterEnclosure >= 0.8f) {
				num = 3;
			} else if (_waterEnclosure >= 0.4f) {
				num = 2;
			}

			if (WaterLevel != num) {
				SetWaterlevel(num);
			}
		} else if (WaterLevel != 0) {
			SetWaterlevel(0);
		}

		if ((byte)(GameState.Current.PlayerData.KeyState & KeyState.Jump) == 0) {
			_canJump = true;
		}
	}

	private Vector3 GetFeetPosition() {
		return _transform.position - new Vector3(0f, 1f, 0f);
	}

	public void ApplyForce(Vector3 v, ForceType type) {
		if (useNewMethod) {
			_externalForce = v * 0.035f * initialDamp;
		} else {
			_externalForce = v * 0.035f;
		}

		_externalForceType = type;
		_hasExternalForce = true;
		_externalForceTime = Time.realtimeSinceStartup + 4f;
	}

	private void UpdateMovement() {
		CheckDuck();

		if (GameState.Current.PlayerData.Is(MoveStates.Flying)) {
			FlyInAir();
		} else if (WaterLevel > 2) {
			MoveInWater();
		} else if (_isOnLatter) {
			MoveOnLadder();
		} else if (IsGrounded) {
			MoveOnGround();
		} else if (WaterLevel == 2) {
			MoveOnWaterRim();
		} else {
			MoveInAir();
		}

		if (_hasExternalForce) {
			if (useNewMethod) {
				if (_externalForceType != ForceType.None) {
					_velocity = Vector3.zero;
					_canJump = false;
					_ungroundedCount = 6;
					GameState.Current.PlayerData.JumpingUpdate();
				}

				_velocity += _externalForce;
				_externalForce *= dynamicDamp;
				_hasExternalForce = _externalForce.sqrMagnitude > 0.01f;
				_externalForceType = ForceType.None;
			} else {
				var externalForceType = _externalForceType;

				if (externalForceType != ForceType.Additive) {
					if (externalForceType == ForceType.Exclusive) {
						_velocity = _externalForce;
					}
				} else {
					_velocity = Vector3.Scale(_velocity, new Vector3(1f, 0.5f, 1f)) + _externalForce;
				}

				Jump(_velocity.y);
				_externalForce = Vector2.zero;
				_hasExternalForce = false;
			}
		}

		_velocity[1] = Mathf.Clamp(_velocity[1], -150f, 150f);
		_collisionFlag = _controller.Move(_velocity * Time.fixedDeltaTime);
		_velocity = _controller.velocity;
		var flag = (_collisionFlag & CollisionFlags.Below) != CollisionFlags.None;

		if (flag) {
			_externalForceTime = 0f;
		}

		if (_externalForceTime < Time.realtimeSinceStartup) {
			var vector = ClampHorizontally(_velocity, 22.8f);
			_velocity = Vector3.Lerp(_velocity, vector, Time.fixedDeltaTime * 3f);
		}

		GameState.Current.PlayerData.Velocity = _velocity;

		if (flag) {
			if (_ungroundedCount > 5 && CharacterLanded != null) {
				CharacterLanded(_velocity.y);
				GameState.Current.PlayerData.LandingUpdate();
			}

			_ungroundedCount = 0;
			IsGrounded = true;
		} else if (!_canJump) {
			_ungroundedCount++;
			IsGrounded = false;
		} else if (_ungroundedCount > 5) {
			IsGrounded = false;
		} else {
			_ungroundedCount++;
			IsGrounded = true;
		}

		GameState.Current.PlayerData.Set(MoveStates.Grounded, IsGrounded);

		if (IsGrounded) {
			GameState.Current.PlayerData.Set(MoveStates.Jumping, false);
		}
	}

	private Vector3 ClampHorizontally(Vector3 vector, float magnitudeMax) {
		var y = vector.y;
		vector.y = 0f;
		vector = vector.normalized * Mathf.Clamp(vector.magnitude, 0f, magnitudeMax);
		vector.y = y;

		return vector;
	}

	private void OnInputChanged(GlobalEvents.InputChanged ev) {
		if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled && GameState.Current.Player != null && GameState.Current.Player.IsWalkingEnabled) {
			GameState.Current.PlayerData.Set(UserInput.GetkeyState(ev.Key), ev.IsDown);
		}
	}

	private void Reset() {
		SetWaterlevel(0);
		_velocity = Vector3.zero;
		_externalForceType = ForceType.Additive;
		_hasExternalForce = false;
		_externalForce = Vector3.zero;
		_canJump = true;
		IsGrounded = true;
		_ungroundedCount = 0;
		Platform = null;
		IsJumpDisabled = false;
	}

	private void ApplyFriction() {
		var velocity = _velocity;
		var magnitude = velocity.magnitude;

		if (magnitude == 0f) {
			return;
		}

		if (magnitude < 0.5f && _acceleration.sqrMagnitude == 0f) {
			if (_isOnLatter) {
				_velocity[1] = 0f;
			}

			_velocity[0] = 0f;
			_velocity[2] = 0f;
		} else {
			var num = 0f;

			if (WaterLevel < 3) {
				if (_isOnLatter || GameState.Current.PlayerData.Is(MoveStates.Grounded)) {
					var num2 = Mathf.Max(_currentEnviroment.StopSpeed, magnitude);
					num += num2 * _currentEnviroment.GroundFriction;
				}
			} else if (WaterLevel > 0) {
				num += Mathf.Max(_currentEnviroment.StopSpeed, magnitude) * _currentEnviroment.WaterFriction * WaterLevel / 3f;
			}

			if (GameState.Current.PlayerData.Is(MoveStates.Flying)) {
				var num2 = Mathf.Max(_currentEnviroment.StopSpeed, magnitude);
				num += num2 * _currentEnviroment.FlyFriction;
			}

			num *= Time.deltaTime;
			var num3 = magnitude - num;

			if (num3 < 0f) {
				num3 = 0f;
			}

			num3 /= magnitude;
			_velocity *= num3;
		}
	}

	private void ApplyAcceleration(Vector3 wishdir, float wishspeed, float accel, bool clamp = false) {
		var num = Vector3.Dot(_velocity, wishdir);
		var num2 = wishspeed - num;

		if (num2 <= 0f) {
			_acceleration = Vector3.zero;

			return;
		}

		_acceleration = accel * wishspeed * wishdir * Time.deltaTime;

		if (useNewMethod && _hasExternalForce) {
			_acceleration *= 0.1f;
		}

		var vector = _velocity + _acceleration;
		var magnitude = vector.magnitude;

		if (magnitude < wishspeed) {
			_velocity += _acceleration;
		} else if (clamp) {
			_velocity = (_velocity + _acceleration).normalized * wishspeed;
			_velocity[1] = vector[1];
		} else {
			_velocity = (_velocity + _acceleration).normalized * magnitude;
		}
	}

	private void CheckDuck() {
		if (WaterLevel < 3 && GameState.Current.PlayerData.Is(MoveStates.Grounded)) {
			if (UserInput.IsPressed(KeyState.Crouch) && !GameState.Current.PlayerData.Is(MoveStates.Ducked)) {
				GameState.Current.PlayerData.Set(MoveStates.Ducked, true);
				_controller.height = 1f;
				_controller.center = new Vector3(0f, -0.5f, 0f);
			} else if (!UserInput.IsPressed(KeyState.Crouch) && GameState.Current.PlayerData.Is(MoveStates.Ducked) && !HasCollision(GetFeetPosition(), 2f)) {
				GameState.Current.PlayerData.Set(MoveStates.Ducked, false);
				_controller.height = 2f;
				_controller.center = new Vector3(0f, 0f, 0f);
			}
		}
	}

	private void Jump(float up) {
		_canJump = false;
		_ungroundedCount = 6;
		_velocity.y = up;
		GameState.Current.PlayerData.JumpingUpdate();
	}

	private bool CheckJump() {
		if (IsJumpDisabled || GameState.Current.PlayerData.Is(MoveStates.Ducked) || (byte)(GameState.Current.PlayerData.KeyState & KeyState.Jump) == 0) {
			return false;
		}

		if (_isOnLatter) {
			return true;
		}

		if (_canJump) {
			Jump(15f);

			return true;
		}

		UserInput.HorizontalDirection.y = 0f;

		return false;
	}

	private bool CheckWaterJump() {
		if ((byte)(GameState.Current.PlayerData.KeyState & KeyState.Jump) == 0 || (_collisionFlag & CollisionFlags.Sides) == CollisionFlags.None) {
			return false;
		}

		if (!_canJump) {
			UserInput.HorizontalDirection.y = 0f;

			return false;
		}

		_velocity.y = 15f;

		return true;
	}

	private void FlyInAir() {
		ApplyFriction();
		var vector = Vector3.zero;

		if (UserInput.IsWalking) {
			vector = UserInput.Rotation * UserInput.HorizontalDirection;
		}

		if (UserInput.VerticalDirection.y != 0f) {
			vector.y = UserInput.VerticalDirection.y;
		}

		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.FlyAcceleration);
	}

	private void MoveInWater() {
		ApplyFriction();
		var vector = Vector3.zero;

		if (UserInput.IsWalking) {
			vector = UserInput.Rotation * UserInput.HorizontalDirection;
		}

		if (UserInput.IsMovingVertically) {
			vector.y = UserInput.VerticalDirection.y;
		}

		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.WaterAcceleration);

		if (_velocity[1] > -3f) {
			ref var ptr = ref _velocity;
			int num2;
			var num = (num2 = 1);
			var num3 = ptr[num2];
			_velocity[num] = num3 - Gravity * 0.1f;
		} else {
			_velocity[1] = Mathf.Lerp(_velocity[1], -3f, Time.deltaTime * 6f);
		}
	}

	private void MoveOnLadder() {
		ApplyFriction();
		var vector = Vector3.zero;

		if (UserInput.IsWalking) {
			vector = UserInput.Rotation * UserInput.HorizontalDirection;
		}

		if (UserInput.IsMovingVertically) {
			vector.y = UserInput.VerticalDirection.y;
		}

		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.GroundAcceleration);
	}

	private void MoveOnWaterRim() {
		ApplyFriction();
		var vector = Vector3.zero;

		if (UserInput.IsWalking) {
			vector = UserInput.Rotation * UserInput.HorizontalDirection;
		}

		if (UserInput.IsMovingDown) {
			vector.y = UserInput.VerticalDirection.y;
		} else if (UserInput.IsMovingUp && _waterEnclosure > 0.8f) {
			vector.y = UserInput.VerticalDirection.y * 0.5f;
		} else {
			vector.y = 0f;
		}

		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.WaterAcceleration, true);

		if (_waterEnclosure < 0.7f || !UserInput.IsMovingVertically) {
			if (_velocity[1] > -3f) {
				ref var ptr = ref _velocity;
				int num2;
				var num = (num2 = 1);
				var num3 = ptr[num2];
				_velocity[num] = num3 - Gravity * 0.1f;
			} else {
				_velocity[1] = Mathf.Lerp(_velocity[1], -3f, Time.deltaTime * 6f);
			}
		} else if (_velocity[1] > 0f && _waterEnclosure < 0.8f) {
			_velocity[1] = Mathf.Lerp(_velocity[1], -1f, Time.deltaTime * 4f);
		}

		CheckWaterJump();
	}

	private void MoveInAir() {
		ApplyFriction();
		var vector = UserInput.Rotation * UserInput.HorizontalDirection;
		vector[1] = 0f;
		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.AirAcceleration);
		ref var ptr = ref _velocity;
		int num2;
		var num = (num2 = 1);
		var num3 = ptr[num2];
		_velocity[num] = num3 - Gravity;
	}

	private void MoveOnGround() {
		if (CheckJump()) {
			if (WaterLevel > 1) {
				MoveInWater();
			} else {
				MoveInAir();
			}

			return;
		}

		ApplyFriction();
		var vector = GameState.Current.PlayerData.HorizontalRotation * UserInput.HorizontalDirection;
		vector[1] = 0f;

		if (vector.sqrMagnitude > 1f) {
			vector.Normalize();
		}

		ApplyAcceleration(vector, 7.6f - GetSpeedModifier(), _currentEnviroment.GroundAcceleration);
		_velocity[1] = -Gravity;
	}

	private void SetWaterlevel(int level) {
		WaterLevel = level;
		GameState.Current.PlayerData.Set(MoveStates.Diving, level == 3);
		GameState.Current.PlayerData.Set(MoveStates.Swimming, level == 2);
		GameState.Current.PlayerData.Set(MoveStates.Wading, level == 1);
	}
}
