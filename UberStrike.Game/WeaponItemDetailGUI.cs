using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class WeaponItemDetailGUI : IBaseItemDetailGUI {
	private UberStrikeItemWeaponView _item;

	public WeaponItemDetailGUI(UberStrikeItemWeaponView item) {
		_item = item;
	}

	public void Draw() {
		if (_item != null) {
			GUITools.ProgressBar(new Rect(14f, 95f, 165f, 12f), LocalizedStrings.Damage, WeaponConfigurationHelper.GetDamageNormalized(_item), ColorScheme.ProgressBar, 64);
			GUITools.ProgressBar(new Rect(14f, 111f, 165f, 12f), LocalizedStrings.RateOfFire, WeaponConfigurationHelper.GetRateOfFireNormalized(_item), ColorScheme.ProgressBar, 64);

			if (_item.ItemClass == UberstrikeItemClass.WeaponCannon || _item.ItemClass == UberstrikeItemClass.WeaponLauncher || _item.ItemClass == UberstrikeItemClass.WeaponSplattergun) {
				GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Velocity, WeaponConfigurationHelper.GetProjectileSpeedNormalized(_item), ColorScheme.ProgressBar, 64);
				GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Impact, WeaponConfigurationHelper.GetSplashRadiusNormalized(_item), ColorScheme.ProgressBar, 64);
			} else if (_item.ItemClass == UberstrikeItemClass.WeaponMelee) {
				var enabled = GUI.enabled;
				GUI.enabled = false;
				GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Accuracy, 0f, ColorScheme.ProgressBar, 64);
				GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Recoil, 0f, ColorScheme.ProgressBar, 64);
				GUI.enabled = enabled;
			} else {
				GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Accuracy, WeaponConfigurationHelper.GetAccuracySpreadNormalized(_item), ColorScheme.ProgressBar, 64);
				GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Recoil, WeaponConfigurationHelper.GetRecoilKickbackNormalized(_item), ColorScheme.ProgressBar, 64);
			}
		}
	}
}
