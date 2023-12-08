using System.Collections.Generic;

public class CompareDebugPage : IComparer<IDebugPage> {
	public int Compare(IDebugPage a, IDebugPage b) {
		return a.Title.CompareTo(b.Title);
	}
}
