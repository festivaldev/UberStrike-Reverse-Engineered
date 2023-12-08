using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Horizontal Aligner")]
[ExecuteInEditMode]
public class UIHorizontalAligner : MonoBehaviour {
	public enum Direction {
		LeftToRight,
		RightToLeft
	}

	public UIHorizontalAligner.Direction direction;
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

		if (this.direction == UIHorizontalAligner.Direction.RightToLeft) {
			list.Reverse();
		}

		float num = 0f;

		foreach (Transform transform2 in list) {
			UIWidget component = transform2.GetComponent<UIWidget>();

			if (component != null) {
				this.SetPivot(component, this.direction == UIHorizontalAligner.Direction.LeftToRight);
			}

			transform2.localPosition = transform2.localPosition.SetX(num);
			float num2 = NGUIMath.CalculateRelativeWidgetBounds(transform2).size.x * transform2.localScale.x + this.padding;

			if (this.direction == UIHorizontalAligner.Direction.LeftToRight) {
				num += num2;
			} else {
				num -= num2;
			}
		}
	}

	private void SetPivot(UIWidget widget, bool leftToRight) {
		if (widget.pivot == UIWidget.Pivot.TopLeft || widget.pivot == UIWidget.Pivot.Top || widget.pivot == UIWidget.Pivot.TopRight) {
			widget.pivot = ((!leftToRight) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
		} else if (widget.pivot == UIWidget.Pivot.Left || widget.pivot == UIWidget.Pivot.Center || widget.pivot == UIWidget.Pivot.Right) {
			widget.pivot = ((!leftToRight) ? UIWidget.Pivot.Right : UIWidget.Pivot.Left);
		} else if (widget.pivot == UIWidget.Pivot.BottomLeft || widget.pivot == UIWidget.Pivot.Bottom || widget.pivot == UIWidget.Pivot.BottomRight) {
			widget.pivot = ((!leftToRight) ? UIWidget.Pivot.BottomRight : UIWidget.Pivot.BottomLeft);
		}
	}
}
