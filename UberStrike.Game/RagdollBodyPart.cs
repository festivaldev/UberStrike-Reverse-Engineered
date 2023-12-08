using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollBodyPart : BaseGameProp, IShootable {
	public override void ApplyDamage(DamageInfo damageInfo) {
		var vector = damageInfo.Force * 0.5f;
		vector += new Vector3(0f, damageInfo.UpwardsForceMultiplier, 0f);
		rigidbody.AddForce(vector, ForceMode.Impulse);
	}
}
