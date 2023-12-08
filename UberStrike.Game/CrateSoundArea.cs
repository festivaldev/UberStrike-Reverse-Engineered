using UnityEngine;

[RequireComponent(typeof(SoundArea))]
public class CrateSoundArea : MonoBehaviour {
	[SerializeField]
	private BoxCollider _boxCollider;

	private void Awake() {
		_boxCollider.isTrigger = true;
		gameObject.layer = 2;
	}
}
