using UnityEngine;

public class SelfDestroy : MonoBehaviour {
	[SerializeField]
	private float _destroyInSeconds = 1f;

	private void Start() {
		Destroy(gameObject, _destroyInSeconds);
	}

	public void SetDelay(float seconds) {
		_destroyInSeconds = seconds;
	}
}
