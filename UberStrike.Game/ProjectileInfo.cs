using UnityEngine;

public class ProjectileInfo {
	public int Id { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 Direction { get; set; }
	public Projectile Projectile { get; set; }

	public ProjectileInfo(int id, Ray ray) {
		Id = id;
		Position = ray.origin;
		Direction = ray.direction;
	}
}
