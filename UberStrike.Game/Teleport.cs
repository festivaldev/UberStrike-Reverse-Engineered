using UnityEngine;

public class Teleport : MonoBehaviour {
	private AudioSource _audio;

	[SerializeField]
	private AudioClip _sound;

	[SerializeField]
	private Transform _spawnPoint;

	private void Awake() {
		_audio = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider c) {
		if (c.tag == "Player") {
			if (_audio) {
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(_sound);
			}

			GameState.Current.Player.SpawnPlayerAt(_spawnPoint.position, _spawnPoint.rotation);
		} else if (c.tag == "Prop") {
			c.transform.position = _spawnPoint.position;
		}
	}
}
