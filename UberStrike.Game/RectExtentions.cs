using UnityEngine;

public static class RectExtentions {
	public static Rect FullExtends(this Rect r) {
		return new Rect(0f, 0f, r.width, r.height);
	}

	public static Rect Lerp(this Rect r, Rect target, float time) {
		return new Rect(Mathf.Lerp(r.x, target.x, time), Mathf.Lerp(r.y, target.y, time), Mathf.Lerp(r.width, target.width, time), Mathf.Lerp(r.height, target.height, time));
	}

	public static Rect Expand(this Rect r, int width, int height) {
		return new Rect(r.x, r.y, r.width + width, r.height + height);
	}

	public static Rect Contract(this Rect r, int horizontalBorder, int verticalBorder) {
		return new Rect(r.x + horizontalBorder, r.y + verticalBorder, r.width - horizontalBorder * 2f, r.height - verticalBorder * 2f);
	}

	public static Rect OffsetBy(this Rect r, Vector2 offset) {
		return new Rect(r.x + offset.x, r.y + offset.y, r.width, r.height);
	}

	public static Rect OffsetBy(this Rect r, float x, float y) {
		return new Rect(r.x + x, r.y + y, r.width, r.height);
	}

	public static Rect Add(this Rect r1, Rect r2) {
		return new Rect(r1.x + r2.x, r1.y + r2.y, r1.width + r2.width, r1.height + r2.height);
	}

	public static Rect Center(this Rect r) {
		return new Rect((Screen.width - r.width) * 0.5f, (Screen.height - r.height) * 0.5f, r.width, r.height);
	}

	public static Rect Center(this Rect r, float width, float height) {
		return new Rect((r.width - width) * 0.5f, (r.height - height) * 0.5f, width, height);
	}

	public static Rect CenterHorizontally(this Rect r, float y, float width, float height) {
		return new Rect((r.width - width) * 0.5f, y, width, height);
	}

	public static Rect CenterVertically(this Rect r, float x, float width, float height) {
		return new Rect(x, (r.height - height) * 0.5f, width, height);
	}

	public static float HalfWidth(this Rect r) {
		return r.width * 0.5f;
	}

	public static float HalfHeight(this Rect r) {
		return r.height * 0.5f;
	}

	public static bool ContainsTouch(this Rect rect, Vector2 touchPosition) {
		var vector = new Vector2(touchPosition.x, Screen.height - touchPosition.y);

		return rect.Contains(vector);
	}
}
