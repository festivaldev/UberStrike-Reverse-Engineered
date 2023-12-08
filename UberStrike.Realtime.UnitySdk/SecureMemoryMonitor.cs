using System;

namespace UberStrike.Realtime.UnitySdk {
	public class SecureMemoryMonitor {
		public static readonly SecureMemoryMonitor Instance = new SecureMemoryMonitor();
		private SecureMemoryMonitor() { }
		private event Action _sender;

		internal event Action AddToMonitor {
			add { _sender = (Action)Delegate.Combine(_sender, value); }
			remove { _sender = (Action)Delegate.Remove(_sender, value); }
		}

		public void PerformCheck() {
			if (_sender != null) {
				_sender();
			}
		}
	}
}
