using UnityEngine;

public class LookToTarget : MonoBehaviour {
	private Transform _follow;
	private Transform transformComponent;

	public Transform follow {
		get { return _follow; }
		set {
			_follow = value;
			enabled = _follow != null;
		}
	}

	private void Start() {
		transformComponent = transform;
	}

	private void Update() {
		if (_follow != null) {
			transformComponent.position = Vector3.Lerp(transformComponent.position, _follow.position, Time.deltaTime);
			transformComponent.rotation = Quaternion.Slerp(transformComponent.rotation, Quaternion.LookRotation(_follow.position - transformComponent.position), 0.8f * Time.deltaTime);
		} else {
			enabled = false;
		}
	}
}
