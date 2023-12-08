using System.Collections.Generic;

public class GameServerNameComparer : IComparer<PhotonServer> {
	public int Compare(PhotonServer a, PhotonServer b) {
		return StaticCompare(a, b);
	}

	public static int StaticCompare(PhotonServer a, PhotonServer b) {
		return string.Compare(b.Name, a.Name);
	}
}
