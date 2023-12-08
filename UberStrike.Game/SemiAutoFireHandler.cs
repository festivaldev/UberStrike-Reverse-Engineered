using UnityEngine;

public class SemiAutoFireHandler : IWeaponFireHandler {
	private BaseWeaponDecorator _weapon;
	private float frequency;
	private float nextShootTime;

	public SemiAutoFireHandler(BaseWeaponDecorator weapon, float frequency) {
		this.frequency = frequency;
		_weapon = weapon;
		IsTriggerPulled = false;
	}

	public bool IsTriggerPulled { get; private set; }

	public bool CanShoot {
		get { return nextShootTime < Time.time; }
	}

	public void OnTriggerPulled(bool pulled) {
		if (pulled && !IsTriggerPulled && Singleton<WeaponController>.Instance.Shoot()) {
			_weapon.PostShoot();
		}

		IsTriggerPulled = pulled;
	}

	public void Update() { }
	public void Stop() { }

	public void RegisterShot() {
		nextShootTime = Time.time + frequency;
	}
}
