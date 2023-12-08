using System.Collections;
using System.Text;
using UnityEngine;

public class ResultLogger : UnityEngine.Object {
	public static void logObject(object result) {
		if (result == null) {
			Debug.Log("attempting to log a null object");

			return;
		}

		if (result.GetType() == typeof(ArrayList)) {
			logArraylist((ArrayList)result);
		} else if (result.GetType() == typeof(Hashtable)) {
			logHashtable((Hashtable)result);
		} else {
			Debug.Log("result is not a hashtable or arraylist");
		}
	}

	public static void logArraylist(ArrayList result) {
		var stringBuilder = new StringBuilder();

		foreach (var obj in result) {
			var hashtable = (Hashtable)obj;
			addHashtableToString(stringBuilder, hashtable);
			stringBuilder.Append("\n--------------------\n");
		}

		Debug.Log(stringBuilder.ToString());
	}

	public static void logHashtable(Hashtable result) {
		var stringBuilder = new StringBuilder();
		addHashtableToString(stringBuilder, result);
		Debug.Log(stringBuilder.ToString());
	}

	public static void addHashtableToString(StringBuilder builder, Hashtable item) {
		foreach (var obj in item) {
			var dictionaryEntry = (DictionaryEntry)obj;

			if (dictionaryEntry.Value is Hashtable) {
				builder.AppendFormat("{0}: ", dictionaryEntry.Key);
				addHashtableToString(builder, (Hashtable)dictionaryEntry.Value);
			} else if (dictionaryEntry.Value is ArrayList) {
				builder.AppendFormat("{0}: ", dictionaryEntry.Key);
				addArraylistToString(builder, (ArrayList)dictionaryEntry.Value);
			} else {
				builder.AppendFormat("{0}: {1}\n", dictionaryEntry.Key, dictionaryEntry.Value);
			}
		}
	}

	public static void addArraylistToString(StringBuilder builder, ArrayList result) {
		foreach (var obj in result) {
			if (obj is Hashtable) {
				addHashtableToString(builder, (Hashtable)obj);
			} else if (obj is ArrayList) {
				addArraylistToString(builder, (ArrayList)obj);
			}

			builder.Append("\n--------------------\n");
		}
	}
}
