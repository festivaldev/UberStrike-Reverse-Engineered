using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GizmoHelper : AutoMonoBehaviour<GizmoHelper> {
	public enum GizmoType {
		Cube,
		Sphere,
		WiredSphere
	}

	public Gizmo CollisionTest = new Gizmo {
		Color = Color.red,
		Type = GizmoType.Sphere
	};

	[SerializeField]
	private List<Gizmo> gizmos = new List<Gizmo>();

	public void AddGizmo(Vector3 position, [Optional] Color color, GizmoType type = GizmoType.Sphere, float size = 0.1f) {
		Debug.Log("AddGizmo " + position);

		gizmos.Add(new Gizmo {
			Position = position,
			Type = type,
			Size = size,
			Color = color
		});
	}

	private void OnDrawGizmos() {
		if (CollisionTest.Size > 0f) {
			Gizmos.color = CollisionTest.Color;
			Gizmos.DrawSphere(CollisionTest.Position, CollisionTest.Size);
		}

		foreach (var gizmo in gizmos) {
			Gizmos.color = gizmo.Color;
			var type = gizmo.Type;

			if (type != GizmoType.Cube) {
				if (type != GizmoType.Sphere) {
					Gizmos.DrawSphere(gizmo.Position, gizmo.Size);
				} else {
					Gizmos.DrawSphere(gizmo.Position, gizmo.Size);
				}
			} else {
				Gizmos.DrawCube(gizmo.Position, Vector3.one * gizmo.Size);
			}
		}

		Gizmos.color = Color.white;
	}

	[Serializable]
	public class Gizmo {
		public Color Color;
		public Vector3 Position;
		public float Size;
		public GizmoType Type;
	}
}
