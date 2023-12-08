using System;
using System.Collections.Generic;
using System.Reflection;

namespace UberStrike.Realtime.UnitySdk {
	public static class ReflectionHelper {
		public const BindingFlags FieldBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
		public const BindingFlags InvokeBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

		public static List<FieldInfo> GetAllFields(Type type, bool inherited) {
			List<FieldInfo> list = new List<FieldInfo>();

			while (type != typeof(object)) {
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				list.AddRange(fields);

				if (!inherited) {
					break;
				}

				type = type.BaseType;
			}

			list.Sort((FieldInfo p, FieldInfo q) => p.Name.CompareTo(q.Name));

			return list;
		}

		public static List<PropertyInfo> GetAllProperties(Type type, bool inherited) {
			List<PropertyInfo> list = new List<PropertyInfo>();

			while (type != typeof(object)) {
				PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				list.AddRange(properties);

				if (!inherited) {
					break;
				}

				type = type.BaseType;
			}

			list.Sort((PropertyInfo p, PropertyInfo q) => p.Name.CompareTo(q.Name));

			return list;
		}

		public static List<MethodInfo> GetAllMethods(Type type, bool inherited) {
			List<MethodInfo> list = new List<MethodInfo>();

			while (type != typeof(object)) {
				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				list.AddRange(methods);

				if (!inherited) {
					break;
				}

				type = type.BaseType;
			}

			list.Sort((MethodInfo p, MethodInfo q) => p.Name.CompareTo(q.Name));

			return list;
		}

		public static void FilterByAttribute<T>(Type attribute, List<T> members) where T : MemberInfo {
			members.RemoveAll((T m) => m.GetCustomAttributes(attribute, false).Length == 0);
		}

		public static MethodInfo GetMethodWithParameters(List<MethodInfo> members, string name, params Type[] args) {
			MethodInfo methodInfo = null;

			foreach (MethodInfo methodInfo2 in members.FindAll((MethodInfo m) => m.Name == name)) {
				bool flag = true;
				ParameterInfo[] parameters = methodInfo2.GetParameters();

				if (parameters.Length == args.Length) {
					for (int i = 0; i < parameters.Length; i++) {
						flag &= parameters[i].ParameterType == args[i];
					}
				}

				if (flag) {
					methodInfo = methodInfo2;
				}
			}

			return methodInfo;
		}
	}
}
