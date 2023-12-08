using UnityEngine;

public interface IProjectile {
	int ID { get; set; }
	Vector3 Explode();
	void Destroy();
}
