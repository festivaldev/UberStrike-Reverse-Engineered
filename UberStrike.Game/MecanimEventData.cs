using UnityEngine;

public class MecanimEventData : MonoBehaviour {
	public Animator animator;
	public UnityEngine.Object animatorController;
	public MecanimEventDataEntry[] data;

	private void Start() {
		if (animator == null) {
			Debug.LogWarning("Do not find animator component.");
			enabled = false;

			return;
		}

		if (animatorController == null) {
			Debug.LogWarning("Please assgin animator in editor. Add emitter at runtime is not currently supported.");
			enabled = false;

			return;
		}

		MecanimEventManager.SetEventDataSource(this);
	}

	private void Update() {
		foreach (var mecanimEvent in MecanimEventManager.GetEvents(animatorController.GetInstanceID(), animator)) {
			if (mecanimEvent.paramType != MecanimEventParamTypes.None) {
				SendMessage(mecanimEvent.functionName, mecanimEvent.parameter, SendMessageOptions.DontRequireReceiver);
			} else {
				SendMessage(mecanimEvent.functionName, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
