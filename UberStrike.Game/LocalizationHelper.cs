using UberStrike.Core.Types;
using UnityEngine;

public static class LocalizationHelper {
	public static bool ValidateMemberName(string name, LocaleType locale = LocaleType.en_US) {
		if (locale != LocaleType.ko_KR) {
			return ValidationUtilities.IsValidMemberName(name);
		}

		return ValidationUtilities.IsValidMemberName(name, "ko-KR");
	}

	public static GUIStyle GetLocalizedStyle(GUIStyle style) {
		return style;
	}
}
