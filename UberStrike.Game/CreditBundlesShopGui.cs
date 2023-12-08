using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class CreditBundlesShopGui {
	private Dictionary<int, float> _alpha = new Dictionary<int, float>();
	private Vector2 bundleScroll = Vector2.zero;
	private int scrollHeight;

	public void Draw(Rect position) {
		var num = Mathf.Max(position.height, scrollHeight);
		bundleScroll = GUI.BeginScrollView(position, bundleScroll, new Rect(0f, 0f, position.width - 17f, num), false, true);
		var creditBundles = Singleton<BundleManager>.Instance.GetCreditBundles();

		if (creditBundles.Count == 0) {
			GUI.Label(new Rect(4f, 4f, position.width - 20f, 24f), "No credit packs are currently on sale.", BlueStonez.label_interparkbold_16pt);
		} else {
			var num2 = 4;
			var num3 = 0;
			var list = new List<string>();
			GUI.Label(new Rect(4f, num2 + 4, position.width - 20f, 20f), "Credit Packs", BlueStonez.label_interparkbold_18pt_left);
			num2 += 30;

			foreach (var bundleUnityView in creditBundles) {
				var num4 = ((num3 % 2 != 1) ? 0 : 187);

				if (num2 < position.height && num2 + 95 > 0) {
					DrawPackSlot(new Rect(num4, num2, 188f, 95f), bundleUnityView);
					list.Add(bundleUnityView.BundleView.IconUrl);
				}

				num2 += ((num3 % 2 != 1) ? 0 : 94);
				num3++;
			}

			if (num3 % 2 == 1) {
				num2 += 94;
			}

			GUI.Label(new Rect(4f, num2, position.width - 8f, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
			scrollHeight = num2;
		}

		GUI.EndScrollView();
	}

	private void DrawPackSlot(Rect position, BundleUnityView bundleUnityView) {
		var id = bundleUnityView.BundleView.Id;
		var flag = position.Contains(Event.current.mousePosition);

		if (!_alpha.ContainsKey(id)) {
			_alpha[id] = 0f;
		}

		_alpha[id] = Mathf.Lerp(_alpha[id], (!flag) ? 0 : 1, Time.deltaTime * ((!flag) ? 10 : 3));
		GUI.BeginGroup(position);
		GUI.Label(new Rect(2f, 2f, position.width - 4f, 79f), GUIContent.none, BlueStonez.gray_background);
		bundleUnityView.Icon.Draw(new Rect(4f, 4f, 75f, 75f));
		GUI.Label(new Rect(81f, 0f, position.width - 80f, 44f), bundleUnityView.BundleView.Name, BlueStonez.label_interparkbold_13pt_left);
		GUI.enabled = GUITools.SaveClickIn(1f);
		BuyButton(position, bundleUnityView);
		GUI.enabled = true;
		GUI.EndGroup();
	}

	private void BuyButton(Rect position, BundleUnityView bundleUnityView) {
		if (GUI.Button(new Rect(81f, 51f, position.width - 110f, 20f), new GUIContent(bundleUnityView.CurrencySymbol + bundleUnityView.Price, "Buy the " + bundleUnityView.BundleView.Name + " pack."), BlueStonez.buttongold_medium)) {
			GUITools.Clicked();

			if (ApplicationDataManager.Channel == ChannelType.Steam) {
				Singleton<BundleManager>.Instance.BuyBundle(bundleUnityView);
			} else {
				PopupSystem.ClearAll();
				PopupSystem.ShowMessage("Purchase Failed", "Sorry, only Steam players can purchase credit bundles.", PopupSystem.AlertType.OK);
			}
		}
	}
}
