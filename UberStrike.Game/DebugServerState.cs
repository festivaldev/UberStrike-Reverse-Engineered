using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugServerState : IDebugPage {
	public string Title {
		get { return "Network"; }
	}

	public void Draw() {
		GUILayout.Space(10f);
		GUILayout.Label(string.Format("GAME: {0}", Singleton<GameStateController>.Instance.Client.Peer.ServerAddress));
		GUILayout.Label("  PeerState: " + Singleton<GameStateController>.Instance.Client.Peer.PeerState);
		GUILayout.Label("  InRoom: " + Singleton<GameStateController>.Instance.Client.IsInsideRoom);
		GUILayout.Label("  Network Time: " + Singleton<GameStateController>.Instance.Client.Peer.ServerTimeInMilliSeconds);
		GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(Singleton<GameStateController>.Instance.Client.Peer.BytesIn).ToString("f2"));
		GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(Singleton<GameStateController>.Instance.Client.Peer.BytesOut).ToString("f2"));
		GUILayout.Space(10f);
		GUILayout.Label(string.Format("COMM: {0}", AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.ServerAddress));
		GUILayout.Label("  PeerState: " + AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.PeerState);
		GUILayout.Label("  Network Time: " + AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.ServerTimeInMilliSeconds);
		GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.BytesIn).ToString("f2"));
		GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.BytesOut).ToString("f2"));
		GUILayout.Label("ALL SERVERS");

		foreach (var photonServer in Singleton<GameServerManager>.Instance.PhotonServerList) {
			GUILayout.Label(string.Concat("  ", photonServer.ConnectionString, " ", photonServer.Latency));
		}
	}
}
