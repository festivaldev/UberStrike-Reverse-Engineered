public interface IWeaponFireHandler {
	bool IsTriggerPulled { get; }
	bool CanShoot { get; }
	void OnTriggerPulled(bool pulled);
	void Update();
	void Stop();
	void RegisterShot();
}
