using System.Collections;
using System.Collections.Generic;

public class PreemptiveCoroutineManager : Singleton<PreemptiveCoroutineManager> {
	public delegate IEnumerator CoroutineFunction();

	private Dictionary<CoroutineFunction, int> coroutineFuncIds;

	private PreemptiveCoroutineManager() {
		coroutineFuncIds = new Dictionary<CoroutineFunction, int>();
	}

	public int IncrementId(CoroutineFunction func) {
		if (coroutineFuncIds.ContainsKey(func)) {
			Dictionary<CoroutineFunction, int> dictionary2;
			var dictionary = (dictionary2 = coroutineFuncIds);
			var num = dictionary2[func];

			return dictionary[func] = num + 1;
		}

		return ResetCoroutineId(func);
	}

	public bool IsCurrent(CoroutineFunction func, int coroutineId) {
		return coroutineFuncIds.ContainsKey(func) && coroutineFuncIds[func] == coroutineId;
	}

	public int ResetCoroutineId(CoroutineFunction func) {
		coroutineFuncIds[func] = 0;

		return 0;
	}
}
