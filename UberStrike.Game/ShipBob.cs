using UnityEngine;

public class ShipBob : MonoBehaviour {
	private Transform _transform;

	[SerializeField]
	private float moveAmount = 0.005f;

	[SerializeField]
	private float rotateAmount = 1f;

	private Vector3 shipRotation;

	private void Awake() {
		_transform = transform;
		shipRotation = _transform.localRotation.eulerAngles;
	}

	private void Update() {
		_transform.position = new Vector3(_transform.position.x, _transform.position.y + Mathf.Sin(Time.time) * moveAmount, _transform.position.z);
		var num = Mathf.Sin(Time.time) * rotateAmount;
		_transform.localRotation = Quaternion.Euler(shipRotation + new Vector3(num, num, num));
	}
}
