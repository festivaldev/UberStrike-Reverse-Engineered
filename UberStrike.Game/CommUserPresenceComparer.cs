using System.Collections.Generic;

public class CommUserPresenceComparer : Comparer<CommUser> {
	public override int Compare(CommUser f1, CommUser f2) {
		if (f1.PresenceIndex == f2.PresenceIndex) {
			return CommUserComparer.UserNameCompare(f1, f2);
		}

		if (f1.PresenceIndex == PresenceType.Offline) {
			return 1;
		}

		if (f2.PresenceIndex == PresenceType.Offline) {
			return -1;
		}

		return CommUserComparer.UserNameCompare(f1, f2);
	}
}
