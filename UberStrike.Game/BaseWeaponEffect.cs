using UnityEngine;

public abstract class BaseWeaponEffect : MonoBehaviour {
	protected BaseWeaponDecorator _decorator;

	public void SetDecorator(BaseWeaponDecorator decorator) {
		_decorator = decorator;
	}

	public abstract void OnShoot();
	public abstract void OnPostShoot();
	public abstract void OnHits(RaycastHit[] hits);
	public abstract void Hide();
}
