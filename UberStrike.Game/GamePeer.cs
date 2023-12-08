using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.Core.Models;
using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.Client;
using UnityEngine;

public class GamePeer : BaseGamePeer {
	private BaseGameRoom currentRoom;
	private int lastRoomJoined;
	private Action onConnectAction;

	public ushort Ping {
		get { return (ushort)Mathf.Clamp(Peer.RoundTripTime / 2, 0, 65535); }
	}

	public bool IsConnectedToLobby { get; private set; }

	public bool IsInsideRoom {
		get { return IsConnected && lastRoomJoined != 0; }
	}

	public GamePeer() : base(50, Application.isEditor) {
		Operations.SendGetGameListUpdates();
	}

	public event Action<PhotonServerLoad> OnServerLoad;

	protected override void OnConnected() {
		Debug.Log("OnConnected");

		if (onConnectAction != null) {
			onConnectAction();
			onConnectAction = null;
		}
	}

	protected override void OnHeartbeatChallenge(string challengeHash) {
		var text = Heartbeat.Instance.CheckHeartbeat(challengeHash);
		Operations.SendSendHeartbeatResponse(PlayerDataManager.AuthToken, text);
	}

	protected override void OnDisconnected(StatusCode status) {
		Debug.LogWarning("#### OnDisconnected");
		OnRoomLeft();
		onConnectAction = null;

		if (IsEnabled && lastRoomJoined != 0) {
			PopupSystem.ClearAll();

			PopupSystem.ShowMessage("Server Disconnection", "You were disconnected from the game.\n Do you want to try to reconnect?", PopupSystem.AlertType.OKCancel, delegate { ReconnectToCurrentGame(); }, delegate {
				lastRoomJoined = 0;
				Singleton<GameStateController>.Instance.LeaveGame();
			});
		}
	}

	protected override void OnError(string message) {
		Singleton<GameStateController>.Instance.UnloadGameMode();

		if (Singleton<SceneLoader>.Instance.CurrentScene != "Menu") {
			Singleton<SceneLoader>.Instance.LoadLevel("Menu");
		}

		PopupSystem.ClearAll();
		PopupSystem.ShowMessage("Server Disconnection", message, PopupSystem.AlertType.OK);
	}

	protected override void OnFullGameList(List<GameRoomData> gameList) {
		IsConnectedToLobby = true;
		Singleton<GameListManager>.Instance.SetGameList(gameList);

		if (PlayPageGUI.Instance) {
			PlayPageGUI.Instance.RefreshGameList();
		}
	}

	protected override void OnGameListUpdate(List<GameRoomData> updatedGames, List<int> removedGames) {
		IsConnectedToLobby = true;

		foreach (var gameRoomData in updatedGames) {
			Singleton<GameListManager>.Instance.AddGame(gameRoomData);
		}

		foreach (var num in removedGames) {
			Singleton<GameListManager>.Instance.RemoveGame(num);
		}

		if (PlayPageGUI.Instance) {
			PlayPageGUI.Instance.RefreshGameList();
		}
	}

	protected override void OnGameListUpdateEnd() {
		IsConnectedToLobby = false;
		Singleton<GameListManager>.Instance.Clear();

		if (PlayPageGUI.Instance) {
			PlayPageGUI.Instance.RefreshGameList();
		}
	}

	protected override void OnRequestPasswordForRoom(string server, int roomId) {
		PopupSystem.ClearAll();
		PopupSystem.Show(new PasswordPopupDialog("Secured Area", "Please enter the password below:", delegate(string password) { JoinGame(server, roomId, password); }, delegate { Singleton<GameStateController>.Instance.LeaveGame(); }));
	}

	protected override void OnRoomEnterFailed(string server, int roomId, string message) {
		PopupSystem.ClearAll();
		PopupSystem.ShowMessage("Failed to join game", message, PopupSystem.AlertType.OK, delegate { Singleton<GameStateController>.Instance.LeaveGame(); });
	}

	protected override void OnRoomEntered(GameRoomData data) {
		Debug.Log("OnRoomJoined " + lastRoomJoined);
		GameState.Current.Reset();
		PopupSystem.ClearAll();
		lastRoomJoined = data.Number;
		GameState.Current.ResetRoundStartTime();
		Peer.FetchServerTimestamp();

		switch (data.GameMode) {
			case GameModeType.DeathMatch: {
				var deathMatchRoom = new DeathMatchRoom(data, this);
				Singleton<GameStateController>.Instance.SetGameMode(deathMatchRoom);
				currentRoom = deathMatchRoom;

				break;
			}
			case GameModeType.TeamDeathMatch: {
				var teamDeathMatchRoom = new TeamDeathMatchRoom(data, this);
				Singleton<GameStateController>.Instance.SetGameMode(teamDeathMatchRoom);
				currentRoom = teamDeathMatchRoom;

				break;
			}
			case GameModeType.EliminationMode: {
				var teamEliminationRoom = new TeamEliminationRoom(data, this);
				Singleton<GameStateController>.Instance.SetGameMode(teamEliminationRoom);
				currentRoom = teamEliminationRoom;

				break;
			}
			default:
				throw new NotImplementedException("GameMode not supported: " + data.GameMode);
		}

		AddRoomLogic(currentRoom, currentRoom.Operations);
		var mapWithId = Singleton<MapManager>.Instance.GetMapWithId(data.MapID);

		if (mapWithId != null) {
			Singleton<MapManager>.Instance.LoadMap(mapWithId, delegate {
				GameStateHelper.EnterGameMode();
				GameState.Current.MatchState.SetState(GameStateId.PregameLoadout);

				foreach (var gameActorInfo in GameState.Current.Players.Values) {
					if (!gameActorInfo.IsSpectator) {
						GameState.Current.InstantiateAvatar(gameActorInfo);
					}
				}

				currentRoom.Operations.SendPowerUpRespawnTimes(PickupItem.GetRespawnDurations());
				List<Vector3> list;
				List<byte> list2;
				Singleton<SpawnPointManager>.Instance.GetAllSpawnPoints(data.GameMode, TeamID.NONE, out list, out list2);
				currentRoom.Operations.SendSpawnPositions(TeamID.NONE, list, list2);
				Singleton<SpawnPointManager>.Instance.GetAllSpawnPoints(data.GameMode, TeamID.RED, out list, out list2);
				currentRoom.Operations.SendSpawnPositions(TeamID.RED, list, list2);
				Singleton<SpawnPointManager>.Instance.GetAllSpawnPoints(data.GameMode, TeamID.BLUE, out list, out list2);
				currentRoom.Operations.SendSpawnPositions(TeamID.BLUE, list, list2);
				AvatarBuilder.UpdateLocalAvatar(Singleton<LoadoutManager>.Instance.Loadout.GetAvatarGear());
				GameState.Current.RoomData = data;

				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUpdatePlayerRoom(new GameRoom {
					Server = new ConnectionAddress(data.Server.ConnectionString),
					Number = data.Number,
					MapId = data.MapID
				});
			});
		} else {
			Debug.LogError("Map not found");
		}
	}

	protected override void OnRoomLeft() {
		Debug.Log("OnRoomLeft " + (currentRoom != null));

		if (currentRoom != null) {
			RemoveRoomLogic(currentRoom, currentRoom.Operations);
			currentRoom = null;
			AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendResetPlayerRoom();
		}
	}

	protected override void OnServerLoadData(PhotonServerLoad data) {
		if (OnServerLoad != null) {
			OnServerLoad(data);
		}
	}

	protected override void OnGetGameInformation(GameRoomData room, List<GameActorInfo> players, int endTime) { }

	protected override void OnDisconnectAndDisablePhoton(string message) {
		AutoMonoBehaviour<CommConnectionManager>.Instance.DisableNetworkConnection(message);
	}

	public new void Disconnect() {
		Debug.Log("Disconnect");

		if (IsConnected) {
			lastRoomJoined = 0;
			base.Disconnect();
		}
	}

	internal void CloseGame(int gameId) {
		if (IsConnected) {
			Operations.SendCloseRoom(gameId, PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
		} else {
			Debug.Log("You are currently not connected to the game server");
		}
	}

	internal void InspectGame(int gameId) {
		Debug.Log("InspectGame operation is not implemented");
	}

	public void CreateGame(GameRoomData data, string password) {
		if (IsConnected) {
			Operations.SendCreateRoom(data, password, "4.7.1", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
		} else {
			onConnectAction = delegate { Operations.SendCreateRoom(data, password, "4.7.1", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash); };
			Connect(data.Server.ConnectionString);
		}
	}

	public void JoinGame(string server, int roomId, string password = "") {
		Debug.Log(string.Concat("JoinGame ", server, ":", roomId, "[current:", Peer.ServerAddress, "]"));

		if (IsConnected) {
			Operations.SendEnterRoom(roomId, password, "4.7.1", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
		} else {
			onConnectAction = delegate { Operations.SendEnterRoom(roomId, password, "4.7.1", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash); };
			Connect(server);
		}
	}

	public void LeaveGame() {
		Operations.SendLeaveRoom();
		OnRoomLeft();
	}

	public void RefreshGameLobby() {
		if (IsConnected) {
			Operations.SendGetGameListUpdates();
		}
	}

	public void EnterGameLobby(string serverAddress) {
		IsConnectedToLobby = true;

		if (IsConnected) {
			Operations.SendGetGameListUpdates();
		} else {
			onConnectAction = delegate { Operations.SendGetGameListUpdates(); };
			Connect(serverAddress);
		}
	}

	private void ReconnectToCurrentGame() {
		if (lastRoomJoined != 0) {
			Singleton<GameStateController>.Instance.UnloadGameMode();
			JoinGame(Peer.ServerAddress, lastRoomJoined, string.Empty);
		} else {
			Singleton<GameStateController>.Instance.LeaveGame();
		}
	}

	private IEnumerator StartReconnectionInSeconds(string server, int roomId, int seconds) {
		yield return new WaitForSeconds(seconds);

		if (roomId != 0) {
			JoinGame(server, roomId, string.Empty);
		} else {
			Debug.LogError("Failed to reconnect because GameRoom is null!");
		}
	}
}
