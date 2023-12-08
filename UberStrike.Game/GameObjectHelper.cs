using UnityEngine;

public static class GameObjectHelper {
	public static GameObject Instantiate(GameObject template, Transform parent, Vector3 localPosition) {
		return Instantiate(template, parent, localPosition, Vector3.one);
	}

	public static GameObject Instantiate(GameObject template, Transform parent, Vector3 localPosition, Vector3 localScale) {
		var gameObject = UnityEngine.Object.Instantiate(template) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = localPosition;
		gameObject.transform.localScale = localScale;

		return gameObject;
	}
}
