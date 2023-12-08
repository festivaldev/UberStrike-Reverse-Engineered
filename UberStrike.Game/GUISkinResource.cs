using UnityEngine;

internal class GUISkinResource : MonoBehaviour {
	[SerializeField]
	private GUISkin blueStonez;

	[SerializeField]
	private GUISkin stormFront;

	private void Awake() {
		BlueStonez.Initialize(blueStonez);
		StormFront.Initialize(stormFront);
	}
}
