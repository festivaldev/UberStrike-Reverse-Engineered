using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

public class AxisInputChannel : IInputChannel {
	public enum AxisReadingMethod {
		All,
		PositiveOnly,
		NegativeOnly
	}

	private string _axis = string.Empty;
	private string _axisName = string.Empty;
	private AxisReadingMethod _axisReading;
	private float _deadRange = 0.1f;
	private float _lastValue;
	private float _value;

	public string Axis {
		get { return _axis; }
	}

	public bool IsPressed {
		get { return _value != 0f; }
	}

	public AxisInputChannel(string axis) {
		_axis = axis;
		_axisName = _axis;
	}

	public AxisInputChannel(string axis, float deadRange) : this(axis) {
		_deadRange = deadRange;
	}

	public AxisInputChannel(string axis, float deadRange, AxisReadingMethod method) : this(axis, deadRange) {
		_axisReading = method;

		if (method != AxisReadingMethod.PositiveOnly) {
			if (method == AxisReadingMethod.NegativeOnly) {
				_axisName += " Up";
			}
		} else {
			_axisName += " Down";
		}
	}

	public string Name {
		get { return _axisName; }
	}

	public float Value {
		get { return _value; }
		set {
			_lastValue = value;
			_value = value;
		}
	}

	public InputChannelType ChannelType {
		get { return InputChannelType.Axis; }
	}

	public bool IsChanged {
		get { return _lastValue != _value; }
	}

	public void Listen() {
		_lastValue = _value;
		_value = RawValue();
		var axisReading = _axisReading;

		if (axisReading != AxisReadingMethod.PositiveOnly) {
			if (axisReading == AxisReadingMethod.NegativeOnly) {
				if (_value > 0f) {
					_value = 0f;
				}
			}
		} else if (_value < 0f) {
			_value = 0f;
		}

		if (Mathf.Abs(_value) < _deadRange) {
			_value = 0f;
		}
	}

	public void Reset() {
		_value = 0f;
		_lastValue = 0f;
	}

	public float RawValue() {
		return Input.GetAxis(_axis);
	}

	public void Serialize(MemoryStream stream) {
		StringProxy.Serialize(stream, _axis);
		SingleProxy.Serialize(stream, _deadRange);
		EnumProxy<AxisReadingMethod>.Serialize(stream, _axisReading);
	}

	public override string ToString() {
		return _axis;
	}

	public override bool Equals(object obj) {
		if (obj is AxisInputChannel) {
			var axisInputChannel = obj as AxisInputChannel;

			if (axisInputChannel.Axis == Axis) {
				return true;
			}
		}

		return false;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	public static AxisInputChannel FromBytes(MemoryStream stream) {
		var text = StringProxy.Deserialize(stream);
		var num = SingleProxy.Deserialize(stream);
		var axisReadingMethod = EnumProxy<AxisReadingMethod>.Deserialize(stream);

		return new AxisInputChannel(text, num, axisReadingMethod);
	}
}
