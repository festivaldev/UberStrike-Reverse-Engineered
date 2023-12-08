using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

public class MouseInputChannel : IInputChannel {
	private int _button;
	private bool _isDown;
	private bool _wasDown;

	public int Button {
		get { return _button; }
	}

	public MouseInputChannel(int button) {
		_button = button;
	}

	public string Name {
		get { return ConvertMouseButtonName(_button); }
	}

	public InputChannelType ChannelType {
		get { return InputChannelType.Mouse; }
	}

	public float Value {
		get { return (!_isDown) ? 0 : 1; }
		set { _isDown = (_wasDown = ((value == 0f) ? false : true)); }
	}

	public bool IsChanged {
		get { return _isDown != _wasDown; }
	}

	public void Listen() {
		_wasDown = _isDown;
		_isDown = Input.GetMouseButton(_button);
	}

	public float RawValue() {
		return (!Input.GetMouseButton(_button)) ? 0 : 1;
	}

	public void Reset() {
		_wasDown = false;
		_isDown = false;
	}

	public void Serialize(MemoryStream stream) {
		Int32Proxy.Serialize(stream, _button);
	}

	public override string ToString() {
		return string.Format("Mouse {0}", _button);
	}

	public override bool Equals(object obj) {
		if (obj is MouseInputChannel) {
			var mouseInputChannel = obj as MouseInputChannel;

			if (mouseInputChannel.Button == Button) {
				return true;
			}
		}

		return false;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	private string ConvertMouseButtonName(int _button) {
		if (_button == 0) {
			return "Left Mousebutton";
		}

		if (_button != 1) {
			return string.Format("Mouse {0}", _button);
		}

		return "Right Mousebutton";
	}

	public static MouseInputChannel FromBytes(MemoryStream stream) {
		var num = Int32Proxy.Deserialize(stream);

		return new MouseInputChannel(num);
	}
}
