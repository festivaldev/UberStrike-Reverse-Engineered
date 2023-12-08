using System.Collections.Generic;

public class CommUserFriendsComparer : Comparer<CommUser> {
	public override int Compare(CommUser f1, CommUser f2) {
		if ((f1.IsFriend || f1.IsClanMember || f1.IsFacebookFriend) && (f2.IsFriend || f2.IsClanMember || f2.IsFacebookFriend)) {
			return CommUserComparer.UserNameCompare(f1, f2);
		}

		if (f2.IsFriend || f2.IsClanMember || f2.IsFacebookFriend) {
			return 1;
		}

		if (f1.IsFriend || f1.IsClanMember || f1.IsFacebookFriend) {
			return -1;
		}

		return CommUserComparer.UserNameCompare(f1, f2);
	}
}
