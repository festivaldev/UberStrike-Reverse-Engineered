using System;
using System.Reflection;
using UnityEngine;

public class RenderSettingsController : MonoBehaviour {
	private const float UNDERWATER_FOG_START = 10f;
	private const float UNDERWATER_FOG_END = 100f;
	private const float FADE_SPEED = 3f;
	private const float TRANSITION_STRENGTH = 5f;
	private const float CHROMATIC_ABERRATION = 10f;
	private static volatile RenderSettingsController _instance;
	private static object _lock = new object();

	[SerializeField]
	private PostEffectsBase[] advancedImageEffects;

	[SerializeField]
	private GameObject advancedWater;

	private Color fogColor;
	private float fogEnd;
	private FogMode fogMode;
	private float fogStart;
	private float interpolationValue;

	[SerializeField]
	private MonoBehaviour[] simpleImageEffects;

	[SerializeField]
	private GameObject simpleWater;

	private UnderWaterEffect underWaterEffect;

	[SerializeField]
	private Color underwaterFogColor;

	private Vignetting vignetteEffect;

	public static RenderSettingsController Instance {
		get {
			if (_instance == null) {
				var @lock = _lock;

				lock (@lock) {
					if (_instance == null) {
						var constructor = typeof(RenderSettingsController).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);

						if (constructor == null || constructor.IsAssembly) {
							throw new Exception(string.Format("A private or protected constructor is missing for '{0}'.", typeof(RenderSettingsController).Name));
						}

						_instance = (RenderSettingsController)constructor.Invoke(null);
					}
				}
			}

			return _instance;
		}
	}

	private void OnEnable() {
		_instance = this;
		fogMode = RenderSettings.fogMode;
		fogColor = RenderSettings.fogColor;
		fogStart = RenderSettings.fogStartDistance;
		fogEnd = RenderSettings.fogEndDistance;

		if (simpleWater != null) {
			simpleWater.SetActive(ApplicationDataManager.IsMobile);
		}

		if (advancedWater != null) {
			advancedWater.SetActive(!ApplicationDataManager.IsMobile);
		}

		EnableImageEffects();

		if (null == underWaterEffect) {
			underWaterEffect = Camera.main.gameObject.AddComponent<UnderWaterEffect>();

			if (underWaterEffect) {
				underWaterEffect.enabled = false;
				underWaterEffect.shader = Shader.Find("CMune/Under Water Effect");
				underWaterEffect.textureRamp = (Texture)Resources.Load("ImageEffects/Underwater_ColorRamp");
			}
		}

		if (null == vignetteEffect && !ApplicationDataManager.IsMobile) {
			vignetteEffect = Camera.main.gameObject.AddComponent<Vignetting>();

			if (vignetteEffect) {
				vignetteEffect.enabled = false;
				vignetteEffect.vignetteShader = Shader.Find("CMune/Vignetting");
				vignetteEffect.chromAberrationShader = Shader.Find("CMune/ChromaticAberration");
				vignetteEffect.separableBlurShader = Shader.Find("CMune/SeparableBlur");
				vignetteEffect.blurSpread = 4f;
				vignetteEffect.intensity = 0f;
			}
		}

		ResetInterpolation();
	}

	private void OnDisable() {
		ResetInterpolation();
	}

	public void EnableImageEffects() {
		foreach (var monoBehaviour in simpleImageEffects) {
			monoBehaviour.enabled = ApplicationDataManager.IsMobile;
		}

		foreach (var postEffectsBase in advancedImageEffects) {
			postEffectsBase.enabled = !ApplicationDataManager.IsMobile && ApplicationDataManager.ApplicationOptions.VideoPostProcessing;
		}
	}

	public void DisableImageEffects() {
		foreach (var monoBehaviour in simpleImageEffects) {
			monoBehaviour.enabled = false;
		}

		foreach (var postEffectsBase in advancedImageEffects) {
			postEffectsBase.enabled = false;
		}

		if (underWaterEffect) {
			underWaterEffect.enabled = false;
		}

		if (vignetteEffect) {
			vignetteEffect.enabled = false;
		}

		interpolationValue = 0f;
	}

	private void Update() {
		if (GameState.Current.MatchState.CurrentStateId != GameStateId.None) {
			if (GameState.Current.PlayerData.IsUnderWater) {
				interpolationValue += Time.deltaTime;
				RenderSettings.fogMode = FogMode.Linear;
			} else {
				interpolationValue -= Time.deltaTime;
				RenderSettings.fogMode = fogMode;
			}

			interpolationValue = Mathf.Clamp01(interpolationValue);
			UpdateSettings(interpolationValue);
		}
	}

	private void UpdateSettings(float value) {
		var num = Mathf.Clamp01(value * 3f);
		var flag = 0f < value;

		if (underWaterEffect) {
			underWaterEffect.enabled = flag;
			underWaterEffect.Weight = num;
		}

		if (vignetteEffect) {
			var flag2 = flag && ApplicationDataManager.ApplicationOptions.VideoPostProcessing;
			vignetteEffect.enabled = flag2;

			if (flag2) {
				var num2 = Mathf.Sin(value * 3.1415927f);
				vignetteEffect.blur = 5f * num2 + value;
				vignetteEffect.chromaticAberration = (5f * num2 + value) * 10f;
			}
		}

		RenderSettings.fogColor = Color.Lerp(fogColor, underwaterFogColor, num);
		RenderSettings.fogStartDistance = Mathf.Lerp(fogStart, 10f, num);
		RenderSettings.fogEndDistance = Mathf.Lerp(fogEnd, 100f, num);
	}

	public void ResetInterpolation() {
		interpolationValue = 0f;
		UpdateSettings(interpolationValue);
	}
}
