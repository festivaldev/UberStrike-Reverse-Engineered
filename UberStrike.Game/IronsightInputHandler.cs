using UberStrike.Core.Models.Views;
using UnityEngine;

public class IronsightInputHandler : WeaponInputHandler {
	protected float _ironSightDelay;
	protected bool _isIronsight;

	public IronsightInputHandler(IWeaponLogic logic, bool isLocal, ZoomInfo zoomInfo, UberStrikeItemWeaponView view) : base(logic, isLocal) {
		_zoomInfo = zoomInfo;

		if (view.HasAutomaticFire) {
			FireHandler = new FullAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		} else {
			FireHandler = new SemiAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		}
	}

	public override void OnSecondaryFire(bool pressed) {
		_isIronsight = pressed;
	}

	public override void Update() {
		FireHandler.Update();
		UpdateIronsight();

		if (_isIronsight) {
			if (!LevelCamera.IsZoomedIn) {
				ZoomIn(_zoomInfo, _weaponLogic.Decorator, 0f, false);
			}
		} else if (LevelCamera.IsZoomedIn) {
			ZoomOut(_zoomInfo, _weaponLogic.Decorator);
		}

		if (!_isIronsight && _ironSightDelay > 0f) {
			_ironSightDelay -= Time.deltaTime;
		}
	}

	public override void Stop() {
		FireHandler.Stop();

		if (_isIronsight) {
			_isIronsight = false;

			if (_isLocal) {
				LevelCamera.ResetZoom();
			}

			if (WeaponFeedbackManager.Instance.IsIronSighted) {
				WeaponFeedbackManager.Instance.ResetIronSight();
			}
		}
	}

	public override bool CanChangeWeapon() {
		return !_isIronsight && _ironSightDelay <= 0f;
	}

	private void UpdateIronsight() {
		if (_isIronsight) {
			if (!WeaponFeedbackManager.Instance.IsIronSighted) {
				WeaponFeedbackManager.Instance.BeginIronSight();
			}
		} else if (WeaponFeedbackManager.Instance.IsIronSighted) {
			WeaponFeedbackManager.Instance.EndIronSight();
		}
	}

	public override void OnPrimaryFire(bool pressed) {
		FireHandler.OnTriggerPulled(pressed);
	}

	public override void OnPrevWeapon() { }
	public override void OnNextWeapon() { }
}
