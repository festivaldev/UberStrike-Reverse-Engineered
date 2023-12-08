using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDKilledBy : MonoBehaviour {
	[SerializeField]
	private UILabel armorLabel;

	[SerializeField]
	private UIHorizontalAligner healthArmorAligner;

	[SerializeField]
	private UILabel healthLabel;

	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UILabel respawnCountdown;

	[SerializeField]
	private UILabel respawnLabel;

	[SerializeField]
	private UIHorizontalAligner weaponAligner;

	[SerializeField]
	private UILabel weaponNameLabel;

	private void Start() {
		GameData.Instance.OnPlayerKilled.AddEvent(delegate(GameActorInfo shooter, GameActorInfo target, UberstrikeItemClass weapon, BodyPart body) {
			if (target == null || target.Cmid != PlayerDataManager.Cmid) {
				return;
			}

			panel.alpha = 1f;

			if (shooter == null || shooter.Cmid == PlayerDataManager.Cmid) {
				nameLabel.text = LocalizedStrings.CongratulationsYouKilledYourself;
				healthArmorAligner.gameObject.SetActive(false);
			} else {
				nameLabel.text = "Killed by " + shooter.PlayerName;
				healthArmorAligner.gameObject.SetActive(true);
				healthLabel.text = Mathf.Clamp(shooter.Health, 0, 200).ToString();
				armorLabel.text = Mathf.Clamp(shooter.ArmorPoints, 0, 200).ToString();
				healthArmorAligner.Reposition();
			}

			respawnCountdown.gameObject.SetActive(false);
			respawnLabel.gameObject.SetActive(false);
		}, this);

		GameData.Instance.PlayerState.AddEvent(delegate(PlayerStateId el) { panel.alpha = (float)((el != PlayerStateId.Killed) ? 0 : 1); }, this);
		panel.alpha = 0f;

		GameData.Instance.OnRespawnCountdown.AddEvent(delegate(int el) {
			respawnCountdown.gameObject.SetActive(el > 0);
			respawnLabel.gameObject.SetActive(el > 0);

			if (el > 0) {
				respawnCountdown.text = el.ToString();
				UITweener.Begin<TweenScale>(respawnCountdown.gameObject, 0.5f);
				UITweener.Begin<TweenAlpha>(respawnCountdown.gameObject, 0.25f);
			}
		}, this);
	}
}
