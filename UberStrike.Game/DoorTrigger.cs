using UnityEngine;

public class DoorTrigger : BaseGameProp {
	private DoorBehaviour _doorLogic;

	private void Awake() {
		gameObject.layer = 21;
	}

	public void SetDoorLogic(DoorBehaviour logic) {
		_doorLogic = logic;
	}

	public override void ApplyDamage(DamageInfo shot) {
		if (_doorLogic) {
			_doorLogic.Open();
		} else {
			Debug.LogError("The DoorCollider " + gameObject.name + " is not assigned to a DoorMechanism!");
		}
	}
}
