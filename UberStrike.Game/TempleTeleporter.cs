using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TempleTeleporter : SecretDoor {
	[SerializeField]
	private float _activationTime = 15f;

	private AudioSource[] _audios;
	private int _doorID;

	[SerializeField]
	private ParticleEmitter _particles;

	[SerializeField]
	private Transform _spawnpoint;

	private float _timeOut;

	[SerializeField]
	private Renderer[] _visuals;

	public int DoorID {
		get { return _doorID; }
	}

	private void Awake() {
		_audios = GetComponents<AudioSource>();
		_particles.emit = false;

		foreach (var renderer in _visuals) {
			renderer.enabled = false;
		}

		_doorID = transform.position.GetHashCode();
	}

	private void OnEnable() {
		EventHandler.Global.AddListener(new Action<GameEvents.DoorOpened>(OnDoorOpenedEvent));
	}

	private void OnDisable() {
		EventHandler.Global.RemoveListener(new Action<GameEvents.DoorOpened>(OnDoorOpenedEvent));
	}

	private void Update() {
		if (_timeOut < Time.time) {
			foreach (var audioSource in _audios) {
				audioSource.Stop();
			}

			_particles.emit = false;

			foreach (var renderer in _visuals) {
				renderer.enabled = false;
			}

			enabled = false;
		}
	}

	private void OnTriggerEnter(Collider c) {
		if (c.tag == "Player" && _timeOut > Time.time) {
			_timeOut = 0f;
			GameState.Current.Player.SpawnPlayerAt(_spawnpoint.position, _spawnpoint.rotation);
		}
	}

	private void OnDoorOpenedEvent(GameEvents.DoorOpened ev) {
		if (DoorID == ev.DoorID) {
			OpenDoor();
		}
	}

	public override void Open() {
		if (GameState.Current.HasJoinedGame) {
			GameState.Current.Actions.OpenDoor(DoorID);
		}

		OpenDoor();
	}

	private void OpenDoor() {
		enabled = true;
		_particles.emit = true;

		foreach (var renderer in _visuals) {
			renderer.enabled = true;
		}

		_timeOut = Time.time + _activationTime;

		foreach (var audioSource in _audios) {
			audioSource.Play();
		}
	}
}
