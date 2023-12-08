using UnityEngine;

[AddComponentMenu("Image Effects/Mobile Bloom")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MobileBloom : MonoBehaviour {
	public float agonyTint;
	private Material apply;
	private Shader bloomShader = new Shader();
	public Color colorMix = Color.white;
	public float colorMixBlend = 0.25f;
	public float intensity = 0.5f;
	public float intensityBoost;
	private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

	private void OnEnable() {
		FindShaders();
		CheckSupport();
		CreateMaterials();
	}

	private void FindShaders() {
		if (!bloomShader) {
			bloomShader = Shader.Find("Cross Platform Shaders/Mobile Bloom");
		}
	}

	private void CreateMaterials() {
		if (!apply) {
			apply = new Material(bloomShader);
			apply.hideFlags = HideFlags.DontSave;
		}
	}

	private bool CheckSupport() {
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures || !bloomShader.isSupported) {
			enabled = false;

			return false;
		}

		rtFormat = ((!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565)) ? RenderTextureFormat.Default : RenderTextureFormat.RGB565);

		return true;
	}

	private void OnDisable() {
		if (apply) {
			DestroyImmediate(apply);
			apply = null;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		agonyTint = Mathf.Clamp01(agonyTint - Time.deltaTime * 1.25f);
		intensityBoost = Mathf.Clamp01(intensityBoost - Time.deltaTime * 0.75f);
		var temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, rtFormat);
		var temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, rtFormat);
		apply.SetColor("_ColorMix", colorMix);
		apply.SetVector("_Parameter", new Vector4(colorMixBlend * 0.25f, 0f, 0f, 1f - intensity - (agonyTint + intensityBoost)));
		Graphics.Blit(source, temporary, apply, (agonyTint >= 0.5f) ? 5 : 1);
		Graphics.Blit(temporary, temporary2, apply, 2);
		Graphics.Blit(temporary2, temporary, apply, 3);
		apply.SetTexture("_Bloom", temporary);
		Graphics.Blit(source, destination, apply, 4);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
