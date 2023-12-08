using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Activate Advanced")]
public class UIButtonActivateExtended : MonoBehaviour {
	[SerializeField]
	private bool _state;

	[SerializeField]
	private bool _switch;

	[SerializeField]
	private GameObject[] _targets;

	private void OnClick() {
		if (_targets.Length == 0) {
			return;
		}

		foreach (var gameObject in _targets) {
			if (gameObject != null) {
				NGUITools.SetActive(gameObject, (!_switch) ? _state : (!gameObject.activeInHierarchy));
			}
		}
	}
}
