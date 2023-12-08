using System.Collections.Generic;
using UnityEngine;

public class ExplosionDebug : AutoMonoBehaviour<ExplosionDebug> {
	public List<Vector3> Hits = new List<Vector3>();
	public Vector3 ImpactPoint;
	public List<Line> Protections = new List<Line>();
	public float Radius;
	public Vector3 TestPoint;

	public void Reset() {
		Hits.Clear();
		Protections.Clear();
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(ImpactPoint, Radius);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(TestPoint, 0.1f);

		for (var i = 0; i < Hits.Count; i++) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(Hits[i], 0.1f);
		}

		for (var j = 0; j < Protections.Count; j++) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Protections[j].Start, Protections[j].End);
		}
	}

	public struct Line {
		public Vector3 Start;
		public Vector3 End;

		public Line(Vector3 start, Vector3 end) {
			Start = start;
			End = end;
		}
	}
}
