using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HUDMobileChatEventStream : MonoBehaviour {
	[SerializeField]
	private UIVerticalAligner aligner;

	[SerializeField]
	private float displayTime = 5f;

	private List<Item> items = new List<Item>();
	private List<ItemGfx> itemsGfx = new List<ItemGfx>();

	[SerializeField]
	private int maxItems = 8;

	[SerializeField]
	private UILabel muteLabel;

	[SerializeField]
	private UILabel spamLabel;

	[SerializeField]
	private UIHorizontalAligner template;

	private UIInput textInput;

	[SerializeField]
	private float ySpace = 20f;

	private void Start() {
		spamLabel.enabled = false;
		textInput = GetComponent<UIInput>();

		if (textInput == null)
			throw new Exception("Chat: no UIInput attached.");

		GameData.Instance.OnHUDChatMessage.AddEvent(AddMessage, this);
		GameData.Instance.OnHUDChatClear.AddEvent(ClearLog, this);
		GameData.Instance.OnHUDChatStartTyping.AddEvent(() => ActivateTextInput(true), this);
		GameData.Instance.OnHUDStreamMessage.AddEvent(AddMessage, this);
		GameData.Instance.OnHUDStreamClear.AddEvent(ClearLog, this);
		GameData.Instance.OnPlayerKilled.AddEvent(HUDDesktopEventStream.HandleKilledMessage, this);
		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted.AddEventAndFire(el => muteLabel.enabled = el, this);

		foreach (Transform transform in template.transform.parent) {
			if (transform != template.transform)
				Destroy(transform.gameObject);
		}

		for (var index = 0; index < maxItems; ++index) {
			var gameObject = GameObjectHelper.Instantiate(template.gameObject, template.transform.parent, new Vector3(0.0f, index * ySpace, 0.0f));
			gameObject.name = "Item " + index;

			itemsGfx.Add(new ItemGfx {
				Label1 = gameObject.transform.Find("1_label").GetComponent<UILabel>(),
				Label2 = gameObject.transform.Find("2_label").GetComponent<UILabel>(),
				Label3 = gameObject.transform.Find("3_label").GetComponent<UILabel>(),
				Aligner = gameObject.GetComponent<UIHorizontalAligner>()
			});
		}

		template.gameObject.SetActive(false);
		ApplyChanges();
		ActivateTextInput(false);
	}

	private void ApplyChanges() {
		for (var i = 0; i < itemsGfx.Count; i++) {
			var itemGfx = itemsGfx[i];
			var item = ((i >= items.Count) ? null : items[i]);
			itemGfx.Aligner.gameObject.SetActive(item != null);

			if (item != null) {
				itemGfx.Label1.text = item.Label1;
				itemGfx.Label1.lineWidth = item.Label1MaxWidth;
				itemGfx.Label1.effectColor = item.Label1EffectColor;
				itemGfx.Label1.effectStyle = item.Label1Effect;
				itemGfx.Label2.text = item.Label2;
				itemGfx.Label3.text = item.Label3;
				itemGfx.Label3.effectColor = item.Label3EffectColor;
				itemGfx.Label3.effectStyle = item.Label3Effect;
				itemGfx.Label1.gameObject.SetActive(itemGfx.Label1.text != string.Empty);
				itemGfx.Label2.gameObject.SetActive(itemGfx.Label2.text != string.Empty);
				itemGfx.Label3.gameObject.SetActive(itemGfx.Label3.text != string.Empty);
				itemGfx.Aligner.Reposition();
			}
		}

		aligner.Reposition();
	}

	private void OnSubmit(string text) {
		text = NGUITools.StripSymbols(text).Trim();
		text = TextUtilities.Trim(text);

		if (text != string.Empty && !GameState.Current.SendChatMessage(text, ChatContext.Player)) {
			spamLabel.enabled = true;
		}

		ActivateTextInput(false);
	}

	public void AddMessage(GameActorInfo player1, string actionString, GameActorInfo player2) {
		items.Add(new Item {
			Label1 = ((!string.IsNullOrEmpty(player1.ClanTag)) ? ("[" + player1.ClanTag + "] " + player1.PlayerName) : player1.PlayerName),
			Label1Effect = UILabel.Effect.Outline,
			Label1EffectColor = HUDDesktopEventStream.GetPlayerColor(player1),
			Label2 = actionString,
			Label3 = ((player2 != null) ? ((!string.IsNullOrEmpty(player2.ClanTag)) ? ("[" + player2.ClanTag + "] " + player2.PlayerName) : player2.PlayerName) : string.Empty),
			Label3Effect = UILabel.Effect.Outline,
			Label3EffectColor = HUDDesktopEventStream.GetPlayerColor(player2),
			TimeEnd = Time.time + displayTime
		});

		if (items.Count > maxItems) {
			items.RemoveAt(0);
		}

		ApplyChanges();
	}

	public void AddMessage(string from, string message, MemberAccessLevel accessLevel) {
		var text = GUIUtils.ColorToNGuiModifier(ColorScheme.GetNameColorByAccessLevel(accessLevel));

		items.Add(new Item {
			Label1 = text + from + ": [-]" + message,
			Label1MaxWidth = 270,
			Label1Effect = UILabel.Effect.Shadow,
			Label1EffectColor = GUIUtils.ColorBlack,
			Label2 = string.Empty,
			Label3 = string.Empty,
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
			ActivateTextInput(true);
		}

		if (GameData.Instance.HUDChatIsTyping && !textInput.selected) {
			ActivateTextInput(false);
		}

		while (items.Count > 0 && Time.time >= items[0].TimeEnd) {
			items.RemoveAt(0);
			ApplyChanges();
		}
	}

	private void ActivateTextInput(bool enabled) {
		GameData.Instance.HUDChatIsTyping = enabled;
		textInput.selected = enabled;

		EventHandler.Global.Fire(new GameEvents.ChatWindow {
			IsEnabled = enabled
		});
	}

	[Serializable]
	public class ItemGfx {
		public UIHorizontalAligner Aligner;
		public UILabel Label1;
		public UILabel Label2;
		public UILabel Label3;
	}

	[Serializable]
	public class Item {
		public string Label1 = string.Empty;
		public UILabel.Effect Label1Effect;
		public Color Label1EffectColor;
		public int Label1MaxWidth;
		public string Label2 = string.Empty;
		public string Label3 = string.Empty;
		public UILabel.Effect Label3Effect;
		public Color Label3EffectColor;
		public float TimeEnd;
	}
}
