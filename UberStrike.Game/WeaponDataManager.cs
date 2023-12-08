using UberStrike.Core.Models.Views;
using UnityEngine;

public static class WeaponDataManager {
	public static Vector3 ApplyDispersion(Vector3 shootingRay, UberStrikeItemWeaponView view, bool ironSight) {
		var num = WeaponConfigurationHelper.GetAccuracySpread(view);

		if (WeaponFeedbackManager.Instance && WeaponFeedbackManager.Instance.IsIronSighted && ironSight) {
			num *= 0.5f;
		}

		var vector = UnityEngine.Random.insideUnitCircle * num * 0.5f;

		return Quaternion.AngleAxis(vector.x, GameState.Current.Player.WeaponCamera.transform.right) * Quaternion.AngleAxis(vector.y, GameState.Current.Player.WeaponCamera.transform.up) * shootingRay;
	}
}
