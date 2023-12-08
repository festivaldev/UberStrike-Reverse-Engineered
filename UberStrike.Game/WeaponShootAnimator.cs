using UnityEngine;

public class WeaponShootAnimator : BaseWeaponEffect {
	private static readonly int SHOOT_STATE = Animator.StringToHash("Base Layer.Shoot");
	private static readonly int SHOOT_PARAM = Animator.StringToHash("Shoot");
	private bool _IsShooting;

	[SerializeField]
	private Animator _weaponAnimator;

	private void Awake() { }
	private void Start() { }

	private void OnEnable() {
		if (_weaponAnimator) {
			_weaponAnimator.SetBool(SHOOT_PARAM, false);
			_IsShooting = false;
		}
	}

	public override void OnShoot() {
		if (_weaponAnimator) {
			_weaponAnimator.SetBool(SHOOT_PARAM, true);
			_IsShooting = true;
		}
	}

	public void FixedUpdate() {
		if (_weaponAnimator && _IsShooting && _weaponAnimator.GetCurrentAnimatorStateInfo(0).nameHash == SHOOT_STATE && !_weaponAnimator.IsInTransition(0)) {
			_weaponAnimator.SetBool(SHOOT_PARAM, false);
			_IsShooting = false;
		}
	}

	public override void OnPostShoot() { }
	public override void OnHits(RaycastHit[] hits) { }

	public override void Hide() {
		if (_weaponAnimator) {
			_weaponAnimator.SetBool(SHOOT_PARAM, false);
			_IsShooting = false;
		}
	}
}
