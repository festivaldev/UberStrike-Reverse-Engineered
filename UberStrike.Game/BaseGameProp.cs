using UnityEngine;

public class BaseGameProp : MonoBehaviour, IShootable {
	[SerializeField]
	protected bool _recieveProjectileDamage = true;

	public bool RecieveProjectileDamage {
		get { return _recieveProjectileDamage; }
	}

	public virtual bool IsVulnerable {
		get { return true; }
	}

	public virtual bool IsLocal {
		get { return false; }
	}

	public Transform Transform {
		get { return transform; }
	}

	public virtual void ApplyDamage(DamageInfo damageInfo) { }
	public virtual void ApplyForce(Vector3 position, Vector3 force) { }
}
