using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDDesktopEventStream : MonoBehaviour {
	[SerializeField]
	private GameObject container;

	[SerializeField]
	private float displayTime = 5f;

	private Queue<Item> items = new Queue<Item>();
	private List<ItemGfx> itemsGfx = new List<ItemGfx>();

	[SerializeField]
	private int maxItems = 8;

	[SerializeField]
	private UIHorizontalAligner template;

	[SerializeField]
	private float ySpace = -17f;

	private void Start() {
		GameData.Instance.OnHUDStreamMessage.AddEvent(AddMessage, this);
		GameData.Instance.OnHUDStreamClear.AddEvent(ClearLog, this);
		GameData.Instance.OnPlayerKilled.AddEvent(HandleKilledMessage, this);

		foreach (var obj in template.transform.parent) {
			var transform = (Transform)obj;

			if (transform != template.transform) {
				Destroy(transform.gameObject);
			}
		}

		for (var i = 0; i < maxItems; i++) {
			var gameObject = GameObjectHelper.Instantiate(template.gameObject, template.transform.parent, new Vector3(0f, i * ySpace, 0f));
			gameObject.name = "Item " + i;

			itemsGfx.Add(new ItemGfx {
				Label1 = gameObject.transform.Find("1_label").GetComponent<UILabel>(),
				Label2 = gameObject.transform.Find("2_label").GetComponent<UILabel>(),
				Label3 = gameObject.transform.Find("3_label").GetComponent<UILabel>(),
				Aligner = gameObject.GetComponent<UIHorizontalAligner>()
			});
		}

		template.gameObject.SetActive(false);
		ApplyChanges();
	}

	private void ApplyChanges() {
		var i = 0;

		foreach (var item in items) {
			var itemGfx = itemsGfx[i];
			itemGfx.Aligner.gameObject.SetActive(true);
			itemGfx.Label1.text = item.Label1;
			itemGfx.Label1.effectColor = item.Label1EffectColor;
			itemGfx.Label2.text = item.Label2;
			itemGfx.Label3.text = item.Label3;
			itemGfx.Label3.effectColor = item.Label3EffectColor;
			itemGfx.Label1.gameObject.SetActive(itemGfx.Label1.text != string.Empty);
			itemGfx.Label2.gameObject.SetActive(itemGfx.Label2.text != string.Empty);
			itemGfx.Label3.gameObject.SetActive(itemGfx.Label3.text != string.Empty);
			itemGfx.Aligner.Reposition();
			i++;
		}

		while (i < maxItems) {
			itemsGfx[i].Aligner.gameObject.SetActive(false);
			i++;
		}
	}

	public void AddMessage(GameActorInfo player1, string actionString, GameActorInfo player2) {
		items.Enqueue(new Item {
			Label1 = ((!string.IsNullOrEmpty(player1.ClanTag)) ? ("[" + player1.ClanTag + "] " + player1.PlayerName) : player1.PlayerName),
			Label1EffectColor = GetPlayerColor(player1),
			Label2 = actionString,
			Label3 = ((player2 != null) ? ((!string.IsNullOrEmpty(player2.ClanTag)) ? ("[" + player2.ClanTag + "] " + player2.PlayerName) : player2.PlayerName) : string.Empty),
			Label3EffectColor = GetPlayerColor(player2),
			TimeEnd = Time.time + displayTime
		});

		if (items.Count > maxItems) {
			items.Dequeue();
		}

		ApplyChanges();
	}

	public void ClearLog() {
		items.Clear();
		ApplyChanges();
	}

	public void DoAnimateDown(bool down) {
		SpringPosition.Begin(container.gameObject, new Vector3(0f, (!down) ? 0f : (-60f), 0f), 10f).onFinished = delegate(SpringPosition el) { el.enabled = false; };
	}

	private void Update() {
		while (items.Count > 0 && Time.time >= items.Peek().TimeEnd) {
			items.Dequeue();
			ApplyChanges();
		}
	}

	public static Color GetPlayerColor(GameActorInfo player) {
		if (player == null) {
			return Color.white;
		}

		if (player.Cmid == PlayerDataManager.Cmid) {
			return Color.green.SetAlpha(0.54901963f);
		}

		if (GameState.Current.GameMode == GameModeType.DeathMatch) {
			return new Color(0.5019608f, 0.5019608f, 0.5019608f, 0.54901963f);
		}

		if (player.TeamID == TeamID.BLUE) {
			return GUIUtils.ColorBlue.SetAlpha(0.54901963f);
		}

		if (player.TeamID == TeamID.RED) {
			return GUIUtils.ColorRed.SetAlpha(0.54901963f);
		}

		return Color.black;
	}

	public static void HandleKilledMessage(GameActorInfo shooter, GameActorInfo target, UberstrikeItemClass weapon, BodyPart bodyPart) {
		var flag = GameState.Current.GameMode == GameModeType.None;

		if (flag) {
			return;
		}

		if (target == null) {
			return;
		}

		if (shooter == null || shooter == target) {
			GameData.Instance.OnHUDStreamMessage.Fire(target, LocalizedStrings.NKilledThemself, null);
		} else {
			var text = string.Empty;

			if (weapon == UberstrikeItemClass.WeaponMelee) {
				text = "smacked";
			} else if (bodyPart == BodyPart.Head) {
				text = "headshot";
			} else if (bodyPart == BodyPart.Nuts) {
				text = "nutshot";
			} else {
				text = "killed";
			}

			GameData.Instance.OnHUDStreamMessage.Fire(shooter, text, target);
		}
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
		public Color Label1EffectColor;
		public string Label2 = string.Empty;
		public string Label3 = string.Empty;
		public Color Label3EffectColor;
		public float TimeEnd;
	}
}
