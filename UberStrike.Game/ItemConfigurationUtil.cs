using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;

public static class ItemConfigurationUtil {
	private static Dictionary<Type, List<FieldInfo>> fields = new Dictionary<Type, List<FieldInfo>>();

	private static List<FieldInfo> GetAllFields(Type type) {
		List<FieldInfo> allFields;

		if (!fields.TryGetValue(type, out allFields)) {
			allFields = ReflectionHelper.GetAllFields(type, true);
			fields[type] = allFields;
		}

		return allFields;
	}

	public static void CopyProperties<T>(T config, BaseUberStrikeItemView item) where T : BaseUberStrikeItemView {
		CloneUtil.CopyAllFields(config, item);

		foreach (var fieldInfo in GetAllFields(config.GetType())) {
			var customPropertyName = GetCustomPropertyName(fieldInfo);

			if (!string.IsNullOrEmpty(customPropertyName) && item.CustomProperties != null && item.CustomProperties.ContainsKey(customPropertyName)) {
				fieldInfo.SetValue(config, Convert(item.CustomProperties[customPropertyName], fieldInfo.FieldType));
			}
		}
	}

	public static void CopyCustomProperties(BaseUberStrikeItemView src, object dst) {
		foreach (var fieldInfo in GetAllFields(dst.GetType())) {
			var customPropertyName = GetCustomPropertyName(fieldInfo);

			if (!string.IsNullOrEmpty(customPropertyName) && src.CustomProperties != null && src.CustomProperties.ContainsKey(customPropertyName)) {
				var obj = Convert(src.CustomProperties[customPropertyName], fieldInfo.FieldType);
				fieldInfo.SetValue(dst, obj);
			}
		}
	}

	private static string GetCustomPropertyName(FieldInfo info) {
		var customAttributes = info.GetCustomAttributes(typeof(CustomPropertyAttribute), true);

		return (customAttributes.Length <= 0) ? string.Empty : ((CustomPropertyAttribute)customAttributes[0]).Name;
	}

	private static object Convert(string value, Type type) {
		if (type == typeof(string)) {
			return value;
		}

		if (type.IsEnum || type == typeof(int)) {
			return int.Parse(value, CultureInfo.InvariantCulture);
		}

		if (type == typeof(float)) {
			return float.Parse(value, CultureInfo.InvariantCulture);
		}

		if (type == typeof(bool)) {
			return bool.Parse(value);
		}

		throw new NotSupportedException("ConfigurableItem has unsupported property of type: " + type);
	}
}
