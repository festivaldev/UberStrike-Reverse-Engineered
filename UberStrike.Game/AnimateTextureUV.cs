using UnityEngine;

public class AnimateTextureUV : MonoBehaviour {
	public int framesPerSecond = 10;
	public int uvAnimationTileX = 1;
	public int uvAnimationTileY = 1;

	private void Update() {
		var num = Mathf.RoundToInt(Time.time * framesPerSecond);
		num %= uvAnimationTileX * uvAnimationTileY;
		var vector = new Vector2(1f / uvAnimationTileX, 1f / uvAnimationTileY);
		var num2 = num % uvAnimationTileX;
		var num3 = num / uvAnimationTileX;
		var vector2 = new Vector2(num2 * vector.x, 1f - vector.y - num3 * vector.y);

		if (renderer) {
			renderer.material.SetTextureOffset("_MainTex", vector2);
			renderer.material.SetTextureScale("_MainTex", vector);
		}
	}
}
