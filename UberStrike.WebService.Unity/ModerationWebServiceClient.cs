using System;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	public static class ModerationWebServiceClient {
		public static Coroutine BanPermanently(string authToken, int targetCmid, Action<bool> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, targetCmid);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IModerationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ModerationWebServiceContract.svc", "BanPermanently", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}
	}
}
