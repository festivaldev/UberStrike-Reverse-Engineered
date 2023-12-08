using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UnityEngine;

public class HUDJoinButtons : MonoBehaviour {
	[SerializeField]
	private UISprite[] bars;

	[SerializeField]
	private UISprite[] blueBars;

	[SerializeField]
	private UIEventReceiver join;

	[SerializeField]
	private UIEventReceiver joinBlue;

	[SerializeField]
	private UIEventReceiver joinRed;

	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UISprite[] redBars;

	[SerializeField]
	private UIEventReceiver spectate;

	private void Start() {
		spectate.gameObject.SetActive(PlayerDataManager.AccessLevel >= MemberAccessLevel.QA);
		GameData.Instance.OnEndOfMatchTimer.AddEvent(() => panel.gameObject.SetActive(true), this);

		joinBlue.OnClicked = () => {
			GamePageManager.Instance.UnloadCurrentPage();
			GameState.Current.Actions.JoinTeam(TeamID.BLUE);
		};

		joinRed.OnClicked = () => {
			GamePageManager.Instance.UnloadCurrentPage();
			GameState.Current.Actions.JoinTeam(TeamID.RED);
		};

		join.OnClicked = () => {
			GamePageManager.Instance.UnloadCurrentPage();
			GameState.Current.Actions.JoinTeam(TeamID.NONE);
		};

		spectate.OnClicked = () => {
			GamePageManager.Instance.UnloadCurrentPage();
			GameState.Current.PlayerData.Team = new Property<TeamID>(TeamID.NONE);
			GameState.Current.Actions.JoinAsSpectator();
		};
	}

	private void OnEnable() {
		var isTeamGame = GameState.Current.IsTeamGame;

		if (GameState.Current.MatchState.CurrentStateId != GameStateId.PregameLoadout) {
			panel.gameObject.SetActive(false);
		}

		joinBlue.gameObject.SetActive(isTeamGame);
		joinRed.gameObject.SetActive(isTeamGame);
		join.gameObject.SetActive(!isTeamGame);

		foreach (var uisprite in blueBars) {
			uisprite.enabled = isTeamGame;
		}

		foreach (var uisprite2 in redBars) {
			uisprite2.enabled = isTeamGame;
		}

		foreach (var uisprite3 in bars) {
			uisprite3.enabled = !isTeamGame;
		}
	}

	private void Update() {
		if (GameState.Current.IsTeamGame) {
			var num = Mathf.CeilToInt(GameState.Current.RoomData.PlayerLimit / 2f);
			joinBlue.GetComponent<UIButton>().isEnabled = GameState.Current.CanJoinBlueTeam;
			var blueTeamPlayerCount = GameState.Current.BlueTeamPlayerCount;
			var num2 = Mathf.Clamp(num, 0, blueBars.Length);

			for (var i = 0; i < blueBars.Length; i++) {
				blueBars[i].enabled = i < num2;
				blueBars[i].color = ((i >= blueTeamPlayerCount) ? GUIUtils.ColorBlack : GUIUtils.ColorBlue);
			}

			joinRed.GetComponent<UIButton>().isEnabled = GameState.Current.CanJoinRedTeam;
			var redTeamPlayerCount = GameState.Current.RedTeamPlayerCount;
			var num3 = Mathf.Clamp(num, 0, redBars.Length);

			for (var j = 0; j < redBars.Length; j++) {
				redBars[j].enabled = j < num3;
				redBars[j].color = ((j >= redTeamPlayerCount) ? GUIUtils.ColorBlack : GUIUtils.ColorRed);
			}
		} else {
			var playerLimit = GameState.Current.RoomData.PlayerLimit;
			var count = GameState.Current.Players.Count;

			for (var k = 0; k < bars.Length; k++) {
				bars[k].enabled = k < playerLimit;
				bars[k].color = ((k >= count) ? GUIUtils.ColorBlack : GUIUtils.ColorBlue);
			}
		}
	}
}
