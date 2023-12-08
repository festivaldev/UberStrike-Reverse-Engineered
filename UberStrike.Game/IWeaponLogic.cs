using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public interface IWeaponLogic {
	BaseWeaponDecorator Decorator { get; }
	void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits);
}
