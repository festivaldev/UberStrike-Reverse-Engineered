using UnityEngine;

public static class UIDraggablePanelHelper {
	public static void SpringToSelection(this UIDraggablePanel dragPanel, GameObject selectedObject, float springStrength) {
		dragPanel.SpringToPosition(selectedObject.transform.position, springStrength);
	}

	public static void SpringToSelection(this UIDraggablePanel dragPanel, Vector3 selectedPosition, float springStrength) {
		dragPanel.SpringToPosition(selectedPosition, springStrength);
	}

	private static void SpringToPosition(this UIDraggablePanel dragPanel, Vector3 positionToSpring, float springStrength) {
		Vector4 clipRange = dragPanel.panel.clipRange;
		Transform cachedTransform = dragPanel.panel.cachedTransform;
		var vector = cachedTransform.localPosition;
		vector.x += clipRange.x;
		vector.y += clipRange.y;
		vector = cachedTransform.parent.TransformPoint(vector);
		dragPanel.currentMomentum = Vector3.zero;
		var vector2 = cachedTransform.InverseTransformPoint(positionToSpring);
		var vector3 = cachedTransform.InverseTransformPoint(vector);
		var vector4 = vector2 - vector3;

		if (dragPanel.scale.x == 0f) {
			vector4.x = 0f;
		}

		if (dragPanel.scale.y == 0f) {
			vector4.y = 0f;
		}

		if (dragPanel.scale.z == 0f) {
			vector4.z = 0f;
		}

		SpringPanel.Begin(dragPanel.gameObject, cachedTransform.localPosition - vector4, springStrength);
	}
}
