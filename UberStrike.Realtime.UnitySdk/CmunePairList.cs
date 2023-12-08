using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk {
	public class CmunePairList<T1, T2> : List<KeyValuePair<T1, T2>> {
		public ICollection<T1> Keys {
			get {
				var l = new List<T1>(Count);
				ForEach(delegate(KeyValuePair<T1, T2> p) { l.Add(p.Key); });

				return l;
			}
		}

		public ICollection<T2> Values {
			get {
				var l = new List<T2>(Count);
				ForEach(delegate(KeyValuePair<T1, T2> p) { l.Add(p.Value); });

				return l;
			}
		}

		public CmunePairList() { }
		public CmunePairList(int capacity) : base(capacity) { }
		public CmunePairList(IEnumerable<KeyValuePair<T1, T2>> collection) : base(collection) { }

		public CmunePairList(IEnumerable<T1> collection1, IEnumerable<T2> collection2) {
			var enumerator = collection1.GetEnumerator();
			var enumerator2 = collection2.GetEnumerator();

			while (enumerator.MoveNext() && enumerator2.MoveNext()) {
				Add(new KeyValuePair<T1, T2>(enumerator.Current, enumerator2.Current));
			}
		}

		public ICollection<KeyValuePair<T1, T2>> GetPairsWithKey(T1 key) {
			return FindAll(delegate(KeyValuePair<T1, T2> p) {
				var key2 = p.Key;

				return key2.Equals(key);
			});
		}

		public ICollection<KeyValuePair<T1, T2>> GetPairsWithValue(T2 value) {
			return FindAll(delegate(KeyValuePair<T1, T2> p) {
				var value2 = p.Value;

				return value2.Equals(value);
			});
		}

		public void Add(T1 first, T2 second) {
			Add(new KeyValuePair<T1, T2>(first, second));
		}

		public void Clamp(int max) {
			if (Count > max) {
				RemoveRange(max, Count - max);
			}
		}
	}
}
