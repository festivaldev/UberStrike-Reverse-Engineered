using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class StringUtils {
	public static T ParseValue<T>(string value) {
		var typeFromHandle = typeof(T);
		var t = default(T);

		try {
			if (typeFromHandle.IsEnum) {
				t = (T)Enum.Parse(typeFromHandle, value);
			} else if (typeFromHandle == typeof(int)) {
				t = (T)((object)int.Parse(value));
			} else if (typeFromHandle == typeof(string)) {
				t = (T)((object)value);
			} else if (typeFromHandle == typeof(DateTime)) {
				t = (T)((object)DateTime.Parse(TextUtilities.Base64Decode(value)));
			} else {
				Debug.LogError(string.Concat("ParseValue couldn't find a conversion of value '", value, "' into type '", typeFromHandle.Name, "'"));
			}
		} catch (Exception ex) {
			Debug.LogError(string.Concat("ParseValue failed converting value '", value, "' into type '", typeFromHandle.Name, "': ", ex.Message));
		}

		return t;
	}
}
