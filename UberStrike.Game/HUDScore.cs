using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class HUDScore : MonoBehaviour {
	[SerializeField]
	private UISprite blueBgr;

	[SerializeField]
	private UILabel blueLabel;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UISprite redBgr;

	[SerializeField]
	private UILabel redLabel;

	[SerializeField]
	private UILabel timerLabel;

	[SerializeField]
	private UILabel titleLabel;

	private void OnEnable() {
		int num = ((GameState.Current.PlayerData.Team != TeamID.BLUE) ? GameState.Current.ScoreRed : GameState.Current.ScoreBlue);
		int num2 = ((GameState.Current.PlayerData.Team != TeamID.BLUE) ? GameState.Current.ScoreBlue : GameState.Current.ScoreRed);
		this.panel.gameObject.SetActive(true);
		this.blueLabel.text = GameState.Current.ScoreBlue.ToString();
		this.redLabel.text = GameState.Current.ScoreRed.ToString();
		bool isTeamGame = GameState.Current.IsTeamGame;
		this.blueLabel.enabled = isTeamGame;
		this.blueBgr.enabled = isTeamGame;
		this.redLabel.enabled = isTeamGame;
		this.redBgr.enabled = isTeamGame;

		if (isTeamGame) {
			if (num > num2) {
				this.titleLabel.text = "Your Team Won!";
			} else if (num < num2) {
				this.titleLabel.text = "Your Team Lost";
			} else {
				this.titleLabel.text = "Draw";
			}
		} else {
			List<GameActorInfo> list = new List<GameActorInfo>(GameState.Current.Players.Values);
			int maxScore = list.Reduce((GameActorInfo player, int prev) => Mathf.Max((int)player.Kills, prev), int.MinValue);
			List<GameActorInfo> list2 = list.FindAll((GameActorInfo el) => (int)el.Kills == maxScore);
			string str = string.Empty;
			list2.ForEach(delegate(GameActorInfo el) { str = str + el.PlayerName + " "; });
			this.titleLabel.text = str + "won!";
		}

		base.StartCoroutine(this.Wait5Seconds());
	}

	private IEnumerator Wait5Seconds() {
		for (int i = 5; i > 0; i--) {
			this.timerLabel.text = i.ToString();
			UITweener.Begin<TweenScale>(this.timerLabel.gameObject, 0.5f);
			UITweener.Begin<TweenAlpha>(this.timerLabel.gameObject, 0.25f);

			yield return new WaitForSeconds(1f);
		}

		this.panel.gameObject.SetActive(false);
		GameData.Instance.OnEndOfMatchTimer.Fire();

		yield break;
	}
}
