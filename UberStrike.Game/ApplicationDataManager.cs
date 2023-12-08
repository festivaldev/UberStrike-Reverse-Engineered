using System;
using System.Collections;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UnityEngine;

public static class ApplicationDataManager {
	public const string HeaderFilename = "UberStrikeHeader";
	public const string MainFilename = "UberStrikeMain";
	public const string StandaloneFilename = "UberStrike";
	public const string Version = "4.7.1";
	public const int MinimalWidth = 989;
	public const int MinimalHeight = 560;
	public static string WebServiceBaseUrl = "https://ws.uberstrike.com/2.0/";
	public static string ImagePath = "http://static.uberstrike.com/images/";
	public static bool IsDebug = true;
	private static float applicationDateTime;
	private static DateTime serverDateTime = DateTime.Now;
	public static bool WebPlayerHasResult = false;

	public static ChannelType Channel {
		get { return ChannelType.Steam; }
	}

	public static ApplicationOptions ApplicationOptions { get; private set; } = new ApplicationOptions();
	public static bool IsOnline { get; set; }

	public static bool IsMobile {
		get { return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android; }
	}

	public static bool IsDesktop {
		get { return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer; }
	}

	public static LocaleType CurrentLocale { get; set; }

	public static string FrameRate {
		get {
			var num = Mathf.Max(Mathf.RoundToInt(Time.smoothDeltaTime * 1000f), 1);

			return string.Format("{0} ({1}ms)", 1000 / num, num);
		}
	}

	public static DateTime ServerDateTime {
		get { return serverDateTime.AddSeconds(Time.time - applicationDateTime); }
		set {
			serverDateTime = value;
			applicationDateTime = Time.realtimeSinceStartup;
		}
	}

	public static void LockApplication(string message = "An error occured that forced UberStrike to halt.") {
		PopupSystem.ClearAll();
		PopupSystem.ShowMessage(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, Application.Quit);
	}

	public static void RefreshWallet() {
		UnityRuntime.StartRoutine(StartRefreshWalletInventory());
	}

	public static void OpenUrl(string title, string url) {
		if (Application.isWebPlayer) {
			Application.ExternalCall("displayMessage", title, url);
		} else {
			if (Screen.fullScreen && Application.platform != RuntimePlatform.WindowsPlayer) {
				ScreenResolutionManager.IsFullScreen = false;
			}

			Application.OpenURL(url);
		}
	}

	public static void OpenBuyCredits() {
		var channel = Channel;

		if (channel != ChannelType.Steam) {
			LoadBuyCreditsPage();
			Debug.LogWarning("Buying credits might not be supported on channel: " + Channel);
		} else {
			LoadBuyCreditsPage();
		}
	}

	private static void LoadBuyCreditsPage() {
		if (!GameState.Current.HasJoinedGame) {
			GameData.Instance.MainMenu.Value = MainMenuState.None;
			MenuPageManager.Instance.LoadPage(PageType.Shop);
		}

		EventHandler.Global.Fire(new ShopEvents.SelectShopArea {
			ShopArea = ShopArea.Credits
		});
	}

	private static IEnumerator StartRefreshWalletInventory() {
		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetMemberWallet());
		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(true));
	}
}
