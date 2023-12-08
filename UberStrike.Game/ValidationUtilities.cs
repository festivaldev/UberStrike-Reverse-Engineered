using System.Collections.Generic;
using System.Text.RegularExpressions;
using UberStrike.Realtime.UnitySdk;

public static class ValidationUtilities {
	private static Dictionary<string, int> map;

	public static bool IsValidEmailAddress(string email) {
		if (TextUtilities.IsNullOrEmpty(email) || email.Length > 100) {
			return false;
		}

		var num = email.IndexOf('@');
		var num2 = email.LastIndexOf('@');

		return num > 0 && num2 == num && num < email.Length - 1 && Regex.IsMatch(email, "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$");
	}

	public static bool IsValidPassword(string password) {
		var flag = false;

		if (!TextUtilities.IsNullOrEmpty(password) && password.Length > 3 && password.Length < 64) {
			flag = true;
		}

		return flag;
	}

	public static bool IsValidMemberName(string memberName) {
		return IsValidMemberName(memberName, "en-US");
	}

	public static bool IsValidMemberName(string memberName, string locale) {
		var flag = false;

		if (!string.IsNullOrEmpty(memberName)) {
			memberName = memberName.Trim();
			int num;

			if (memberName.Equals(TextUtilities.CompleteTrim(memberName))) {
				var text = string.Empty;

				if (locale != null) {
					if (map == null) {
						map = new Dictionary<string, int>(1) {
							{
								"ko-KR", 0
							}
						};
					}

					if (map.TryGetValue(locale, out num)) {
						if (num == 0) {
							text = "\\p{IsHangulSyllables}";

							goto IL_8B;
						}
					}
				}

				text = string.Empty;
				IL_8B:
				flag = Regex.IsMatch(memberName, string.Concat("^[a-zA-Z0-9 .!_\\-<>{}~@#$%^&*()=+|:?", text, "]{", 3, ",", 18, "}$"));
			}

			num = memberName.ToLower().IndexOf("admin");

			if (!num.Equals(-1) || !memberName.ToLower().IndexOf("cmune").Equals(-1)) {
				flag = false;
			}
		}

		return flag;
	}
}
