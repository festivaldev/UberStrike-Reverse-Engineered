using System;
using UnityEngine;

public class MuzzleHeatWave : BaseWeaponEffect {
	[SerializeField]
	private float _distortion = 64f;

	[SerializeField]
	private float _duration = 0.25f;

	private float _elapsedTime;

	[SerializeField]
	private float _maxSize = 0.05f;

	private float _normalizedTime;
	private Renderer _renderer;
	private float _s;

	[SerializeField]
	private float _startSize;

	private Transform _transform;

	private void Awake() {
		_transform = transform;
		_renderer = renderer;

		if (_renderer == null) {
			throw new Exception("No Renderer attached to HeatWave script on GameObject " + gameObject.name);
		}
	}

	private void Start() {
		_renderer.enabled = false;
		enabled = false;
	}

	private void Update() {
		if (_transform && _renderer) {
			_elapsedTime += Time.deltaTime;
			_normalizedTime = _elapsedTime / _duration;
			_s = Mathf.Lerp(_startSize, _maxSize, _normalizedTime);
			_renderer.material.SetFloat("_BumpAmt", (1f - _normalizedTime) * _distortion);
			_transform.localScale = new Vector3(_s, _s, _s);
			_transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - _transform.position);

			if (_elapsedTime > _duration) {
				_transform.localScale = new Vector3(0f, 0f, 0f);
				_renderer.enabled = false;
				enabled = false;
			}
		}
	}

	public override void OnShoot() {
		if (SystemInfo.supportsImageEffects) {
			_elapsedTime = 0f;
			_transform.rotation = Quaternion.FromToRotation(Vector3.up, Camera.main.transform.position - _transform.position);
			_renderer.enabled = true;
			enabled = true;
		}
	}

	public override void OnPostShoot() { }

	public override void Hide() {
		if (!_renderer) {
			_renderer = renderer;
		}

		if (_renderer) {
			_renderer.enabled = false;
		}
	}

	public override void OnHits(RaycastHit[] hits) { }
}
