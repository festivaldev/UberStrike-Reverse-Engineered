using System;
using System.Collections;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader> {
	private const float FadeTime = 1f;
	private Texture2D _blackTexture;
	private Color _color;
	public string CurrentScene { get; private set; }
	public bool IsLoading { get; private set; }

	private SceneLoader() {
		_blackTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
	}

	public void LoadLevel(string level, Action onSuccess = null) {
		if (!IsLoading) {
			if (GameState.Current.Avatar.Decorator != null) {
				GameState.Current.Avatar.Decorator.transform.parent = null;
			}

			UnityRuntime.StartRoutine(LoadLevelAsync(level, onSuccess));
		} else {
			Debug.LogError("Trying to load level twice!");
		}
	}

	private IEnumerator LoadLevelAsync(string level, Action onSuccess) {
		IsLoading = true;
		CurrentScene = level;
		AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGUI;

		for (var f = _color.a * 1f; f < 1f; f += Time.deltaTime) {
			yield return new WaitForEndOfFrame();

			_color.a = f / 1f;
		}

		_color.a = 1f;
		Application.LoadLevel(level);

		yield return new WaitForEndOfFrame();

		if (level == "Menu") {
			AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.HomeSceneBackground);
			MenuPageManager.Instance.LoadPage(PageType.Home);
		} else {
			Application.LoadLevelAdditive("DesktopHUD");
		}

		for (var f2 = 0f; f2 < 1f; f2 += Time.deltaTime) {
			yield return new WaitForEndOfFrame();

			_color.a = 1f - f2 / 1f;
		}

		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui -= OnGUI;
		IsLoading = false;

		if (onSuccess != null) {
			onSuccess();
		}
	}

	private void OnGUI() {
		GUI.depth = 8;
		GUI.color = _color;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _blackTexture);
		GUI.color = Color.white;
	}
}
