using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InstantMultiHitWeapon : BaseWeaponLogic {
	private BaseWeaponDecorator _decorator;
	private UberStrikeItemWeaponView _view;
	private int ShotgunGauge;

	public override BaseWeaponDecorator Decorator {
		get { return _decorator; }
	}

	public InstantMultiHitWeapon(WeaponItem item, BaseWeaponDecorator decorator, int shotGauge, IWeaponController controller, UberStrikeItemWeaponView view) : base(item, controller) {
		ShotgunGauge = shotGauge;
		_view = view;
		_decorator = decorator;
	}

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits) {
		var dictionary = new Dictionary<BaseGameProp, ShotPoint>(ShotgunGauge);
		HitPoint hitPoint = null;
		var array = new RaycastHit[ShotgunGauge];
		var num = Controller.NextProjectileId();
		var num2 = 1000;

		for (var i = 0; i < ShotgunGauge; i++) {
			var vector = WeaponDataManager.ApplyDispersion(ray.direction, _view, false);
			RaycastHit raycastHit;

			if (Physics.Raycast(ray.origin, vector, out raycastHit, num2, (!Controller.IsLocal) ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask)) {
				if (hitPoint == null) {
					hitPoint = new HitPoint(raycastHit.point, TagUtil.GetTag(raycastHit.collider));
				}

				var component = raycastHit.collider.GetComponent<BaseGameProp>();

				if (component) {
					ShotPoint shotPoint;

					if (dictionary.TryGetValue(component, out shotPoint)) {
						shotPoint.AddPoint(raycastHit.point);
					} else {
						dictionary.Add(component, new ShotPoint(raycastHit.point, num));
					}
				}

				array[i] = raycastHit;
			} else {
				array[i].point = ray.origin + ray.direction * 1000f;
				array[i].normal = raycastHit.normal;
			}
		}

		Decorator.PlayImpactSoundAt(hitPoint);
		hits = new CmunePairList<BaseGameProp, ShotPoint>(dictionary.Count);

		foreach (var keyValuePair in dictionary) {
			hits.Add(keyValuePair.Key, keyValuePair.Value);
		}

		if (Decorator) {
			Decorator.ShowShootEffect(array);
		}

		OnHits(hits);
	}
}
