using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class GameServerConnection : MonoBehaviour {
	public string connectionString = "192.168.0.116:5155";

	private void Start() {
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(Singleton<GameStateController>.Instance.Client);
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(Singleton<GameStateController>.Instance.Client.Operations);
	}

	private void OnGUI() {
		var client = Singleton<GameStateController>.Instance.Client;

		if (GUI.Button(new Rect(100f, 10f, 200f, 20f), (!client.IsConnected) ? "Connect" : "Disconnect")) {
			if (client.IsConnected) {
				client.Disconnect();
			} else {
				client.Connect(connectionString);
			}
		}

		GUI.Label(new Rect(100f, 30f, 200f, 20f), "Status: " + client.Peer.PeerState);

		if (client.IsConnected) {
			if (GUI.Button(new Rect(100f, 60f, 200f, 20f), "Enter")) {
				client.Operations.SendCreateRoom(new GameRoomData {
					GameMode = GameModeType.DeathMatch,
					TimeLimit = 10,
					PlayerLimit = 10,
					KillLimit = 10
				}, string.Empty, "4.7.1", PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
			}

			if (GUI.Button(new Rect(100f, 80f, 200f, 20f), "Leave")) {
				client.Operations.SendLeaveRoom();
			}
		}
	}
}
