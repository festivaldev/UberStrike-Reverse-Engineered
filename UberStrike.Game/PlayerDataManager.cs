using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using Steamworks;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager> {
	private Dictionary<int, ClanMemberView> _clanMembers = new Dictionary<int, ClanMemberView>();
	private Dictionary<int, PublicProfileView> _facebookFriends = new Dictionary<int, PublicProfileView>();
	private Dictionary<int, PublicProfileView> _friends = new Dictionary<int, PublicProfileView>();
	private Color _localPlayerSkinColor = Color.white;
	private ClanView _playerClanData;
	private PlayerStatisticsView _serverLocalPlayerPlayerStatisticsView;
	private float _updateLoadoutTime;
	public static string MagicHash { get; set; }

	public static bool IsTestBuild {
		get { return false; }
	}

	public float GearWeight {
		get { return Mathf.Clamp01((GameState.Current.PlayerData.ArmorCarried / 2 + 40) / 100f); }
	}

	public int FriendsCount {
		get { return _friends.Count + _facebookFriends.Count; }
	}

	public static string SteamId {
		get { return SteamUser.GetSteamID().ToString(); }
	}

	public PlayerStatisticsView ServerLocalPlayerStatisticsView {
		get { return _serverLocalPlayerPlayerStatisticsView; }
	}

	public static Color SkinColor {
		get { return Instance._localPlayerSkinColor; }
	}

	public IEnumerable<PublicProfileView> FriendList {
		get { return _friends.Values; }
		set {
			_friends.Clear();

			if (value != null) {
				foreach (var publicProfileView in value) {
					_friends[publicProfileView.Cmid] = publicProfileView;
				}
			}
		}
	}

	public IEnumerable<PublicProfileView> FacebookFriends {
		get { return _facebookFriends.Values; }
		set {
			_facebookFriends.Clear();

			if (value != null) {
				foreach (var publicProfileView in value) {
					if (!_friends.ContainsKey(publicProfileView.Cmid)) {
						_facebookFriends[publicProfileView.Cmid] = publicProfileView;
					}
				}
			}
		}
	}

	public List<PublicProfileView> MergedFriends {
		get {
			var list = new List<PublicProfileView>(FriendList);
			list.AddRange(FacebookFriends);

			return list;
		}
	}

	public static bool IsPlayerLoggedIn {
		get { return Cmid > 0; }
	}

	public static MemberAccessLevel AccessLevel { get; private set; }
	public static int Cmid { get; private set; }
	public static string Name { get; set; }
	public static string Email { get; private set; }
	public static int Credits { get; private set; }
	public static int Points { get; set; }
	public static int PlayerExperience { get; private set; }
	public static int PlayerLevel { get; private set; }
	public static string AuthToken { get; set; }

	public static ClanView ClanData {
		set {
			Instance._playerClanData = value;
			Instance._clanMembers.Clear();

			if (value != null) {
				ClanID = value.GroupId;

				if (value.Members != null) {
					foreach (var clanMemberView in value.Members) {
						Instance._clanMembers[clanMemberView.Cmid] = clanMemberView;

						if (clanMemberView.Cmid == Cmid) {
							Instance.RankInClan = clanMemberView.Position;
						}
					}
				}
			} else {
				ClanID = 0;
				Instance._clanMembers.Clear();
				Instance.RankInClan = GroupPosition.Member;
			}
		}
	}

	public static bool IsPlayerInClan {
		get { return ClanID > 0; }
	}

	public static int ClanID { get; set; }
	public GroupPosition RankInClan { get; set; }

	public static string ClanName {
		get { return (Instance._playerClanData == null) ? string.Empty : Instance._playerClanData.Name; }
	}

	public static string ClanTag {
		get { return (Instance._playerClanData == null) ? string.Empty : Instance._playerClanData.Tag; }
	}

	public static string ClanMotto {
		get { return (Instance._playerClanData == null) ? string.Empty : Instance._playerClanData.Motto; }
	}

	public static DateTime ClanFoundingDate {
		get { return (Instance._playerClanData == null) ? DateTime.Now : Instance._playerClanData.FoundingDate; }
	}

	public static string ClanOwnerName {
		get { return (Instance._playerClanData == null) ? string.Empty : Instance._playerClanData.OwnerName; }
	}

	public static int ClanMembersLimit {
		get { return (Instance._playerClanData == null) ? 0 : Instance._playerClanData.MembersLimit; }
	}

	public int ClanMembersCount {
		get { return (_playerClanData == null) ? 0 : _playerClanData.Members.Count; }
	}

	public List<ClanMemberView> ClanMembers {
		get { return (_playerClanData == null) ? new List<ClanMemberView>(0) : _playerClanData.Members; }
	}

	public static bool CanInviteToClan {
		get { return Instance.RankInClan == GroupPosition.Leader || Instance.RankInClan == GroupPosition.Officer; }
	}

	public static string NameAndTag {
		get { return (!IsPlayerInClan) ? Name : string.Format("[{0}] {1}", ClanTag, Name); }
	}

	private PlayerDataManager() {
		_serverLocalPlayerPlayerStatisticsView = new PlayerStatisticsView();
		_playerClanData = new ClanView();
	}

	public void AddFriend(PublicProfileView view) {
		_friends[view.Cmid] = view;
	}

	public void RemoveFriend(int friendCmid) {
		_friends.Remove(friendCmid);
	}

	public void SetLocalPlayerMemberView(MemberView memberView) {
		Cmid = memberView.PublicProfile.Cmid;
		AccessLevel = memberView.PublicProfile.AccessLevel;
		Name = memberView.PublicProfile.Name;
		Points = memberView.MemberWallet.Points;
		Credits = memberView.MemberWallet.Credits;
	}

	public void SetPlayerStatisticsView(PlayerStatisticsView value) {
		if (value != null) {
			_serverLocalPlayerPlayerStatisticsView = value;
			PlayerExperience = value.Xp;
			PlayerLevel = XpPointsUtil.GetLevelForXp(value.Xp);
		}
	}

	public void UpdatePlayerStats(StatsCollection stats, StatsCollection best) {
		var serverLocalPlayerStatisticsView = ServerLocalPlayerStatisticsView;
		var num = serverLocalPlayerStatisticsView.Xp + GameState.Current.Statistics.GainedXp;
		var levelForXp = XpPointsUtil.GetLevelForXp(num);
		SetPlayerStatisticsView(new PlayerStatisticsView(serverLocalPlayerStatisticsView.Cmid, serverLocalPlayerStatisticsView.Splats + stats.GetKills(), serverLocalPlayerStatisticsView.Splatted + stats.Deaths, serverLocalPlayerStatisticsView.Shots + stats.GetShots(), serverLocalPlayerStatisticsView.Hits + stats.GetHits(), serverLocalPlayerStatisticsView.Headshots + stats.Headshots, serverLocalPlayerStatisticsView.Nutshots + stats.Nutshots, num, levelForXp, new PlayerPersonalRecordStatisticsView((serverLocalPlayerStatisticsView.PersonalRecord.MostHeadshots <= best.Headshots) ? best.Headshots : serverLocalPlayerStatisticsView.PersonalRecord.MostHeadshots, (serverLocalPlayerStatisticsView.PersonalRecord.MostNutshots <= best.Nutshots) ? best.Nutshots : serverLocalPlayerStatisticsView.PersonalRecord.MostNutshots, (serverLocalPlayerStatisticsView.PersonalRecord.MostConsecutiveSnipes <= best.ConsecutiveSnipes) ? best.ConsecutiveSnipes : serverLocalPlayerStatisticsView.PersonalRecord.MostConsecutiveSnipes, 0, (serverLocalPlayerStatisticsView.PersonalRecord.MostSplats <= best.GetKills()) ? best.GetKills() : serverLocalPlayerStatisticsView.PersonalRecord.MostSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostDamageDealt <= best.GetDamageDealt()) ? best.GetDamageDealt() : serverLocalPlayerStatisticsView.PersonalRecord.MostDamageDealt, (serverLocalPlayerStatisticsView.PersonalRecord.MostDamageReceived <= best.DamageReceived) ? best.DamageReceived : serverLocalPlayerStatisticsView.PersonalRecord.MostDamageReceived, (serverLocalPlayerStatisticsView.PersonalRecord.MostArmorPickedUp <= best.ArmorPickedUp) ? best.ArmorPickedUp : serverLocalPlayerStatisticsView.PersonalRecord.MostArmorPickedUp, (serverLocalPlayerStatisticsView.PersonalRecord.MostHealthPickedUp <= best.HealthPickedUp) ? best.HealthPickedUp : serverLocalPlayerStatisticsView.PersonalRecord.MostHealthPickedUp, (serverLocalPlayerStatisticsView.PersonalRecord.MostMeleeSplats <= best.MeleeKills) ? best.MeleeKills : serverLocalPlayerStatisticsView.PersonalRecord.MostMeleeSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostMachinegunSplats <= best.MachineGunKills) ? best.MachineGunKills : serverLocalPlayerStatisticsView.PersonalRecord.MostMachinegunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostShotgunSplats <= best.ShotgunSplats) ? best.ShotgunSplats : serverLocalPlayerStatisticsView.PersonalRecord.MostShotgunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostSniperSplats <= best.SniperKills) ? best.SniperKills : serverLocalPlayerStatisticsView.PersonalRecord.MostSniperSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostSplattergunSplats <= best.SplattergunKills) ? best.SplattergunKills : serverLocalPlayerStatisticsView.PersonalRecord.MostSplattergunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostCannonSplats <= best.CannonKills) ? best.CannonKills : serverLocalPlayerStatisticsView.PersonalRecord.MostCannonSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostLauncherSplats <= best.LauncherKills) ? best.LauncherKills : serverLocalPlayerStatisticsView.PersonalRecord.MostLauncherSplats), new PlayerWeaponStatisticsView(serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalSplats + stats.MeleeKills, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalSplats + stats.MachineGunKills, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalSplats + stats.ShotgunSplats, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalSplats + stats.SniperKills, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalSplats + stats.SplattergunKills, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalSplats + stats.CannonKills, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalSplats + stats.LauncherKills, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalShotsFired + stats.MeleeShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalShotsHit + stats.MeleeShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalDamageDone + stats.MeleeDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalShotsFired + stats.MachineGunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalShotsHit + stats.MachineGunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalDamageDone + stats.MachineGunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalShotsFired + stats.ShotgunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalShotsHit + stats.ShotgunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalDamageDone + stats.ShotgunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalShotsFired + stats.SniperShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalShotsHit + stats.SniperShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalDamageDone + stats.SniperDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalShotsFired + stats.SplattergunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalShotsHit + stats.SplattergunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalDamageDone + stats.SplattergunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalShotsFired + stats.CannonShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalShotsHit + stats.CannonShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalDamageDone + stats.CannonDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalShotsFired + stats.LauncherShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalShotsHit + stats.LauncherShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalDamageDone + stats.LauncherDamageDone)));
	}

	private void HandleWebServiceError() { }

	public void SetSkinColor(Color skinColor) {
		_localPlayerSkinColor = skinColor;
	}

	private LoadoutView CreateLocalPlayerLoadoutView() {
		return new LoadoutView(0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearBoots), Cmid, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearFace), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem3), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearGloves), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHead), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearLowerBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponMelee), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem3), AvatarType.LutzRavinoff, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearUpperBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponPrimary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponSecondary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponTertiary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo), ColorConverter.ColorToHex(_localPlayerSkinColor));
	}

	public IEnumerator StartGetMemberWallet() {
		if (Cmid < 1) {
			Debug.LogError("Player CMID is invalid! Have you called AuthenticationManager.StartAuthenticateMember?");
			ApplicationDataManager.LockApplication("The authentication process failed. Please sign in on www.uberstrike.com and restart UberStrike.");
		} else {
			var popupDialog = PopupSystem.ShowMessage("Updating", "Updating your points and credits balance...", PopupSystem.AlertType.None);

			yield return UserWebServiceClient.GetMemberWallet(AuthToken, OnGetMemberWalletEventReturn, delegate { });
			yield return new WaitForSeconds(0.5f);

			PopupSystem.HideMessage(popupDialog);
		}
	}

	public IEnumerator StartSetLoadout() {
		if (_updateLoadoutTime == 0f) {
			_updateLoadoutTime = Time.time + 5f;

			while (_updateLoadoutTime > Time.time) {
				yield return new WaitForEndOfFrame();
			}

			_updateLoadoutTime = 0f;

			yield return UserWebServiceClient.SetLoadout(AuthToken, CreateLocalPlayerLoadoutView(), delegate(MemberOperationResult ev) {
				if (Singleton<GameStateController>.Instance.Client.IsConnected) {
					Singleton<GameStateController>.Instance.Client.Operations.SendUpdateLoadout();
				}

				if (ev != MemberOperationResult.Ok) {
					Debug.LogError("SetLoadout failed with error=" + ev);
				}
			}, delegate { });
		} else {
			_updateLoadoutTime = Time.time + 5f;
		}
	}

	public IEnumerator StartGetLoadout() {
		if (!Singleton<ItemManager>.Instance.ValidateItemMall()) {
			PopupSystem.ShowMessage("Error Getting Shop Data", "The shop is empty, perhaps there\nwas an error getting the Shop data?", PopupSystem.AlertType.OK, HandleWebServiceError);

			yield break;
		}

		yield return UserWebServiceClient.GetLoadout(AuthToken, delegate(LoadoutView ev) {
			if (ev != null) {
				CheckLoadoutForExpiredItems(ev);
				Singleton<LoadoutManager>.Instance.UpdateLoadout(ev);
				GameState.Current.Avatar.SetLoadout(new Loadout(Singleton<LoadoutManager>.Instance.Loadout));
				_localPlayerSkinColor = ColorConverter.HexToColor(ev.SkinColor);
			} else {
				ApplicationDataManager.LockApplication("It seems that you account is corrupted. Please visit support.uberstrike.com for advice.");
			}
		}, delegate { ApplicationDataManager.LockApplication("There was an error getting your loadout."); });
	}

	public IEnumerator StartGetMember() {
		yield return UserWebServiceClient.GetMember(AuthToken, OnGetMemberEventReturn, delegate { ApplicationDataManager.LockApplication("There was an error getting your player data."); });
	}

	private void OnGetMemberWalletEventReturn(MemberWalletView ev) {
		NotifyPointsAndCreditsChanges(ev.Points, ev.Credits);
		UpdateSecurePointsAndCredits(ev.Points, ev.Credits);
	}

	private void OnGetMemberEventReturn(UberstrikeUserViewModel ev) {
		NotifyPointsAndCreditsChanges(ev.CmuneMemberView.MemberWallet.Points, ev.CmuneMemberView.MemberWallet.Credits);
		SetPlayerStatisticsView(ev.UberstrikeMemberView.PlayerStatisticsView);
		SetLocalPlayerMemberView(ev.CmuneMemberView);
	}

	private void NotifyPointsAndCreditsChanges(int newPoints, int newCredits) {
		if (newPoints != Points) {
			GlobalUIRibbon.Instance.AddPointsEvent(newPoints - Points);
		}

		if (newCredits != Credits) {
			GlobalUIRibbon.Instance.AddCreditsEvent(newCredits - Credits);
		}
	}

	public bool ValidateMemberData() {
		return Cmid > 0 && _serverLocalPlayerPlayerStatisticsView.Cmid > 0;
	}

	public void AttributeXp(int xp) {
		PlayerExperience += xp;
		PlayerLevel = XpPointsUtil.GetLevelForXp(PlayerExperience);
		_serverLocalPlayerPlayerStatisticsView.Xp = PlayerExperience;
		_serverLocalPlayerPlayerStatisticsView.Level = PlayerLevel;
	}

	public void UpdateSecurePointsAndCredits(int points, int credits) {
		Points = points;
		Credits = credits;
	}

	public void CheckLoadoutForExpiredItems(LoadoutView view) {
		view = new LoadoutView(view.LoadoutId, (!IsExpired(view.Backpack, "Backpack")) ? view.Backpack : 0, (!IsExpired(view.Boots, "Boots")) ? view.Boots : 0, view.Cmid, (!IsExpired(view.Face, "Face")) ? view.Face : 0, (!IsExpired(view.FunctionalItem1, "FunctionalItem1")) ? view.FunctionalItem1 : 0, (!IsExpired(view.FunctionalItem2, "FunctionalItem2")) ? view.FunctionalItem2 : 0, (!IsExpired(view.FunctionalItem3, "FunctionalItem3")) ? view.FunctionalItem3 : 0, (!IsExpired(view.Gloves, "Gloves")) ? view.Gloves : 0, (!IsExpired(view.Head, "Head")) ? view.Head : 0, (!IsExpired(view.LowerBody, "LowerBody")) ? view.LowerBody : 0, (!IsExpired(view.MeleeWeapon, "MeleeWeapon")) ? view.MeleeWeapon : 0, (!IsExpired(view.QuickItem1, "QuickItem1")) ? view.QuickItem1 : 0, (!IsExpired(view.QuickItem2, "QuickItem2")) ? view.QuickItem2 : 0, (!IsExpired(view.QuickItem3, "QuickItem3")) ? view.QuickItem3 : 0, view.Type, (!IsExpired(view.UpperBody, "UpperBody")) ? view.UpperBody : 0, (!IsExpired(view.Weapon1, "Weapon1")) ? view.Weapon1 : 0, (!IsExpired(view.Weapon1Mod1, "Weapon1Mod1")) ? view.Weapon1Mod1 : 0, (!IsExpired(view.Weapon1Mod2, "Weapon1Mod2")) ? view.Weapon1Mod2 : 0, (!IsExpired(view.Weapon1Mod3, "Weapon1Mod3")) ? view.Weapon1Mod3 : 0, (!IsExpired(view.Weapon2, "Weapon2")) ? view.Weapon2 : 0, (!IsExpired(view.Weapon2Mod1, "Weapon2Mod1")) ? view.Weapon2Mod1 : 0, (!IsExpired(view.Weapon2Mod2, "Weapon2Mod2")) ? view.Weapon2Mod2 : 0, (!IsExpired(view.Weapon2Mod3, "Weapon2Mod3")) ? view.Weapon2Mod3 : 0, (!IsExpired(view.Weapon3, "Weapon3")) ? view.Weapon3 : 0, (!IsExpired(view.Weapon3Mod1, "Weapon3Mod1")) ? view.Weapon3Mod1 : 0, (!IsExpired(view.Weapon3Mod2, "Weapon3Mod2")) ? view.Weapon3Mod2 : 0, (!IsExpired(view.Weapon3Mod3, "Weapon3Mod3")) ? view.Weapon3Mod3 : 0, (!IsExpired(view.Webbing, "Webbing")) ? view.Webbing : 0, view.SkinColor);
	}

	private bool IsExpired(int itemId, string debug) {
		return !Singleton<InventoryManager>.Instance.Contains(itemId);
	}

	public static bool IsClanMember(int cmid) {
		return Instance._clanMembers.ContainsKey(cmid);
	}

	public static bool IsFriend(int cmid) {
		return Instance._friends.ContainsKey(cmid);
	}

	public static bool IsFacebookFriend(int cmid) {
		return Instance._facebookFriends.ContainsKey(cmid);
	}

	public static bool TryGetFriend(int cmid, out PublicProfileView view) {
		return Instance._friends.TryGetValue(cmid, out view) && view != null;
	}

	public static bool TryGetClanMember(int cmid, out ClanMemberView view) {
		return Instance._clanMembers.TryGetValue(cmid, out view) && view != null;
	}
}
