using UnityEngine;

public class PlayerDamageFeedback : MonoBehaviour {
	private int colorIndex;
	public Color[] DamageColors;
	private Material damageSplat;
	public float Factor;

	private void Awake() {
		damageSplat = renderer.material;
	}

	public void RandomizeDamageFeedbackcolor() {
		colorIndex = UnityEngine.Random.Range(0, 5);
	}

	public void ShowDamageFeedback(float damage) {
		DamageColors[colorIndex].a = damage * Factor;
		damageSplat.color = DamageColors[colorIndex];
	}
}
