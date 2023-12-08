using UnityEngine;

public class ExplosionController {
	public void EmitBlast(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters) {
		var zero = Vector3.zero;

		if (parameters.ParticleEmitter != null) {
			for (var i = 0; i < parameters.ParticleCount; i++) {
				var num = UnityEngine.Random.Range(parameters.MinSize, parameters.MaxSize);
				var num2 = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				parameters.ParticleEmitter.Emit(hitPoint, zero, num, num2, Color.red);
			}
		}
	}

	public void EmitDust(Vector3 hitPoint, Vector3 hitNormal, ExplosionDustParameters parameters) {
		var vector = Vector3.zero;

		if (parameters.ParticleEmitter != null) {
			for (var i = 0; i < parameters.ParticleCount; i++) {
				vector = UnityEngine.Random.insideUnitSphere * 0.2f;
				hitPoint += UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(parameters.MinStartPositionSize, parameters.MinStartPositionSize);
				var num = UnityEngine.Random.Range(parameters.MinSize, parameters.MaxSize);
				var num2 = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				parameters.ParticleEmitter.Emit(hitPoint, vector, num, num2, Color.red);
			}
		}
	}

	public void EmitRing(Vector3 hitPoint, Vector3 hitNormal, ExplosionRingParameters parameters) {
		var zero = Vector3.zero;
		var startSize = parameters.StartSize;
		var num = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);

		if (parameters.ParticleEmitter != null) {
			parameters.ParticleEmitter.Emit(hitPoint, zero, startSize, num, Color.red);
		}
	}

	public void EmitSmoke(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters) {
		var vector = Vector3.zero;

		if (parameters.ParticleEmitter != null) {
			for (var i = 0; i < parameters.ParticleCount; i++) {
				var num = UnityEngine.Random.Range(parameters.MinSize, parameters.MaxSize);
				var num2 = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				vector = UnityEngine.Random.insideUnitSphere * 0.3f;
				parameters.ParticleEmitter.Emit(hitPoint, vector, num, num2, Color.red);
			}
		}
	}

	public void EmitSpark(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters) {
		var vector = Vector3.zero;

		if (parameters.ParticleEmitter != null) {
			for (var i = 0; i < parameters.ParticleCount; i++) {
				var num = UnityEngine.Random.Range(parameters.MinSize, parameters.MaxSize);
				var num2 = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				vector = UnityEngine.Random.insideUnitSphere * parameters.Speed;
				parameters.ParticleEmitter.Emit(hitPoint, vector, num, num2, Color.red);
			}
		}
	}

	public void EmitTrail(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters) {
		var vector = Vector3.zero;

		if (parameters.ParticleEmitter != null) {
			for (var i = 0; i < parameters.ParticleCount; i++) {
				var num = UnityEngine.Random.Range(parameters.MinSize, parameters.MaxSize);
				var num2 = UnityEngine.Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				vector = UnityEngine.Random.insideUnitSphere * parameters.Speed;
				parameters.ParticleEmitter.Emit(hitPoint, vector, num, num2, Color.red);
			}
		}
	}
}
