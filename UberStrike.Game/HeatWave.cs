using System;
using UnityEngine;

public class HeatWave : MonoBehaviour {
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

	private void Update() {
		if (_transform && _renderer) {
			_elapsedTime += Time.deltaTime;
			_normalizedTime = _elapsedTime / _duration;
			_s = Mathf.Lerp(_startSize, _maxSize, _normalizedTime);

			if (_renderer.material) {
				_renderer.material.SetFloat("_BumpAmt", (1f - _normalizedTime) * _distortion);
			}

			_transform.localScale = new Vector3(_s, _s, _s);

			if (Camera.main) {
				_transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - _transform.position);
			}

			if (_elapsedTime > _duration && gameObject) {
				Destroy(gameObject);
			}
		}
	}
}
