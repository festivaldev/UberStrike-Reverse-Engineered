using System;
using System.Collections.Generic;
using Cmune.Core.Models;
using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Realtime.Client;

public class GamePeerAction : BaseGamePeer {
	private Action _onConnect;
	private GamePeerAction() : base(100) { }

	public static void KickPlayer(string connection, int cmid) {
		var peer = new GamePeerAction();
		peer.Connect(connection);
		peer._onConnect = delegate { peer.Operations.SendKickPlayer(cmid, PlayerDataManager.AuthToken, PlayerDataManager.MagicHash); };
	}

	protected override void OnConnected() {
		if (_onConnect != null) {
			_onConnect();
		}

		Disconnect();
	}

	protected override void OnHeartbeatChallenge(string challengeHash) { }

	protected override void OnDisconnected(StatusCode status) {
		Dispose();
	}

	protected override void OnConnectionFail(string endpointAddress) {
		Dispose();
	}

	protected override void OnError(string message) {
		Dispose();
	}

	protected override void OnServerLoadData(PhotonServerLoad data) { }
	protected override void OnFullGameList(List<GameRoomData> gameList) { }
	protected override void OnGameListUpdate(List<GameRoomData> updatedGames, List<int> removedGames) { }
	protected override void OnGameListUpdateEnd() { }
	protected override void OnGetGameInformation(GameRoomData room, List<GameActorInfo> players, int endTime) { }
	protected override void OnRoomEntered(GameRoomData game) { }
	protected override void OnRoomLeft() { }
	protected override void OnRoomEnterFailed(string server, int roomId, string message) { }
	protected override void OnRequestPasswordForRoom(string server, int roomId) { }
	protected override void OnDisconnectAndDisablePhoton(string message) { }
}
