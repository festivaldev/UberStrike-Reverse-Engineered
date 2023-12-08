using System;
using System.Collections.Generic;
using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	public static class RelationshipWebServiceClient {
		public static Coroutine SendContactRequest(string authToken, int receiverCmid, string message, Action callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, receiverCmid);
				StringProxy.Serialize(memoryStream, message);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "SendContactRequest", memoryStream.ToArray(), delegate {
					if (callback != null) {
						callback();
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetContactRequests(string authToken, Action<List<ContactRequestView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "GetContactRequests", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<ContactRequestView>.Deserialize(new MemoryStream(data), ContactRequestViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine AcceptContactRequest(string authToken, int contactRequestId, Action<PublicProfileView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, contactRequestId);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "AcceptContactRequest", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(PublicProfileViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine DeclineContactRequest(string authToken, int contactRequestId, Action<bool> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, contactRequestId);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "DeclineContactRequest", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine DeleteContact(string authToken, int contactCmid, Action<MemberOperationResult> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, contactCmid);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "DeleteContact", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(EnumProxy<MemberOperationResult>.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetContactsByGroups(string authToken, bool populateFacebookIds, Action<List<ContactGroupView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				BooleanProxy.Serialize(memoryStream, populateFacebookIds);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", "GetContactsByGroups", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<ContactGroupView>.Deserialize(new MemoryStream(data), ContactGroupViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}
	}
}
