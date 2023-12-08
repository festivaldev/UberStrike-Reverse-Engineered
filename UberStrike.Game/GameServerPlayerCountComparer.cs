using System.Collections.Generic;
using Cmune.Core.Models;

public class GameServerPlayerCountComparer : IComparer<PhotonServer> {
	public int Compare(PhotonServer a, PhotonServer b) {
		return StaticCompare(a, b);
	}

	public static int StaticCompare(PhotonServer a, PhotonServer b) {
		var num = 1;

		if (a.Data.PlayersConnected == b.Data.PlayersConnected) {
			return string.Compare(b.Name, a.Name);
		}

		return (((a.Data.State != PhotonServerLoad.Status.Alive) ? 1000 : a.Data.PlayersConnected) <= ((b.Data.State != PhotonServerLoad.Status.Alive) ? 1000 : b.Data.PlayersConnected)) ? (num * -1) : num;
	}
}
