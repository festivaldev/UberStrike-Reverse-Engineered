using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SecretTrigger : BaseGameProp {
	[SerializeField]
	private float _activationTime = 15f;

	private SecretBehaviour _reciever;
	private float _showVisualsEndTime;

	[SerializeField]
	private Renderer[] _visuals;

	public float ActivationTimeOut {
		get { return _showVisualsEndTime; }
	}

	private void Awake() {
		gameObject.layer = 21;
	}

	private void OnDisable() {
		foreach (var renderer in _visuals) {
			renderer.material.SetColor("_Color", Color.black);
		}
	}

	private void Update() {
		if (_showVisualsEndTime > Time.time) {
			foreach (var renderer in _visuals) {
				renderer.material.SetColor("_Color", new Color((Mathf.Sin(Time.time * 4f) + 1f) * 0.3f, 0f, 0f));
			}
		} else {
			enabled = false;
		}
	}

	public override void ApplyDamage(DamageInfo shot) {
		if (_reciever) {
			enabled = true;
			_showVisualsEndTime = Time.time + _activationTime;
			_reciever.SetTriggerActivated(this);
		} else {
			Debug.LogError("The SecretTrigger " + gameObject.name + " is not assigned to a SecretReciever!");
		}
	}

	public void SetSecretReciever(SecretBehaviour logic) {
		_reciever = logic;
	}
}
