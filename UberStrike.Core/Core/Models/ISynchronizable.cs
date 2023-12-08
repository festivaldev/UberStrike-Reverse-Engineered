using System.Collections.Generic;

namespace UberStrike.Core.Models {
	public interface ISynchronizable {
		SortedList<int, object> Changes { get; }
	}
}
