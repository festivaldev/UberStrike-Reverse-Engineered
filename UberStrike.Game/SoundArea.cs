using UnityEngine;

public class SoundArea : MonoBehaviour {
	[SerializeField]
	private FootStepSoundType _footStep;

	private void OnTriggerEnter(Collider other) {
		SetFootStep(other);
	}

	private void OnTriggerStay(Collider other) {
		SetFootStep(other);
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Avatar") {
			var component = other.GetComponent<CharacterTrigger>();

			if (component && component.Character.Avatar != null && component.Character.Avatar.Decorator && GameState.Current.Map != null) {
				component.Character.Avatar.Decorator.CurrentFootStep = GameState.Current.Map.DefaultFootStep;
			}
		}
	}

	private void SetFootStep(Collider other) {
		if (other.tag == "Avatar") {
			var component = other.GetComponent<CharacterTrigger>();

			if (component && component.Character.Avatar != null && component.Character.Avatar.Decorator) {
				component.Character.Avatar.Decorator.CurrentFootStep = _footStep;
			}
		}
	}
}
