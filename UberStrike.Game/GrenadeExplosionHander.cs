public class GrenadeExplosionHander : IWeaponFireHandler {
	public GrenadeExplosionHander() {
		IsTriggerPulled = false;
	}

	public bool IsTriggerPulled { get; private set; }

	public bool CanShoot {
		get { return true; }
	}

	public void OnTriggerPulled(bool pulled) {
		IsTriggerPulled = pulled;

		if (pulled) {
			Singleton<ProjectileManager>.Instance.RemoveAllLimitedProjectiles();
		}
	}

	public void Update() { }

	public void Stop() {
		IsTriggerPulled = false;
	}

	public void RegisterShot() { }
}
