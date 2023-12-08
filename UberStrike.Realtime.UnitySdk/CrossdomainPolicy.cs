using System;
using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk {
	public static class CrossdomainPolicy {
		private static Dictionary<string, bool?> _dict = new Dictionary<string, bool?>(20);
		public static Func<string, Action, IEnumerator> CheckPolicyRoutine = Default;

		private static IEnumerator Default(string address, Action callback) {
			SetPolicyValue(address, true);
			callback();

			yield break;
		}

		public static bool HasValidPolicy(string address) {
			var dict = _dict;
			bool? flag;

			lock (dict) {
				if (!_dict.TryGetValue(address, out flag)) {
					return false;
				}
			}

			return flag != null && flag.Value;
		}

		public static bool HasPolicyEntry(string address) {
			var dict = _dict;
			bool? flag;

			lock (dict) {
				_dict.TryGetValue(address, out flag);
			}

			return flag != null;
		}

		public static void RemovePolicyEntry(string address) {
			var dict = _dict;

			lock (dict) {
				_dict.Remove(address);
			}
		}

		public static void SetPolicyValue(string address, bool b) {
			var dict = _dict;

			lock (dict) {
				_dict[address] = b;
			}
		}
	}
}
