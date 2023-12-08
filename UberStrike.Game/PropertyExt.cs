using System;
using UnityEngine;

public static class PropertyExt {
	public static void Fire(this Property<Tuple> property) {
		property.Fire(new Tuple());
	}

	public static void Fire<T1>(this Property<TupleOne<T1>> property, T1 v1) {
		property.Fire(new TupleOne<T1>(v1));
	}

	public static void Fire<T1, T2>(this Property<TupleTwo<T1, T2>> property, T1 v1, T2 v2) {
		property.Fire(new TupleTwo<T1, T2>(v1, v2));
	}

	public static void Fire<T1, T2, T3>(this Property<TupleThree<T1, T2, T3>> property, T1 v1, T2 v2, T3 v3) {
		property.Fire(new TupleThree<T1, T2, T3>(v1, v2, v3));
	}

	public static void Fire<T1, T2, T3, T4>(this Property<TupleFour<T1, T2, T3, T4>> property, T1 v1, T2 v2, T3 v3, T4 v4) {
		property.Fire(new TupleFour<T1, T2, T3, T4>(v1, v2, v3, v4));
	}

	public static void AddEvent(this Property<Tuple> property, Action action, MonoBehaviour mb) {
		property.AddEvent(delegate(Tuple el) { action(); }, mb);
	}

	public static void AddEvent<T1>(this Property<TupleOne<T1>> property, Action<T1> action, MonoBehaviour mb) {
		property.AddEvent(delegate(TupleOne<T1> el) { action(el.El1); }, mb);
	}

	public static void AddEvent<T1, T2>(this Property<TupleTwo<T1, T2>> property, Action<T1, T2> action, MonoBehaviour mb) {
		property.AddEvent(delegate(TupleTwo<T1, T2> el) { action(el.El1, el.El2); }, mb);
	}

	public static void AddEvent<T1, T2, T3>(this Property<TupleThree<T1, T2, T3>> property, Action<T1, T2, T3> action, MonoBehaviour mb) {
		property.AddEvent(delegate(TupleThree<T1, T2, T3> el) { action(el.El1, el.El2, el.El3); }, mb);
	}

	public static void AddEvent<T1, T2, T3, T4>(this Property<TupleFour<T1, T2, T3, T4>> property, Action<T1, T2, T3, T4> action, MonoBehaviour mb) {
		property.AddEvent(delegate(TupleFour<T1, T2, T3, T4> el) { action(el.El1, el.El2, el.El3, el.El4); }, mb);
	}

	public static void AddEventAndFire(this Property<Tuple> property, Action action, MonoBehaviour mb) {
		property.AddEventAndFire(delegate(Tuple el) { action(); }, mb);
	}

	public static void AddEventAndFire<T1>(this Property<TupleOne<T1>> property, Action<T1> action, MonoBehaviour mb) {
		property.AddEventAndFire(delegate(TupleOne<T1> el) { action(el.El1); }, mb);
	}

	public static void AddEventAndFire<T1, T2>(this Property<TupleTwo<T1, T2>> property, Action<T1, T2> action, MonoBehaviour mb) {
		property.AddEventAndFire(delegate(TupleTwo<T1, T2> el) { action(el.El1, el.El2); }, mb);
	}

	public static void AddEventAndFire<T1, T2, T3>(this Property<TupleThree<T1, T2, T3>> property, Action<T1, T2, T3> action, MonoBehaviour mb) {
		property.AddEventAndFire(delegate(TupleThree<T1, T2, T3> el) { action(el.El1, el.El2, el.El3); }, mb);
	}

	public static void AddEventAndFire<T1, T2, T3, T4>(this Property<TupleFour<T1, T2, T3, T4>> property, Action<T1, T2, T3, T4> action, MonoBehaviour mb) {
		property.AddEventAndFire(delegate(TupleFour<T1, T2, T3, T4> el) { action(el.El1, el.El2, el.El3, el.El4); }, mb);
	}
}
