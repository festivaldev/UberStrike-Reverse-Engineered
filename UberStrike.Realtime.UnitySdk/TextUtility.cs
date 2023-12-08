using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace UberStrike.Realtime.UnitySdk {
	public static class TextUtility {
		public static string ConvertText(string textToSecure) {
			var text = HtmlEncode(textToSecure);
			text = text.Replace("`", "&#96;");
			text = text.Replace("\u00b4", "&acute");
			text = text.Replace("'", "&#39");
			text = text.Replace("-", "&#45;");
			text = text.Replace("!", "&#33;");

			return text.Replace("?", "&#63;");
		}

		public static string HtmlEncode(string text) {
			if (text == null) {
				return null;
			}

			var stringBuilder = new StringBuilder(text.Length);
			var length = text.Length;

			for (var i = 0; i < length; i++) {
				var c = text[i];

				switch (c) {
					case '<':
						stringBuilder.Append("&lt;");

						break;
					default:
						if (c != '"') {
							if (c != '&') {
								if (text[i] > '\u009f') {
									stringBuilder.Append("&#");
									stringBuilder.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
									stringBuilder.Append(";");
								} else {
									stringBuilder.Append(text[i]);
								}
							} else {
								stringBuilder.Append("&amp;");
							}
						} else {
							stringBuilder.Append("&quot;");
						}

						break;
					case '>':
						stringBuilder.Append("&gt;");

						break;
				}
			}

			return stringBuilder.ToString();
		}

		public static string ProtectSqlField(string textToSecure) {
			return textToSecure.Replace("'", "''");
		}

		public static string ConvertTextForJavaScript(string textToSecure) {
			var text = textToSecure.Replace("'", string.Empty);

			return text.Replace("|", string.Empty);
		}

		public static long InetAToN(string addressIP) {
			var num = 0L;

			if (addressIP.Equals("::1")) {
				addressIP = "127.0.0.1";
			}

			if (!IsNullOrEmpty(addressIP)) {
				var array = addressIP.Split('.');

				if (array.Length == 4) {
					var flag = true;
					var num2 = 0;
					var num3 = 0L;

					for (var i = array.Length - 1; i >= 0; i--) {
						var flag2 = int.TryParse(array[i], out num2);

						if (flag2 && num2 >= 0 && num2 < 256) {
							num3 += num2 % 256 * (long)Math.Pow(256.0, 3 - i);
						} else {
							flag = false;
						}
					}

					if (flag) {
						num = num3;
					}
				}
			}

			return num;
		}

		public static string InetNToA(long networkAddress) {
			var str = string.Empty;

			if (networkAddress <= uint.MaxValue) {
				var num1 = networkAddress / 16777216L;

				if (num1 == 0L) {
					num1 = byte.MaxValue;
					networkAddress += 16777216L;
				} else if (num1 < 0L) {
					if (networkAddress % 16777216L == 0L) {
						num1 += 256L;
					} else {
						num1 += byte.MaxValue;

						if (num1 == 128L)
							networkAddress += 2147483648L;
						else
							networkAddress += 16777216L * (256L - num1);
					}
				} else
					networkAddress -= 16777216L * num1;

				networkAddress %= 16777216L;
				var num2 = networkAddress / 65536L;
				networkAddress %= 65536L;
				var num3 = networkAddress / 256L;
				networkAddress %= 256L;
				var num4 = networkAddress;
				str = num1 + "." + num2 + "." + num3 + "." + num4;
			}

			return str;
		}

		public static bool IsNumeric(string numericText) {
			var flag = true;

			if (!IsNullOrEmpty(numericText)) {
				if (numericText.StartsWith("-")) {
					numericText = numericText.Replace("-", string.Empty);
				}

				foreach (var c in numericText) {
					flag = char.IsNumber(c);

					if (!flag) {
						return flag;
					}
				}
			} else {
				flag = false;
			}

			return flag;
		}

		public static string ShortenText(string input, int maxSize, bool addPoints) {
			var text = input;

			if (maxSize < input.Length && maxSize > 3) {
				text = text.Substring(0, maxSize - 3);

				if (addPoints) {
					text += "...";
				}
			}

			return text;
		}

		public static bool IsNullOrEmpty(string value) {
			var flag = true;

			if (!string.IsNullOrEmpty(value)) {
				value = value.Trim();

				if (!string.IsNullOrEmpty(value)) {
					flag = false;
				}
			}

			return flag;
		}

		public static List<int> IndexOfAll(string haystack, string needle) {
			var num = 0;
			var list = new List<int>();

			if (!IsNullOrEmpty(haystack) && !IsNullOrEmpty(needle)) {
				var length = needle.Length;
				int num2;

				do {
					num2 = haystack.IndexOf(needle);

					if (num2 > -1) {
						haystack = haystack.Substring(num2 + length);
						list.Add(num2 + num);
						num += num2 + length;
					}
				} while (num2 > -1 && !IsNullOrEmpty(haystack));
			}

			return list;
		}

		public static string Base64Encode(string data) {
			var text = string.Empty;

			if (data != null) {
				var array = new byte[data.Length];
				array = Encoding.UTF8.GetBytes(data);
				text = Convert.ToBase64String(array);
			}

			return text;
		}

		public static string Base64Decode(string data) {
			var text = string.Empty;

			if (data != null) {
				var utf8Encoding = new UTF8Encoding();
				var decoder = utf8Encoding.GetDecoder();
				var array = Convert.FromBase64String(data);
				var charCount = decoder.GetCharCount(array, 0, array.Length);
				var array2 = new char[charCount];
				decoder.GetChars(array, 0, array.Length, array2, 0);
				text = new string(array2);
			}

			return text;
		}

		public static byte[] StringToByteArray(string inputString) {
			var utf8Encoding = new UTF8Encoding();

			return utf8Encoding.GetBytes(inputString);
		}

		public static string CompleteTrim(string text) {
			if (text != null) {
				text = text.Trim();
				text = Regex.Replace(text, "\\s+", " ");
			}

			return text;
		}

		public static bool TryParseFacebookId(string handle, out long facebookId) {
			var flag = false;
			facebookId = 0L;
			var flag2 = long.TryParse(handle, out facebookId);

			if (flag2 && facebookId > 0L) {
				flag = true;
			}

			return flag;
		}

		public static bool TryParseMySpaceId(string handle, out int mySpaceId) {
			var flag = false;
			mySpaceId = 0;
			var flag2 = int.TryParse(handle, out mySpaceId);

			if (flag2 && mySpaceId > 0) {
				flag = true;
			}

			return flag;
		}

		public static bool TryParseCyworldId(string handle, out int cyworldId) {
			var flag = false;
			cyworldId = 0;
			var flag2 = int.TryParse(handle, out cyworldId);

			if (flag2 && cyworldId > 0) {
				flag = true;
			}

			return flag;
		}

		public static string Join<T>(string separator, List<T> list) {
			var text = string.Empty;

			if (list != null && list.Count > 0) {
				var array = new string[list.Count];

				for (var i = 0; i < list.Count; i++) {
					var array2 = array;
					var num = i;
					var t = list[i];
					array2[num] = t.ToString();
				}

				text = string.Join(separator, array);
			}

			return text;
		}
	}
}
