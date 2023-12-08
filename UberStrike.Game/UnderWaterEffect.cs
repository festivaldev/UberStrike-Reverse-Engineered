using UnityEngine;

[AddComponentMenu("Image Effects/UnderWater")]
[ExecuteInEditMode]
public class UnderWaterEffect : ImageEffectBase {
	public Vector2 center = new Vector2(0.5f, 0.5f);
	private float effectWeight;
	public float fadeDistance = 10f;
	public float maxAngle = 7f;
	public Vector2 radius = new Vector2(0.5f, 0.5f);
	public Texture textureRamp;

	public float Weight {
		set { effectWeight = value; }
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (Camera.main) {
			Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		}

		var num = camera.farClipPlane - camera.nearClipPlane;
		var num2 = fadeDistance / num;
		var num3 = Mathf.Cos(Time.time) * maxAngle;
		material.SetTexture("_RampTex", textureRamp);
		material.SetFloat("_FadeDistance", num2);
		material.SetFloat("_EffectWeight", effectWeight);
		ImageEffects.RenderDistortion(material, source, destination, num3, center, radius);
	}

	public void Update() { }
}
