using UnityEngine;

public class SoundOnTouchArea : MonoBehaviour {
	[SerializeField]
	private Transform source;

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Avatar") {
			var component = other.GetComponent<CharacterTrigger>();

			if (component && component.Character.IsLocal) {
				source.position = GameState.Current.PlayerData.Position + Vector3.down;
			}
		}
	}
}
