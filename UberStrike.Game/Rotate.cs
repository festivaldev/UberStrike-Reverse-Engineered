using UnityEngine;

public class Rotate : MonoBehaviour {
	private Transform _t;

	private void Start() {
		_t = transform;
	}

	private void Update() {
		_t.Rotate(Vector3.up, Time.deltaTime * 2f, Space.Self);
	}

	private void OnDrawGizmos() {
		if (!_t) {
			_t = transform;
		}

		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(_t.position, _t.forward);
	}
}
