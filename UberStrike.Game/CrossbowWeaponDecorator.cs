using UnityEngine;

public class CrossbowWeaponDecorator : BaseWeaponDecorator {
	[SerializeField]
	private ArrowProjectile _arrowProjectile;

	protected override void ShowImpactEffects(RaycastHit hit, Vector3 direction, Vector3 muzzlePosition, float distance, bool playSound) {
		CreateArrow(hit, direction);
		base.ShowImpactEffects(hit, direction, muzzlePosition, distance, playSound);
	}

	private void CreateArrow(RaycastHit hit, Vector3 direction) {
		if (_arrowProjectile && hit.collider != null) {
			var quaternion = default(Quaternion);
			quaternion = Quaternion.FromToRotation(Vector3.back, direction * -1f);
			var arrowProjectile = Instantiate(_arrowProjectile, hit.point, quaternion) as ArrowProjectile;

			if (hit.collider.gameObject.layer == 18) {
				if (GameState.Current.Avatar.Decorator) {
					arrowProjectile.gameObject.transform.parent = GameState.Current.Avatar.Decorator.GetBone(BoneIndex.Hips);

					foreach (var renderer in arrowProjectile.GetComponentsInChildren<Renderer>(true)) {
						renderer.enabled = false;
					}
				}
			} else if (hit.collider.gameObject.layer == 20) {
				arrowProjectile.SetParent(hit.collider.transform);
			}

			arrowProjectile.Destroy(15);
		}
	}
}
