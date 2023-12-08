using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackHud : Singleton<DamageFeedbackHud> {
	private const float PEAKTIME = 0.04f;
	private const float ENDTIME = 0.08f;
	private List<DamageFeedbackMark> _damageFeedbackMarkList;
	public bool Enabled { get; set; }

	private DamageFeedbackHud() {
		_damageFeedbackMarkList = new List<DamageFeedbackMark>();
		Enabled = true;
	}

	public void Draw() {
		if (!Enabled) {
			return;
		}

		for (var i = 0; i < _damageFeedbackMarkList.Count; i++) {
			var num = _damageFeedbackMarkList[i].DamageDirection;
			var vector = Quaternion.AngleAxis(-num, Vector3.up) * Vector3.back;
			vector = Camera.main.transform.InverseTransformDirection(vector);
			num = Quaternion.LookRotation(vector).eulerAngles.y;
			GUIUtility.RotateAroundPivot(num, new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
			GUI.color = new Color(0.975f, 0.201f, 0.135f, _damageFeedbackMarkList[i].DamageAlpha);
			var num2 = Mathf.RoundToInt(128f * _damageFeedbackMarkList[i].DamageAmount);
			GUI.DrawTexture(new Rect(Screen.width * 0.5f - num2 * 0.5f, Screen.height * 0.5f - 256f, num2, 128f), HudTextures.DamageFeedbackMark);
			GUI.matrix = Matrix4x4.identity;
		}

		GUI.color = Color.white;
	}

	public void Update() {
		if (_damageFeedbackMarkList.Count > 0) {
			for (var i = 0; i < _damageFeedbackMarkList.Count; i++) {
				if (_damageFeedbackMarkList[i].DamageAlpha < 0f) {
					_damageFeedbackMarkList.RemoveAt(i);
				}
			}

			for (var j = 0; j < _damageFeedbackMarkList.Count; j++) {
				_damageFeedbackMarkList[j].DamageAlpha -= Time.deltaTime * 0.5f;
			}
		}
	}

	public void AddDamageMark(float normalizedDamage, float horizontalAngle) {
		_damageFeedbackMarkList.Add(new DamageFeedbackMark(normalizedDamage, horizontalAngle));
		LevelCamera.DoFeedback(LevelCamera.FeedbackType.GetDamage, Vector3.back, 0.1f, normalizedDamage, 0.04f, 0.08f, 10f, Vector3.forward);
	}

	public void ClearAll() {
		_damageFeedbackMarkList.Clear();
	}

	public class DamageFeedbackMark {
		public float DamageAlpha;
		public float DamageAmount;
		public float DamageDirection;

		public DamageFeedbackMark(float normalizedDamage, float horizontalAngle) {
			DamageAlpha = 1f;
			DamageAmount = normalizedDamage;
			DamageDirection = horizontalAngle;
		}
	}
}
