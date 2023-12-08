using System.Collections.Generic;
using UnityEngine;

public class ReflectionFx : MonoBehaviour {
	public Color clearColor = Color.black;
	public float clipPlaneOffset = 0.07f;
	private Dictionary<Camera, bool> helperCameras;
	private Texture[] initialReflectionTextures;
	private Vector3 oldpos = Vector3.zero;
	private Camera reflectionCamera;
	public LayerMask reflectionMask;
	public string reflectionSampler = "_ReflectionTex";
	public Material[] reflectiveMaterials;
	public Transform[] reflectiveObjects;
	private Transform reflectiveSurfaceHeight;
	public Shader replacementShader;

	private void Start() {
		initialReflectionTextures = new Texture2D[reflectiveMaterials.Length];

		for (var i = 0; i < reflectiveMaterials.Length; i++) {
			initialReflectionTextures[i] = reflectiveMaterials[i].GetTexture(reflectionSampler);
		}

		if (!SystemInfo.supportsRenderTextures) {
			enabled = false;
		}
	}

	private void OnDisable() {
		if (initialReflectionTextures == null) {
			return;
		}

		for (var i = 0; i < reflectiveMaterials.Length; i++) {
			reflectiveMaterials[i].SetTexture(reflectionSampler, initialReflectionTextures[i]);
		}
	}

	private void LateUpdate() {
		Transform transform = null;
		var num = float.PositiveInfinity;
		var position = Camera.main.transform.position;

		foreach (var transform2 in reflectiveObjects) {
			if (transform2.renderer.isVisible) {
				var sqrMagnitude = (position - transform2.position).sqrMagnitude;

				if (sqrMagnitude < num) {
					num = sqrMagnitude;
					transform = transform2;
				}
			}
		}

		if (transform == null) {
			return;
		}

		reflectiveSurfaceHeight = transform;
		RenderHelpCameras(Camera.main);

		if (helperCameras != null) {
			helperCameras.Clear();
		}
	}

	public void RenderHelpCameras(Camera currentCam) {
		if (helperCameras == null) {
			helperCameras = new Dictionary<Camera, bool>();
		}

		if (!helperCameras.ContainsKey(currentCam)) {
			helperCameras.Add(currentCam, false);
		}

		if (helperCameras[currentCam]) {
			return;
		}

		if (reflectionCamera == null) {
			reflectionCamera = CreateReflectionCameraFor(currentCam);

			foreach (var material in reflectiveMaterials) {
				material.SetTexture(reflectionSampler, reflectionCamera.targetTexture);
			}
		}

		RenderReflectionFor(currentCam, reflectionCamera);
		helperCameras[currentCam] = true;
	}

	private void RenderReflectionFor(Camera cam, Camera reflectCamera) {
		if (reflectCamera == null) {
			return;
		}

		SaneCameraSettings(reflectCamera);
		reflectCamera.backgroundColor = clearColor;
		reflectCamera.enabled = true;
		GL.SetRevertBackfacing(true);
		var transform = reflectiveSurfaceHeight;
		var eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		var position = transform.transform.position;
		position.y = transform.position.y;
		var up = transform.transform.up;
		var num = -Vector3.Dot(up, position) - clipPlaneOffset;
		var vector = new Vector4(up.x, up.y, up.z, num);
		var matrix4x = Matrix4x4.zero;
		matrix4x = CalculateReflectionMatrix(matrix4x, vector);
		oldpos = cam.transform.position;
		var vector2 = matrix4x.MultiplyPoint(oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		var vector3 = CameraSpacePlane(reflectCamera, position, up, 1f);
		var matrix4x2 = cam.projectionMatrix;
		matrix4x2 = CalculateObliqueMatrix(matrix4x2, vector3);
		reflectCamera.projectionMatrix = matrix4x2;
		reflectCamera.transform.position = vector2;
		var eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		reflectCamera.RenderWithShader(replacementShader, "Reflection");
		GL.SetRevertBackfacing(false);
	}

	private Camera CreateReflectionCameraFor(Camera cam) {
		var text = this.gameObject.name + "Reflection" + cam.name;
		Debug.Log("Created internal reflection camera " + text);
		var gameObject = GameObject.Find(text);

		if (!gameObject) {
			gameObject = new GameObject(text, typeof(Camera));
		}

		if (!gameObject.GetComponent(typeof(Camera))) {
			gameObject.AddComponent(typeof(Camera));
		}

		var camera = gameObject.camera;
		camera.backgroundColor = clearColor;
		camera.clearFlags = CameraClearFlags.Color;
		SetStandardCameraParameter(camera, reflectionMask);

		if (!camera.targetTexture) {
			camera.targetTexture = CreateTextureFor(cam);
		}

		return camera;
	}

	private RenderTexture CreateTextureFor(Camera cam) {
		var renderTextureFormat = RenderTextureFormat.RGB565;

		if (!SystemInfo.SupportsRenderTextureFormat(renderTextureFormat)) {
			renderTextureFormat = RenderTextureFormat.Default;
		}

		var num = 0.5f;

		return new RenderTexture(Mathf.FloorToInt(cam.pixelWidth * num), Mathf.FloorToInt(cam.pixelHeight * num), 24, renderTextureFormat) {
			hideFlags = HideFlags.DontSave
		};
	}

	private void SaneCameraSettings(Camera helperCam) {
		helperCam.depthTextureMode = DepthTextureMode.None;
		helperCam.backgroundColor = Color.black;
		helperCam.clearFlags = CameraClearFlags.Color;
		helperCam.renderingPath = RenderingPath.Forward;
	}

	private void SetStandardCameraParameter(Camera cam, LayerMask mask) {
		cam.backgroundColor = Color.black;
		cam.enabled = false;
		cam.cullingMask = reflectionMask;
	}

	private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane) {
		var vector = projection.inverse * new Vector4(sgn(clipPlane.x), sgn(clipPlane.y), 1f, 1f);
		var vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, vector));
		projection[2] = vector2.x - projection[3];
		projection[6] = vector2.y - projection[7];
		projection[10] = vector2.z - projection[11];
		projection[14] = vector2.w - projection[15];

		return projection;
	}

	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane) {
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;

		return reflectionMat;
	}

	private static float sgn(float a) {
		if (a > 0f) {
			return 1f;
		}

		if (a < 0f) {
			return -1f;
		}

		return 0f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign) {
		var vector = pos + normal * clipPlaneOffset;
		var worldToCameraMatrix = cam.worldToCameraMatrix;
		var vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		var vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;

		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}
}
