using System.Collections;
using UnityEngine;

public class QuickItemSfx : MonoBehaviour {
	private bool _isShortAudio;

	[SerializeField]
	private AudioClip _normalLoopAudio;

	[SerializeField]
	private GameObject _robotPiecesPrefab;

	[SerializeField]
	private Transform _robotTransform;

	[SerializeField]
	private AudioClip _shortLoopAudio;

	public int ID { get; set; }

	public bool IsShortAudio {
		get { return _isShortAudio; }
		set {
			_isShortAudio = value;
			var componentInChildren = GetComponentInChildren<AudioSource>();

			if (componentInChildren != null) {
				componentInChildren.clip = ((!_isShortAudio) ? _normalLoopAudio : _shortLoopAudio);
			}
		}
	}

	public Transform Parent { get; set; }
	public Vector3 Offset { get; set; }

	public void Play(int robotLifeTime, int scrapsLifeTime, bool isInstant) {
		IsShortAudio = isInstant;
		var componentInChildren = GetComponentInChildren<AudioSource>();

		if (componentInChildren != null) {
			componentInChildren.Play();
		}

		StartCoroutine(StopEffectAfterSeconds(robotLifeTime, scrapsLifeTime));
	}

	public void Explode(int scrapsLifeTime) {
		var gameObject = Instantiate(_robotPiecesPrefab, _robotTransform.position, Quaternion.identity) as GameObject;

		if (gameObject != null) {
			var componentInChildren = gameObject.GetComponentInChildren<RobotPiecesLogic>();
			componentInChildren.ExplodeRobot(gameObject, scrapsLifeTime);
		}

		Destroy();
	}

	public void Destroy() {
		UnityEngine.Object.Destroy(gameObject);
	}

	private IEnumerator StopEffectAfterSeconds(int robotLifeTime, int scrapsLifeTime) {
		yield return new WaitForSeconds(robotLifeTime / 1000);

		Singleton<QuickItemSfxController>.Instance.RemoveEffect(ID);
		Explode(scrapsLifeTime);
	}
}
