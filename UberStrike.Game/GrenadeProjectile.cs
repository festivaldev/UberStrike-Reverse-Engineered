using UnityEngine;

public class GrenadeProjectile : Projectile {
	public bool Sticky { get; set; }

	protected override void Start() {
		base.Start();

		if (Detonator != null) {
			Detonator.Direction = Vector3.zero;
		}

		Rigidbody.useGravity = true;
		Rigidbody.AddRelativeTorque(UnityEngine.Random.insideUnitSphere.normalized * 10f);
	}

	protected override void OnTriggerEnter(Collider c) {
		if (!IsProjectileExploded) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				Singleton<ProjectileManager>.Instance.RemoveProjectile(ID);
				GameState.Current.Actions.RemoveProjectile(ID, true);
			}

			PlayBounceSound(c.transform.position);
		}
	}

	protected override void OnCollisionEnter(Collision c) {
		if (!IsProjectileExploded) {
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer)) {
				Singleton<ProjectileManager>.Instance.RemoveProjectile(ID);
				GameState.Current.Actions.RemoveProjectile(ID, true);
			} else if (Sticky) {
				Rigidbody.isKinematic = true;
				collider.isTrigger = true;

				if (c.contacts.Length > 0) {
					transform.position = c.contacts[0].point + c.contacts[0].normal * collider.bounds.extents.sqrMagnitude;
				}
			}

			PlayBounceSound(c.transform.position);
		}
	}
}
