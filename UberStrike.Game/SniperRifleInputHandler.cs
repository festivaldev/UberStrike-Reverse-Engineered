using UberStrike.Core.Models.Views;

public class SniperRifleInputHandler : WeaponInputHandler {
	protected const float ZOOM = 4f;
	protected bool _scopeOpen;
	protected float _zoom;

	public SniperRifleInputHandler(IWeaponLogic logic, bool isLocal, ZoomInfo zoomInfo, UberStrikeItemWeaponView view) : base(logic, isLocal) {
		_zoomInfo = zoomInfo;

		if (view.HasAutomaticFire) {
			FireHandler = new FullAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		} else {
			FireHandler = new SemiAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		}
	}

	public override void OnSecondaryFire(bool pressed) {
		_scopeOpen = pressed;
		Update();
	}

	public override void OnPrevWeapon() {
		_zoom = -4f;
	}

	public override void OnNextWeapon() {
		_zoom = 4f;
	}

	public override void Update() {
		FireHandler.Update();

		if (_scopeOpen) {
			if (!LevelCamera.IsZoomedIn || _zoom != 0f) {
				ZoomIn(_zoomInfo, _weaponLogic.Decorator, _zoom, true);
				_zoom = 0f;
				EventHandler.Global.Fire(new GameEvents.PlayerZoomIn());
				GameState.Current.PlayerData.IsZoomedIn.Value = true;
			}
		} else if (LevelCamera.IsZoomedIn) {
			ZoomOut(_zoomInfo, _weaponLogic.Decorator);
			GameState.Current.PlayerData.IsZoomedIn.Value = false;
		}
	}

	public override bool CanChangeWeapon() {
		return !_scopeOpen;
	}

	public override void Stop() {
		FireHandler.Stop();

		if (_scopeOpen) {
			_scopeOpen = false;

			if (_isLocal) {
				LevelCamera.ResetZoom();
			}
		}
	}

	public override void OnPrimaryFire(bool pressed) {
		FireHandler.OnTriggerPulled(pressed);
	}
}
