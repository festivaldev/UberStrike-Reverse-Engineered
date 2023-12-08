using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu ("Image Effects/Screen Overlay")]
public class ScreenOverlay : PostEffectsBase {
	
	public enum OverlayBlendMode {
		AddSub = 0,
		ScreenBlend = 1,
		Multiply = 2,
        Overlay = 3,
        AlphaBlend = 4,	
	}
	
	public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;
	public float intensity = 1.0f;
	public Texture2D texture;
			
	public Shader overlayShader;
	
	private Material overlayMaterial = null;
	
	protected virtual void OnDisable()
	{
	    if (overlayMaterial)
	        DestroyImmediate(overlayMaterial);
	}
	
	protected virtual bool CheckResources () {
		CheckSupport (false);
		
		overlayMaterial = CheckShaderAndCreateMaterial (overlayShader, overlayMaterial);
		
		if(!isSupported)
			ReportAutoDisable ();
		return isSupported;
	}
	
	protected virtual void OnRenderImage (RenderTexture source, RenderTexture destination) {		
		if(CheckResources()==false) {
			Graphics.Blit (source, destination);
			return;
		}
		overlayMaterial.SetFloat ("_Intensity", intensity);
		overlayMaterial.SetTexture ("_Overlay", texture);
		Graphics.Blit (source, destination, overlayMaterial, (int)blendMode);
	}
}