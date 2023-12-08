using UberStrike.Core.Models;
using UnityEngine;

public class ArmorPickupItem : PickupItem {
	public enum Category {
		Gold,
		Silver,
		Bronze
	}

	[SerializeField]
	private Category _armorPoints;

	protected override bool CanPlayerPickup {
		get { return GameState.Current.PlayerData.ArmorPoints < 200; }
	}

	protected override bool OnPlayerPickup() {
		if (CanPlayerPickup) {
			var num = 0;

			switch (_armorPoints) {
				case Category.Gold:
					num = 100;
					GameData.Instance.OnItemPickup.Fire("Uber Armor", PickUpMessageType.Armor100);

					break;
				case Category.Silver:
					num = 50;
					GameData.Instance.OnItemPickup.Fire("Big Armor", PickUpMessageType.Armor50);

					break;
				case Category.Bronze:
					num = 5;
					GameData.Instance.OnItemPickup.Fire("Mini Armor", PickUpMessageType.Armor5);

					break;
			}

			GameState.Current.PlayerData.ArmorPoints.Value += num;

			switch (_armorPoints) {
				case Category.Gold:
					PlayLocalPickupSound(GameAudio.GoldArmor2D);

					break;
				case Category.Silver:
					PlayLocalPickupSound(GameAudio.SilverArmor2D);

					break;
				case Category.Bronze:
					PlayLocalPickupSound(GameAudio.ArmorShard2D);

					break;
				default:
					PlayLocalPickupSound(GameAudio.ArmorShard2D);

					break;
			}

			GameState.Current.Actions.PickupPowerup(PickupID, PickupItemType.Armor, num);

			if (GameState.Current.IsSinglePlayer) {
				StartCoroutine(StartHidingPickupForSeconds(_respawnTime));
			}

			return true;
		}

		return false;
	}

	protected override void OnRemotePickup() {
		switch (_armorPoints) {
			case Category.Gold:
				PlayRemotePickupSound(GameAudio.GoldArmor, transform.position);

				break;
			case Category.Silver:
				PlayRemotePickupSound(GameAudio.SilverArmor, transform.position);

				break;
			case Category.Bronze:
				PlayRemotePickupSound(GameAudio.ArmorShard, transform.position);

				break;
			default:
				PlayRemotePickupSound(GameAudio.ArmorShard, transform.position);

				break;
		}
	}
}
