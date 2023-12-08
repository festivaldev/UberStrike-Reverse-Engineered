using UnityEngine;

public static class ParticleEmissionSystem {
	public static void TrailParticles(Vector3 emitPoint, Vector3 direction, TrailParticleConfiguration particleConfiguration, Vector3 muzzlePosition, float distance) {
		if (particleConfiguration.ParticleEmitter != null) {
			var num = 200f;
			var vector = direction * num;
			var num2 = distance / num * 0.9f;

			if (distance > 3f) {
				particleConfiguration.ParticleEmitter.Emit(muzzlePosition + direction * 3f, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), num2, particleConfiguration.ParticleColor);
			}
		}
	}

	public static void FireParticles(Vector3 hitPoint, Vector3 hitNormal, FireParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var vector = Vector3.zero;
			var quaternion = Quaternion.FromToRotation(Vector3.up, hitNormal);
			var vector2 = Vector3.zero;

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				vector.x = 0f + UnityEngine.Random.Range(0f, 0.001f);
				vector.y = 2f + UnityEngine.Random.Range(0f, 0.4f);
				vector.z = 0f + UnityEngine.Random.Range(0f, 0.001f);
				vector = quaternion * vector;
				vector2 = hitPoint;
				vector2.x += UnityEngine.Random.Range(0f, 0.2f);
				vector2.z += UnityEngine.Random.Range(0f, 0.4f) * -1f;
				particleConfiguration.ParticleEmitter.Emit(vector2, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}

	public static void WaterCircleParticles(Vector3 hitPoint, Vector3 hitNormal, FireParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var zero = Vector3.zero;

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				zero.x = UnityEngine.Random.Range(0f, 0.3f);
				zero.z = UnityEngine.Random.Range(0f, 0.3f);
				particleConfiguration.ParticleEmitter.Emit(hitPoint, zero, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}

	public static void WaterSplashParticles(Vector3 hitPoint, Vector3 hitNormal, FireParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var zero = Vector3.zero;

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				zero.x = UnityEngine.Random.Range(0f, 0.3f);
				zero.y = 2f + UnityEngine.Random.Range(0f, 0.3f);
				zero.z = UnityEngine.Random.Range(0f, 0.3f);
				particleConfiguration.ParticleEmitter.Emit(hitPoint, zero, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}

	public static void HitMaterialParticles(Vector3 hitPoint, Vector3 hitNormal, ParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var vector = Vector3.zero;
			var quaternion = default(Quaternion);
			quaternion = Quaternion.FromToRotation(Vector3.back, hitNormal);

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				var vector2 = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
				vector.x = vector2.x;
				vector.y = vector2.y;
				vector.z = UnityEngine.Random.Range(particleConfiguration.ParticleMinZVelocity, particleConfiguration.ParticleMaxZVelocity) * -1f;
				vector = quaternion * vector;
				particleConfiguration.ParticleEmitter.Emit(hitPoint, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}

	public static void HitMaterialRotatingParticles(Vector3 hitPoint, Vector3 hitNormal, ParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var vector = Vector3.zero;
			var quaternion = default(Quaternion);
			quaternion = Quaternion.FromToRotation(Vector3.back, hitNormal);

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				var vector2 = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
				vector.x = vector2.x;
				vector.y = vector2.y;
				vector.z = UnityEngine.Random.Range(particleConfiguration.ParticleMinZVelocity, particleConfiguration.ParticleMaxZVelocity) * -1f;
				vector = quaternion * vector;
				particleConfiguration.ParticleEmitter.Emit(hitPoint, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor, UnityEngine.Random.Range(0f, 360f), 0f);
			}
		}
	}

	public static void HitMateriaHalfSphericParticles(Vector3 hitPoint, Vector3 hitNormal, ParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var vector = Vector3.zero;
			var quaternion = default(Quaternion);
			quaternion = Quaternion.FromToRotation(Vector3.back, hitNormal);

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				vector = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);

				if (vector.z > 0f) {
					vector.z *= -1f;
				}

				vector = quaternion * vector;
				particleConfiguration.ParticleEmitter.Emit(hitPoint, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}

	public static void HitMateriaFullSphericParticles(Vector3 hitPoint, Vector3 hitNormal, ParticleConfiguration particleConfiguration) {
		if (particleConfiguration.ParticleEmitter != null) {
			var vector = Vector3.zero;

			for (var i = 0; i < particleConfiguration.ParticleCount; i++) {
				vector = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
				particleConfiguration.ParticleEmitter.Emit(hitPoint, vector, UnityEngine.Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), UnityEngine.Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
			}
		}
	}
}
