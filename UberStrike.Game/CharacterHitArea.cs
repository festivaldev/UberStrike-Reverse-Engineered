using UberStrike.Core.Models;
using UnityEngine;

public class CharacterHitArea : BaseGameProp {
	[SerializeField]
	private BodyPart _part;

	public IShootable Shootable { get; set; }

	public override bool IsLocal {
		get { return Shootable != null && Shootable.IsLocal; }
	}

	public override void ApplyDamage(DamageInfo shot) {
		shot.BodyPart = _part;

		if (Shootable != null) {
			if (Shootable.IsVulnerable) {
				if (_part == BodyPart.Head || _part == BodyPart.Nuts) {
					shot.Damage += (short)(shot.Damage * shot.CriticalStrikeBonus);
				}

				Shootable.ApplyDamage(shot);
			}
		} else {
			Debug.LogError("No character set to the body part!");
		}
	}
}
