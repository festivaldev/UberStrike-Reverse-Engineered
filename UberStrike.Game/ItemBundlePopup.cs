using UnityEngine;

public class ItemBundlePopup : LotteryPopupDialog {
	private BundleUnityView _bundleUnityView;
	private ShopItemGrid _lotteryItemGrid;

	public ItemBundlePopup(BundleUnityView bundleUnityView) {
		_bundleUnityView = bundleUnityView;
		Title = bundleUnityView.BundleView.Name;
		Text = bundleUnityView.BundleView.Description;
		Width = 388;
		Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
		_lotteryItemGrid = new ShopItemGrid(bundleUnityView.BundleView.BundleItemViews);
	}

	protected override void DrawPlayGUI(Rect rect) {
		GUI.color = ColorScheme.HudTeamBlue;
		var num = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(Title)).x * 2.5f;
		GUI.DrawTexture(new Rect((rect.width - num) * 0.5f, -29f, num, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 30f), Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
		GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), Text, BlueStonez.label_interparkbold_13pt);
		var num2 = 288;
		var num3 = (Width - num2 - 6) / 2;
		var num4 = 323;
		GUI.BeginGroup(new Rect(num3, 75f, num2, num4), BlueStonez.item_slot_large);
		var rect2 = new Rect((num2 - 282) / 2, (num4 - 317) / 2, 282f, 317f);
		_bundleUnityView.Image.Draw(rect2);
		_lotteryItemGrid.Show = rect2.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile;
		_lotteryItemGrid.Draw(new Rect(0f, 0f, num2, num4));
		GUI.EndGroup();

		if (GUI.Button(new Rect(rect.width * 0.5f - 95f, rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_left)) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
			PopupSystem.HideMessage(this);
			var previousItem = Singleton<BundleManager>.Instance.GetPreviousItem(_bundleUnityView);

			if (previousItem != null) {
				PopupSystem.Show(new ItemBundlePopup(previousItem));
			}
		}

		if (GUI.Button(new Rect(rect.width * 0.5f + 75f, rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_right)) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ButtonClick);
			PopupSystem.HideMessage(this);
			var nextItem = Singleton<BundleManager>.Instance.GetNextItem(_bundleUnityView);

			if (nextItem != null) {
				PopupSystem.Show(new ItemBundlePopup(nextItem));
			}
		}

		GUI.enabled = !_bundleUnityView.IsOwned && _bundleUnityView.IsValid && GUITools.SaveClickIn(1f);
		BuyButton(rect, _bundleUnityView);
		GUI.enabled = true;
	}

	private void BuyButton(Rect position, BundleUnityView bundleUnityView) { }
}
