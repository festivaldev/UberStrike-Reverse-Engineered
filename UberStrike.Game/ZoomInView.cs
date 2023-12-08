using UnityEngine;

public class ZoomInView : MonoBehaviour {
	public bool isShown;

	[SerializeField]
	private UISprite leftBg;

	public float PADDING = 2f;

	[SerializeField]
	private UISprite rightBg;

	[SerializeField]
	private UITexture zoomReticle;

	private void UpdateReticleSize() {
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(gameObject);
		float pixelSizeAdjustment = uiroot.pixelSizeAdjustment;
		Vector3 localScale = leftBg.cachedTransform.localScale;
		localScale.x = 2f * (Screen.width * 0.5f * pixelSizeAdjustment - zoomReticle.cachedTransform.localScale.x * 0.5f + PADDING * pixelSizeAdjustment);
		leftBg.cachedTransform.localScale = localScale;
		rightBg.cachedTransform.localScale = localScale;
	}

	public void Show(bool show) {
		zoomReticle.gameObject.SetActive(show);
		leftBg.gameObject.SetActive(show);
		rightBg.gameObject.SetActive(show);
		isShown = show;
	}

	private void LateUpdate() {
		if (isShown) {
			UpdateReticleSize();
		}
	}
}
