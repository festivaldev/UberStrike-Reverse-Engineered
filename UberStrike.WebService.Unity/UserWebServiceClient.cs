using System;
using System.Collections.Generic;
using System.IO;
using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity {
	public static class UserWebServiceClient {
		public static Coroutine ChangeMemberName(string authToken, string name, string locale, string machineId, Action<MemberOperationResult> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, name);
				StringProxy.Serialize(memoryStream, locale);
				StringProxy.Serialize(memoryStream, machineId);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "ChangeMemberName", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(EnumProxy<MemberOperationResult>.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine IsDuplicateMemberName(string username, Action<bool> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, username);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "IsDuplicateMemberName", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GenerateNonDuplicatedMemberNames(string username, Action<List<string>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, username);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GenerateNonDuplicatedMemberNames", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<string>.Deserialize(new MemoryStream(data), StringProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMemberWallet(string authToken, Action<MemberWalletView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetMemberWallet", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(MemberWalletViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetInventory(string authToken, Action<List<ItemInventoryView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetInventory", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<ItemInventoryView>.Deserialize(new MemoryStream(data), ItemInventoryViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetCurrencyDeposits(string authToken, int pageIndex, int elementPerPage, Action<CurrencyDepositsViewModel> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, pageIndex);
				Int32Proxy.Serialize(memoryStream, elementPerPage);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetCurrencyDeposits", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(CurrencyDepositsViewModelProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetItemTransactions(string authToken, int pageIndex, int elementPerPage, Action<ItemTransactionsViewModel> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, pageIndex);
				Int32Proxy.Serialize(memoryStream, elementPerPage);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetItemTransactions", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ItemTransactionsViewModelProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetPointsDeposits(string authToken, int pageIndex, int elementPerPage, Action<PointDepositsViewModel> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, pageIndex);
				Int32Proxy.Serialize(memoryStream, elementPerPage);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetPointsDeposits", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(PointDepositsViewModelProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetLoadout(string authToken, Action<LoadoutView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetLoadout", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(LoadoutViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine SetLoadout(string authToken, LoadoutView loadoutView, Action<MemberOperationResult> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				LoadoutViewProxy.Serialize(memoryStream, loadoutView);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "SetLoadout", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(EnumProxy<MemberOperationResult>.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMember(string authToken, Action<UberstrikeUserViewModel> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetMember", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(UberstrikeUserViewModelProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMemberSessionData(string authToken, Action<MemberSessionDataView> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetMemberSessionData", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(MemberSessionDataViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}

			return coroutine;
		}

		public static Coroutine GetMemberListSessionData(List<string> authTokens, Action<List<MemberSessionDataView>> callback, Action<Exception> handler) {
			Coroutine coroutine;

			using (var memoryStream = new MemoryStream()) {
				ListProxy<string>.Serialize(memoryStream, authTokens, StringProxy.Serialize);

				coroutine = MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", "GetMemberListSessionData", memoryStream.ToArray(), delegate(byte[] data) {
					if (callback != null) {
						callback(ListProxy<MemberSessionDataView>.Deserialize(new MemoryStream(data), MemberSessionDataViewProxy.Deserialize));
					}
				}, handler));
			}

			return coroutine;
		}
	}
}
