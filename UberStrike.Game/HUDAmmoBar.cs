using System.Collections;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDAmmoBar : MonoBehaviour {
	[SerializeField]
	private float animateSpeed = 200f;

	[SerializeField]
	private UISprite bar;

	private float baseScale;
	private float baseWidth;

	[SerializeField]
	private UISprite bgr;

	[SerializeField]
	private UISprite icon;

	private float oldValue = -1f;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private float PulseScale = 7f;

	[SerializeField]
	private float PulseSpeed = 20f;

	[SerializeField]
	private UILabel text;

	private void OnEnable() {
		GameState.Current.PlayerData.ActiveWeapon.Fire();
	}

	private void Start() {
		baseWidth = bgr.transform.localScale.x;
		baseScale = text.transform.localScale.x;

		GameState.Current.PlayerData.ActiveWeapon.AddEventAndFire(delegate(WeaponSlot el) {
			if (el != null) {
				var currentWeapon = Singleton<WeaponController>.Instance.GetCurrentWeapon();
				oldValue = AmmoDepot.AmmoOfClass(currentWeapon.View.ItemClass);
				OnChanged();
			}
		}, this);

		GameState.Current.PlayerData.Ammo.AddEvent(delegate(int el) { OnChanged(); }, this);
	}

	private void OnChanged() {
		var currentWeapon = Singleton<WeaponController>.Instance.GetCurrentWeapon();
		var flag = currentWeapon != null && currentWeapon.View.ItemClass != UberstrikeItemClass.WeaponMelee;
		panel.alpha = (float)((!flag) ? 0 : 1);

		if (!flag) {
			return;
		}

		var num = AmmoDepot.AmmoOfClass(currentWeapon.View.ItemClass);
		var num2 = AmmoDepot.MaxAmmoOfClass(currentWeapon.View.ItemClass);
		StopAllCoroutines();

		if (num != oldValue) {
			StartCoroutine(PulseCrt(num >= oldValue));
		}

		StartCoroutine(AnimateCrt(num, num2));
	}

	private IEnumerator AnimateCrt(int value, int maxValue) {
		panel.alpha = 1f;

		do {
			oldValue = Mathf.MoveTowards(oldValue, value, Time.deltaTime * animateSpeed);
			bgr.transform.localScale = bgr.transform.localScale.SetX(Mathf.Max(maxValue, oldValue) / maxValue * baseWidth);
			bar.transform.localScale = bgr.transform.localScale.SetX(oldValue / maxValue * baseWidth);
			text.text = Mathf.FloorToInt(oldValue).ToString();

			yield return 0;
		} while (value != oldValue);
	}

	private IEnumerator PulseCrt(bool up) {
		var time = 0f;

		for (;;) {
			time = Mathf.Min(time + Time.deltaTime * PulseSpeed, 3.1415927f);
			var pulse = Mathf.Sin(time) * PulseScale;
			text.transform.localScale = (Vector3.one * (baseScale + pulse * ((!up) ? (-1) : 1))).SetZ(1f);

			if (time >= 3.1415927f) {
				break;
			}

			yield return 0;
		}

		yield break;
		yield break;
	}
}
