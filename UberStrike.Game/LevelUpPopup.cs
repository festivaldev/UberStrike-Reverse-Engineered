using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopup : IPopupDialog {
	private Action _action;
	private ShopItemGrid _itemGrid;
	private int _level;
	protected int Height = 330;
	protected int Width = 650;
	public bool IsWaiting { get; set; }
	public LevelUpPopup(int level, Action action = null) : this(level, level - 1, action) { }

	public LevelUpPopup(int newLevel, int previousLevel, Action action = null) {
		_action = action;
		_level = newLevel;
		Title = "Level Up";
		Text = "Congratulations, you reached level " + _level + "!";
		Width = 388;
		Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
		var list = new List<ShopItemView>();

		for (var i = newLevel; i > previousLevel; i--) {
			list.AddRange(GetItemsUnlocked(i));
		}

		AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.LevelUp);
		_itemGrid = new ShopItemGrid(list);
		_itemGrid.Show = true;
	}

	public string Text { get; set; }
	public string Title { get; set; }

	public GuiDepth Depth {
		get { return GuiDepth.Event; }
	}

	public void OnGUI() {
		var position = GetPosition();
		GUI.Box(position, GUIContent.none, BlueStonez.window);
		GUITools.PushGUIState();
		GUI.BeginGroup(position);
		DrawPlayGUI(position);
		GUI.EndGroup();
		GUITools.PopGUIState();

		if (IsWaiting) {
			WaitingTexture.Draw(position.center);
		}
	}

	public void OnHide() { }

	private Rect GetPosition() {
		var num = (Screen.width - Width) * 0.5f;
		var num2 = GlobalUIRibbon.Instance.Height() + (Screen.height - GlobalUIRibbon.Instance.Height() - Height) * 0.5f;

		return new Rect(num, num2, Width, Height);
	}

	private List<ShopItemView> GetItemsUnlocked(int level) {
		var list = new List<ShopItemView>();

		if (level > 1) {
			foreach (var unityItem in Singleton<ItemManager>.Instance.ShopItems) {
				if (unityItem.View.LevelLock == level && unityItem.View.IsForSale) {
					list.Add(new ShopItemView(unityItem.View.ID));
				}
			}
		}

		return list;
	}

	private void DrawPlayGUI(Rect rect) {
		GUI.color = ColorScheme.HudTeamBlue;
		var num = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(Title)).x * 2.5f;
		GUI.DrawTexture(new Rect((rect.width - num) * 0.5f, -29f, num, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 30f), Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
		GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), Text, BlueStonez.label_interparkbold_16pt);
		var num2 = 288;
		var num3 = (Width - num2 - 6) / 2;
		var num4 = 323;
		var count = _itemGrid.Items.Count;
		GUI.BeginGroup(new Rect(num3, 75f, num2, num4), BlueStonez.item_slot_large);
		var rect2 = new Rect((num2 - 282) / 2, (num4 - 317) / 2, 282f, 317f);
		GUI.DrawTexture(rect2, UberstrikeIcons.LevelUpPopup);

		if (count > 0) {
			_itemGrid.Draw(new Rect(0f, 0f, num2, num4));
		}

		GUI.EndGroup();

		if (count > 0) {
			GUI.Label(new Rect(30f, rect.height - 107f, rect.width - 60f, 40f), string.Format("You unlocked {0} new item{1}.", count, (count != 1) ? "s" : string.Empty), BlueStonez.label_interparkbold_16pt);
		}

		var num5 = -70;

		if (GUI.Button(new Rect(rect.width * 0.5f + num5, rect.height - 47f, 140f, 30f), "OK", BlueStonez.buttongold_large_price)) {
			PopupSystem.HideMessage(this);

			if (_action != null) {
				_action();
			}
		}
	}
}
