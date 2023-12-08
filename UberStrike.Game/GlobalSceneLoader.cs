using System;
using System.Collections;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class GlobalSceneLoader : MonoBehaviour {
	private const float FadeTime = 1f;
	private Texture2D _blackTexture;
	private Color _color;

	[SerializeField]
	private GUISkin popupSkin;

	[SerializeField]
	private string TestCommServer;

	[SerializeField]
	private string TestGameServer;

	[SerializeField]
	private bool UseTestPhotonServers;

	public static string ErrorMessage { get; private set; }

	public static bool IsError {
		get { return !string.IsNullOrEmpty(ErrorMessage); }
	}

	public static bool IsInitialised { get; set; }
	public static float GlobalSceneProgress { get; private set; }
	public static bool IsGlobalSceneLoaded { get; private set; }
	public static float ItemAssetBundleProgress { get; private set; }
	public static bool IsItemAssetBundleLoaded { get; private set; }
	public static bool IsItemAssetBundleDownloading { get; private set; }

	private void Awake() {
		PopupSkin.Initialize(popupSkin);
		_blackTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
		_color = Color.black;
	}

	private void OnGUI() {
		GUI.depth = 8;
		GUI.color = _color;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _blackTexture);
		GUI.color = Color.white;
	}

	private IEnumerator Start() {
		Application.runInBackground = true;
		Application.LoadLevel("Menu");
		Configuration.WebserviceBaseUrl = ApplicationDataManager.WebServiceBaseUrl;

		yield return StartCoroutine(BeginAuthenticateApplication());

		GlobalSceneProgress = 1f;
		IsGlobalSceneLoaded = true;
		ItemAssetBundleProgress = 1f;
		IsItemAssetBundleLoaded = true;
		InitializeGlobalScene();

		yield return new WaitForSeconds(1f);

		for (var f = 0f; f < 1f; f += Time.deltaTime) {
			yield return new WaitForEndOfFrame();

			_color.a = 1f - f / 1f;
		}

		Debug.Log("Start LoginByChannel");

		if (PlayerDataManager.IsTestBuild) {
			PopupSystem.ShowMessage("Warning", "This is a test build, do not distribute!", PopupSystem.AlertType.OK, delegate { Singleton<AuthenticationManager>.Instance.LoginByChannel(); });
		} else {
			Singleton<AuthenticationManager>.Instance.LoginByChannel();
		}

		yield return new WaitForSeconds(1f);

		Destroy(gameObject);
	}

	private void InitializeGlobalScene() {
		ApplicationDataManager.CurrentLocale = LocaleType.en_US;
		ApplicationDataManager.ApplicationOptions.Initialize();
		StartCoroutine(GUITools.StartScreenSizeListener(1f));

		if (ApplicationDataManager.ApplicationOptions.IsUsingCustom) {
			QualitySettings.masterTextureLimit = ApplicationDataManager.ApplicationOptions.VideoTextureQuality;
			QualitySettings.vSyncCount = ApplicationDataManager.ApplicationOptions.VideoVSyncCount;
			QualitySettings.antiAliasing = ApplicationDataManager.ApplicationOptions.VideoAntiAliasing;
		} else {
			QualitySettings.SetQualityLevel(ApplicationDataManager.ApplicationOptions.VideoQualityLevel);
		}

		AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
		AutoMonoBehaviour<SfxManager>.Instance.UpdateMasterVolume();
		AutoMonoBehaviour<SfxManager>.Instance.UpdateMusicVolume();
		AutoMonoBehaviour<SfxManager>.Instance.UpdateEffectsVolume();
		AutoMonoBehaviour<InputManager>.Instance.ReadAllKeyMappings();
	}

	private IEnumerator BeginAuthenticateApplication() {
		Debug.Log("BeginAuthenticateApplication " + Configuration.WebserviceBaseUrl);

		yield return ApplicationWebServiceClient.AuthenticateApplication("4.7.1", ApplicationDataManager.Channel, string.Empty, delegate(AuthenticateApplicationView callback) { OnAuthenticateApplication(callback); }, delegate(Exception exception) { OnAuthenticateApplicationException(exception); });

		Debug.Log("Connected to : " + Configuration.WebserviceBaseUrl);
	}

	private void OnAuthenticateApplication(AuthenticateApplicationView ev) {
		try {
			IsInitialised = true;

			if (ev != null && ev.IsEnabled) {
				Configuration.EncryptionInitVector = ev.EncryptionInitVector;
				Configuration.EncryptionPassPhrase = ev.EncryptionPassPhrase;
				ApplicationDataManager.IsOnline = true;

				if (!UseTestPhotonServers) {
					Singleton<GameServerManager>.Instance.CommServer = new PhotonServer(ev.CommServer);
					Singleton<GameServerManager>.Instance.AddPhotonGameServers(ev.GameServers.FindAll(i => i.UsageType == PhotonUsageType.All));
				} else {
					Singleton<GameServerManager>.Instance.CommServer = new PhotonServer(TestCommServer, PhotonUsageType.CommServer);
					Singleton<GameServerManager>.Instance.AddTestPhotonGameServer(1000, new PhotonServer(TestGameServer, PhotonUsageType.All));
				}

				if (ev.WarnPlayer) {
					HandleVersionWarning();
				}
			} else {
				Debug.Log(string.Concat("OnAuthenticateApplication failed with 4.7.1/", ApplicationDataManager.Channel, ": ", ErrorMessage));
				ErrorMessage = "Please update.";
				HandleVersionError();
			}
		} catch (Exception ex) {
			ErrorMessage = ex.Message + " " + ex.StackTrace;
			Debug.LogError(string.Concat("OnAuthenticateApplication crashed with 4.7.1/", ApplicationDataManager.Channel, ": ", ErrorMessage));
			HandleApplicationAuthenticationError("There was a problem loading UberStrike. Please check your internet connection and try again.");
		}
	}

	private void OnAuthenticateApplicationException(Exception exception) {
		ErrorMessage = exception.Message;
		Debug.LogError(string.Concat("An exception occurred while authenticating the application with 4.7.1/", ApplicationDataManager.Channel, ": ", exception.Message));
		HandleApplicationAuthenticationError("There was a problem loading UberStrike. Please check your internet connection and try again.");
	}

	private void RetryAuthentiateApplication() {
		ErrorMessage = string.Empty;
		StartCoroutine(BeginAuthenticateApplication());
	}

	private void HandleApplicationAuthenticationError(string message) {
		var channel = ApplicationDataManager.Channel;

		switch (channel) {
			case ChannelType.IPhone:
			case ChannelType.IPad:
			case ChannelType.Android:
				PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, RetryAuthentiateApplication);

				break;
			default:
				if (channel != ChannelType.WebPortal && channel != ChannelType.WebFacebook) {
					PopupSystem.ShowError(LocalizedStrings.Error, message + "This client type is not supported.", PopupSystem.AlertType.OK, Application.Quit);
				} else {
					PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.None);
				}

				break;
		}
	}

	private void HandleConfigurationMissingError(string message) {
		var channel = ApplicationDataManager.Channel;

		switch (channel) {
			case ChannelType.IPhone:
			case ChannelType.IPad:
			case ChannelType.Android:
				PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, Application.Quit);

				break;
			default:
				if (channel != ChannelType.WebPortal && channel != ChannelType.WebFacebook) {
					PopupSystem.ShowError(LocalizedStrings.Error, message + "This client type is not supported.", PopupSystem.AlertType.OK, Application.Quit);
				} else {
					PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.None);
				}

				break;
		}
	}

	private void HandleVersionWarning() {
		var channel = ApplicationDataManager.Channel;

		switch (channel) {
			case ChannelType.IPhone:
			case ChannelType.IPad:
				PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. Click OK to update from the App Store.", PopupSystem.AlertType.OKCancel, OpenIosAppStoreUpdatesPage, Singleton<AuthenticationManager>.Instance.LoginByChannel);

				break;
			case ChannelType.Android:
				PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. Click OK to update from our website.", PopupSystem.AlertType.OKCancel, OpenAndroidAppStoreUpdatesPage, Singleton<AuthenticationManager>.Instance.LoginByChannel);

				break;
			default:
				if (channel != ChannelType.WebPortal && channel != ChannelType.WebFacebook) {
					PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is not supported. Please update from our website.\n(Invalid Channel: " + ApplicationDataManager.Channel + ")", PopupSystem.AlertType.OK);
				} else {
					PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. You should refresh your browser.", PopupSystem.AlertType.OK, Singleton<AuthenticationManager>.Instance.LoginByChannel);
				}

				break;
		}
	}

	private void HandleVersionError() {
		var channel = ApplicationDataManager.Channel;

		switch (channel) {
			case ChannelType.IPhone:
			case ChannelType.IPad:
				PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please update from the App Store.", PopupSystem.AlertType.OK, OpenIosAppStoreUpdatesPage);

				break;
			case ChannelType.Android:
				PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please update from our website.", PopupSystem.AlertType.OK, OpenAndroidAppStoreUpdatesPage);

				break;
			default:
				if (channel != ChannelType.WebPortal && channel != ChannelType.WebFacebook) {
					PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is not supported. Please update from our website.\n(Invalid Channel: " + ApplicationDataManager.Channel + ")", PopupSystem.AlertType.OK);
				} else {
					PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please refresh your browser.", PopupSystem.AlertType.None);
				}

				break;
		}
	}

	private void OpenIosAppStoreUpdatesPage() {
		ApplicationDataManager.OpenUrl(string.Empty, "itms-apps://itunes.com/apps/uberstrike");
	}

	private void OpenAndroidAppStoreUpdatesPage() {
		ApplicationDataManager.OpenUrl(string.Empty, "market://details?id=com.cmune.uberstrike.android");
	}
}
