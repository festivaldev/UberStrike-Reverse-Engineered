using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AvatarAnimationController : MonoBehaviour {
	private const float IK_FADE_IN_SPEED = 10f;
	private const float IK_FADE_OUT_SPEED = 15f;
	private const int TURN_THRESHOLD = 45;

	public enum AnimationLayer {
		Base,
		Weapons,
		Shop,
		Dance
	}

	private Transform _AnchorChest;
	private Transform _IKAnchor;
	private Transform _IKLeftHand;
	private Transform _IKRightHand;
	private float _IKWeight;
	private float _LookAtWeight;
	private int animationLayerMask = 6;
	private int gearTrigger;
	private bool jumpTrigger;
	private float nextRandomUpdate;
	private bool shootTrigger;
	private ICharacterState state;
	private float turnAround;
	private bool weaponSwitch;
	public Animator Animator { get; private set; }

	private void Awake() {
		Animator = GetComponent<Animator>();
	}

	private void OnEnable() {
		_AnchorChest = transform.Find("Hips/Spine/Chest/Anchor_Chest");
		_IKAnchor = transform.Find("IK_Anchor");

		if (_IKAnchor) {
			_IKRightHand = _IKAnchor.transform.Find("IK_Hand_R");
			_IKLeftHand = _IKAnchor.transform.Find("IK_Hand_R/IK_Hand_L");
		}
	}

	public void SetCharacter(ICharacterState state) {
		this.state = state;
	}

	public void Jump() {
		jumpTrigger = true;
	}

	public void Shoot() {
		shootTrigger = true;
	}

	public bool IsLayerEnabled(AnimationLayer layer) {
		return (animationLayerMask & (1 << (int)layer)) != 0;
	}

	public void EnableLayer(AnimationLayer layer, bool enable) {
		if (enable) {
			animationLayerMask |= 1 << (int)layer;
		} else {
			animationLayerMask &= ~(1 << (int)layer);
		}
	}

	private void Update() {
		Animator.SetInteger(ControlFields.GearType, gearTrigger);

		if (state != null) {
			var num = Vector3.Magnitude(new Vector3(state.Velocity.x, 0f, state.Velocity.z));
			var flag = false;
			var flag2 = false;

			if (Mathf.DeltaAngle(state.HorizontalRotation.eulerAngles.y, turnAround) > 45f) {
				flag = true;
				turnAround = state.HorizontalRotation.eulerAngles.y;
			} else if (Mathf.DeltaAngle(state.HorizontalRotation.eulerAngles.y, turnAround) < -45f) {
				flag2 = true;
				turnAround = state.HorizontalRotation.eulerAngles.y;
			}

			var vector = Quaternion.Inverse(state.HorizontalRotation) * state.Velocity;

			if (state.KeyState != KeyState.Still) {
				var zero = Vector3.zero;
				var num2 = 0f;

				if ((byte)(state.KeyState & KeyState.Forward) != 0) {
					zero.z += 1f;
				}

				if ((byte)(state.KeyState & KeyState.Backward) != 0) {
					zero.z -= 1f;
				}

				if ((byte)(state.KeyState & KeyState.Left) != 0) {
					zero.x += 1f;
				}

				if ((byte)(state.KeyState & KeyState.Right) != 0) {
					zero.x -= 1f;
				}

				zero.Normalize();

				if (zero.magnitude > 0f) {
					num2 = Quaternion.LookRotation(zero).eulerAngles.y;
				}

				Animator.SetFloat(ControlFields.Direction, num2, 0.2f, Time.fixedDeltaTime);
			}

			Animator.SetFloat(ControlFields.WalkingSpeed, num);
			Animator.SetFloat(ControlFields.SpeedZ, vector.z);
			Animator.SetFloat(ControlFields.SpeedX, vector.x);
			Animator.SetFloat(ControlFields.TurnAround, turnAround);
			Animator.SetBool(ControlFields.IsShooting, state.Player.IsFiring || shootTrigger);
			Animator.SetBool(ControlFields.IsGrounded, (byte)(state.MovementState & MoveStates.Grounded) != 0);
			Animator.SetBool(ControlFields.IsJumping, jumpTrigger);
			Animator.SetBool(ControlFields.IsPaused, state.Player.Is(PlayerStates.Paused));
			Animator.SetBool(ControlFields.IsSquatting, state.Is(MoveStates.Ducked));
			Animator.SetBool(ControlFields.IsWalking, (byte)(state.KeyState & KeyState.Walking) != 0);
			Animator.SetBool(ControlFields.IsSwimming, (byte)(state.MovementState & (MoveStates.Swimming | MoveStates.Diving)) != 0);
			Animator.SetBool(ControlFields.IsTurningLeft, flag);
			Animator.SetBool(ControlFields.IsTurningRight, flag2);
			var num3 = state.VerticalRotation;

			if (num3 > 180f) {
				num3 -= 360f;
			}

			num3 = Mathf.Clamp(num3, -70f, 70f);
			var localEulerAngles = _IKAnchor.transform.localEulerAngles;
			localEulerAngles.x = num3;
			_IKAnchor.transform.localEulerAngles = localEulerAngles;
		}

		EnableLayer(AnimationLayer.Shop, !GameState.Current.IsMultiplayer);

		if (!GameState.Current.IsMultiplayer && !Animator.GetCurrentAnimatorStateInfo(2).IsTag("ShopIdle")) {
			EnableLayer(AnimationLayer.Weapons, false);
		} else {
			EnableLayer(AnimationLayer.Weapons, true);
		}

		UpdateLayerWeight(AnimationLayer.Weapons, true);
		UpdateLayerWeight(AnimationLayer.Shop);
		shootTrigger = false;
		jumpTrigger = false;
		gearTrigger = 0;
		Animator.SetBool(ControlFields.WeaponSwitch, weaponSwitch);

		if (weaponSwitch) {
			weaponSwitch = false;
		}
	}

	private void OnAnimatorIK() {
		if (_AnchorChest && _IKAnchor) {
			_IKAnchor.transform.position = _AnchorChest.transform.position;
		}

		if (_IKLeftHand && _IKRightHand) {
			var flag = Animator.GetCurrentAnimatorStateInfo(1).IsTag("IK");
			var flag2 = Animator.GetCurrentAnimatorStateInfo(1).IsTag("Melee");
			var flag3 = IsLayerEnabled(AnimationLayer.Weapons);
			var layerWeight = Animator.GetLayerWeight(1);

			if (flag3 && (flag || flag2)) {
				_LookAtWeight = Mathf.Lerp(_LookAtWeight, 1f, Time.deltaTime * 10f);
			} else {
				_LookAtWeight = Mathf.Lerp(_LookAtWeight, 0f, Time.deltaTime * 15f);
			}

			var position = _IKLeftHand.transform.position;
			position.y += 0.2f;
			Animator.SetLookAtPosition(position);
			Animator.SetLookAtWeight(layerWeight * _LookAtWeight);

			if (flag3 && flag) {
				_IKWeight = Mathf.Lerp(_IKWeight, 1f, Time.deltaTime * 10f);
			} else {
				_IKWeight = Mathf.Lerp(_IKWeight, 0f, Time.deltaTime * 15f);
			}

			var num = layerWeight * _IKWeight;
			SetIK(AvatarIKGoal.LeftHand, _IKLeftHand.transform, num);
			SetIK(AvatarIKGoal.RightHand, _IKRightHand.transform, num);
		}
	}

	private void SetIK(AvatarIKGoal goal, Transform goalTransform, float weight) {
		Animator.SetIKPositionWeight(goal, weight);
		Animator.SetIKRotationWeight(goal, weight);
		Animator.SetIKPosition(goal, goalTransform.position);
		Animator.SetIKRotation(goal, goalTransform.rotation);
	}

	private void UpdateLayerWeight(AnimationLayer layer, bool smooth = false) {
		float num = (!IsLayerEnabled(layer)) ? 0 : 1;

		if (smooth) {
			var num2 = Mathf.Lerp(Animator.GetLayerWeight((int)layer), num, Time.deltaTime * 7.5f);
			Animator.SetLayerWeight((int)layer, num2);
		} else {
			Animator.SetLayerWeight((int)layer, num);
		}
	}

	public void TriggerGearAnimation(UberstrikeItemClass itemClass) {
		ChangeWeaponType(0);

		switch (itemClass) {
			case UberstrikeItemClass.GearBoots:
				gearTrigger = 5;

				break;
			case UberstrikeItemClass.GearHead:
			case UberstrikeItemClass.GearFace:
				gearTrigger = 1;

				break;
			case UberstrikeItemClass.GearUpperBody:
			case UberstrikeItemClass.GearHolo:
				gearTrigger = 3;

				break;
			case UberstrikeItemClass.GearLowerBody:
				gearTrigger = 4;

				break;
			case UberstrikeItemClass.GearGloves:
				gearTrigger = 2;

				break;
		}
	}

	public void ChangeWeaponType(UberstrikeItemClass itemClass) {
		if (Animator != null) {
			weaponSwitch = true;

			switch (itemClass) {
				case UberstrikeItemClass.WeaponMelee:
					Animator.SetInteger(ControlFields.WeaponClass, 1);

					return;
				case UberstrikeItemClass.WeaponMachinegun:
				case UberstrikeItemClass.WeaponCannon:
				case UberstrikeItemClass.WeaponSplattergun:
				case UberstrikeItemClass.WeaponLauncher:
					Animator.SetInteger(ControlFields.WeaponClass, 3);

					return;
				case UberstrikeItemClass.WeaponShotgun:
					Animator.SetInteger(ControlFields.WeaponClass, 4);

					return;
				case UberstrikeItemClass.WeaponSniperRifle:
					Animator.SetInteger(ControlFields.WeaponClass, 2);

					return;
			}

			Animator.SetInteger(ControlFields.WeaponClass, 0);
		}
	}

	private class ControlFields {
		public static readonly int SpeedZ = Animator.StringToHash("SpeedZ");
		public static readonly int SpeedX = Animator.StringToHash("SpeedX");
		public static readonly int IsSquatting = Animator.StringToHash("IsSquatting");
		public static readonly int IsPaused = Animator.StringToHash("IsPaused");
		public static readonly int WalkingSpeed = Animator.StringToHash("WalkingSpeed");
		public static readonly int TurnAround = Animator.StringToHash("TurnAround");
		public static readonly int IsSwimming = Animator.StringToHash("IsSwimming");
		public static readonly int IsWalking = Animator.StringToHash("IsWalking");
		public static readonly int IsJumping = Animator.StringToHash("IsJumping");
		public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
		public static readonly int Direction = Animator.StringToHash("Direction");
		public static readonly int IsShooting = Animator.StringToHash("IsShooting");
		public static readonly int WeaponClass = Animator.StringToHash("WeaponClass");
		public static readonly int WeaponSwitch = Animator.StringToHash("WeaponSwitch");
		public static readonly int IsTurningLeft = Animator.StringToHash("IsTurningLeft");
		public static readonly int IsTurningRight = Animator.StringToHash("IsTurningRight");
		public static readonly int Random = Animator.StringToHash("Random");
		public static readonly int GearType = Animator.StringToHash("GearType");
		public static readonly int IsDance = Animator.StringToHash("IsDance");
	}

	private class AnimationStates {
		public static readonly int Shooting = Animator.StringToHash("Weapons.Shooting");
		public static readonly int Jump = Animator.StringToHash("Base.Jumping.Jump");
		public static readonly int Idle = Animator.StringToHash("Base.Idle");
	}
}
