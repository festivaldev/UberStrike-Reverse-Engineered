using UberStrike.Core.Models.Views;
using UnityEngine;

public class MinigunInputHandler : WeaponInputHandler {
	protected bool _isGunWarm;
	private bool _isTriggerPulled;
	protected bool _isWarmupPlayed;
	protected float _warmTime;
	private MinigunWeaponDecorator _weapon;

	public MinigunInputHandler(IWeaponLogic logic, bool isLocal, MinigunWeaponDecorator weapon, UberStrikeItemWeaponView view) : base(logic, isLocal) {
		_weapon = weapon;
		FireHandler = new FullAutoFireHandler(weapon, WeaponConfigurationHelper.GetRateOfFire(view));
	}

	public override void Update() {
		if (!_weapon) {
			return;
		}

		if (_warmTime < _weapon.MaxWarmUpTime) {
			if (_isGunWarm || _isTriggerPulled) {
				if (!_isWarmupPlayed) {
					_isWarmupPlayed = true;
					_weapon.PlayWindUpSound(_warmTime);
				}

				_warmTime += Time.deltaTime;

				if (_warmTime >= _weapon.MaxWarmUpTime) {
					_weapon.PlayDuringSound();
				}

				_weapon.SpinWeaponHead();
			}

			FireHandler.OnTriggerPulled(false);
		} else if (_isTriggerPulled) {
			FireHandler.OnTriggerPulled(true);
		} else if (_isGunWarm) {
			_weapon.SpinWeaponHead();
			FireHandler.OnTriggerPulled(false);
		} else {
			FireHandler.OnTriggerPulled(false);
		}

		if (!_isGunWarm && !_isTriggerPulled) {
			if (_warmTime > 0f) {
				_warmTime -= Time.deltaTime;

				if (_warmTime < 0f) {
					_warmTime = 0f;
				}

				if (_isWarmupPlayed) {
					_weapon.PlayWindDownSound((1f - _warmTime / _weapon.MaxWarmUpTime) * _weapon.MaxWarmDownTime);
				}
			}

			_isWarmupPlayed = false;
		}

		FireHandler.Update();
	}

	public override void OnSecondaryFire(bool pressed) {
		_isGunWarm = pressed;
	}

	public override bool CanChangeWeapon() {
		return !_isGunWarm;
	}

	public override void Stop() {
		_warmTime = 0f;
		_isGunWarm = false;
		_isWarmupPlayed = false;
		_isTriggerPulled = false;
		FireHandler.Stop();

		if (_weapon) {
			_weapon.StopSound();
		}
	}

	public override void OnPrimaryFire(bool pressed) {
		_isTriggerPulled = pressed;
	}

	public override void OnPrevWeapon() { }
	public override void OnNextWeapon() { }
}
