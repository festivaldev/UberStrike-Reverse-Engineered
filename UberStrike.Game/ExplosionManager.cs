using UnityEngine;

public class ExplosionManager : Singleton<ExplosionManager> {
	public HeatWave HeatWavePrefab { get; set; }
	private ExplosionManager() { }

	public void PlayExplosionSound(Vector3 point, AudioClip clip) {
		if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane && GameState.Current.Map.WaterPlaneHeight > point.y) {
			if (UnityEngine.Random.Range(0, 2) == 0) {
				clip = GameAudio.UnderwaterExplosion1;
			} else {
				clip = GameAudio.UnderwaterExplosion2;
			}
		}

		if (clip != null) {
			AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(clip, point);
		}
	}

	public void ShowExplosionEffect(Vector3 point, Vector3 normal, string tag, ParticleConfigurationType effectType) {
		switch (tag) {
			case "Wood":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.WoodEffect, point, normal);

				return;
			case "Stone":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.StoneEffect, point, normal);

				return;
			case "Metal":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.MetalEffect, point, normal);

				return;
			case "Sand":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.SandEffect, point, normal);

				return;
			case "Grass":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.GrassEffect, point, normal);

				return;
			case "Avatar":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.Splat, point, normal);

				return;
			case "Water":
				ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.WaterEffect, point, normal);

				return;
		}

		ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.Default, point, normal);
	}
}
