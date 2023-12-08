using UnityEngine;

public class SplashScreen : MonoBehaviour {
	private float _blurBackgroundAlpha = 1f;
	private Vector2 barSize = new Vector2(300f, 20f);
	private Texture blurScreen;

	[SerializeField]
	private Texture2D initializingScreen;

	[SerializeField]
	private Texture2D initializingScreenBlurred;

	private Texture2D loadingBarBackground;
	private Texture mainScreen;

	private float TotalProgress {
		get { return GlobalSceneLoader.GlobalSceneProgress; }
	}

	private void Start() {
		loadingBarBackground = new Texture2D(1, 1, TextureFormat.RGB24, false);

		loadingBarBackground.SetPixels(new[] {
			Color.white
		});

		loadingBarBackground.Apply(false);
		mainScreen = initializingScreen;
		blurScreen = initializingScreenBlurred;
	}

	private void OnGUI() {
		GUI.skin = PopupSkin.Skin;
		var label_loading = PopupSkin.label_loading;
		var num = (Mathf.Sin(Time.time * 2f) + 1.3f) * 0.5f;

		if (!GlobalSceneLoader.IsError) {
			label_loading.normal.textColor = label_loading.normal.textColor.SetAlpha(num);

			if (GlobalSceneLoader.IsGlobalSceneLoaded && GlobalSceneLoader.IsItemAssetBundleLoaded) {
				_blurBackgroundAlpha = Mathf.Lerp(_blurBackgroundAlpha, 0f, Time.deltaTime);
			}

			var num2 = Screen.width / (float)mainScreen.width;
			var num3 = Screen.height / (float)mainScreen.height;
			var num4 = ((num2 <= num3) ? num3 : num2);
			var rect = new Rect(Screen.width / 2 - mainScreen.width * num4 / 2f, Screen.height / 2 - mainScreen.height * num4 / 2f, mainScreen.width * num4, mainScreen.height * num4);
			GUI.depth = 100;
			GUI.color = new Color(1f, 1f, 1f, 1f);
			GUI.DrawTexture(rect, mainScreen);
			GUI.color = new Color(1f, 1f, 1f, 1f - _blurBackgroundAlpha);
			GUI.DrawTexture(rect, blurScreen);
			GUI.color = Color.white;

			if (Application.internetReachability == NetworkReachability.NotReachable) {
				label_loading.normal.textColor = label_loading.normal.textColor.SetAlpha(1f);
				GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "No internet connection available", label_loading);
			} else if (!GlobalSceneLoader.IsGlobalSceneLoaded) {
				var vector = label_loading.CalcSize(new GUIContent("Loading game. Please wait..."));
				GUITools.LabelShadow(new Rect(0f, Screen.height - 150, Screen.width, vector.Height()), "Loading game. Please wait...", label_loading, label_loading.normal.textColor);
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				GUI.DrawTexture(new Rect(Screen.width * 0.5f - barSize.x * 0.5f, Screen.height - 150 + barSize.Height() + 8f, barSize.x, 8f), loadingBarBackground);
				GUI.color = Color.white;
				GUI.DrawTexture(new Rect(Screen.width * 0.5f - barSize.x * 0.5f, Screen.height - 150 + barSize.Height() + 8f, Mathf.RoundToInt(TotalProgress * barSize.x), 8f), loadingBarBackground);
			} else if (!GlobalSceneLoader.IsInitialised) {
				var vector2 = label_loading.CalcSize(new GUIContent("Connecting..."));
				GUITools.LabelShadow(new Rect(0f, Screen.height - 150, Screen.width, vector2.Height()), "Connecting...", label_loading, label_loading.normal.textColor);
			}
		} else if (!PopupSystem.IsAnyPopupOpen) {
			label_loading.normal.textColor = label_loading.normal.textColor.SetAlpha(1f);

			if (GUI.Button(new Rect(0f, 0f, Screen.width, Screen.height), "There was a problem loading UberStrike. Try reloading or visit support.uberstrike.com if the problem persists.", label_loading)) {
				Application.Quit();
			}
		}
	}
}
