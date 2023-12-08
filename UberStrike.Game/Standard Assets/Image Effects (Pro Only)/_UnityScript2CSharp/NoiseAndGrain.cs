using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu ("Image Effects/Noise And Grain (Overlay)")]
public class NoiseAndGrain : PostEffectsBase {

	public float strength = 1.0f;
	public float blackIntensity = 1.0f;
	public float whiteIntensity = 1.0f;

	public float redChannelNoise = 0.975f;
	public float greenChannelNoise = 0.875f;
	public float blueChannelNoise = 1.2f;

	public float redChannelTiling = 24.0f;
	public float greenChannelTiling = 28.0f;
	public float blueChannelTiling = 34.0f;

	public FilterMode filterMode = FilterMode.Bilinear;
			
	public Shader noiseShader;
	public Texture2D noiseTexture;

	private Material noiseMaterial = null;
	
	protected virtual void OnDisable()
	{
	    if (noiseMaterial)
	        DestroyImmediate(noiseMaterial);
	}
	
	protected virtual bool CheckResources () {
		CheckSupport (false);
		
		noiseMaterial = CheckShaderAndCreateMaterial (noiseShader, noiseMaterial);
		
		if(!isSupported)
			ReportAutoDisable ();
		return isSupported;
	}
	
	protected virtual void OnRenderImage (RenderTexture source, RenderTexture destination) {		
		if(CheckResources()==false) {
			Graphics.Blit (source, destination);
			return;
		}
							
		noiseMaterial.SetVector ("_NoisePerChannel", new Vector3(redChannelNoise, greenChannelNoise, blueChannelNoise));
		noiseMaterial.SetVector ("_NoiseTilingPerChannel", new Vector3(redChannelTiling, greenChannelTiling, blueChannelTiling));
		noiseMaterial.SetVector ("_NoiseAmount", new Vector3(strength, blackIntensity, whiteIntensity));
		noiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
	   	noiseTexture.filterMode = filterMode; 

		DrawNoiseQuadGrid (source, destination, noiseMaterial, noiseTexture, 0);
	}
		
	static void DrawNoiseQuadGrid (RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr) {
		RenderTexture.active = dest;
		
		float noiseSize = (noise.width * 1.0f);
		
		float tileSize = noiseSize;
		
		float subDs = (1.0f * source.width) / tileSize;
	       
		fxMaterial.SetTexture ("_MainTex", source);	        
	                
		GL.PushMatrix ();
		GL.LoadOrtho ();	
			
		float aspectCorrection = (1.0f * source.width) / (1.0f * source.height);
		float stepSizeX = 1.0f / subDs;
		float stepSizeY = stepSizeX * aspectCorrection; 
	   	float texTile = tileSize / (noise.width * 1.0f);
	   		    	    	
		fxMaterial.SetPass (passNr);	
		
	    GL.Begin (GL.QUADS);
	    
	   	for (float x1 = 0.0f; x1 < 1.0f; x1 += stepSizeX) {
	   		for (float y1 = 0.0f; y1 < 1.0f; y1 += stepSizeY) { 
	   			
	   			float tcXStart = Random.Range (0.0f, 1.0f);
	   			float tcYStart = Random.Range (0.0f, 1.0f);
	   			
	   			tcXStart = Mathf.Floor(tcXStart*noiseSize) / noiseSize;
	   			tcYStart = Mathf.Floor(tcYStart*noiseSize) / noiseSize;
	   			
	   			//var texTileMod : float = Mathf.Sign (Random.Range (-1.0f, 1.0f));
	   			float texTileMod = 1.0f / noiseSize;
							
			    GL.MultiTexCoord2 (0, tcXStart, tcYStart); 
			    GL.MultiTexCoord2 (1, 0.0f, 0.0f); 
			    GL.Vertex3 (x1, y1, 0.1f);
			    GL.MultiTexCoord2 (0, tcXStart + texTile * texTileMod, tcYStart); 
			    GL.MultiTexCoord2 (1, 1.0f, 0.0f); 
			    GL.Vertex3 (x1 + stepSizeX, y1, 0.1f);
			    GL.MultiTexCoord2 (0, tcXStart + texTile * texTileMod, tcYStart + texTile * texTileMod); 
			    GL.MultiTexCoord2 (1, 1.0f, 1.0f); 
			    GL.Vertex3 (x1 + stepSizeX, y1 + stepSizeY, 0.1f);
			    GL.MultiTexCoord2 (0, tcXStart, tcYStart + texTile * texTileMod); 
			    GL.MultiTexCoord2 (1, 0.0f, 1.0f); 
			    GL.Vertex3 (x1, y1 + stepSizeY, 0.1f);
	   		}
	   	}
	    	
		GL.End ();
	    GL.PopMatrix ();
	}
}