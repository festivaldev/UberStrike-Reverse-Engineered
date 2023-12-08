using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk {
	public class LimitedQueue<T> : IEnumerable, IEnumerable<T> {
		private int _capacity;
		private List<T> _list;
		public T LastItem { get; private set; }

		public T this[int index] {
			get { return _list[index]; }
			set { _list[index] = value; }
		}

		public int Count {
			get { return _list.Count; }
		}

		public LimitedQueue(int capacity) {
			_capacity = capacity;
			_list = new List<T>(capacity);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return _list.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator() {
			return _list.GetEnumerator();
		}

		public bool Contains(T item) {
			return _list.Contains(item);
		}

		public bool Remove(T item) {
			return _list.Remove(item);
		}

		public bool EnqueueUnique(T item) {
			var num = _list.RemoveAll(p => p.Equals(item));
			Enqueue(item);

			return num == 0;
		}

		public void Enqueue(T item) {
			if (_list.Count + 1 > _capacity) {
				LastItem = Dequeue();
			} else {
				LastItem = default(T);
			}

			_list.Add(item);
		}

		public T Dequeue() {
			var t = default(T);

			if (_list.Count > 0) {
				t = _list[0];
				_list.RemoveAt(0);
			}

			return t;
		}

		public T Peek() {
			if (_list.Count > 0) {
				return _list[0];
			}

			return default(T);
		}

		public T Tail() {
			if (_list.Count > 0) {
				return _list[_list.Count - 1];
			}

			return default(T);
		}

		public void Clear() {
			_list.Clear();
		}
	}
}
