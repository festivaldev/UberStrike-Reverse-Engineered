using UnityEngine;

public class MoveTrailrendererObject : MonoBehaviour {
	private float _alpha = 1f;

	[SerializeField]
	private float _duration = 0.1f;

	[SerializeField]
	private LineRenderer _lineRenderer;

	private float _locationOnPath;
	private bool _move;
	private float _timeToArrive = 1f;

	public void MoveTrail(Vector3 destination, Vector3 muzzlePosition, float distance) {
		if (_lineRenderer != null) {
			_alpha = 1f;
			_move = true;
			_lineRenderer.SetPosition(0, muzzlePosition);
			_lineRenderer.SetPosition(1, destination);
			_timeToArrive = Time.time + _duration;
		}
	}

	private void Update() {
		if (_move) {
			_locationOnPath = 1f - (_timeToArrive - Time.time);
			_alpha = Mathf.Lerp(_alpha, 0f, _locationOnPath);
			var color = _lineRenderer.material.GetColor("_TintColor");
			color.a = _alpha;
			_lineRenderer.materials[0].SetColor("_TintColor", color);

			if (Time.time >= _timeToArrive) {
				_move = false;
				Destroy(gameObject);
			}
		}
	}
}
