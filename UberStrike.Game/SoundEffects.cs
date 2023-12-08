using UnityEngine;

public class SoundEffects : MonoBehaviour {
	public static SoundEffects Instance;
	public SoundEffectTunable Health_100_200_Increase;
	public SoundEffectTunable HealthHeartbeat_0_25;
	public SoundEffectTunable HealthNoise_0_25;

	private void Awake() {
		Instance = this;
		DontDestroyOnLoad(Instance.gameObject);
	}
}
