using System.Collections.Generic;

public class CommUserNameComparer : Comparer<CommUser> {
	public override int Compare(CommUser f1, CommUser f2) {
		return CommUserComparer.UserNameCompare(f1, f2);
	}
}
