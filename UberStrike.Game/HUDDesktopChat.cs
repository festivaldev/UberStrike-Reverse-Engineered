using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HUDDesktopChat : MonoBehaviour {
	[SerializeField]
	private UIVerticalAligner aligner;

	[SerializeField]
	private float displayTime = 5f;

	[SerializeField]
	private UILabel inputLabel;

	[SerializeField]
	private UISprite inputLabelBgr;

	private List<Item> items = new List<Item>();
	private List<ItemGfx> itemsGfx = new List<ItemGfx>();
	private float lastSpammingTime;

	[SerializeField]
	private int maxItems = 8;

	[SerializeField]
	private UILabel muteLabel;

	private bool skipNextEnter;

	[SerializeField]
	private UILabel spamLabel;

	[SerializeField]
	private UILabel template;

	private UIInput textInput;

	[SerializeField]
	private float ySpace = -17f;

	private void Start() {
		spamLabel.enabled = false;
		textInput = GetComponent<UIInput>();

		if (textInput == null)
			throw new Exception("Chat: no UIInput attached.");

		GameData.Instance.OnHUDChatMessage.AddEvent(AddMessage, this);
		GameData.Instance.OnHUDChatClear.AddEvent(ClearLog, this);
		GameData.Instance.OnHUDChatStartTyping.AddEvent(() => ActivateTextInput(true), this);
		GameData.Instance.GameState.AddEvent(el => ActivateTextInput(false), this);
		GameData.Instance.PlayerState.AddEvent(el => ActivateTextInput(false), this);
		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted.AddEventAndFire(el => muteLabel.enabled = el, this);

		foreach (Transform transform in template.transform.parent) {
			if (transform != template.transform)
				Destroy(transform.gameObject);
		}

		for (var index = 0; index < maxItems; ++index) {
			var gameObject = GameObjectHelper.Instantiate(template.gameObject, template.transform.parent, new Vector3(0.0f, index * ySpace, 0.0f), template.transform.localScale);
			gameObject.name = "Item " + index;

			itemsGfx.Add(new ItemGfx {
				Label = gameObject.GetComponent<UILabel>()
			});
		}

		template.gameObject.SetActive(false);
		ApplyChanges();
		ActivateTextInput(false);
	}

	private void OnEnable() {
		spamLabel.enabled = false;
	}

	private void ApplyChanges() {
		for (var i = 0; i < itemsGfx.Count; i++) {
			var itemGfx = itemsGfx[i];
			var item = ((i >= items.Count) ? null : items[i]);
			itemGfx.Label.gameObject.SetActive(item != null);

			if (item != null) {
				if (item.From != string.Empty) {
					itemGfx.Label.color = item.color;
					itemGfx.Label.supportEncoding = item.accessLevel > MemberAccessLevel.Default;
					itemGfx.Label.text = item.From + ": " + item.Message;
				} else {
					itemGfx.Label.text = string.Empty;
				}
			}
		}

		aligner.Reposition();
	}

	private void OnSubmit(string text) {
		text = NGUITools.StripSymbols(text).Trim();
		text = TextUtilities.Trim(text);

		if (!string.IsNullOrEmpty(text) && !GameState.Current.SendChatMessage(text, ChatContext.Player)) {
			spamLabel.enabled = true;
			lastSpammingTime = Time.time;
		}

		OnInputChanged();
		ActivateTextInput(false);
		skipNextEnter = true;
		textInput.text = string.Empty;
	}

	private void OnInputChanged(object input = null) {
		textInput.text = textInput.text.Replace(textInput.caratChar, string.Empty);
		inputLabel.text = textInput.text;
		var num = NGUIMath.CalculateRelativeWidgetBounds(inputLabel.transform).size.y * inputLabel.transform.localScale.y;
		inputLabelBgr.transform.localScale = inputLabelBgr.transform.localScale.SetY(num + inputLabel.transform.localScale.y);
	}

	public void AddMessage(string from, string message, MemberAccessLevel accessLevel) {
		items.Add(new Item {
			From = from,
			Message = message,
			ColorMod = GUIUtils.ColorToNGuiModifier(ColorScheme.GetNameColorByAccessLevel(accessLevel)),
			accessLevel = accessLevel,
			color = ColorScheme.GetNameColorByAccessLevel(accessLevel),
			TimeEnd = Time.time + displayTime
		});

		if (items.Count > maxItems) {
			items.RemoveAt(0);
		}

		ApplyChanges();
	}

	public void ClearLog() {
		items.Clear();
		ApplyChanges();
	}

	private void Update() {
		if (Input.GetKeyUp(KeyCode.Return) && !PopupSystem.IsAnyPopupOpen) {
			if (!skipNextEnter) {
				ActivateTextInput(!GameData.Instance.HUDChatIsTyping);
			}

			skipNextEnter = false;
		}

		if (GameData.Instance.HUDChatIsTyping && !textInput.selected) {
			ActivateTextInput(false);
		}

		while (items.Count > 0 && Time.time >= items[0].TimeEnd) {
			items.RemoveAt(0);
			ApplyChanges();
		}

		if (spamLabel.enabled && Time.time >= lastSpammingTime + 5f) {
			spamLabel.enabled = false;
		}
	}

	private void ActivateTextInput(bool enabled) {
		enabled &= !AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted;
		GameData.Instance.HUDChatIsTyping = enabled;
		textInput.selected = enabled;

		if (enabled) {
			OnInputChanged();
		}

		inputLabel.enabled = enabled;
		inputLabelBgr.enabled = enabled;

		EventHandler.Global.Fire(new GameEvents.ChatWindow {
			IsEnabled = enabled
		});
	}

	[Serializable]
	public class ItemGfx {
		public UILabel Label;
	}

	[Serializable]
	public class Item {
		public MemberAccessLevel accessLevel;
		public Color color;
		public string ColorMod;
		public string From = string.Empty;
		public string Message = string.Empty;
		public float TimeEnd;
	}
}
