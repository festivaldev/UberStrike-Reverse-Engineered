using System.Collections.Generic;
using UnityEngine;

public class MapConfiguration : MonoBehaviour {
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private FootStepSoundType _defaultFootStep = FootStepSoundType.Sand;

	[SerializeField]
	private Transform _defaultSpawnPoint;

	[SerializeField]
	private Transform _defaultViewPoint;

	[SerializeField]
	private bool _isEnabled = true;

	[SerializeField]
	private GameObject _spawnPoints;

	[SerializeField]
	protected GameObject _staticContentParent;

	[SerializeField]
	private Transform _waterPlane;

	private Dictionary<AudioSource, float> audioSources;

	public bool IsEnabled {
		get { return _isEnabled; }
	}

	public Transform DefaultSpawnPoint {
		get {
			Transform transform;

			try {
				if (_defaultSpawnPoint) {
					transform = _defaultSpawnPoint;
				} else {
					Debug.LogError("No DefaultSpawnPoint assigned for " + gameObject.name);
					transform = _spawnPoints.transform.GetChild(0).GetChild(0);
				}
			} catch {
				Debug.LogError("No DefaultSpawnPoint assigned for " + gameObject.name);
				transform = this.transform;
			}

			return transform;
		}
	}

	public string SceneName { get; private set; }

	public Camera Camera {
		get { return _camera; }
	}

	public FootStepSoundType DefaultFootStep {
		get { return _defaultFootStep; }
	}

	public Transform DefaultViewPoint {
		get { return _defaultViewPoint; }
	}

	public GameObject SpawnPoints {
		get { return _spawnPoints; }
	}

	public bool HasWaterPlane {
		get { return _waterPlane != null; }
	}

	public float WaterPlaneHeight {
		get { return (!_waterPlane) ? float.MinValue : _waterPlane.position.y; }
	}

	private void Awake() {
		if (_defaultViewPoint == null) {
			_defaultViewPoint = transform;
		}

		Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(SpawnPoints.GetComponentsInChildren<SpawnPoint>(true));
		GameState.Current.Map = this;
		SceneName = Singleton<SceneLoader>.Instance.CurrentScene;
	}

	public void UpdateVolumes(float volume = 1f) {
		foreach (var keyValuePair in audioSources) {
			keyValuePair.Key.volume = keyValuePair.Value * volume;
		}
	}

	private void Start() {
		audioSources = new Dictionary<AudioSource, float>();

		foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
			audioSources.Add(audioSource, audioSource.volume);
		}

		UpdateVolumes(ApplicationDataManager.ApplicationOptions.AudioMusicVolume);
	}
}
