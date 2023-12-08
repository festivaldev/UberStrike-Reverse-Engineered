using System.Collections;
using UnityEngine;

public class BulletTrail : BaseWeaponEffect {
	private Animation _animation;
	private AnimationState _clip;
	private Renderer[] _renderers = new Renderer[0];
	private float _trailDuration = 0.1f;

	private void Awake() {
		_animation = GetComponentInChildren<Animation>();

		if (_animation) {
			_clip = _animation[_animation.clip.name];
			_clip.wrapMode = WrapMode.Once;
			_trailDuration = _clip.length;
			_animation.playAutomatically = false;
		}

		_renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in _renderers) {
			renderer.enabled = false;
		}
	}

	public override void OnShoot() {
		foreach (var renderer in _renderers) {
			renderer.enabled = true;
		}

		if (_animation) {
			var num = _trailDuration / _clip.length;
			_clip.speed = num;
			_animation.Play();
		}

		StartCoroutine(StartTrailEffect(_trailDuration));
	}

	public override void OnPostShoot() { }
	public override void Hide() { }
	public override void OnHits(RaycastHit[] hits) { }

	private IEnumerator StartTrailEffect(float time) {
		yield return new WaitForSeconds(time);

		foreach (var r in _renderers) {
			r.enabled = false;
		}
	}
}
