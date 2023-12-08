using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using Steamworks;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.WebService.Unity;
using UnityEngine;

public class AuthenticationManager : Singleton<AuthenticationManager> {
	private ProgressPopupDialog _progress;
	private Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;
	public bool IsAuthComplete { get; private set; }

	private AuthenticationManager() {
		_progress = new ProgressPopupDialog(LocalizedStrings.SettingUp, LocalizedStrings.ProcessingLogin);
	}

	public void SetAuthComplete(bool enabled) {
		IsAuthComplete = enabled;
	}

	public void LoginByChannel() {
		var @string = PlayerPrefs.GetString("CurrentSteamUser", string.Empty);
		Debug.Log(string.Format("SteamWorks SteamID:{0}, PlayerPrefs SteamID:{1}", PlayerDataManager.SteamId, @string));

		if (string.IsNullOrEmpty(@string) || @string != PlayerDataManager.SteamId) {
			Debug.Log(string.Format("No SteamID saved. Using SteamWorks SteamID:{0}", PlayerDataManager.SteamId));
			PopupSystem.ShowMessage(string.Empty, "Have you played UberStrike before?", PopupSystem.AlertType.OKCancel, delegate { UnityRuntime.StartRoutine(StartLoginMemberSteam(true)); }, "No", delegate { PopupSystem.ShowMessage(string.Empty, "Do you want to upgrade an UberStrike.com or Facebook account?\n\nNOTE: This will permenantly link your UberStrike account to this Steam ID", PopupSystem.AlertType.OKCancel, delegate { UnityRuntime.StartRoutine(StartLoginMemberSteam(true)); }, "No", delegate { UnityRuntime.StartRoutine(StartLoginMemberSteam(false)); }, "Yes"); }, "Yes");
		} else {
			Debug.Log(string.Format("Login using saved SteamID:{0}", @string));
			UnityRuntime.StartRoutine(StartLoginMemberSteam(true));
		}
	}

	public IEnumerator StartLoginMemberEmail(string emailAddress, string password) {
		if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password)) {
			ShowLoginErrorPopup(LocalizedStrings.Error, "Your login credentials are not correct. Please try to login again.");

			yield break;
		}

		_progress.Text = "Authenticating Account";
		_progress.Progress = 0.1f;
		PopupSystem.Show(_progress);
		MemberAuthenticationResultView authenticationView = null;

		if (ApplicationDataManager.Channel == ChannelType.Steam) {
			yield return AuthenticationWebServiceClient.LinkSteamMember(emailAddress, password, PlayerDataManager.SteamId, SystemInfo.deviceUniqueIdentifier, delegate(MemberAuthenticationResultView ev) {
				authenticationView = ev;
				PlayerPrefs.SetString("CurrentSteamUser", PlayerDataManager.SteamId);
				PlayerPrefs.Save();
			}, delegate { });
		} else {
			yield return AuthenticationWebServiceClient.LoginMemberEmail(emailAddress, password, ApplicationDataManager.Channel, SystemInfo.deviceUniqueIdentifier, delegate(MemberAuthenticationResultView ev) { authenticationView = ev; }, delegate { });
		}

		if (authenticationView == null) {
			ShowLoginErrorPopup(LocalizedStrings.Error, "The login could not be processed. Please check your internet connection and try again.");

			yield break;
		}

		yield return UnityRuntime.StartRoutine(CompleteAuthentication(authenticationView));
	}

	public IEnumerator StartLoginMemberSteam(bool directSteamLogin) {
		if (directSteamLogin) {
			_progress.Text = "Authenticating with Steam";
			_progress.Progress = 0.05f;
			PopupSystem.Show(_progress);
			m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
			var ticket = new byte[1024];
			uint pcbTicket;
			var authTicket = SteamUser.GetAuthSessionTicket(ticket, 1024, out pcbTicket);
			var num = (int)pcbTicket;
			var authToken = num.ToString();
			var machineId = SystemInfo.deviceUniqueIdentifier;
			MemberAuthenticationResultView authenticationView = null;
			_progress.Text = "Authenticating with UberStrike";
			_progress.Progress = 0.1f;

			yield return AuthenticationWebServiceClient.LoginSteam(PlayerDataManager.SteamId, authToken, machineId, delegate(MemberAuthenticationResultView result) {
				authenticationView = result;
				PlayerPrefs.SetString("CurrentSteamUser", PlayerDataManager.SteamId);
				PlayerPrefs.Save();
			}, delegate(Exception error) {
				Debug.LogError("Account authentication error: " + error);
				ShowLoginErrorPopup(LocalizedStrings.Error, "There was an error logging you in. Please try again or contact us at http://support.cmune.com");
			});

			yield return UnityRuntime.StartRoutine(CompleteAuthentication(authenticationView));
		} else {
			PopupSystem.ClearAll();

			yield return PanelManager.Instance.OpenPanel(PanelType.Login);
		}
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback) {
		Debug.Log(string.Concat("[", 163, " - GetAuthSessionTicketResponse] - ", pCallback.m_hAuthTicket, " -- ", pCallback.m_eResult));
	}

	private IEnumerator CompleteAuthentication(MemberAuthenticationResultView authView, bool isRegistrationLogin = false) {
		if (authView == null) {
			Debug.LogError("Account authentication error: MemberAuthenticationResultView was null, isRegistrationLogin: " + isRegistrationLogin);
			ShowLoginErrorPopup(LocalizedStrings.Error, "There was an error logging you in. Please try again or contact us at http://support.cmune.com");

			yield break;
		}

		if (authView.MemberAuthenticationResult == MemberAuthenticationResult.IsBanned || authView.MemberAuthenticationResult == MemberAuthenticationResult.IsIpBanned) {
			ApplicationDataManager.LockApplication(LocalizedStrings.YourAccountHasBeenBanned);

			yield break;
		}

		if (authView.MemberAuthenticationResult == MemberAuthenticationResult.InvalidEsns) {
			Debug.Log("Result: " + authView.MemberAuthenticationResult);
			ShowLoginErrorPopup(LocalizedStrings.Error, "Sorry this account is linked already.");

			yield break;
		}

		if (authView.MemberAuthenticationResult != MemberAuthenticationResult.Ok) {
			Debug.Log("Result: " + authView.MemberAuthenticationResult);
			ShowLoginErrorPopup(LocalizedStrings.Error, "Your login credentials are not correct. Please try to login again.");

			yield break;
		}

		Singleton<PlayerDataManager>.Instance.SetLocalPlayerMemberView(authView.MemberView);
		PlayerDataManager.AuthToken = authView.AuthToken;

		if (!PlayerDataManager.IsTestBuild) {
			PlayerDataManager.MagicHash = UberDaemon.Instance.GetMagicHash(authView.AuthToken);
			Debug.Log("Magic Hash:" + PlayerDataManager.MagicHash);
		}

		ApplicationDataManager.ServerDateTime = authView.ServerTime;
		EventHandler.Global.Fire(new GlobalEvents.Login(authView.MemberView.PublicProfile.AccessLevel));
		_progress.Text = LocalizedStrings.LoadingFriendsList;
		_progress.Progress = 0.2f;

		yield return UnityRuntime.StartRoutine(Singleton<CommsManager>.Instance.GetContactsByGroups());

		_progress.Text = LocalizedStrings.LoadingCharacterData;
		_progress.Progress = 0.3f;

		yield return ApplicationWebServiceClient.GetConfigurationData("4.7.1", delegate(ApplicationConfigurationView appConfigView) { XpPointsUtil.Config = appConfigView; }, delegate { ApplicationDataManager.LockApplication(LocalizedStrings.ErrorLoadingData); });

		Singleton<PlayerDataManager>.Instance.SetPlayerStatisticsView(authView.PlayerStatisticsView);
		_progress.Text = LocalizedStrings.LoadingMapData;
		_progress.Progress = 0.5f;
		var mapsLoadedSuccessfully = false;

		yield return ApplicationWebServiceClient.GetMaps("4.7.1", DefinitionType.StandardDefinition, delegate(List<MapView> callback) { mapsLoadedSuccessfully = Singleton<MapManager>.Instance.InitializeMapsToLoad(callback); }, delegate { ApplicationDataManager.LockApplication(LocalizedStrings.ErrorLoadingMaps); });

		if (!mapsLoadedSuccessfully) {
			ShowLoginErrorPopup(LocalizedStrings.Error, LocalizedStrings.ErrorLoadingMapsSupport);
			PopupSystem.HideMessage(_progress);

			yield break;
		}

		_progress.Progress = 0.6f;
		_progress.Text = LocalizedStrings.LoadingWeaponAndGear;

		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetShop());

		if (!Singleton<ItemManager>.Instance.ValidateItemMall()) {
			PopupSystem.HideMessage(_progress);

			yield break;
		}

		_progress.Progress = 0.7f;
		_progress.Text = LocalizedStrings.LoadingPlayerInventory;

		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(false));

		_progress.Progress = 0.8f;
		_progress.Text = LocalizedStrings.GettingPlayerLoadout;

		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetLoadout());

		if (!Singleton<LoadoutManager>.Instance.ValidateLoadout()) {
			ShowLoginErrorPopup(LocalizedStrings.ErrorGettingPlayerLoadout, LocalizedStrings.ErrorGettingPlayerLoadoutSupport);

			yield break;
		}

		_progress.Progress = 0.85f;
		_progress.Text = LocalizedStrings.LoadingPlayerStatistics;

		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetMember());

		if (!Singleton<PlayerDataManager>.Instance.ValidateMemberData()) {
			ShowLoginErrorPopup(LocalizedStrings.ErrorGettingPlayerStatistics, LocalizedStrings.ErrorPlayerStatisticsSupport);

			yield break;
		}

		_progress.Progress = 0.9f;
		_progress.Text = LocalizedStrings.LoadingClanData;

		yield return ClanWebServiceClient.GetMyClanId(PlayerDataManager.AuthToken, delegate(int id) { PlayerDataManager.ClanID = id; }, delegate { });

		if (PlayerDataManager.ClanID > 0) {
			yield return ClanWebServiceClient.GetOwnClan(PlayerDataManager.AuthToken, PlayerDataManager.ClanID, delegate(ClanView ev) { Singleton<ClanDataManager>.Instance.SetClanData(ev); }, delegate { });
		}

		GameState.Current.Avatar.SetDecorator(AvatarBuilder.CreateLocalAvatar());
		GameState.Current.Avatar.UpdateAllWeapons();

		yield return new WaitForEndOfFrame();

		Singleton<InboxManager>.Instance.Initialize();

		yield return new WaitForEndOfFrame();

		Singleton<BundleManager>.Instance.Initialize();

		yield return new WaitForEndOfFrame();

		PopupSystem.HideMessage(_progress);

		if (!authView.IsAccountComplete) {
			PanelManager.Instance.OpenPanel(PanelType.CompleteAccount);
		} else {
			MenuPageManager.Instance.LoadPage(PageType.Home);
			IsAuthComplete = true;
		}

		Debug.LogWarning(string.Format("AuthToken:{0}, MagicHash:{1}", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash));
	}

	public void StartLogout() {
		UnityRuntime.StartRoutine(Logout());
	}

	private IEnumerator Logout() {
		if (GameState.Current.HasJoinedGame) {
			Singleton<GameStateController>.Instance.LeaveGame();

			yield return new WaitForSeconds(3f);
		}

		MenuPageManager.Instance.LoadPage(PageType.Home);
		MenuPageManager.Instance.UnloadCurrentPage();
		GlobalUIRibbon.Instance.Hide();

		if (GameState.Current.Avatar.Decorator != null) {
			AvatarBuilder.Destroy(GameState.Current.Avatar.Decorator.gameObject);
		}

		GameState.Current.Avatar.SetDecorator(null);
		Singleton<PlayerDataManager>.Instance.Dispose();
		Singleton<InventoryManager>.Instance.Dispose();
		Singleton<LoadoutManager>.Instance.Dispose();
		Singleton<ClanDataManager>.Instance.Dispose();
		Singleton<ChatManager>.Instance.Dispose();
		Singleton<InboxManager>.Instance.Dispose();
		Singleton<TransactionHistory>.Instance.Dispose();
		Singleton<BundleManager>.Instance.Dispose();
		Singleton<GameStateController>.Instance.ResetClient();
		AutoMonoBehaviour<CommConnectionManager>.Instance.Reconnect();
		InboxThread.Current = null;
		EventHandler.Global.Fire(new GlobalEvents.Logout());
		GameData.Instance.MainMenu.Value = MainMenuState.Logout;
		Application.Quit();
	}

	private void ShowLoginErrorPopup(string title, string message) {
		Debug.Log("Login Error!");
		PopupSystem.HideMessage(_progress);

		PopupSystem.ShowMessage(title, message, PopupSystem.AlertType.OK, delegate {
			LoginPanelGUI.ErrorMessage = string.Empty;
			LoginByChannel();
		});
	}
}
