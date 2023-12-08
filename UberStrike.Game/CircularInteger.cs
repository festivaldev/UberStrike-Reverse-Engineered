using System;

public class CircularInteger {
	private int _current;
	private int _length;
	private int _lower;

	public int Current {
		get { return _current + _lower; }
		set {
			if (value >= _lower + _length && value < _lower) {
				throw new Exception("CircularInteger: Assigned value not in range!");
			}

			_current = value - _lower;
		}
	}

	public int Next {
		get {
			_current = (_current + 1) % _length;

			return Current;
		}
	}

	public int Prev {
		get {
			_current = (_current + _length - 1) % _length;

			return Current;
		}
	}

	public int First {
		get {
			_current = 0;

			return Current;
		}
	}

	public int Last {
		get {
			_current = _length - 1;

			return Current;
		}
	}

	public int Range {
		get { return _length; }
	}

	public CircularInteger(int lowerBound, int upperBound) {
		SetRange(lowerBound, upperBound);
	}

	public void SetRange(int lowerBound, int upperBound) {
		if (lowerBound >= upperBound) {
			throw new Exception("CircularInteger ctor failed because lowerBound greater than upperBound");
		}

		_current = 0;
		_lower = lowerBound;
		_length = upperBound - lowerBound + 1;
	}

	public void Reset() {
		_current = 0;
	}
}
