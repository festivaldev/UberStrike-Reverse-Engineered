using UnityEngine;

public class MecanimEventEmitter : MonoBehaviour {
	public Animator animator;
	public UnityEngine.Object animatorController;
	public MecanimEventEmitTypes emitType;

	private void Start() {
		if (animator == null) {
			Debug.LogWarning("Do not find animator component.");
			enabled = false;

			return;
		}

		if (animatorController == null) {
			Debug.LogWarning("Please assgin animator in editor. Add emitter at runtime is not currently supported.");
			enabled = false;
		}
	}

	private void Update() {
		var events = MecanimEventManager.GetEvents(animatorController.GetInstanceID(), animator);

		foreach (var mecanimEvent in events) {
			MecanimEvent.SetCurrentContext(mecanimEvent);
			var mecanimEventEmitTypes = emitType;

			if (mecanimEventEmitTypes != MecanimEventEmitTypes.Upwards) {
				if (mecanimEventEmitTypes != MecanimEventEmitTypes.Broadcast) {
					if (mecanimEvent.paramType != MecanimEventParamTypes.None) {
						SendMessage(mecanimEvent.functionName, mecanimEvent.parameter, SendMessageOptions.DontRequireReceiver);
					} else {
						SendMessage(mecanimEvent.functionName, SendMessageOptions.DontRequireReceiver);
					}
				} else if (mecanimEvent.paramType != MecanimEventParamTypes.None) {
					BroadcastMessage(mecanimEvent.functionName, mecanimEvent.parameter, SendMessageOptions.DontRequireReceiver);
				} else {
					BroadcastMessage(mecanimEvent.functionName, SendMessageOptions.DontRequireReceiver);
				}
			} else if (mecanimEvent.paramType != MecanimEventParamTypes.None) {
				SendMessageUpwards(mecanimEvent.functionName, mecanimEvent.parameter, SendMessageOptions.DontRequireReceiver);
			} else {
				SendMessageUpwards(mecanimEvent.functionName, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
