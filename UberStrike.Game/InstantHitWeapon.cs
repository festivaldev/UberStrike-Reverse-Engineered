using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InstantHitWeapon : BaseWeaponLogic {
	private BaseWeaponDecorator _decorator;
	private bool _supportIronSight;
	private UberStrikeItemWeaponView _view;

	public override BaseWeaponDecorator Decorator {
		get { return _decorator; }
	}

	public InstantHitWeapon(WeaponItem item, BaseWeaponDecorator decorator, IWeaponController controller, UberStrikeItemWeaponView view) : base(item, controller) {
		_view = view;
		_decorator = decorator;
		_supportIronSight = view.WeaponSecondaryAction == 2;
	}

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits) {
		hits = null;
		var vector = WeaponDataManager.ApplyDispersion(ray.direction, _view, _supportIronSight);
		var num = Controller.NextProjectileId();
		RaycastHit raycastHit;

		if (Physics.Raycast(ray.origin, vector, out raycastHit, 1000f, (!Controller.IsLocal) ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask)) {
			var hitPoint = new HitPoint(raycastHit.point, TagUtil.GetTag(raycastHit.collider));
			var component = raycastHit.collider.GetComponent<BaseGameProp>();

			if (component) {
				hits = new CmunePairList<BaseGameProp, ShotPoint>(1);
				hits.Add(component, new ShotPoint(raycastHit.point, num));
			}

			Decorator.PlayImpactSoundAt(hitPoint);
		} else {
			raycastHit.point = ray.origin + ray.direction * 1000f;
		}

		if (Decorator) {
			Decorator.ShowShootEffect(new[] {
				raycastHit
			});
		}

		OnHits(hits);
	}
}
