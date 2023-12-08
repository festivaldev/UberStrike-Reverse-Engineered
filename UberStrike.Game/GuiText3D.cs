using System.Collections;
using UnityEngine;

public class GuiText3D : MonoBehaviour {
	private GUIText _guiText;
	private Material _material;
	private Transform _transform;
	private Vector3 _viewportPosition;
	private Vector3 fadeDir = Vector3.zero;
	private Color finalColor;
	public Camera mCamera;
	public Color mColor = Color.black;
	public Vector3 mFadeDirection = Vector2.up;
	public bool mFadeOut = true;
	public Font mFont;
	public float mLifeTime = 5f;
	public float mMaxDistance = 20f;
	public Transform mTarget;
	public string mText;
	private Color startColor;
	private float time;

	private void Awake() {
		_transform = transform;
	}

	private void Start() {
		_guiText = gameObject.AddComponent(typeof(GUIText)) as GUIText;
		_guiText.alignment = TextAlignment.Center;
		_guiText.anchor = TextAnchor.MiddleCenter;

		if (mCamera == null || mTarget == null || mFont == null) {
			Destroy(gameObject);

			return;
		}

		_guiText.font = mFont;
		_guiText.text = mText;
		_guiText.material = mFont.material;
		_material = _guiText.material;
		startColor = _material.color;
		finalColor = _material.color;

		if (mFadeOut) {
			finalColor.a = 0f;
		}
	}

	private void LateUpdate() {
		if (mCamera != null && mTarget != null && (mLifeTime < 0f || mLifeTime > time)) {
			time += Time.deltaTime;
			_viewportPosition = mCamera.WorldToViewportPoint(mTarget.localPosition);

			if (mFadeOut && mLifeTime > 0f) {
				_material.color = Color.Lerp(startColor, finalColor, time / mLifeTime);
			} else {
				var num = Mathf.Clamp01(_viewportPosition.z / mMaxDistance);
				_material.color = Color.Lerp(startColor, finalColor, num);
			}

			fadeDir += Time.deltaTime * mFadeDirection;
			_transform.localPosition = _viewportPosition + fadeDir;
		} else {
			Destroy(gameObject);
		}
	}

	private IEnumerator startShowGuiText(float mLifeTime) {
		var time = 0f;
		var fadeDir = Vector3.zero;
		var startColor = _material.color;
		var finalColor = _material.color;

		if (mFadeOut) {
			finalColor.a = 0f;
		}

		while (mCamera != null && mTarget != null && (mLifeTime < 0f || mLifeTime > time)) {
			time += Time.deltaTime;
			_viewportPosition = mCamera.WorldToViewportPoint(mTarget.localPosition);

			if (mFadeOut && mLifeTime > 0f) {
				_material.color = Color.Lerp(startColor, finalColor, time / mLifeTime);
			} else {
				var dist = Mathf.Clamp01(_viewportPosition.z / mMaxDistance);
				_material.color = Color.Lerp(startColor, finalColor, dist);
			}

			fadeDir += Time.deltaTime * mFadeDirection;
			_transform.localPosition = _viewportPosition + fadeDir;

			yield return new WaitForEndOfFrame();
		}

		Destroy(gameObject);
	}
}
