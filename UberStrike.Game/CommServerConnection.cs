using UnityEngine;

public class CommServerConnection : MonoBehaviour {
	public string connectionString = "192.168.0.116:5055";

	private void Start() {
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(AutoMonoBehaviour<CommConnectionManager>.Instance.Client);
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Operations);
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby);
		AutoMonoBehaviour<RealtimeUnitTest>.Instance.Add(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations);
	}

	private void OnGUI() {
		var client = AutoMonoBehaviour<CommConnectionManager>.Instance.Client;

		if (GUI.Button(new Rect(100f, 10f, 200f, 20f), (!client.IsConnected) ? "Connect" : "Disconnect")) {
			if (client.IsConnected) {
				client.Disconnect();
			} else {
				client.Connect(connectionString);
			}
		}

		GUI.Label(new Rect(100f, 30f, 200f, 20f), "Status: " + client.Peer.PeerState);
	}
}
