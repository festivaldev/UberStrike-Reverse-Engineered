using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Core.ViewModel;
using UberStrike.Realtime.Client;
using UnityEngine;

public class CommPeer : BaseCommPeer {
	public LobbyRoom Lobby { get; private set; }

	public CommPeer() : base(100, Application.isEditor) {
		Lobby = new LobbyRoom();
		AddRoomLogic(Lobby, Lobby.Operations);
	}

	protected override void OnConnected() {
		if (PlayerDataManager.IsPlayerLoggedIn) {
			Operations.SendAuthenticationRequest(PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
			Singleton<ChatManager>.Instance.UpdateFriendSection();
		}
	}

	protected override void OnDisconnected(StatusCode status) { }
	protected override void OnError(string message) { }
	protected override void OnLoadData(ServerConnectionView data) { }

	protected override void OnLobbyEntered() {
		Lobby.SendContactList();

		if (GameState.Current.RoomData != null && GameState.Current.RoomData.Server != null) {
			Lobby.UpdatePlayerRoom(new GameRoom {
				Server = new ConnectionAddress(GameState.Current.RoomData.Server.ConnectionString),
				Number = GameState.Current.RoomData.Number,
				MapId = GameState.Current.RoomData.MapID
			});
		}
	}

	protected override void OnDisconnectAndDisablePhoton(string message) {
		AutoMonoBehaviour<CommConnectionManager>.Instance.DisableNetworkConnection(message);
	}

	protected override void OnHeartbeatChallenge(string challengeHash) {
		var text = Heartbeat.Instance.CheckHeartbeat(challengeHash);
		Operations.SendSendHeartbeatResponse(PlayerDataManager.AuthToken, text);
	}
}
