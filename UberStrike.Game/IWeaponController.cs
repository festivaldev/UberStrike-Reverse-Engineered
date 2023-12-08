public interface IWeaponController {
	byte PlayerNumber { get; }
	int Cmid { get; }
	bool IsLocal { get; }
	int NextProjectileId();
	void UpdateWeaponDecorator(IUnityItem item);
}
