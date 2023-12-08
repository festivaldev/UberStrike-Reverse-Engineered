using System.Collections;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDNotifications : MonoBehaviour {
	[SerializeField]
	private float defaultFadeInSpeed = 20f;

	[SerializeField]
	private float defaultFadeOutSpeed = 5f;

	private int killCounter;

	[SerializeField]
	private UILabel labelBig;

	[SerializeField]
	private UILabel labelSmall;

	private float lastKillTime;

	[SerializeField]
	private UIPanel panel;

	private void Start() {
		GameData.Instance.OnNotification.AddEvent(delegate(string el) { Show(el, string.Empty); }, this);
		GameData.Instance.OnNotificationFull.AddEvent(delegate(string el1, string el2, float duration) { Show(el1, el2, duration); }, this);

		GameData.Instance.GameState.AddEventAndFire(delegate(GameStateId el) {
			if (el == GameStateId.PrepareNextRound) {
				Show(GetGameModeName(), GetGameModeHint(), 0f);
			} else if (el == GameStateId.MatchRunning) {
				Show(string.Empty, "Fight!");
			} else {
				Hide();
			}
		}, this);

		GameData.Instance.OnPlayerKilled.AddEvent(OnPlayerKilled, this);
		panel.alpha = 0f;
	}

	private void OnPlayerKilled(GameActorInfo shooter, GameActorInfo target, UberstrikeItemClass weapon, BodyPart bodyPart) {
		if (target == null) {
			return;
		}

		if (shooter != null && shooter.Cmid == PlayerDataManager.Cmid && target.Cmid != PlayerDataManager.Cmid) {
			var flag = Time.time < lastKillTime + 10f;
			killCounter = ((!flag) ? 1 : (killCounter + 1));
			lastKillTime = Time.time;

			if (weapon == UberstrikeItemClass.WeaponMelee) {
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.Smackdown);
			} else if (bodyPart == BodyPart.Head) {
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.HeadShot);
			} else if (bodyPart == BodyPart.Nuts) {
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.GotNutshotKill);
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NutShot);
			}

			var text = string.Empty;

			if (killCounter == 2) {
				text = "DOUBLE KILL";
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.DoubleKill, 1000UL);
			} else if (killCounter == 3) {
				text = "TRIPLE KILL";
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.TripleKill, 1000UL);
			} else if (killCounter == 4) {
				text = "QUAD KILL";
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.QuadKill, 1000UL);
			} else if (killCounter == 5) {
				text = "MEGA KILL";
				AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.MegaKill, 1000UL);
			} else if (killCounter > 5) {
				text = "UBER KILL";

				if (killCounter == 6) {
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.UberKill, 1000UL);
				}
			}

			Show(text, "You killed " + target.PlayerName);
		}
	}

	private string GetGameModeName() {
		switch (GameState.Current.GameMode) {
			case GameModeType.DeathMatch:
				return LocalizedStrings.DeathMatch;
			case GameModeType.TeamDeathMatch:
				return LocalizedStrings.TeamDeathMatch;
			case GameModeType.EliminationMode:
				return LocalizedStrings.TeamElimination;
			default:
				return string.Empty;
		}
	}

	private string GetGameModeHint() {
		if (GameState.Current.GameMode == GameModeType.DeathMatch) {
			return "Get as many kills as you can before the time runs out";
		}

		return "Get as many kills for your team as you can\nbefore the time runs out";
	}

	public void Hide(bool immediate = false) {
		StopAllCoroutines();

		if (immediate) {
			panel.alpha = 0f;
		} else {
			StartCoroutine(HideCrt(defaultFadeOutSpeed));
		}
	}

	public void Show(string textBig, string textSmall, float duration = 1f) {
		Show(textBig, textSmall, defaultFadeInSpeed, defaultFadeOutSpeed, duration);
	}

	public void Show(string textBig, string textSmall, float fadeInSpeed, float fadeOutSpeed, float duration = 1f) {
		StopAllCoroutines();
		StartCoroutine(ShowCrt(textBig, textSmall, fadeInSpeed, fadeOutSpeed, duration));
	}

	public IEnumerator ShowCrt(string textBig, string textSmall, float fadeInSpeed, float fadeOutSpeed, float duration) {
		if (labelBig.text != textBig || labelSmall.text != textSmall) {
			panel.alpha = 0f;
		}

		labelBig.text = textBig;
		labelSmall.text = textSmall;

		while (panel.alpha < 1f) {
			panel.alpha = Mathf.MoveTowards(panel.alpha, 1f, Time.deltaTime * fadeInSpeed);

			yield return 0;
		}

		if (duration > 0f) {
			yield return new WaitForSeconds(duration);
			yield return StartCoroutine(HideCrt(fadeOutSpeed));
		}
	}

	private IEnumerator HideCrt(float fadeOutSpeed) {
		while (panel.alpha > 0f) {
			panel.alpha = Mathf.MoveTowards(panel.alpha, 0f, Time.deltaTime * fadeOutSpeed);

			yield return 0;
		}
	}
}
