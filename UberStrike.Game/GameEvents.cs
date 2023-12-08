using UnityEngine;

public static class GameEvents {
	public class RespawnCountdown {
		public int Countdown { get; set; }
	}

	public class MatchCountdown {
		public int Countdown { get; set; }
	}

	public class SpawnPosition {
		public Vector3 Position { get; set; }
		public float Rotation { get; set; }
	}

	public class PlayerRespawn {
		public Vector3 Position { get; set; }
		public float Rotation { get; set; }
	}

	public class PlayerPause { }

	public class PlayerUnpause { }

	public class PlayerIngame { }

	public class PlayerZoomIn { }

	public class PlayerZoomOut { }

	public class PlayerLeft {
		public int Cmid { get; set; }
	}

	public class PlayerDamage {
		public float Angle { get; set; }
		public float DamageValue { get; set; }
	}

	public class WaitingForPlayers { }

	public class RoundRunning { }

	public class MatchEnd { }

	public class PlayerDied { }

	public class PlayerSpectator { }

	public class FollowPlayer { }

	public class ChatWindow {
		public bool IsEnabled { get; set; }
	}

	public class PickupItemReset { }

	public class PickupItemChanged {
		public int Id { get; private set; }
		public bool Enable { get; private set; }

		public PickupItemChanged(int id, bool enable) {
			Enable = enable;
			Id = id;
		}
	}

	public class DoorOpened {
		public int DoorID { get; private set; }

		public DoorOpened(int doorID) {
			DoorID = doorID;
		}
	}
}
