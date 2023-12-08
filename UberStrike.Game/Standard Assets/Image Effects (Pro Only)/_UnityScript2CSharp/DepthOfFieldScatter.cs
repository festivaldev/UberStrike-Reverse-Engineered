using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu ("Image Effects/Depth of Field (HDR, Scatter, Lens Blur)")]
public class DepthOfFieldScatter : PostEffectsBase {	
    public bool visualizeFocus = false;

	public float focalLength = 10.0f;
	public float focalSize = 0.05f; 
	public float aperture = 10.0f;

	public Transform focalTransform = null;

	public float maxBlurSize = 2.0f; 
	
	public enum BlurQuality {
		Low = 0,
		Medium = 1,
		High = 2,
	}
	
	public enum BlurResolution {
		High = 0,
		Low = 1,
	}
	 
	public BlurQuality blurQuality = BlurQuality.Medium;
	public BlurResolution blurResolution = BlurResolution.Low;

	public bool foregroundBlur = false;	
	public float foregroundOverlap = 0.55f;

	public Shader dofHdrShader;		

	private float focalDistance01 = 10.0f;	
	private Material dofHdrMaterial = null;		        
        	
	protected virtual bool CheckResources () {		
		CheckSupport (true);
	
		dofHdrMaterial = CheckShaderAndCreateMaterial (dofHdrShader, dofHdrMaterial); 
		
		if(!isSupported)
			ReportAutoDisable ();
		return isSupported;		  
	}
	
	protected virtual float FocalDistance01 (float worldDist) {
		return GetComponent<Camera>().WorldToViewportPoint((worldDist-GetComponent<Camera>().nearClipPlane) * GetComponent<Camera>().transform.forward + GetComponent<Camera>().transform.position).z / (GetComponent<Camera>().farClipPlane-GetComponent<Camera>().nearClipPlane);	
	}
			
	protected virtual void OnRenderImage (RenderTexture source, RenderTexture destination) {		
		if(CheckResources () == false) {
			Graphics.Blit (source, destination);
			return;
		}
		
		int i = 0;
		float internalBlurWidth = maxBlurSize;
		float blurRtDivider = blurResolution == BlurResolution.High ? 1 : 2;
		
		// clamp values so they make sense

		if (aperture < 0.0f) aperture = 0.0f;
		if (maxBlurSize < 0.0f) maxBlurSize = 0.0f;
		focalSize = Mathf.Clamp(focalSize, 0.0f, 0.3f);
					
		// focal & coc calculations

		focalDistance01 = focalTransform ? (GetComponent<Camera>().WorldToViewportPoint (focalTransform.position)).z / (GetComponent<Camera>().farClipPlane) : FocalDistance01 (focalLength);
		
		bool isInHdr = source.format == RenderTextureFormat.ARGBHalf;
		
		RenderTexture scene = blurRtDivider > 1 ? RenderTexture.GetTemporary ((int)(source.width/blurRtDivider), (int)(source.height/blurRtDivider), 0, source.format) : null;			
		if (scene) scene.filterMode = FilterMode.Bilinear;
		RenderTexture rtLow = RenderTexture.GetTemporary ((int)(source.width/(2*blurRtDivider)), (int)(source.height/(2*blurRtDivider)), 0, source.format);		
		RenderTexture rtLow2 = RenderTexture.GetTemporary ((int)(source.width/(2*blurRtDivider)),(int)(source.height/(2*blurRtDivider)), 0, source.format);			
		if (rtLow) rtLow.filterMode = FilterMode.Bilinear;
		if (rtLow2) rtLow2.filterMode = FilterMode.Bilinear;
	
		dofHdrMaterial.SetVector ("_CurveParams", new Vector4 (0.0f, focalSize, aperture/10.0f, focalDistance01));
		
		// foreground blur
		
		if (foregroundBlur) {			
			RenderTexture rtLowTmp = RenderTexture.GetTemporary ((int)(source.width/(2*blurRtDivider)), (int)(source.height/(2*blurRtDivider)), 0, source.format);		
		
			// Capture foreground CoC only in alpha channel and increase CoC radius
			Graphics.Blit (source, rtLow2, dofHdrMaterial, 4); 
			dofHdrMaterial.SetTexture("_FgOverlap", rtLow2); 
			
			float fgAdjustment = internalBlurWidth * foregroundOverlap * 0.225f; 
			dofHdrMaterial.SetVector ("_Offsets", new Vector4 (0.0f, fgAdjustment , 0.0f, fgAdjustment));
			Graphics.Blit (rtLow2, rtLowTmp, dofHdrMaterial, 2);
			dofHdrMaterial.SetVector ("_Offsets", new Vector4 (fgAdjustment, 0.0f, 0.0f, fgAdjustment));		
			Graphics.Blit (rtLowTmp, rtLow, dofHdrMaterial, 2);	 			
			
			dofHdrMaterial.SetTexture("_FgOverlap", null); // NEW: not needed anymore
			// apply adjust FG coc back to high rez coc texture
			Graphics.Blit(rtLow, source, dofHdrMaterial, 7);	
			
			RenderTexture.ReleaseTemporary(rtLowTmp);					
		}
		else 
			dofHdrMaterial.SetTexture("_FgOverlap", null); // ugly FG overlaps as a result
		
		// capture remaing CoC (fore & *background*)
		
		Graphics.Blit (source, source, dofHdrMaterial, foregroundBlur ? 3 : 0);		
		
		RenderTexture cocRt = source;
		
		if(blurRtDivider>1) {
			Graphics.Blit (source, scene, dofHdrMaterial, 6);		
			cocRt = scene;	
		}
		
		// spawn a few low rez parts in high rez image to get a bigger blur
		// resulting quality is higher than directly blending preblurred buffers
		
		Graphics.Blit(cocRt, rtLow2, dofHdrMaterial, 6); 
		Graphics.Blit(rtLow2, cocRt, dofHdrMaterial, 8);
		
		//  blur and apply to color buffer 
		
		int blurPassNumber = 10;
		switch(blurQuality) {
			case BlurQuality.Low:
				blurPassNumber = blurRtDivider > 1 ? 13 : 10;
				break;
			case BlurQuality.Medium:
				blurPassNumber = blurRtDivider > 1 ? 12 : 11;
				break;
			case BlurQuality.High:
				blurPassNumber = blurRtDivider > 1 ? 15 : 14;
				break;				
			default:
				Debug.Log("DOF couldn't find valid blur quality setting", transform);
				break;
		}
		
		if(visualizeFocus) {
			Graphics.Blit (source, destination, dofHdrMaterial, 1);
		}
		else { 		 
			dofHdrMaterial.SetVector ("_Offsets", new Vector4 (0.0f, 0.0f , 0.0f, internalBlurWidth));
			dofHdrMaterial.SetTexture("_LowRez", cocRt); // only needed in low resolution profile. and then, ugh, we get an ugly transition from nonblur -> blur areas
			Graphics.Blit (source, destination, dofHdrMaterial, blurPassNumber);	 
		}
		
		if(rtLow) RenderTexture.ReleaseTemporary(rtLow);
		if(rtLow2) RenderTexture.ReleaseTemporary(rtLow2);		
		if(scene) RenderTexture.ReleaseTemporary(scene); 
	}	
}
