using UnityEngine;

public class PickupBounce : MonoBehaviour {
	private float origPosY;
	private float startOffset;

	private void Awake() {
		origPosY = transform.position.y;
		startOffset = UnityEngine.Random.value * 3f;
	}

	private void FixedUpdate() {
		transform.Rotate(new Vector3(0f, 2f, 0f));
		transform.position = new Vector3(transform.position.x, origPosY + Mathf.Sin((startOffset + Time.realtimeSinceStartup) * 4f) * 0.08f, transform.position.z);
	}
}
