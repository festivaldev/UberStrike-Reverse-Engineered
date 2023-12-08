using UnityEngine;

public class MobileShadowReceiver : MonoBehaviour {
	public void OnWillRenderObject() {
		Camera camera = null;

		for (var i = 0; i < Camera.allCameras.Length; i++) {
			if (Camera.allCameras[i].name == "Shadow Camera") {
				camera = Camera.allCameras[i];

				break;
			}
		}

		if (camera != null) {
			var matrix4x = camera.projectionMatrix * camera.worldToCameraMatrix * renderer.localToWorldMatrix;
			renderer.material.SetMatrix("_LocalToShadowMatrix", matrix4x);
		}
	}
}
