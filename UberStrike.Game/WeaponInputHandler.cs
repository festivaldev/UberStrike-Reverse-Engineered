using UnityEngine;

public abstract class WeaponInputHandler {
	protected bool _isLocal;
	protected IWeaponLogic _weaponLogic;
	protected ZoomInfo _zoomInfo;
	public IWeaponFireHandler FireHandler { get; protected set; }

	protected WeaponInputHandler(IWeaponLogic logic, bool isLocal) {
		_isLocal = isLocal;
		_weaponLogic = logic;
	}

	protected static void ZoomIn(ZoomInfo zoomInfo, BaseWeaponDecorator weapon, float zoom, bool hideWeapon) {
		if (weapon) {
			if (!LevelCamera.IsZoomedIn) {
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperScopeIn);
			} else if (zoom < 0f && zoomInfo.CurrentMultiplier != zoomInfo.MinMultiplier) {
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperZoomIn);
			} else if (zoom > 0f && zoomInfo.CurrentMultiplier != zoomInfo.MaxMultiplier) {
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperZoomOut);
			}

			zoomInfo.CurrentMultiplier = Mathf.Clamp(zoomInfo.CurrentMultiplier + zoom, zoomInfo.MinMultiplier, zoomInfo.MaxMultiplier);
			LevelCamera.DoZoomIn(75f / zoomInfo.CurrentMultiplier, 20f, hideWeapon);
			UserInput.ZoomSpeed = 0.5f;
		}
	}

	protected static void ZoomOut(ZoomInfo zoomInfo, BaseWeaponDecorator weapon) {
		LevelCamera.DoZoomOut(75f, 10f);
		UserInput.ZoomSpeed = 1f;

		if (zoomInfo != null) {
			zoomInfo.CurrentMultiplier = zoomInfo.DefaultMultiplier;
		}

		if (weapon) {
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperScopeOut);
		}
	}

	public abstract void OnPrimaryFire(bool pressed);
	public abstract void OnSecondaryFire(bool pressed);
	public abstract void OnPrevWeapon();
	public abstract void OnNextWeapon();
	public abstract void Update();
	public abstract bool CanChangeWeapon();
	public virtual void Stop() { }
}
