using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

public class KeyInputChannel : IInputChannel {
	private bool _isDown;
	private string _name = string.Empty;
	private KeyCode _positiveKey;
	private bool _wasDown;

	public KeyCode Key {
		get { return _positiveKey; }
	}

	public KeyInputChannel(KeyCode positiveKey) {
		_positiveKey = positiveKey;
		InitKeyName();
	}

	public string Name {
		get { return _name; }
	}

	public InputChannelType ChannelType {
		get { return InputChannelType.Key; }
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
		_isDown = Input.GetKey(_positiveKey);
	}

	public void Reset() {
		_wasDown = false;
		_isDown = false;
	}

	public float RawValue() {
		return (!Input.GetKey(_positiveKey)) ? 0 : 1;
	}

	public void Serialize(MemoryStream stream) {
		EnumProxy<KeyCode>.Serialize(stream, _positiveKey);
	}

	private void InitKeyName() {
		var positiveKey = _positiveKey;

		switch (positiveKey) {
			case KeyCode.Keypad0:
				_name = "Keypad 0";

				break;
			case KeyCode.Keypad1:
				_name = "Keypad 1";

				break;
			case KeyCode.Keypad2:
				_name = "Keypad 2";

				break;
			case KeyCode.Keypad3:
				_name = "Keypad 3";

				break;
			case KeyCode.Keypad4:
				_name = "Keypad 4";

				break;
			case KeyCode.Keypad5:
				_name = "Keypad 5";

				break;
			case KeyCode.Keypad6:
				_name = "Keypad 6";

				break;
			case KeyCode.Keypad7:
				_name = "Keypad 7";

				break;
			case KeyCode.Keypad8:
				_name = "Keypad 8";

				break;
			case KeyCode.Keypad9:
				_name = "Keypad 9";

				break;
			case KeyCode.KeypadPeriod:
				_name = "Keypad Period";

				break;
			case KeyCode.KeypadDivide:
				_name = "Keypad Divide";

				break;
			case KeyCode.KeypadMultiply:
				_name = "Keypad Multiply";

				break;
			case KeyCode.KeypadMinus:
				_name = "Keypad Minus";

				break;
			case KeyCode.KeypadPlus:
				_name = "Keypad Plus";

				break;
			case KeyCode.KeypadEnter:
				_name = "Keypad Enter";

				break;
			case KeyCode.KeypadEquals:
				_name = "Keypad Equals";

				break;
			case KeyCode.UpArrow:
				_name = "Up Arrow";

				break;
			case KeyCode.DownArrow:
				_name = "Down Arrow";

				break;
			case KeyCode.RightArrow:
				_name = "Right Arrow";

				break;
			case KeyCode.LeftArrow:
				_name = "Left Arrow";

				break;
			default:
				switch (positiveKey) {
					case KeyCode.Alpha0:
						_name = "0";

						break;
					case KeyCode.Alpha1:
						_name = "1";

						break;
					case KeyCode.Alpha2:
						_name = "2";

						break;
					case KeyCode.Alpha3:
						_name = "3";

						break;
					case KeyCode.Alpha4:
						_name = "4";

						break;
					case KeyCode.Alpha5:
						_name = "5";

						break;
					case KeyCode.Alpha6:
						_name = "6";

						break;
					case KeyCode.Alpha7:
						_name = "7";

						break;
					case KeyCode.Alpha8:
						_name = "8";

						break;
					case KeyCode.Alpha9:
						_name = "9";

						break;
					default:
						_name = _positiveKey.ToString();

						break;
				}

				break;
			case KeyCode.RightShift:
				_name = "Right Shift";

				break;
			case KeyCode.LeftShift:
				_name = "Left Shift";

				break;
			case KeyCode.RightControl:
				_name = "Right Ctrl";

				break;
			case KeyCode.LeftControl:
				_name = "Left Ctrl";

				break;
			case KeyCode.RightAlt:
				_name = "Right Alt";

				break;
			case KeyCode.LeftAlt:
				_name = "Left Alt";

				break;
		}
	}

	public override string ToString() {
		return _positiveKey.ToString();
	}

	public override bool Equals(object obj) {
		if (obj is KeyInputChannel) {
			var keyInputChannel = obj as KeyInputChannel;

			if (keyInputChannel.Key == Key) {
				return true;
			}
		}

		return false;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	public static KeyInputChannel FromBytes(MemoryStream stream) {
		var keyCode = EnumProxy<KeyCode>.Deserialize(stream);

		return new KeyInputChannel(keyCode);
	}
}
