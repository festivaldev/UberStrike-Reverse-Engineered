using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimator : BaseWeaponEffect {
	private Animator _animator;

	private void Awake() {
		_animator = GetComponent<Animator>();
	}

	private IEnumerator StartResetParameters() {
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.1f);

		_animator.SetBool("Shoot", false);
	}

	public override void OnShoot() {
		_animator.SetBool("Shoot", true);
		StartCoroutine(StartResetParameters());
	}

	public override void Hide() { }
	public override void OnPostShoot() { }
	public override void OnHits(RaycastHit[] hits) { }

	private class WeaponCommand {
		public const string Shoot = "Shoot";
	}
}
