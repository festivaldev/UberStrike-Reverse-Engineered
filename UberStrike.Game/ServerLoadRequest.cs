using System;
using System.Collections.Generic;
using Cmune.Core.Models;
using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Realtime.Client;
using UnityEngine;

public class ServerLoadRequest : BaseGamePeer {
	public enum RequestStateType {
		None,
		Waiting,
		Running,
		Down
	}

	private Action _callback;
	public RequestStateType RequestState { get; private set; }
	public PhotonServer Server { get; private set; }

	private ServerLoadRequest(PhotonServer server, Action callback) : base(100) {
		_callback = callback;
		Server = server;
	}

	public static ServerLoadRequest Run(PhotonServer server, Action callback) {
		var serverLoadRequest = new ServerLoadRequest(server, callback);
		serverLoadRequest.Run();

		return serverLoadRequest;
	}

	public void Run() {
		if (Peer.PeerState == PeerStateValue.Disconnected) {
			RequestState = RequestStateType.Waiting;

			if (Server.Data.State == PhotonServerLoad.Status.NotReachable) {
				Server.Data.State = PhotonServerLoad.Status.None;
			}

			Connect(Server.ConnectionString);
		}
	}

	protected override void OnHeartbeatChallenge(string challengeHash) { }

	protected override void OnConnectionFail(string endpointAddress) {
		Debug.LogError(endpointAddress + " is down");
		RequestState = RequestStateType.Down;
		Server.Data.State = PhotonServerLoad.Status.NotReachable;
	}

	protected override void OnServerLoadData(PhotonServerLoad data) {
		Server.Data = data;
		Server.Data.PlayersConnected = Server.Data.PeersConnected;
		Server.Data.Latency = Peer.RoundTripTime;
		Server.Data.State = PhotonServerLoad.Status.Alive;
		RequestState = RequestStateType.Running;
		Disconnect();
	}

	protected override void OnConnected() {
		Operations.SendGetServerLoad();
	}

	protected override void OnDisconnected(StatusCode status) {
		if (RequestState != RequestStateType.Running) {
			RequestState = RequestStateType.Down;
			Server.Data.State = PhotonServerLoad.Status.NotReachable;
		}

		_callback();
	}

	protected override void OnError(string message) { }
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
