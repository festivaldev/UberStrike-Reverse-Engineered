public static class CommUserComparer {
	public static int UserNameCompare(CommUser f1, CommUser f2) {
		return string.Compare(f1.ShortName, f2.ShortName, true);
	}
}
