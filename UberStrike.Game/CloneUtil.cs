using System;
using UberStrike.Realtime.UnitySdk;

public static class CloneUtil {
	public static T Clone<T>(T instance) where T : class {
		var type = instance.GetType();
		var constructor = type.GetConstructor(new Type[0]);

		if (constructor != null) {
			var t = constructor.Invoke(new object[0]) as T;
			CopyAllFields(t, instance);

			return t;
		}

		return (T)null;
	}

	public static void CopyAllFields<T>(T destination, T source) where T : class {
		foreach (var fieldInfo in ReflectionHelper.GetAllFields(source.GetType(), true)) {
			fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
		}
	}

	public static void CopyFields(object dst, object src) {
		foreach (var propertyInfo in src.GetType().GetProperties()) {
			if (propertyInfo.CanWrite) {
				dst.GetType().GetProperty(propertyInfo.Name).SetValue(dst, propertyInfo.GetValue(src, null), null);
			}
		}
	}
}
