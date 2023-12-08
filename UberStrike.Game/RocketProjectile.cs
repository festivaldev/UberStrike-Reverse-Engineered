using UnityEngine;

public class RocketProjectile : Projectile {
	[SerializeField]
	private Light _light;

	[SerializeField]
	private float _smokeAmount = 1f;

	[SerializeField]
	private Color _smokeColor = Color.white;

	[SerializeField]
	private ParticleEmitter _smokeEmitter;

	[SerializeField]
	private ParticleRenderer _smokeRenderer;

	public Color SmokeColor {
		get { return _smokeColor; }
		set {
			_smokeColor = value;

			if (_smokeRenderer) {
				_smokeRenderer.material.SetColor("_TintColor", _smokeColor);
			}
		}
	}

	public float SmokeAmount {
		get { return _smokeAmount; }
		set {
			_smokeAmount = value;

			if (_smokeEmitter) {
				_smokeEmitter.minEmission = _smokeAmount * 10f;
				_smokeEmitter.maxEmission = _smokeAmount * 20f;
			}
		}
	}

	protected override void Awake() {
		base.Awake();
		SmokeColor = _smokeColor;
		SmokeAmount = _smokeAmount;

		if (_light != null) {
			_light.enabled = Application.isWebPlayer;
		}
	}

	protected override void OnTriggerEnter(Collider c) {
		if (!IsProjectileExploded && LayerUtil.IsLayerInMask(CollisionMask, c.gameObject.layer)) {
			Singleton<ProjectileManager>.Instance.RemoveProjectile(ID);
			GameState.Current.Actions.RemoveProjectile(ID, true);
		}
	}

	protected override void OnCollisionEnter(Collision c) {
		if (!IsProjectileExploded && c.gameObject && LayerUtil.IsLayerInMask(CollisionMask, c.gameObject.layer)) {
			Singleton<ProjectileManager>.Instance.RemoveProjectile(ID);
			GameState.Current.Actions.RemoveProjectile(ID, true);
		}
	}
}
