using System;
using System.Collections.Generic;
using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	public static class ApplicationWebServiceClient {
		public static Coroutine AuthenticateApplication(string clientVersion, ChannelType channel, string publicKey, Action<AuthenticateApplicationView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, clientVersion);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				StringProxy.Serialize(memoryStream, publicKey);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", "AuthenticateApplication", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(AuthenticateApplicationViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMaps(string clientVersion, DefinitionType clientType, Action<List<MapView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, clientVersion);
				EnumProxy<DefinitionType>.Serialize(memoryStream, clientType);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", "GetMaps", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<MapView>.Deserialize(new MemoryStream(data), MapViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetConfigurationData(string clientVersion, Action<ApplicationConfigurationView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, clientVersion);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", "GetConfigurationData", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ApplicationConfigurationViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine SetMatchScore(string clientVersion, MatchStats scoringView, string serverAuthentication, Action callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, clientVersion);
				MatchStatsProxy.Serialize(memoryStream, scoringView);
				StringProxy.Serialize(memoryStream, serverAuthentication);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", "SetMatchScore", memoryStream.ToArray(), delegate {
					if (callback != null) {
						callback();
					}
				}, handler));
			}

			return coroutine;
		}
	}
}
