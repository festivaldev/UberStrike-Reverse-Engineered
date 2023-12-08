using Cmune.DataCenter.Common.Entities;

public static class GlobalEvents {
	public class Logout { }

	public class Login {
		public MemberAccessLevel AccessLevel { get; private set; }

		public Login(MemberAccessLevel level) {
			AccessLevel = level;
		}
	}

	public class MobileBackPressed { }

	public class GlobalUIRibbonChanged { }

	public class ScreenshotTaken { }

	public class InputAssignment { }

	public class InputChanged {
		public GameInputKey Key { get; private set; }
		public float Value { get; private set; }

		public bool IsDown {
			get { return Value != 0f; }
		}

		public InputChanged(GameInputKey key, float value) {
			Key = key;
			Value = value;
		}
	}

	public class CameraWidthChanged { }

	public class ScreenResolutionChanged { }

	public class ClanCreated { }

	public class GamePageChanged { }
}
