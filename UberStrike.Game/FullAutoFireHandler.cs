using UberStrike.Core.Models;
using UnityEngine;

public class FullAutoFireHandler : IWeaponFireHandler {
	private float frequency;
	private int shootCounter;
	private float shootingStartTime;
	private BaseWeaponDecorator weapon;
	public bool IsShooting { get; private set; }

	public FullAutoFireHandler(BaseWeaponDecorator weapon, float frequency) {
		this.weapon = weapon;
		this.frequency = frequency;
	}

	public bool IsTriggerPulled { get; private set; }

	public bool CanShoot {
		get { return shootingStartTime + frequency * shootCounter <= Time.time; }
	}

	public void OnTriggerPulled(bool pulled) {
		IsTriggerPulled = pulled;
	}

	public void Update() {
		if (IsTriggerPulled && !IsShooting && CanShoot && Singleton<WeaponController>.Instance.CheckAmmoCount()) {
			GameState.Current.PlayerData.Set(PlayerStates.Shooting, true);
			IsShooting = true;
			shootingStartTime = Time.time;
			shootCounter = 0;
		}

		if (IsShooting) {
			Singleton<WeaponController>.Instance.Shoot();
		}

		if (IsShooting && (!IsTriggerPulled || !Singleton<WeaponController>.Instance.CheckAmmoCount())) {
			GameState.Current.PlayerData.Set(PlayerStates.Shooting, false);
			IsShooting = false;

			if (weapon) {
				weapon.PostShoot();
			}
		}
	}

	public void Stop() {
		GameState.Current.PlayerData.Set(PlayerStates.Shooting, false);
		IsTriggerPulled = false;
		IsShooting = false;
	}

	public void RegisterShot() {
		shootCounter++;
	}
}
