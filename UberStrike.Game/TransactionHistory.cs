using System.Collections.Generic;
using UberStrike.Core.ViewModel;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class TransactionHistory : Singleton<TransactionHistory> {
	private const float RowHeight = 23f;
	private const float ButtonWidth = 100f;
	private const float ButtonHeight = 32f;
	private const int ElementsPerPage = 15;
	private static string DATE_FORMAT = "yyyy/MM/dd";
	private string[] _creditsTableColumnHeadingArray;
	private TransactionCache<CurrencyDepositsViewModel> _creditTransactions;
	private string[] _itemsTableColumnHeadingArray;
	private TransactionCache<ItemTransactionsViewModel> _itemTransactions;
	private string _nextPageButtonLabel;
	private string[] _pointsTableColumnHeadingArray;
	private TransactionCache<PointDepositsViewModel> _pointTransactions;
	private string _prevPageButtonLabel;
	private Vector2 _scrollControls;
	private int _selectedTab;
	private GUIContent[] _tabs;

	private TransactionHistory() {
		_itemTransactions = new TransactionCache<ItemTransactionsViewModel>();
		_pointTransactions = new TransactionCache<PointDepositsViewModel>();
		_creditTransactions = new TransactionCache<CurrencyDepositsViewModel>();

		_tabs = new[] {
			new GUIContent("Items"),
			new GUIContent("Points"),
			new GUIContent("Credits")
		};

		_itemsTableColumnHeadingArray = new[] {
			"Date",
			"Item Name",
			"Points",
			"Credits",
			"Duration"
		};

		_pointsTableColumnHeadingArray = new[] {
			"Date",
			"Points",
			"Type"
		};

		_creditsTableColumnHeadingArray = new[] {
			"Transaction Key",
			"Date",
			"Cost",
			"Credits",
			"Points",
			"Bundle Name"
		};

		_prevPageButtonLabel = "Prev Page";
		_nextPageButtonLabel = "Next Page";
	}

	public void DrawPanel(Rect panelRect) {
		GUI.BeginGroup(panelRect, GUIContent.none, BlueStonez.window_standard_grey38);
		DrawTabs(new Rect(2f, 5f, panelRect.width - 4f, 30f));
		DrawTable(new Rect(2f, 35f, panelRect.width - 4f, panelRect.height - 35f));
		GUI.EndGroup();
	}

	private void DrawTable(Rect panelRect) {
		var rect = new Rect(panelRect.x + 5f, panelRect.y, panelRect.width - 10f, 25f);
		var rect2 = new Rect(panelRect.x + 5f, panelRect.y + 30f, panelRect.width - 10f, panelRect.height - 35f - 32f - 5f);
		var rect3 = new Rect(0f, rect2.y + rect2.height, panelRect.width, panelRect.height - rect2.height);
		var selectedTab = (TransactionType)_selectedTab;

		if (selectedTab == TransactionType.Item) {
			DrawItemsTableHeadingBar(rect);
			DrawItemsTableContent(rect2);
			DrawItemsButtons(rect3);
		} else if (selectedTab == TransactionType.Point) {
			DrawPointsTableHeadingBar(rect);
			DrawPointsTableContent(rect2);
			DrawPointsButtons(rect3);
		} else if (selectedTab == TransactionType.Credit) {
			DrawCreditsTableHeadingBar(rect);
			DrawCreditsTableContent(rect2);
			DrawCreditsButtons(rect3);
		}
	}

	private void DrawTabs(Rect tabRect) {
		var num = UnityGUI.Toolbar(tabRect, _selectedTab, _tabs, _tabs.Length, BlueStonez.tab_medium);

		if (num != _selectedTab) {
			_selectedTab = num;
			GetCurrentTransactions();
		}
	}

	private float GetColumnOffset(AccountArea area, int index, float totalWidth) {
		if (area == AccountArea.Items) {
			switch (index) {
				case 0:
					return 0f;
				case 1:
					return 100f;
				case 2:
					return 100f + Mathf.Max(totalWidth, 400f) - 300f;
				case 3:
					return 100f + Mathf.Max(totalWidth, 400f) - 300f + 50f;
				case 4:
					return 100f + Mathf.Max(totalWidth, 400f) - 300f + 50f + 50f;
				default:
					return 0f;
			}
		}

		if (area == AccountArea.Points) {
			return index * Mathf.RoundToInt(totalWidth / 3f);
		}

		if (area != AccountArea.Credits) {
			return 0f;
		}

		switch (index) {
			case 0:
				return 0f;
			case 1:
				return 150f;
			case 2:
				return 220f;
			case 3:
				return 270f;
			case 4:
				return 320f;
			case 5:
				return 370f;
			default:
				return 0f;
		}
	}

	private float GetColumnWidth(AccountArea area, int index, float totalWidth) {
		if (area == AccountArea.Items) {
			switch (index) {
				case 0:
					return 151f;
				case 1:
					return Mathf.Max(totalWidth, 400f) - 300f + 1f;
				case 2:
					return 51f;
				case 3:
					return 51f;
				case 4:
					return 100f;
				default:
					return 0f;
			}
		}

		if (area == AccountArea.Points) {
			switch (index) {
				case 0:
					return Mathf.RoundToInt(totalWidth / 3f) + 1;
				case 1:
					return Mathf.RoundToInt(totalWidth / 3f) + 1;
				case 2:
					return Mathf.RoundToInt(totalWidth / 3f);
				default:
					return 0f;
			}
		}

		if (area != AccountArea.Credits) {
			return 0f;
		}

		switch (index) {
			case 0:
				return 151f;
			case 1:
				return 71f;
			case 2:
				return 51f;
			case 3:
				return 51f;
			case 4:
				return 51f;
			case 5:
				return Mathf.Max(totalWidth, 450f) - 370f;
			default:
				return 0f;
		}
	}

	private void DrawItemsTableHeadingBar(Rect headingRect) {
		GUI.BeginGroup(headingRect);

		for (var i = 0; i < _itemsTableColumnHeadingArray.Length; i++) {
			var rect = new Rect(GetColumnOffset(AccountArea.Items, i, headingRect.width), 0f, GetColumnWidth(AccountArea.Items, i, headingRect.width), headingRect.height);
			GUI.Button(rect, string.Empty, BlueStonez.box_grey50);
			GUI.Label(rect, new GUIContent(_itemsTableColumnHeadingArray[i]), BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();
	}

	private void DrawItemsTableContent(Rect scrollViewRect) {
		GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);

		if (_itemTransactions.CurrentPage != null) {
			_scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), _scrollControls, new Rect(0f, 0f, scrollViewRect.width - 17f, _itemTransactions.CurrentPage.ItemTransactions.Count * 23f));
			var num = 0f;

			foreach (var itemTransactionView in _itemTransactions.CurrentPage.ItemTransactions) {
				var itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemTransactionView.ItemId);
				var text = ((itemInShop == null) ? string.Format("item[{0}]", itemTransactionView.ItemId) : TextUtility.ShortenText(itemInShop.Name, 20, true));
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Items, 0, scrollViewRect.width), num, GetColumnWidth(AccountArea.Items, 0, scrollViewRect.width), 23f), itemTransactionView.WithdrawalDate.ToString(DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Items, 1, scrollViewRect.width), num, GetColumnWidth(AccountArea.Items, 1, scrollViewRect.width), 23f), text, BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Items, 2, scrollViewRect.width), num, GetColumnWidth(AccountArea.Items, 2, scrollViewRect.width), 23f), itemTransactionView.Points.ToString(), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Items, 3, scrollViewRect.width), num, GetColumnWidth(AccountArea.Items, 3, scrollViewRect.width), 23f), itemTransactionView.Credits.ToString(), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Items, 4, scrollViewRect.width), num, GetColumnWidth(AccountArea.Items, 4, scrollViewRect.width), 23f), ShopUtils.PrintDuration(itemTransactionView.Duration), BlueStonez.label_interparkmed_11pt);
				num += 23f;
			}

			GUITools.EndScrollView();
		}
	}

	private void DrawItemsButtons(Rect rect) {
		var button = BlueStonez.button;
		GUI.enabled = _itemTransactions.CurrentPageIndex != 0;

		if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(_prevPageButtonLabel), button)) {
			_itemTransactions.CurrentPageIndex--;
			AsyncGetItemTransactions();
		}

		GUI.enabled = true;

		if (_itemTransactions.ElementCount > 0) {
			GUI.Label(new Rect((rect.x + rect.width) / 2f - 100f, rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", _itemTransactions.CurrentPageIndex + 1, _itemTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
			GUI.enabled = _itemTransactions.CurrentPageIndex + 1 < _itemTransactions.PageCount;

			if (GUITools.Button(new Rect(rect.x + rect.width - 100f - 2f, rect.y + 5f, 100f, 32f), new GUIContent(_nextPageButtonLabel), button)) {
				_itemTransactions.CurrentPageIndex++;
				AsyncGetItemTransactions();
			}

			GUI.enabled = true;
		}
	}

	private void DrawPointsTableHeadingBar(Rect headingRect) {
		GUI.BeginGroup(headingRect);

		for (var i = 0; i < _pointsTableColumnHeadingArray.Length; i++) {
			var rect = new Rect(GetColumnOffset(AccountArea.Points, i, headingRect.width), 0f, GetColumnWidth(AccountArea.Points, i, headingRect.width), headingRect.height);
			GUI.Button(rect, string.Empty, BlueStonez.box_grey50);
			GUI.Label(rect, new GUIContent(_pointsTableColumnHeadingArray[i]), BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();
	}

	private void DrawPointsTableContent(Rect scrollViewRect) {
		GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);

		if (_pointTransactions.CurrentPage != null) {
			_scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), _scrollControls, new Rect(0f, 0f, scrollViewRect.width - 17f, _pointTransactions.CurrentPage.PointDeposits.Count * 23f));
			var num = 0f;

			foreach (var pointDepositView in _pointTransactions.CurrentPage.PointDeposits) {
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Points, 0, scrollViewRect.width), num, GetColumnWidth(AccountArea.Points, 0, scrollViewRect.width), 23f), pointDepositView.DepositDate.ToString(DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Points, 1, scrollViewRect.width), num, GetColumnWidth(AccountArea.Points, 1, scrollViewRect.width), 23f), pointDepositView.Points.ToString(), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Points, 2, scrollViewRect.width), num, GetColumnWidth(AccountArea.Points, 2, scrollViewRect.width), 23f), pointDepositView.DepositType.ToString(), BlueStonez.label_interparkmed_11pt);
				num += 23f;
			}

			GUITools.EndScrollView();
		}
	}

	private void DrawPointsButtons(Rect rect) {
		var button = BlueStonez.button;
		GUI.enabled = _pointTransactions.CurrentPageIndex != 0;

		if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(_prevPageButtonLabel), button)) {
			_pointTransactions.CurrentPageIndex--;
			AsyncGetPointsDeposits();
		}

		GUI.enabled = true;

		if (_pointTransactions.ElementCount > 0) {
			GUI.Label(new Rect((rect.x + rect.width) / 2f - 100f, rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", _pointTransactions.CurrentPageIndex + 1, _pointTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
			GUI.enabled = _pointTransactions.CurrentPageIndex + 1 < _pointTransactions.PageCount;

			if (GUITools.Button(new Rect(rect.x + rect.width - 100f - 2f, rect.y + 5f, 100f, 32f), new GUIContent(_nextPageButtonLabel), button)) {
				_pointTransactions.CurrentPageIndex++;
				AsyncGetPointsDeposits();
			}

			GUI.enabled = true;
		}
	}

	private void DrawCreditsTableHeadingBar(Rect headingRect) {
		GUI.BeginGroup(headingRect);

		for (var i = 0; i < _creditsTableColumnHeadingArray.Length; i++) {
			var rect = new Rect(GetColumnOffset(AccountArea.Credits, i, headingRect.width), 0f, GetColumnWidth(AccountArea.Credits, i, headingRect.width), headingRect.height);
			GUI.Button(rect, string.Empty, BlueStonez.box_grey50);
			GUI.Label(rect, new GUIContent(_creditsTableColumnHeadingArray[i]), BlueStonez.label_interparkmed_11pt);
		}

		GUI.EndGroup();
	}

	private void DrawCreditsTableContent(Rect scrollViewRect) {
		GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);

		if (_creditTransactions.CurrentPage != null) {
			_scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), _scrollControls, new Rect(0f, 0f, scrollViewRect.width - 17f, _creditTransactions.CurrentPage.CurrencyDeposits.Count * 23f));
			var num = 0f;

			foreach (var currencyDepositView in _creditTransactions.CurrentPage.CurrencyDeposits) {
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 0, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 0, scrollViewRect.width), 23f), TextUtility.ShortenText(currencyDepositView.TransactionKey, 20, true), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 1, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 1, scrollViewRect.width), 23f), currencyDepositView.DepositDate.ToString(DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 2, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 2, scrollViewRect.width), 23f), currencyDepositView.CurrencyLabel + currencyDepositView.Cash.ToString("#0.00"), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 3, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 3, scrollViewRect.width), 23f), currencyDepositView.Credits.ToString(), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 4, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 4, scrollViewRect.width), 23f), currencyDepositView.Points.ToString(), BlueStonez.label_interparkmed_11pt);
				GUI.Label(new Rect(GetColumnOffset(AccountArea.Credits, 5, scrollViewRect.width), num, GetColumnWidth(AccountArea.Credits, 5, scrollViewRect.width), 23f), TextUtility.ShortenText(currencyDepositView.BundleName, 14, true), BlueStonez.label_interparkmed_11pt);
				num += 23f;
			}

			GUITools.EndScrollView();
		}
	}

	private void DrawCreditsButtons(Rect rect) {
		var button = BlueStonez.button;
		GUI.enabled = _creditTransactions.CurrentPageIndex != 0;

		if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(_prevPageButtonLabel), button)) {
			_creditTransactions.CurrentPageIndex--;
			AsyncGetCurrencyDeposits();
		}

		GUI.enabled = true;

		if (_creditTransactions.ElementCount > 0) {
			GUI.Label(new Rect((rect.x + rect.width) / 2f - 100f, rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", _creditTransactions.CurrentPageIndex + 1, _creditTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
			GUI.enabled = _creditTransactions.CurrentPageIndex + 1 < _creditTransactions.PageCount;

			if (GUITools.Button(new Rect(rect.x + rect.width - 100f - 2f, rect.y + 5f, 100f, 32f), new GUIContent(_nextPageButtonLabel), button)) {
				_creditTransactions.CurrentPageIndex++;
				AsyncGetCurrencyDeposits();
			}

			GUI.enabled = true;
		}
	}

	private void AsyncGetItemTransactions() {
		if (_itemTransactions.CurrentPageNeedsRefresh) {
			var nextPageIndex = _itemTransactions.CurrentPageIndex;

			UserWebServiceClient.GetItemTransactions(PlayerDataManager.AuthToken, nextPageIndex + 1, 15, delegate(ItemTransactionsViewModel ev) {
				_itemTransactions.SetPage(nextPageIndex, ev);
				_itemTransactions.ElementCount = ev.TotalCount;
			}, delegate { });
		}
	}

	private void AsyncGetCurrencyDeposits() {
		if (_creditTransactions.CurrentPageNeedsRefresh) {
			var nextPageIndex = _creditTransactions.CurrentPageIndex;

			UserWebServiceClient.GetCurrencyDeposits(PlayerDataManager.AuthToken, nextPageIndex + 1, 15, delegate(CurrencyDepositsViewModel ev) {
				_creditTransactions.SetPage(nextPageIndex, ev);
				_creditTransactions.ElementCount = ev.TotalCount;
			}, delegate { });
		}
	}

	private void AsyncGetPointsDeposits() {
		if (_pointTransactions.CurrentPageNeedsRefresh) {
			var nextPageIndex = _pointTransactions.CurrentPageIndex;

			UserWebServiceClient.GetPointsDeposits(PlayerDataManager.AuthToken, nextPageIndex + 1, 15, delegate(PointDepositsViewModel ev) {
				_pointTransactions.SetPage(nextPageIndex, ev);
				_pointTransactions.ElementCount = ev.TotalCount;
			}, delegate { });
		}
	}

	public void GetCurrentTransactions() {
		switch (_selectedTab) {
			case 0:
				AsyncGetItemTransactions();

				break;
			case 1:
				AsyncGetPointsDeposits();

				break;
			case 2:
				AsyncGetCurrencyDeposits();

				break;
		}
	}

	private enum TransactionType {
		Item,
		Point,
		Credit
	}

	private enum AccountArea {
		Items,
		Points,
		Credits
	}

	public class TransactionCache<T> {
		private float _refreshLastPage;
		public SortedList<int, T> PageCache { get; private set; }
		public int CurrentPageIndex { get; set; }

		public T CurrentPage {
			get {
				if (PageCache.ContainsKey(CurrentPageIndex)) {
					return PageCache[CurrentPageIndex];
				}

				return default(T);
			}
		}

		public bool CurrentPageNeedsRefresh {
			get { return CurrentPage == null || (CurrentPageIndex > 0 && CurrentPageIndex == PageCount - 1 && _refreshLastPage < Time.time); }
		}

		public int ElementCount { get; set; }

		public int PageCount {
			get { return Mathf.CeilToInt(ElementCount / 15) + 1; }
		}

		public TransactionCache() {
			PageCache = new SortedList<int, T>();
		}

		public void SetPage(int index, T page) {
			PageCache[index] = page;

			if (index + 1 == PageCount) {
				_refreshLastPage = Time.time + 30f;
			}
		}
	}
}
