using System;

namespace Cmune.Core.Models {
	[Serializable]
	public class PhotonServerLoad {
		public enum Status {
			None,
			Alive,
			NotReachable
		}

		public int Latency;
		public Status State;
		public DateTime TimeStamp;
		public int PeersConnected { get; set; }
		public int PlayersConnected { get; set; }
		public int RoomsCreated { get; set; }
		public float MaxPlayerCount { get; set; }
	}
}
