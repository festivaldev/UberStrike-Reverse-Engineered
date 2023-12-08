using System.Collections;
using UnityEngine;

public class HUDHealthBar : MonoBehaviour {
	private readonly float CRITICAL_VALUE = 25f;
	private readonly float NORMAL_MAX = 100f;

	[SerializeField]
	private float animateSpeed = 200f;

	[SerializeField]
	private UISprite bar;

	private float baseScale;
	private float baseWidth;

	[SerializeField]
	private UISprite bgr;

	[SerializeField]
	private float criticalBlinkingSpeed = 6.5f;

	private int criticalHealthLastTime;

	[SerializeField]
	private UISprite icon;

	private Color normalBarColor;
	private float oldValue = -1f;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private float PulseScale = 7f;

	[SerializeField]
	private float PulseSpeed = 20f;

	[SerializeField]
	private UILabel text;

	private void Start() {
		baseWidth = bgr.transform.localScale.x;
		baseScale = text.transform.localScale.x;
		normalBarColor = bar.color;
		GameState.Current.PlayerData.Health.AddEventAndFire(OnHealthPoints, this);
	}

	private void OnEnable() {
		GameState.Current.PlayerData.Health.Fire();
	}

	public void OnHealthPoints(int value, int oldValue) {
		if (GameData.Instance.GameState != GameStateId.None && oldValue > 0 && oldValue <= 100 && value > 100) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(SoundEffects.Instance.Health_100_200_Increase.Interpolate(value, 100f, 200f));
		}

		StopAllCoroutines();

		if (value != oldValue) {
			StartCoroutine(PulseCrt(value >= oldValue));
		}

		StartCoroutine(AnimateCrt(value));
	}

	private IEnumerator AnimateCrt(int value) {
		panel.alpha = 1f;

		while (value != oldValue) {
			oldValue = Mathf.MoveTowards(oldValue, value, Time.deltaTime * animateSpeed);
			bgr.transform.localScale = bgr.transform.localScale.SetX(Mathf.Max(NORMAL_MAX, oldValue) / NORMAL_MAX * baseWidth);
			bar.transform.localScale = bgr.transform.localScale.SetX(oldValue / NORMAL_MAX * baseWidth);
			bar.color = ((oldValue <= CRITICAL_VALUE) ? new Color(1f, 0.23529412f, 0.1882353f) : normalBarColor);
			text.text = Mathf.FloorToInt(oldValue).ToString();

			if (oldValue == 0f || oldValue > CRITICAL_VALUE) {
				AutoMonoBehaviour<SfxManager>.Instance.StopLoopedAudioClip();
			}

			yield return 0;
		}

		if (oldValue > CRITICAL_VALUE || oldValue <= 0f) {
			yield break;
		}

		AutoMonoBehaviour<SfxManager>.Instance.PlayLoopedAudioClip(SoundEffects.Instance.HealthNoise_0_25.Interpolate(oldValue, 0f, CRITICAL_VALUE));

		for (;;) {
			var time = Time.time * criticalBlinkingSpeed;
			panel.alpha = Mathf.Clamp01(Mathf.Sin(time) + 1f);

			if (time % 6.2831855f >= 3.1415927f && criticalHealthLastTime != (int)(time / 6.2831855f)) {
				criticalHealthLastTime = (int)(time / 6.2831855f);
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(SoundEffects.Instance.HealthHeartbeat_0_25.Interpolate(oldValue, 0f, CRITICAL_VALUE));
			}

			yield return 0;
		}
	}

	private IEnumerator PulseCrt(bool up) {
		var time = 0f;

		for (;;) {
			time = Mathf.Min(time + Time.deltaTime * PulseSpeed, 3.1415927f);
			var pulse = Mathf.Sin(time) * PulseScale;
			text.transform.localScale = (Vector3.one * (baseScale + pulse * ((!up) ? (-1) : 1))).SetZ(1f);

			if (time >= 3.1415927f) {
				break;
			}

			yield return 0;
		}

		yield break;
		yield break;
	}
}
