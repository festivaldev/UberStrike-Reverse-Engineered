using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : BaseWeaponEffect {
	private Animation _animation;
	private AnimationState _clip;
	private float _flashDuration = 0.1f;
	private float _muzzleFlashEnd;
	private List<Renderer> _renderers = new List<Renderer>();

	private void Awake() {
		_animation = GetComponentInChildren<Animation>();

		if (_animation) {
			_clip = _animation[_animation.clip.name];
			_clip.wrapMode = WrapMode.Once;
			_flashDuration = _clip.length;
			_animation.playAutomatically = false;
		}

		_muzzleFlashEnd = 0f;
		_renderers.AddRange(GetComponentsInChildren<Renderer>());

		foreach (var renderer in _renderers) {
			renderer.enabled = false;
		}
	}

	public override void Hide() {
		_muzzleFlashEnd = 0f;

		if (_clip) {
			_clip.normalizedTime = 1f;
		}

		foreach (var renderer in _renderers) {
			renderer.enabled = false;
		}
	}

	public override void OnShoot() {
		foreach (var renderer in _renderers) {
			renderer.enabled = true;
		}

		transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));

		if (_animation) {
			var num = _clip.length / _flashDuration;
			_clip.speed = num;
			_clip.time = 0f;
			_animation.Play();
		}

		_muzzleFlashEnd = Time.time + _flashDuration;
	}

	public override void OnPostShoot() { }

	private void Update() {
		if (_muzzleFlashEnd < Time.time) {
			foreach (var renderer in _renderers) {
				renderer.enabled = false;
			}
		}
	}

	public override void OnHits(RaycastHit[] hits) { }
}
