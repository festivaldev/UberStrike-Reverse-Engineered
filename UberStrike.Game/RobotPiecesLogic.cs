using System.Collections;
using UnityEngine;

internal class RobotPiecesLogic : MonoBehaviour {
	[SerializeField]
	private GameObject _robotPieces;

	[SerializeField]
	private AudioClip[] _robotScrapsDestructionAudios;

	public void ExplodeRobot(GameObject robotObject, int lifeTimeMilliSeconds) {
		if (_robotPieces != null) {
			foreach (var rigidbody in _robotPieces.GetComponentsInChildren<Rigidbody>()) {
				rigidbody.AddExplosionForce(5f, transform.position, 2f, 0f, ForceMode.Impulse);
			}
		}

		if (_robotScrapsDestructionAudios != null && _robotScrapsDestructionAudios.Length > 0) {
			var audioClip = _robotScrapsDestructionAudios[UnityEngine.Random.Range(0, _robotScrapsDestructionAudios.Length)];

			if (audioClip) {
				audio.clip = audioClip;
				audio.Play();
			}
		}

		StartCoroutine(DestroyRobotPieces(robotObject, lifeTimeMilliSeconds));
	}

	public void PlayRobotScrapsDestructionAudio() {
		if (_robotScrapsDestructionAudios != null && _robotScrapsDestructionAudios.Length > 0) {
			var audioClip = _robotScrapsDestructionAudios[UnityEngine.Random.Range(0, _robotScrapsDestructionAudios.Length)];

			if (audioClip) {
				audio.clip = audioClip;
				audio.Play();
			}
		}
	}

	private IEnumerator DestroyRobotPieces(GameObject robotObject, int lifeTimeMilliSeconds) {
		yield return new WaitForSeconds(lifeTimeMilliSeconds / 1000);

		PlayRobotScrapsDestructionAudio();

		yield return new WaitForSeconds(audio.clip.length);

		Destroy(robotObject);
	}
}
