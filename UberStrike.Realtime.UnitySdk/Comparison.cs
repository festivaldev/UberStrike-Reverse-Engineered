using System.Collections;

namespace UberStrike.Realtime.UnitySdk {
	public static class Comparison {
		public static bool IsEqual(object a, object b) {
			if (ReferenceEquals(a, b)) {
				return true;
			}

			if (a == null || b == null) {
				return false;
			}

			if (a is ICollection && b is ICollection) {
				return IsSequenceEqual(a as ICollection, b as ICollection);
			}

			return a.Equals(b);
		}

		private static bool IsSequenceEqual(ICollection a1, ICollection a2) {
			if (a1 != null && a2 != null) {
				var flag = true;
				var enumerator = a1.GetEnumerator();
				var enumerator2 = a2.GetEnumerator();

				while (flag && enumerator.MoveNext() && enumerator2.MoveNext()) {
					if (enumerator.Current is ICollection && enumerator2.Current is ICollection) {
						flag = IsSequenceEqual(enumerator.Current as ICollection, enumerator2.Current as ICollection);
					} else {
						flag = enumerator.Current != null && enumerator.Current.Equals(enumerator2.Current);
					}
				}

				return flag;
			}

			return false;
		}
	}
}
