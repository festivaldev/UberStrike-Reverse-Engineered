using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Horizontal Space")]
[ExecuteInEditMode]
public class UIHorizontalSpace : UIWidget {
	public float Width;

	public override Vector2 relativeSize {
		get { return new Vector2(Width, 0f); }
	}
}
