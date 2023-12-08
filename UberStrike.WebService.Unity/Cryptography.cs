namespace UberStrike.WebService.Unity {
	public static class Cryptography {
		public static ICryptographyPolicy Policy = new NullCryptographyPolicy();

		public static string SHA256Encrypt(string inputString) {
			return Policy.SHA256Encrypt(inputString);
		}

		public static byte[] RijndaelEncrypt(byte[] inputClearText, string passPhrase, string initVector) {
			return Policy.RijndaelEncrypt(inputClearText, passPhrase, initVector);
		}

		public static byte[] RijndaelDecrypt(byte[] inputCipherText, string passPhrase, string initVector) {
			return Policy.RijndaelDecrypt(inputCipherText, passPhrase, initVector);
		}
	}
}
