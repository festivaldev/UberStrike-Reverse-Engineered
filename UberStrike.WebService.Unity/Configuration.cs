using System;

namespace UberStrike.WebService.Unity {
	public static class Configuration {
		public static string WebserviceBaseUrl = "http://localhost:9000/";
		public static string EncryptionInitVector = string.Empty;
		public static string EncryptionPassPhrase = string.Empty;
		public static Action<string> RequestLogger;
		public static bool SimulateWebservicesFail = false;
	}
}
