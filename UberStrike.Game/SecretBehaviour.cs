using System;
using UnityEngine;

public class SecretBehaviour : MonoBehaviour {
	[SerializeField]
	private Door[] _doors;

	private void Awake() {
		foreach (var door in _doors) {
			foreach (var secretTrigger in door.Trigger) {
				secretTrigger.SetSecretReciever(this);
			}
		}
	}

	public void SetTriggerActivated(SecretTrigger trigger) {
		foreach (var door in _doors) {
			door.CheckAllTriggers();
		}
	}

	[Serializable]
	public class Door {
		public string _description;

		[SerializeField]
		private SecretDoor _door;

		[SerializeField]
		private SecretTrigger[] _trigger;

		public SecretTrigger[] Trigger {
			get { return _trigger; }
		}

		public void CheckAllTriggers() {
			var flag = true;

			foreach (var secretTrigger in _trigger) {
				flag &= secretTrigger.ActivationTimeOut > Time.time;
			}

			if (flag) {
				_door.Open();
			}
		}
	}
}
