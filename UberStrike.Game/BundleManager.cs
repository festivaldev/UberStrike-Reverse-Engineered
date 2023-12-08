using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using Steamworks;
using UberStrike.WebService.Unity;
using UnityEngine;

public class BundleManager : Singleton<BundleManager> {
	private BasePopupDialog _appStorePopup;
	private Dictionary<BundleCategoryType, List<BundleUnityView>> _bundlesPerCategory;
	private float dialogTimer;
	private Callback<MicroTxnAuthorizationResponse_t> MicroTxnCallback;
	public int Count { get; private set; }
	public bool CanMakeMasPayments { get; private set; }

	public IEnumerable<BundleUnityView> AllItemBundles {
		get {
			foreach (var category in _bundlesPerCategory) {
				if (category.Key != BundleCategoryType.None) {
					foreach (var box in category.Value) {
						yield return box;
					}
				}
			}
		}
	}

	public IEnumerable<BundleUnityView> AllBundles {
		get {
			foreach (var bundleUnityViews in _bundlesPerCategory.Values) {
				foreach (var bundleUnityView in bundleUnityViews) {
					yield return bundleUnityView;
				}
			}
		}
	}

	private BundleManager() {
		_bundlesPerCategory = new Dictionary<BundleCategoryType, List<BundleUnityView>>();
	}

	private void OnMicroTxnCallback(MicroTxnAuthorizationResponse_t param) {
		Debug.Log("Steam MicroTxnParams: " + param);

		if (param.m_bAuthorized > 0) {
			ShopWebServiceClient.FinishBuyBundleSteam(param.m_ulOrderID.ToString(), delegate(bool success) {
				if (success) {
					PopupSystem.ClearAll();
					PopupSystem.ShowMessage("Purchase Successful", "Thank you, your purchase was successful.", PopupSystem.AlertType.OK, delegate { ApplicationDataManager.RefreshWallet(); });
				} else {
					Debug.Log("Managed error from WebServices");
					PopupSystem.ClearAll();
					PopupSystem.ShowMessage("Purchase Failed", "Sorry, there was a problem processing your payment. Please visit support.uberstrike.com for help.", PopupSystem.AlertType.OK);
				}
			}, delegate(Exception ex) {
				Debug.Log(ex.Message);
				PopupSystem.ClearAll();
				PopupSystem.ShowMessage("Purchase Failed", "Sorry, there was a problem processing your payment. Please visit support.uberstrike.com for help.", PopupSystem.AlertType.OK);
			});
		} else {
			Debug.Log("Purchase canceled");
			PopupSystem.ClearAll();
		}
	}

	public List<BundleUnityView> GetCreditBundles() {
		var list = new List<BundleUnityView>();
		_bundlesPerCategory.TryGetValue(BundleCategoryType.None, out list);

		return list;
	}

	public void Initialize() {
		MicroTxnCallback = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnCallback);
		ShopWebServiceClient.GetBundles(ApplicationDataManager.Channel, delegate(List<BundleView> bundles) { SetBundles(bundles); }, delegate { Debug.LogError("Error getting " + ApplicationDataManager.Channel + " bundles from the server."); });
	}

	private void SetBundles(List<BundleView> bundleViews) {
		if (bundleViews != null && bundleViews.Count > 0) {
			foreach (var bundleView in bundleViews) {
				List<BundleUnityView> list;

				if (!_bundlesPerCategory.TryGetValue(bundleView.Category, out list)) {
					list = new List<BundleUnityView>();
					_bundlesPerCategory[bundleView.Category] = list;
				}

				list.Add(new BundleUnityView(bundleView));
			}

			Count = 0;

			foreach (var bundleUnityView in AllBundles) {
				bundleUnityView.CurrencySymbol = "$";
				bundleUnityView.Price = bundleUnityView.BundleView.USDPrice.ToString("N2");
				bundleUnityView.IsOwned = false;
				Count++;
			}
		} else {
			Debug.LogError("SetBundles: Bundles received from the server were null or empty!");
		}
	}

	public IEnumerator StartCancelDialogTimer() {
		if (dialogTimer < 5f) {
			dialogTimer = 5f;
		}

		while (_appStorePopup != null && dialogTimer > 0f) {
			dialogTimer -= Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		if (_appStorePopup != null) {
			_appStorePopup.SetAlertType(PopupSystem.AlertType.Cancel);
		}
	}

	public void BuyBundle(BundleUnityView bundle) {
		Debug.Log("Trying to buy bundle with id id: " + bundle.BundleView.Id);
		var id = bundle.BundleView.Id;
		var steamId = PlayerDataManager.SteamId;
		var authToken = PlayerDataManager.AuthToken;

		ShopWebServiceClient.BuyBundleSteam(id, steamId, authToken, delegate(bool success) {
			if (!success) {
				Debug.Log("Starting steam payment failed! (Handled WS Error)");
				PopupSystem.ClearAll();
				PopupSystem.ShowMessage("Purchase Failed", "Sorry, there was a problem processing your payment. Please visit support.uberstrike.com for help.", PopupSystem.AlertType.OK);
			}
		}, delegate(Exception ex) {
			Debug.Log(ex.Message);
			PopupSystem.ClearAll();
			PopupSystem.ShowMessage("Purchase Failed", "Sorry, there was a problem processing your payment. Please visit support.uberstrike.com for help.", PopupSystem.AlertType.OK);
		});

		_appStorePopup = PopupSystem.ShowMessage("In App Purchase", "Purchasing, please wait...", PopupSystem.AlertType.None) as BasePopupDialog;
		UnityRuntime.StartRoutine(StartCancelDialogTimer());
	}

	private bool IsItemPackOwned(List<BundleItemView> items) {
		if (items.Count > 0) {
			foreach (var bundleItemView in items) {
				if (!Singleton<InventoryManager>.Instance.Contains(bundleItemView.ItemId)) {
					return false;
				}
			}

			return true;
		}

		return false;
	}

	public BundleUnityView GetNextItem(BundleUnityView currentItem) {
		var list = new List<BundleUnityView>(AllItemBundles);

		if (list.Count <= 0) {
			return currentItem;
		}

		var num = list.FindIndex(i => i == currentItem);

		if (num < 0) {
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		var num2 = (num + 1) % list.Count;

		return list[num2];
	}

	public BundleUnityView GetPreviousItem(BundleUnityView currentItem) {
		var list = new List<BundleUnityView>(AllItemBundles);

		if (list.Count <= 0) {
			return currentItem;
		}

		var num = list.FindIndex(i => i == currentItem);

		if (num < 0) {
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		var num2 = (num - 1 + list.Count) % list.Count;

		return list[num2];
	}
}
