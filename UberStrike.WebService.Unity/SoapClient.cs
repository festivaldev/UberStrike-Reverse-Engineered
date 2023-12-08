using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	internal static class SoapClient {
		private static int _requestId;

		private static void LogRequest(int id, float time, int sizeBytes, string interfaceName, string serviceName, string methodName) {
			if (Configuration.RequestLogger != null) {
				var text = (sizeBytes / 1000f).ToString();
				Configuration.RequestLogger(string.Format("[REQ] ID:{0} Time:{1:N2} Size:{2:N2}Kb Service:{3} Interface:{4} Method:{5}", id, time, text, serviceName, interfaceName, methodName));
			}
		}

		private static void LogResponse(int id, float time, string message, float duration, int sizeBytes) {
			if (Configuration.RequestLogger != null) {
				var text = (sizeBytes / 1000f).ToString();
				Configuration.RequestLogger(string.Format("[RSP] ID:{0} Time:{1:N2} Size:{2:N2}Kb Duration:{3:N2}s Status:{4}", id, time, text, duration, message));
			}
		}

		public static IEnumerator MakeRequest(string interfaceName, string serviceName, string methodName, byte[] data, Action<byte[]> requestCallback, Action<Exception> exceptionHandler) {
			var requestId = _requestId++;
			var postData = string.Concat("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><", methodName, " xmlns=\"http://tempuri.org/\"><data>", Convert.ToBase64String(data), "</data></", methodName, "></s:Body></s:Envelope>");
			var byteArray = Encoding.UTF8.GetBytes(postData);
			var headers = new Dictionary<string, string>();
			headers.Add("SOAPAction", string.Concat("\"http://tempuri.org/", interfaceName, "/", methodName, "\""));
			headers.Add("Content-type", "text/xml; charset=utf-8");
			var doc = new XmlDocument();
			var startTime = Time.realtimeSinceStartup;
			LogRequest(requestId, startTime, data.Length, interfaceName, serviceName, methodName);

			yield return new WaitForEndOfFrame();

			if (WebServiceStatistics.IsEnabled) {
				WebServiceStatistics.RecordWebServiceBegin(methodName, byteArray.Length);
			}

			byte[] returnData = null;

			using (var request = new WWW(Configuration.WebserviceBaseUrl + serviceName, byteArray, headers)) {
				yield return request;

				if (WebServiceStatistics.IsEnabled) {
					WebServiceStatistics.RecordWebServiceEnd(methodName, request.bytes.Length, request.isDone && string.IsNullOrEmpty(request.error));
				}

				try {
					if (Configuration.SimulateWebservicesFail) {
						throw new Exception("Simulated Webservice fail when calling " + interfaceName + "/" + methodName);
					}

					if (!request.isDone || !string.IsNullOrEmpty(request.error)) {
						LogResponse(requestId, Time.realtimeSinceStartup, request.error, Time.time - startTime, 0);

						throw new Exception(string.Concat(request.error, "\nWWW Url: ", Configuration.WebserviceBaseUrl, "\nService: ", serviceName, "\nMethod: ", methodName));
					}

					if (!string.IsNullOrEmpty(request.text)) {
						try {
							doc.LoadXml(request.text);
							var result = doc.GetElementsByTagName(methodName + "Result");

							if (result.Count <= 0) {
								LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);

								throw new Exception(string.Concat("WWW Request to ", Configuration.WebserviceBaseUrl, serviceName, " failed with content", request.text));
							}

							returnData = Convert.FromBase64String(result[0].InnerXml);

							if (returnData.Length == 0) {
								LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);

								throw new Exception(string.Concat("WWW Request to ", Configuration.WebserviceBaseUrl, serviceName, " failed with content", request.text));
							}

							LogResponse(requestId, Time.realtimeSinceStartup, "OK", Time.realtimeSinceStartup - startTime, request.bytes.Length);
						} catch {
							LogResponse(requestId, Time.time, request.text, Time.realtimeSinceStartup - startTime, 0);

							throw new Exception(string.Concat("Error reading XML return for method call ", interfaceName, "/", methodName, ":", request.text));
						}
					}

					if (requestCallback != null) {
						requestCallback(returnData);
					}
				} catch (Exception ex) {
					var e = ex;

					if (exceptionHandler != null) {
						exceptionHandler(e);
					} else {
						Debug.LogError("SoapClient Unhandled Exception: " + e.Message + "\n" + e.StackTrace);
					}
				}
			}
		}
	}
}
