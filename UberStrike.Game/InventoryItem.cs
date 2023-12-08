using System;
using UnityEngine;

public class InventoryItem {
	private IUnityItem _item;

	public IUnityItem Item {
		get { return _item; }
	}

	public int DaysRemaining {
		get { return (IsPermanent || ExpirationDate == null) ? 0 : Mathf.CeilToInt((float)ExpirationDate.Value.Subtract(ApplicationDataManager.ServerDateTime).TotalHours / 24f); }
	}

	public int AmountRemaining { get; set; }
	public bool IsPermanent { get; set; }
	public DateTime? ExpirationDate { get; set; }
	public bool IsHighlighted { get; set; }

	public bool IsValid {
		get { return IsPermanent || DaysRemaining > 0; }
	}

	public InventoryItem(IUnityItem item) {
		_item = item;
	}
}
