using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VolumeEnviromentSettings : MonoBehaviour {
	public EnviromentSettings Settings;

	private void Awake() {
		collider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			GameState.Current.Player.MoveController.SetEnviroment(Settings, this.collider.bounds);

			if (Settings.Type == EnviromentSettings.TYPE.WATER) {
				var y = GameState.Current.Player.MoveController.Velocity.y;

				if (y < -20f) {
					AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.BigSplash, collider.transform.position);
				} else if (y < -10f) {
					AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.MediumSplash, collider.transform.position);
				}
			}
		}
	}

	private void OnTriggerExit(Collider c) {
		if (c.tag == "Player") {
			GameState.Current.Player.MoveController.ResetEnviroment();
		}
	}
}
