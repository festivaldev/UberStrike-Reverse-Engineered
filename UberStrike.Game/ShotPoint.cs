using UnityEngine;

public class ShotPoint {
	private Vector3 _aggregatedPoint;
	public int ProjectileId { get; private set; }
	public int Count { get; private set; }

	public Vector3 MidPoint {
		get { return _aggregatedPoint / Count; }
	}

	public ShotPoint(Vector3 point, int projectileId) {
		AddPoint(point);
		ProjectileId = projectileId;
	}

	public void AddPoint(Vector3 point) {
		_aggregatedPoint += point;
		Count++;
	}
}
