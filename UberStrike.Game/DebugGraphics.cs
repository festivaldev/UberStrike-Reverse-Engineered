using UnityEngine;

public class DebugGraphics : IDebugPage {
	public string Title {
		get { return "Graphics"; }
	}

	public void Draw() {
		GUILayout.Label("graphicsDeviceID: " + SystemInfo.graphicsDeviceID);
		GUILayout.Label("graphicsDeviceNameD: " + SystemInfo.graphicsDeviceName);
		GUILayout.Label("graphicsDeviceVendorD: " + SystemInfo.graphicsDeviceVendor);
		GUILayout.Label("graphicsDeviceVendorIDD: " + SystemInfo.graphicsDeviceVendorID);
		GUILayout.Label("graphicsDeviceVersionD: " + SystemInfo.graphicsDeviceVersion);
		GUILayout.Label("graphicsMemorySizeD: " + SystemInfo.graphicsMemorySize);
		GUILayout.Label("graphicsPixelFillrateD: " + SystemInfo.graphicsPixelFillrate);
		GUILayout.Label("graphicsShaderLevelD: " + SystemInfo.graphicsShaderLevel);
		GUILayout.Label("supportedRenderTargetCountD: " + SystemInfo.supportedRenderTargetCount);
		GUILayout.Label("supportsImageEffectsD: " + SystemInfo.supportsImageEffects);
		GUILayout.Label("supportsRenderTexturesD: " + SystemInfo.supportsRenderTextures);
		GUILayout.Label("supportsShadowsD: " + SystemInfo.supportsShadows);
		GUILayout.Label("supportsVertexPrograms: " + SystemInfo.supportsVertexPrograms);
		QualitySettings.pixelLightCount = CmuneGUI.HorizontalScrollbar("pixelLightCount: ", QualitySettings.pixelLightCount, 0, 10);
		QualitySettings.masterTextureLimit = CmuneGUI.HorizontalScrollbar("masterTextureLimit: ", QualitySettings.masterTextureLimit, 0, 20);
		QualitySettings.maxQueuedFrames = CmuneGUI.HorizontalScrollbar("maxQueuedFrames: ", QualitySettings.maxQueuedFrames, 0, 10);
		QualitySettings.maximumLODLevel = CmuneGUI.HorizontalScrollbar("maximumLODLevel: ", QualitySettings.maximumLODLevel, 0, 7);
		QualitySettings.vSyncCount = CmuneGUI.HorizontalScrollbar("vSyncCount: ", QualitySettings.vSyncCount, 0, 2);
		QualitySettings.antiAliasing = CmuneGUI.HorizontalScrollbar("antiAliasing: ", QualitySettings.antiAliasing, 0, 4);
		QualitySettings.lodBias = CmuneGUI.HorizontalScrollbar("lodBias: ", QualitySettings.lodBias, 0, 4);
		Shader.globalMaximumLOD = CmuneGUI.HorizontalScrollbar("globalMaximumLOD: ", Shader.globalMaximumLOD, 100, 600);
	}
}
