public class Tuple { }

public class TupleOne<T1> {
	public T1 El1;

	public TupleOne(T1 el1) {
		El1 = el1;
	}
}

public class TupleTwo<T1, T2> {
	public T1 El1;
	public T2 El2;

	public TupleTwo(T1 el1, T2 el2) {
		El1 = el1;
		El2 = el2;
	}
}

public class TupleThree<T1, T2, T3> {
	public T1 El1;
	public T2 El2;
	public T3 El3;

	public TupleThree(T1 el1, T2 el2, T3 el3) {
		El1 = el1;
		El2 = el2;
		El3 = el3;
	}
}

public class TupleFour<T1, T2, T3, T4> {
	public T1 El1;
	public T2 El2;
	public T3 El3;
	public T4 El4;

	public TupleFour(T1 el1, T2 el2, T3 el3, T4 el4) {
		El1 = el1;
		El2 = el2;
		El3 = el3;
		El4 = el4;
	}
}
