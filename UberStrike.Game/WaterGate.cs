using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WaterGate : SecretDoor {
	private float _currentTime;
	private int _doorID;

	[SerializeField]
	private DoorElement[] _elements;

	[SerializeField]
	private float _maxTime = 1f;

	private DoorState _state;
	private float _timeToClose;

	public int DoorID {
		get { return _doorID; }
	}

	private void Awake() {
		_state = DoorState.Closed;

		foreach (var doorElement in _elements) {
			doorElement.ClosedPosition = doorElement.Element.transform.localPosition;
		}

		_doorID = transform.position.GetHashCode();
	}

	public override void Open() {
		GameState.Current.Actions.OpenDoor(DoorID);
		OpenDoor();
	}

	private void OpenDoor() {
		switch (_state) {
			case DoorState.Closed:
				_state = DoorState.Opening;
				_currentTime = 0f;

				break;
			case DoorState.Open:
				_timeToClose = Time.time + 2f;

				break;
			case DoorState.Closing:
				_state = DoorState.Opening;
				_currentTime = _maxTime - _currentTime;

				break;
		}

		if (audio) {
			audio.Play();
		}
	}

	private void OnEnable() {
		EventHandler.Global.AddListener(new Action<GameEvents.DoorOpened>(OnDoorOpenedEvent));
	}

	private void OnDisable() {
		EventHandler.Global.RemoveListener(new Action<GameEvents.DoorOpened>(OnDoorOpenedEvent));
	}

	private void OnDoorOpenedEvent(GameEvents.DoorOpened ev) {
		if (DoorID == ev.DoorID) {
			OpenDoor();
		}
	}

	private void OnTriggerEnter(Collider c) {
		if (c.tag == "Player") {
			Open();
		}
	}

	private void OnTriggerStay(Collider c) {
		if (c.tag == "Player") {
			_timeToClose = Time.time + 2f;
		}
	}

	private void Update() {
		if (_state == DoorState.Opening) {
			_currentTime += Time.deltaTime;

			foreach (var doorElement in _elements) {
				doorElement.Element.transform.localPosition = Vector3.Lerp(doorElement.ClosedPosition, doorElement.OpenPosition, _currentTime / _maxTime);
			}

			if (_currentTime >= _maxTime) {
				_state = DoorState.Open;
				_timeToClose = Time.time + 2f;

				if (audio) {
					audio.Stop();
				}
			}
		} else if (_state == DoorState.Open) {
			if (_timeToClose < Time.time) {
				_state = DoorState.Closing;
				_currentTime = 0f;

				if (audio) {
					audio.Play();
				}
			}
		} else if (_state == DoorState.Closing) {
			_currentTime += Time.deltaTime;

			foreach (var doorElement2 in _elements) {
				doorElement2.Element.transform.localPosition = Vector3.Lerp(doorElement2.OpenPosition, doorElement2.ClosedPosition, _currentTime / _maxTime);
			}

			if (_currentTime >= _maxTime) {
				_state = DoorState.Closed;

				if (audio) {
					audio.Stop();
				}
			}
		}
	}

	private enum DoorState {
		Closed,
		Opening,
		Open,
		Closing
	}

	[Serializable]
	public class DoorElement {
		[HideInInspector]
		public Vector3 ClosedPosition;

		[HideInInspector]
		public Quaternion ClosedRotation;

		public GameObject Element;
		public Vector3 OpenPosition;
	}
}
