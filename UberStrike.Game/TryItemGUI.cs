using System;
using UnityEngine;

public class TryItemGUI : MonoBehaviour {
	private LoadoutArea _currentLoadoutArea;

	private void OnGUI() {
		if (PopupSystem.IsAnyPopupOpen || PanelManager.IsAnyPanelOpen) {
			return;
		}

		var currentLoadoutArea = _currentLoadoutArea;

		if (currentLoadoutArea == LoadoutArea.Gear) {
			DrawResetGear();
		}
	}

	private void OnEnable() {
		EventHandler.Global.AddListener(new Action<ShopEvents.LoadoutAreaChanged>(OnLoadoutAreaChanged));
	}

	private void OnDisable() {
		EventHandler.Global.RemoveListener(new Action<ShopEvents.LoadoutAreaChanged>(OnLoadoutAreaChanged));
	}

	private void DrawResetGear() {
		var num = Mathf.Max((Screen.width - 584) * 0.5f, 170f);
		var num2 = (Screen.width - 584 - num) * 0.5f;

		if (Singleton<TemporaryLoadoutManager>.Instance.IsGearLoadoutModified && GUITools.Button(new Rect(2f + num2, Screen.height - 60, num, 32f), new GUIContent("Reset Avatar"), BlueStonez.button_white)) {
			Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout();
		}
	}

	public void OnLoadoutAreaChanged(ShopEvents.LoadoutAreaChanged ev) {
		_currentLoadoutArea = ev.Area;
	}
}
