using UnityEngine;

[RequireComponent(typeof(Animation))]
public class WeaponHeadAnimation : BaseWeaponEffect {
	private Animation _animation;
	private AnimationState _animState;
	private float _speed;

	private void Awake() {
		_animation = GetComponent<Animation>();

		if (_animation && _animation.clip) {
			_animation.playAutomatically = false;
			_animState = _animation[_animation.clip.name];
		}
	}

	private void Update() {
		if (_speed > 0f) {
			if (_animState) {
				_animState.speed = _speed;
			}

			_speed = Mathf.Lerp(_speed, -0.1f, Time.deltaTime);
		} else if (_animation.isPlaying) {
			_animation.Stop();
		}
	}

	public override void OnShoot() {
		_speed = 1f;

		if (_animation) {
			if (!_animation.isPlaying) {
				_animation.Play();
			}
		} else {
			Debug.LogError("No animation for weapon head!");
		}
	}

	public override void OnPostShoot() { }
	public override void OnHits(RaycastHit[] hits) { }

	public override void Hide() {
		if (_animation && _animation.isPlaying) {
			_animation.Stop();
		}
	}

	public void SetSpeed(float speed) {
		_speed = speed;
	}
}
