using System;
using System.Collections.Generic;
using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	public static class PrivateMessageWebServiceClient {
		public static Coroutine GetAllMessageThreadsForUser(string authToken, int pageNumber, Action<List<MessageThreadView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, pageNumber);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "GetAllMessageThreadsForUser", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<MessageThreadView>.Deserialize(new MemoryStream(data), MessageThreadViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetThreadMessages(string authToken, int otherCmid, int pageNumber, Action<List<PrivateMessageView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);
				Int32Proxy.Serialize(memoryStream, pageNumber);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "GetThreadMessages", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<PrivateMessageView>.Deserialize(new MemoryStream(data), PrivateMessageViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine SendMessage(string authToken, int receiverCmid, string content, Action<PrivateMessageView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, receiverCmid);
				StringProxy.Serialize(memoryStream, content);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "SendMessage", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(PrivateMessageViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMessageWithIdForCmid(string authToken, int messageId, Action<PrivateMessageView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, messageId);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "GetMessageWithIdForCmid", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(PrivateMessageViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine MarkThreadAsRead(string authToken, int otherCmid, Action callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "MarkThreadAsRead", memoryStream.ToArray(), delegate {
					if (callback != null) {
						callback();
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine DeleteThread(string authToken, int otherCmid, Action callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "UberStrike.DataCenter.WebService.CWS.PrivateMessageWebServiceContract.svc", "DeleteThread", memoryStream.ToArray(), delegate {
					if (callback != null) {
						callback();
					}
				}, handler));
			}

			return coroutine;
		}
	}
}
