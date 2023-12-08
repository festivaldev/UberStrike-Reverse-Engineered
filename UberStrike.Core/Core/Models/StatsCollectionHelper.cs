using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UberStrike.Core.Models {
	public static class StatsCollectionHelper {
		private static List<PropertyInfo> properties = new List<PropertyInfo>();

		static StatsCollectionHelper() {
			var array = typeof(StatsCollection).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var propertyInfo in array) {
				if (propertyInfo.PropertyType == typeof(int) && propertyInfo.CanRead && propertyInfo.CanWrite) {
					properties.Add(propertyInfo);
				}
			}
		}

		public static string ToString(StatsCollection instance) {
			var stringBuilder = new StringBuilder();

			foreach (var propertyInfo in properties) {
				stringBuilder.AppendFormat("{0}:{1}\n", propertyInfo.Name, propertyInfo.GetValue(instance, null));
			}

			return stringBuilder.ToString();
		}

		public static void Reset(StatsCollection instance) {
			foreach (var propertyInfo in properties) {
				propertyInfo.SetValue(instance, 0, null);
			}
		}

		public static void TakeBestValues(StatsCollection instance, StatsCollection that) {
			foreach (var propertyInfo in properties) {
				var num = (int)propertyInfo.GetValue(instance, null);
				var num2 = (int)propertyInfo.GetValue(that, null);

				if (num < num2) {
					propertyInfo.SetValue(instance, num2, null);
				}
			}
		}

		public static void AddAllValues(StatsCollection instance, StatsCollection that) {
			foreach (var propertyInfo in properties) {
				var num = (int)propertyInfo.GetValue(instance, null);
				var num2 = (int)propertyInfo.GetValue(that, null);
				propertyInfo.SetValue(instance, num + num2, null);
			}
		}
	}
}
