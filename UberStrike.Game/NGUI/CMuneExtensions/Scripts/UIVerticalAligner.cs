using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Vertical Aligner")]
[ExecuteInEditMode]
public class UIVerticalAligner : MonoBehaviour {
	public enum Direction {
		TopToBottom,
		BottomToTop
	}

	public UIVerticalAligner.Direction direction;
	public bool hideInactive = true;
	private bool mStarted;
	public float padding;

	[SerializeField]
	private bool repositionNow;

	public bool sorted = true;

	public void Reposition() {
		this.repositionNow = true;
	}

	private void Start() {
		this.mStarted = true;
		this.Reposition();
	}

	private void LateUpdate() {
		if (this.repositionNow) {
			this.repositionNow = false;
			this.DoReposition();
		}
	}

	private void DoReposition() {
		if (!this.mStarted) {
			this.repositionNow = true;

			return;
		}

		List<Transform> list = new List<Transform>();

		foreach (object obj in base.transform) {
			Transform transform = (Transform)obj;

			if (transform != null && transform.gameObject && (!this.hideInactive || NGUITools.GetActive(transform.gameObject))) {
				list.Add(transform);
			}
		}

		if (this.sorted) {
			list.Sort((Transform el1, Transform el2) => el1.name.CompareTo(el2.name));
		}

		if (this.direction == UIVerticalAligner.Direction.BottomToTop) {
			list.Reverse();
		}

		float num = 0f;

		foreach (Transform transform2 in list) {
			UIWidget component = transform2.GetComponent<UIWidget>();

			if (component != null) {
				this.SetPivot(component, this.direction == UIVerticalAligner.Direction.TopToBottom);
			}

			transform2.localPosition = transform2.localPosition.SetY(num);
			float num2 = NGUIMath.CalculateRelativeWidgetBounds(transform2).size.y * transform2.localScale.y + this.padding;

			if (this.direction == UIVerticalAligner.Direction.TopToBottom) {
				num -= num2;
			} else {
				num += num2;
			}
		}
	}

	private void SetPivot(UIWidget widget, bool topToBottom) {
		if (widget.pivot == UIWidget.Pivot.TopLeft || widget.pivot == UIWidget.Pivot.Left || widget.pivot == UIWidget.Pivot.BottomLeft) {
			widget.pivot = ((!topToBottom) ? UIWidget.Pivot.BottomLeft : UIWidget.Pivot.TopLeft);
		} else if (widget.pivot == UIWidget.Pivot.Top || widget.pivot == UIWidget.Pivot.Center || widget.pivot == UIWidget.Pivot.Bottom) {
			widget.pivot = ((!topToBottom) ? UIWidget.Pivot.Right : UIWidget.Pivot.Left);
		} else if (widget.pivot == UIWidget.Pivot.TopRight || widget.pivot == UIWidget.Pivot.Right || widget.pivot == UIWidget.Pivot.BottomRight) {
			widget.pivot = ((!topToBottom) ? UIWidget.Pivot.BottomRight : UIWidget.Pivot.TopRight);
		}
	}
}
