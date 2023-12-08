using System;
using UnityEngine;

// pseudo image effect that displays useful info for your image effects
[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu ("Image Effects/Camera Info")]
public class CameraInfo : MonoBehaviour {

	// display current depth texture mode
	public DepthTextureMode currentDepthMode;
	// render path
	public RenderingPath currentRenderPath;
	// number of official image fx used
	public int recognizedPostFxCount = 0;
	
#if UNITY_EDITOR	
	protected virtual void Start () {
		UpdateInfo ();		
	}

	protected virtual void Update () {
		if (currentDepthMode != GetComponent<Camera>().depthTextureMode)
			GetComponent<Camera>().depthTextureMode = currentDepthMode;
		if (currentRenderPath != GetComponent<Camera>().actualRenderingPath)
			GetComponent<Camera>().renderingPath = currentRenderPath;
			
		UpdateInfo ();
	}
	
	protected virtual void UpdateInfo () {
		currentDepthMode = GetComponent<Camera>().depthTextureMode;
		currentRenderPath = GetComponent<Camera>().actualRenderingPath;
		PostEffectsBase[] fx = gameObject.GetComponents<PostEffectsBase> ();
		int fxCount = 0;
		foreach (PostEffectsBase post in fx) 
			if (post.enabled)
				fxCount++;
		recognizedPostFxCount = fxCount;		
	}
#endif
}
