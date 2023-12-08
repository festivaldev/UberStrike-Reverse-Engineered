using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
	public void Destroy() {
		UnityEngine.Object.Destroy(gameObject);
	}

	public void Destroy(int timeDelay) {
		UnityEngine.Object.Destroy(gameObject, timeDelay);
	}

	public void SetParent(Transform parent) {
		transform.parent = parent;
	}
}
