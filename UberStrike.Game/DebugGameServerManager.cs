using UnityEngine;

public class DebugGameServerManager : IDebugPage {
	public string Title {
		get { return "Requests"; }
	}

	public void Draw() {
		foreach (var serverLoadRequest in Singleton<GameServerManager>.Instance.ServerRequests) {
			GUILayout.Label(string.Concat(serverLoadRequest.Server.Name, " ", serverLoadRequest.Server.ConnectionString, ", Latency: ", serverLoadRequest.Server.Latency, " - ", serverLoadRequest.Server.IsValid));
			GUILayout.Label(string.Concat("States: ", serverLoadRequest.RequestState, " ", serverLoadRequest.Server.Data.State, ", PeerState: ", serverLoadRequest.Peer.PeerState));
			GUILayout.Space(10f);
		}
	}
}
