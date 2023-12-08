using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameState {
	public static readonly GameState Current = new GameState();
	public readonly GameActions Actions = new GameActions();
	public readonly Avatar Avatar = new Avatar(Loadout.Empty, true);
	public readonly Dictionary<int, CharacterConfig> Avatars = new Dictionary<int, CharacterConfig>();
	public readonly PlayerLeadAudio LeadStatus = new PlayerLeadAudio();
	public readonly StateMachine<GameStateId> MatchState = new StateMachine<GameStateId>();
	public readonly Dictionary<int, GameActorInfo> Players = new Dictionary<int, GameActorInfo>();
	public readonly StateMachine<PlayerStateId> PlayerState = new StateMachine<PlayerStateId>();
	public readonly RemotePlayerInterpolator RemotePlayerStates = new RemotePlayerInterpolator();
	public readonly EndOfMatchStats Statistics = new EndOfMatchStats();
	private LocalPlayer player;
	private int roundStartTime;

	public LocalPlayer Player {
		get {
			if (player == null && PrefabManager.Instance != null) {
				player = PrefabManager.Instance.InstantiateLocalPlayer();
			}

			return player;
		}
	}

	public PlayerData PlayerData { get; private set; }
	public MapConfiguration Map { get; set; }
	public GameRoomData RoomData { get; set; }
	public int RoundsPlayed { get; set; }
	public int ScoreRed { get; private set; }
	public int ScoreBlue { get; private set; }
	public int BlueTeamPlayerCount { get; private set; }
	public int RedTeamPlayerCount { get; private set; }
	public int PlayerCountReadyForNextRound { get; private set; }

	public bool IsInGame {
		get {
			switch (MatchState.CurrentStateId) {
				case GameStateId.None:
				case GameStateId.PregameLoadout:
				case GameStateId.EndOfMatch:
					return false;
			}

			return true;
		}
	}

	public bool IsMatchRunning {
		get { return MatchState.CurrentStateId == GameStateId.MatchRunning; }
	}

	public bool IsEndOfMatchState {
		get { return MatchState.CurrentStateId == GameStateId.EndOfMatch; }
	}

	public bool IsInAnyGameState {
		get { return MatchState.CurrentStateId != GameStateId.None; }
	}

	public bool IsPlayerPaused {
		get { return PlayerState.CurrentStateId == PlayerStateId.Paused; }
	}

	public bool IsPlayerDead {
		get { return PlayerState.CurrentStateId == PlayerStateId.Killed || PlayerState.CurrentStateId == PlayerStateId.Spectating; }
	}

	public bool IsPlaying {
		get { return PlayerState.CurrentStateId == PlayerStateId.Playing || PlayerState.CurrentStateId == PlayerStateId.Spectating; }
	}

	public bool IsWaitingForPlayers {
		get { return MatchState.CurrentStateId == GameStateId.WaitingForPlayers; }
	}

	public bool HasJoinedGame {
		get { return MatchState.CurrentStateId != GameStateId.None; }
	}

	public bool IsLocalAvatarLoaded {
		get { return Avatars.ContainsKey(PlayerDataManager.Cmid); }
	}

	public bool IsSinglePlayer {
		get { return !IsMultiplayer; }
	}

	public bool IsGameAboutToEnd {
		get { return GameTime >= RoomData.TimeLimit - 1; }
	}

	public bool CanJoinRedTeam {
		get { return IsAccessAllowed || (!IsGameFull && RedTeamPlayerCount <= BlueTeamPlayerCount); }
	}

	public bool CanJoinBlueTeam {
		get { return IsAccessAllowed || (!IsGameFull && BlueTeamPlayerCount <= RedTeamPlayerCount); }
	}

	public bool CanJoinGame {
		get { return IsAccessAllowed || !IsGameFull; }
	}

	public bool IsGameFull {
		get { return RoomData.ConnectedPlayers >= RoomData.PlayerLimit; }
	}

	public bool IsAccessAllowed {
		get { return PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator; }
	}

	public float GameTime {
		get { return Mathf.Max((float)((Singleton<GameStateController>.Instance.Client.ServerTimeTicks - roundStartTime) / 1000.0), 0f); }
	}

	public GameModeType GameMode {
		get { return RoomData.GameMode; }
	}

	public bool IsMultiplayer {
		get { return RoomData.GameMode != GameModeType.None; }
	}

	public bool IsTeamGame {
		get { return GameMode == GameModeType.TeamDeathMatch || GameMode == GameModeType.EliminationMode; }
	}

	private GameState() {
		MatchState.OnChanged += delegate(GameStateId el) { GameData.Instance.GameState.Value = el; };
		PlayerState.OnChanged += delegate(PlayerStateId el) { GameData.Instance.PlayerState.Value = el; };
		PlayerData = new PlayerData();
		Reset();

		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += delegate {
			if (IsInGame) {
				if (IsLocalAvatarLoaded) {
					PlayerData.SendUpdates();
				}

				RemotePlayerStates.Update();
			}

			MatchState.Update();
			PlayerState.Update();
		};
	}

	public bool HasAvatarLoaded(int cmid) {
		return Avatars.ContainsKey(cmid);
	}

	public void ResetRoundStartTime() {
		roundStartTime = Singleton<GameStateController>.Instance.Client.ServerTimeTicks;
	}

	public void Reset() {
		Actions.Clear();
		PlayerData.Reset();
		MatchState.Reset();
		PlayerState.Reset();

		RoomData = new GameRoomData {
			GameMode = GameModeType.None
		};

		foreach (var characterConfig in Avatars.Values) {
			characterConfig.Destroy();
		}

		RemotePlayerStates.Reset();
		Avatars.Clear();
		Players.Clear();
	}

	public bool TryGetPlayerAvatar(int cmid, out CharacterConfig character) {
		return Avatars.TryGetValue(cmid, out character) && character != null;
	}

	public bool TryGetActorInfo(int cmid, out GameActorInfo player) {
		return Players.TryGetValue(cmid, out player) && player != null;
	}

	public void UnloadAvatar(int cmid) {
		CharacterConfig characterConfig;

		if (Avatars.TryGetValue(cmid, out characterConfig)) {
			if (characterConfig) {
				characterConfig.Destroy();
			}

			Avatars.Remove(cmid);
		}

		Players.Remove(cmid);
	}

	public void EmitRemoteProjectile(int cmid, Vector3 origin, Vector3 direction, byte slot, int projectileID, bool explode) {
		CharacterConfig characterConfig;

		if (TryGetPlayerAvatar(cmid, out characterConfig)) {
			if (characterConfig.Avatar.Decorator.AnimationController) {
				characterConfig.Avatar.Decorator.AnimationController.Shoot();
			}

			var projectile = characterConfig.WeaponSimulator.EmitProjectile(cmid, characterConfig.State.Player.PlayerId, origin, direction, (LoadoutSlotType)slot, projectileID, explode);

			if (projectile != null) {
				Singleton<ProjectileManager>.Instance.AddProjectile(projectile, projectileID);
			}
		}
	}

	public void UpdateTeamCounter() {
		var num = 0;
		BlueTeamPlayerCount = num;
		RedTeamPlayerCount = num;

		foreach (var gameActorInfo in Players.Values) {
			if (gameActorInfo.TeamID == TeamID.BLUE) {
				BlueTeamPlayerCount++;
			} else if (gameActorInfo.TeamID == TeamID.RED) {
				RedTeamPlayerCount++;
			}
		}
	}

	public void SingleBulletFire(int cmid) {
		CharacterConfig characterConfig;

		if (TryGetPlayerAvatar(cmid, out characterConfig) && characterConfig.State.Player.IsAlive && !characterConfig.IsLocal) {
			if (characterConfig.Avatar.Decorator.AnimationController) {
				characterConfig.Avatar.Decorator.AnimationController.Shoot();
			}

			characterConfig.WeaponSimulator.Shoot(characterConfig.State);
		}
	}

	public void QuickItemEvent(int cmid, byte eventType, int robotLifeTime, int scrapsLifeTime, bool isInstant) {
		CharacterConfig characterConfig;

		if (TryGetPlayerAvatar(cmid, out characterConfig)) {
			Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(characterConfig, (QuickItemLogic)eventType, robotLifeTime, scrapsLifeTime, isInstant);
		}
	}

	public void ActivateQuickItem(int cmid, QuickItemLogic logic, int robotLifeTime, int scrapsLifeTime, bool isInstant) {
		CharacterConfig characterConfig;

		if (TryGetPlayerAvatar(cmid, out characterConfig)) {
			Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(characterConfig, logic, robotLifeTime, scrapsLifeTime, isInstant);
		}
	}

	public void UpdatePlayersReady() {
		PlayerCountReadyForNextRound = 0;

		foreach (var gameActorInfo in Players.Values) {
			if (gameActorInfo.IsReadyForGame) {
				PlayerCountReadyForNextRound++;
			}
		}
	}

	public void UpdateTeamScore(int blueScore, int redScore) {
		ScoreRed = redScore;
		ScoreBlue = blueScore;
		Current.PlayerData.BlueTeamScore.Value = blueScore;
		Current.PlayerData.RedTeamScore.Value = redScore;
		var num = RoomData.KillLimit - Math.Max(redScore, blueScore);
		Current.PlayerData.RemainingKills.Value = num;

		if (MatchState.CurrentStateId == GameStateId.MatchRunning) {
			LeadStatus.PlayKillsLeftAudio(num);
		}

		var teamID = PlayerData.Player.TeamID;

		if (teamID != TeamID.BLUE) {
			if (teamID == TeamID.RED) {
				LeadStatus.UpdateLeadStatus(redScore, blueScore, num > 0 && MatchState.CurrentStateId == GameStateId.MatchRunning);
			}
		} else {
			LeadStatus.UpdateLeadStatus(blueScore, redScore, num > 0 && MatchState.CurrentStateId == GameStateId.MatchRunning);
		}
	}

	private void UpdateDeathmatchScore() {
		var num = 0;

		foreach (var gameActorInfo in Players.Values) {
			if (gameActorInfo.Cmid != PlayerDataManager.Cmid && num < gameActorInfo.Kills) {
				num = gameActorInfo.Kills;
			}
		}
	}

	public void PlayerKilled(int shooter, int target, UberstrikeItemClass weaponClass, BodyPart bodyPart, Vector3 direction) {
		CharacterConfig characterConfig;

		if (Avatars.TryGetValue(target, out characterConfig) && !characterConfig.IsDead) {
			Avatars[target].SetDead(direction, BodyPart.Body, target, weaponClass);
			var valueOrDefault = Players.GetValueOrDefault(shooter);
			var valueOrDefault2 = Players.GetValueOrDefault(target);

			if (valueOrDefault2 == null) {
				Debug.LogError("Kill target is null " + target);
			}

			GameData.Instance.OnPlayerKilled.Fire(valueOrDefault, valueOrDefault2, weaponClass, bodyPart);

			if (target == PlayerDataManager.Cmid) {
				EventHandler.Global.Fire(new GameEvents.PlayerDied());
			}
		}
	}

	public void PlayerDamaged(DamageEvent damageEvent) {
		if (Player != null) {
			foreach (var keyValuePair in damageEvent.Damage) {
				EventHandler.Global.Fire(new GameEvents.PlayerDamage {
					Angle = Conversion.Byte2Angle(keyValuePair.Key),
					DamageValue = keyValuePair.Value
				});

				if ((damageEvent.DamageEffectFlag & 1) != 0) {
					Player.DamageFactor = damageEvent.DamgeEffectValue;
				}
			}
		}
	}

	public void StartMatch(int roundNumber, int endTime) {
		roundStartTime = endTime - RoomData.TimeLimit * 1000;
		LeadStatus.Reset();
		Singleton<GameStateController>.Instance.Client.Peer.FetchServerTimestamp();
		CheatDetection.SyncSystemTime();
		LevelCamera.ResetFeedback();
		Current.PlayerData.RemainingKills.Value = RoomData.KillLimit;
		Current.PlayerData.RemainingTime.Value = 0;
	}

	public void UpdatePlayerStatistics(StatsCollection totalStats, StatsCollection bestPerLife) {
		var playerLevel = PlayerDataManager.PlayerLevel;

		if (playerLevel > 0 && playerLevel < XpPointsUtil.Config.MaxLevel) {
			Singleton<PlayerDataManager>.Instance.UpdatePlayerStats(totalStats, bestPerLife);

			if (PlayerDataManager.PlayerLevel != playerLevel) {
				PopupSystem.Show(new LevelUpPopup(PlayerDataManager.PlayerLevel, playerLevel));
			}

			GlobalUIRibbon.Instance.AddXPEvent(totalStats.Xp);
		}

		if (totalStats.Points > 0) {
			PlayerDataManager.Points += totalStats.Points;
			GlobalUIRibbon.Instance.AddPointsEvent(totalStats.Points);
		}
	}

	public AchievementType GetPlayersFirstAchievement(EndOfMatchData endOfMatchData) {
		var achievementType = AchievementType.None;
		var statsSummary = endOfMatchData.MostValuablePlayers.Find(p => p.Cmid == PlayerDataManager.Cmid);

		if (statsSummary != null) {
			var list = new List<AchievementType>();

			foreach (var keyValuePair in statsSummary.Achievements) {
				list.Add((AchievementType)keyValuePair.Key);
			}

			if (list.Count > 0) {
				achievementType = list[0];
			}
		}

		return achievementType;
	}

	public void EmitRemoteQuickItem(Vector3 origin, Vector3 direction, int itemId, byte playerNumber, int projectileID) {
		var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemId);

		if (itemInShop != null) {
			if (itemInShop.Prefab) {
				var grenadeProjectile = itemInShop.Prefab.GetComponent<QuickItem>() as IGrenadeProjectile;

				try {
					var grenadeProjectile2 = grenadeProjectile.Throw(origin, direction);

					if (playerNumber == PlayerData.Player.PlayerId) {
						grenadeProjectile2.SetLayer(UberstrikeLayer.LocalProjectile);
					} else {
						grenadeProjectile2.SetLayer(UberstrikeLayer.RemoteProjectile);
					}

					Singleton<ProjectileManager>.Instance.AddProjectile(grenadeProjectile2, projectileID);
				} catch (Exception ex) {
					Debug.LogWarning(string.Concat("OnEmitQuickItem failed because Item is not a projectile: ", itemId, "/", playerNumber, "/", projectileID));
					Debug.LogException(ex);
				}
			}
		} else {
			Debug.LogError(string.Concat("OnEmitQuickItem failed because item not found: ", itemId, "/", playerNumber, "/", projectileID));
		}
	}

	public void PlayerLeftGame(int cmid) {
		try {
			EventHandler.Global.Fire(new GameEvents.PlayerLeft {
				Cmid = cmid
			});

			GameActorInfo gameActorInfo;

			if (Players.TryGetValue(cmid, out gameActorInfo)) {
				GameData.Instance.OnHUDStreamMessage.Fire(gameActorInfo, LocalizedStrings.LeftTheGame, null);
				Debug.Log(string.Concat("<< OnPlayerLeftGame ", gameActorInfo.PlayerName, " ", MatchState.CurrentStateId));

				if (gameActorInfo.Cmid == PlayerDataManager.Cmid) {
					Player.SetCurrentCharacterConfig(null);
				} else {
					RemotePlayerStates.RemoveCharacterInfo(gameActorInfo.PlayerId);
				}
			}

			UnloadAvatar(cmid);
		} catch (Exception ex) {
			Debug.LogException(ex);
		} finally {
			Singleton<ChatManager>.Instance.SetGameSection(RoomData.Server.ConnectionString, RoomData.Number, RoomData.MapID, Players.Values);
		}
	}

	public void AllPlayerDeltas(List<GameActorInfoDelta> players) {
		var flag = false;
		var flag2 = false;

		foreach (var gameActorInfoDelta in players) {
			try {
				if (gameActorInfoDelta.Changes.Count > 0) {
					PlayerDelta(gameActorInfoDelta);

					if (gameActorInfoDelta.Changes.ContainsKey(GameActorInfoDelta.Keys.TeamID)) {
						flag = true;
					}

					if (gameActorInfoDelta.Changes.ContainsKey(GameActorInfoDelta.Keys.Kills)) {
						flag2 = true;
					}
				}
			} catch (Exception ex) {
				Debug.LogException(ex);
			}
		}

		if (flag) {
			UpdateTeamCounter();
		}

		if (flag2 && GameMode == GameModeType.DeathMatch) {
			UpdateDeathmatchScore();
		}
	}

	public void PlayerDelta(GameActorInfoDelta update) {
		if (update.Id == PlayerData.Player.PlayerId) {
			PlayerData.DeltaUpdate(update);
		} else {
			RemotePlayerStates.DeltaUpdate(update);
		}
	}

	public void AllPositionUpdate(List<PlayerMovement> positions, ushort gameFrame) {
		foreach (var playerMovement in positions) {
			if (playerMovement.Number != PlayerData.Player.PlayerId) {
				RemotePlayerStates.PositionUpdate(playerMovement, gameFrame);
			}
		}
	}

	public void RespawnLocalPlayerAt(Vector3 position, Quaternion rotation) {
		Player.SpawnPlayerAt(position, rotation);
		CharacterConfig characterConfig;
		GameActorInfo gameActorInfo;

		if (Avatars.TryGetValue(PlayerDataManager.Cmid, out characterConfig)) {
			characterConfig.Reset();
		} else if (TryGetActorInfo(PlayerDataManager.Cmid, out gameActorInfo)) {
			InstantiateAvatar(gameActorInfo);
		}
	}

	public void PlayerRespawned(int cmid, Vector3 position, byte rotation) {
		GameActorInfo gameActorInfo;

		if (TryGetActorInfo(cmid, out gameActorInfo)) {
			if (gameActorInfo.Cmid == PlayerDataManager.Cmid && gameActorInfo.TeamID == TeamID.NONE && GameMode != GameModeType.DeathMatch) {
				Debug.LogWarning("PlayerRespawned failed, invalid team for gamemode");
				Singleton<GameStateController>.Instance.LeaveGame();

				return;
			}

			if (!Avatars.ContainsKey(cmid)) {
				InstantiateAvatar(gameActorInfo);
			}

			CharacterConfig characterConfig;

			if (Avatars.TryGetValue(cmid, out characterConfig)) {
				RemotePlayerStates.UpdatePositionHard(characterConfig.State.Player.PlayerId, position);
				characterConfig.Reset();
			}

			if (cmid == PlayerDataManager.Cmid) {
				EventHandler.Global.Fire(new GameEvents.PlayerRespawn {
					Position = position,
					Rotation = Conversion.Byte2Angle(rotation)
				});
			}
		} else {
			Debug.LogError(string.Format("PlayerRespawned failed {0} because not found in the list of players!", cmid));
		}
	}

	public void InstantiateAvatar(GameActorInfo info) {
		if (!Avatars.ContainsKey(info.Cmid)) {
			if (info.Cmid == PlayerDataManager.Cmid) {
				var characterConfig = PrefabManager.Instance.InstantiateLocalCharacter();
				Avatars.Add(info.Cmid, characterConfig);
				ConfigureAvatar(info, characterConfig, true);
			} else {
				var characterConfig2 = PrefabManager.Instance.InstantiateRemoteCharacter();
				Avatars.Add(info.Cmid, characterConfig2);
				ConfigureAvatar(info, characterConfig2, false);
			}
		} else {
			Debug.LogError(string.Format("Failed call of InstantiateAvatar {0} because already existing!", info.Cmid));
		}
	}

	private void ConfigureAvatar(GameActorInfo info, CharacterConfig character, bool isLocal) {
		if (character != null && info != null) {
			if (isLocal) {
				Player.SetCurrentCharacterConfig(character);
				Player.MoveController.IsLowGravity = GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, RoomData.GameFlags);
				character.Initialize(PlayerData, Avatar);
			} else {
				var avatar = new Avatar(new Loadout(info.Gear, info.Weapons), false);
				avatar.SetDecorator(AvatarBuilder.CreateRemoteAvatar(avatar.Loadout.GetAvatarGear(), info.SkinColor));
				character.Initialize(RemotePlayerStates.GetState(info.PlayerId), avatar);
				GameData.Instance.OnHUDStreamMessage.Fire(info, LocalizedStrings.JoinedTheGame, null);
			}

			if (!info.IsAlive) {
				character.SetDead(Vector3.zero);
			}
		} else {
			Debug.LogError(string.Format("OnAvatarLoaded failed because loaded Avatar is {0} and Info is {1}", character != null, info != null));
		}
	}

	public bool SendChatMessage(string message, ChatContext context) {
		message = ChatMessageFilter.Cleanup(message);

		if (!string.IsNullOrEmpty(message) && !ChatMessageFilter.IsSpamming(message)) {
			GameStateHelper.OnChatMessage(PlayerDataManager.Cmid, PlayerDataManager.Name, message, PlayerDataManager.AccessLevel, (byte)ChatManager.CurrentChatContext);
			Actions.ChatMessage(message, (byte)ChatManager.CurrentChatContext);

			return true;
		}

		return false;
	}
}
