using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

public class ButtonInputChannel : IInputChannel {
	private string _button = string.Empty;
	private bool _isDown;
	private bool _wasDown;

	public string Button {
		get { return _button; }
	}

	public bool IsPressed {
		get { return _isDown; }
	}

	public ButtonInputChannel(string button) {
		_button = button;
	}

	public string Name {
		get { return _button; }
	}

	public float Value {
		get { return (!_isDown) ? 0 : 1; }
	}

	public InputChannelType ChannelType {
		get { return InputChannelType.Axis; }
	}

	public bool IsChanged {
		get { return _wasDown != _isDown; }
	}

	public void Listen() {
		_wasDown = _isDown;
		_isDown = Input.GetButton(_button) && !Input.GetMouseButton(0);
	}

	public void Reset() {
		_wasDown = false;
		_isDown = false;
	}

	public float RawValue() {
		return (!Input.GetButton(_button)) ? 0 : 1;
	}

	public void Serialize(MemoryStream stream) {
		StringProxy.Serialize(stream, _button);
	}

	public override string ToString() {
		return _button;
	}

	public override bool Equals(object obj) {
		if (obj is ButtonInputChannel) {
			var buttonInputChannel = obj as ButtonInputChannel;

			if (buttonInputChannel.Button == Button) {
				return true;
			}
		}

		return false;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	public static ButtonInputChannel FromBytes(MemoryStream stream) {
		var text = StringProxy.Deserialize(stream);

		return new ButtonInputChannel(text);
	}
}
