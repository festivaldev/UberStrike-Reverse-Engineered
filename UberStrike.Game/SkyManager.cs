using UnityEngine;

public class SkyManager : MonoBehaviour {
	private float _cloudXAxisRot = 0.005f;
	private float _cloudXAxisRotIndex = 0.001f;
	private float _cloudYAxisRot = 0.005f;
	private float _cloudYAxisRotIndex = 0.001f;
	private Vector2 _dayCloudHorizonMoveVector = new Vector2(0f, 0f);
	private Vector2 _dayCloudMoveVector = new Vector2(0f, 0f);

	[SerializeField]
	private float _dayNightCycle;

	[SerializeField]
	private Color _daySkyColor;

	[SerializeField]
	private Color _horizonColor;

	private Material _skyMaterial;

	[SerializeField]
	private Color _sunsetColor;

	[SerializeField]
	private float _sunsetOffset;

	[SerializeField]
	private float _sunsetVisibility;

	public float DayNightCycle {
		get { return _dayNightCycle; }
		set { _dayNightCycle = value; }
	}

	public float CloudXAxisRot {
		get { return _cloudXAxisRot; }
		set { _cloudXAxisRot = value; }
	}

	public float CloudYAxisRot {
		get { return _cloudYAxisRot; }
		set { _cloudYAxisRot = value; }
	}

	private void OnEnable() {
		_skyMaterial = new Material(renderer.material);
	}

	private void OnDisable() {
		renderer.material = _skyMaterial;
	}

	private void Update() {
		_dayCloudMoveVector.x = _dayCloudMoveVector.x + Time.deltaTime * _cloudXAxisRot;
		_dayCloudHorizonMoveVector.y = _dayCloudHorizonMoveVector.y + Time.deltaTime * _cloudYAxisRot;

		if (_dayCloudMoveVector.x > 1f) {
			_dayCloudMoveVector.x = 0f;

			if (_cloudXAxisRot > 0.008f) {
				_cloudXAxisRotIndex = -0.001f;
			}

			if (_cloudXAxisRot < 0.002f) {
				_cloudXAxisRotIndex = 0.001f;
			}

			_cloudXAxisRot += _cloudXAxisRotIndex;
		}

		if (_dayCloudHorizonMoveVector.y > 1f) {
			_dayCloudHorizonMoveVector.y = 0f;

			if (_cloudYAxisRot > 0.008f) {
				_cloudYAxisRotIndex = -0.001f;
			}

			if (_cloudYAxisRot < 0.002f) {
				_cloudYAxisRotIndex = 0.001f;
			}

			_cloudYAxisRot += _cloudYAxisRotIndex;
		}

		renderer.material.SetTextureOffset("_DayCloudTex", _dayCloudMoveVector);
		renderer.material.SetTextureOffset("_NightCloudTex", _dayCloudHorizonMoveVector);
		_dayNightCycle = Mathf.Clamp01(_dayNightCycle);
		renderer.material.SetFloat("_DayNightCycle", Mathf.Clamp01(_dayNightCycle));
		_sunsetOffset = Mathf.Clamp01(_sunsetOffset);
		renderer.material.SetFloat("_SunsetOffset", Mathf.Clamp01(_sunsetOffset));
		_sunsetVisibility = Mathf.Clamp01(_sunsetVisibility);
		renderer.material.SetFloat("_SunsetVisibility", _sunsetVisibility);
		renderer.material.SetColor("_HorizonColor", _horizonColor);
		renderer.material.SetColor("_DaySkyColor", _daySkyColor);
		renderer.material.SetColor("_SunSetColor", _sunsetColor);
	}
}
