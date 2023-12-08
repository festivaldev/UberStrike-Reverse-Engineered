using UnityEngine;

public class InitSceneLoader : MonoBehaviour {
	private void Awake() {
		if (!GlobalSceneLoader.IsInitialised) {
			Application.LoadLevel("InitScene");
		}
	}
}
