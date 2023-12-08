using System.Collections;
using UnityEngine;

public class HUDArmorBar : MonoBehaviour {
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
	private UISprite icon;

	private float oldValue = -1f;

	[SerializeField]
	private float PulseScale = 7f;

	[SerializeField]
	private float PulseSpeed = 20f;

	[SerializeField]
	private UILabel text;

	private void Start() {
		baseWidth = bgr.transform.localScale.x;
		baseScale = text.transform.localScale.x;
		GameState.Current.PlayerData.ArmorPoints.AddEventAndFire(OnArmorPoints, this);
	}

	public void OnArmorPoints(int value) {
		StopAllCoroutines();

		if (value != oldValue) {
			StartCoroutine(PulseCrt(value >= oldValue));
		}
	}

	private void Update() {
		var value = GameState.Current.PlayerData.ArmorPoints.Value;

		if (value != oldValue) {
			oldValue = Mathf.MoveTowards(oldValue, value, Time.deltaTime * animateSpeed);
			bgr.transform.localScale = bgr.transform.localScale.SetX(Mathf.Max(NORMAL_MAX, oldValue) / NORMAL_MAX * baseWidth);
			bar.transform.localScale = bgr.transform.localScale.SetX(oldValue / NORMAL_MAX * baseWidth);
			text.text = Mathf.FloorToInt(oldValue).ToString();
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
