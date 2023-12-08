using UnityEngine;

public class HitPoint {
	public Vector3 Point { get; private set; }
	public string Tag { get; private set; }

	public HitPoint(Vector3 p, string t) {
		Point = p;
		Tag = t;
	}
}
