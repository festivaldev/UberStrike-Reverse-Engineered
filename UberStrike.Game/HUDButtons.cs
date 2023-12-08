using UberStrike.Core.Types;
using UnityEngine;

public class HUDButtons : MonoBehaviour {
	[SerializeField]
	private UIEventReceiver changeTeamButton;

	[SerializeField]
	private UIEventReceiver continueButton;

	[SerializeField]
	private UIEventReceiver loadoutButton;

	[SerializeField]
	private UILabel loadoutButtonLabel;

	[SerializeField]
	private UIEventReceiver respawnButton;

	private void Start() {
		continueButton.gameObject.SetActive(false);
		respawnButton.gameObject.SetActive(false);
		changeTeamButton.gameObject.SetActive(false);

		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el) {
			var flag = el == PlayerStateId.Paused;
			var flag2 = el == PlayerStateId.Killed;
			var flag3 = GameState.Current.GameMode == GameModeType.None;
			respawnButton.gameObject.SetActive(flag2 && flag3);
			continueButton.gameObject.SetActive(flag);
			changeTeamButton.gameObject.SetActive(flag && GameStateHelper.CanChangeTeam());
			loadoutButton.gameObject.SetActive(flag || flag2);
			loadoutButtonLabel.text = ((!flag || flag3) ? "Loadout" : "Chat");
		}, this);

		GameData.Instance.OnRespawnCountdown.AddEvent(delegate(int el) {
			var flag4 = el == 0;
			respawnButton.gameObject.SetActive(flag4);
			changeTeamButton.gameObject.SetActive(flag4 && GameStateHelper.CanChangeTeam());
		}, this);

		continueButton.OnClicked = delegate {
			if (PanelManager.Instance != null && (PanelManager.Instance.IsPanelOpen(PanelType.Options) || PanelManager.Instance.IsPanelOpen(PanelType.Help))) {
				return;
			}

			InputManager.SkipFrame = Time.frameCount;
			GameState.Current.PlayerState.PopState();
			EventHandler.Global.Fire(new GameEvents.PlayerUnpause());
			GamePageManager.Instance.UnloadCurrentPage();
		};

		respawnButton.OnClicked = delegate {
			RenderSettingsController.Instance.ResetInterpolation();

			if (PanelManager.Instance != null && (PanelManager.Instance.IsPanelOpen(PanelType.Options) || PanelManager.Instance.IsPanelOpen(PanelType.Help))) {
				return;
			}

			respawnButton.gameObject.SetActive(false);
			changeTeamButton.gameObject.SetActive(false);

			if (GameState.Current.GameMode == GameModeType.None) {
				GameStateHelper.RespawnLocalPlayerAtRandom();
				GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
			} else {
				GameState.Current.Actions.RequestRespawn();
			}

			GamePageManager.Instance.UnloadCurrentPage();
		};

		changeTeamButton.OnClicked = delegate {
			respawnButton.gameObject.SetActive(false);
			changeTeamButton.gameObject.SetActive(false);
			GamePageManager.Instance.UnloadCurrentPage();
			GameData.Instance.OnNotification.Fire("Changing Team...");
			GameState.Current.Actions.ChangeTeam();

			if (GameData.Instance.PlayerState.Value == PlayerStateId.Killed) {
				GameState.Current.Actions.RequestRespawn();
			}
		};

		loadoutButton.OnClicked = delegate {
			if (GamePageManager.IsCurrentPage(IngamePageType.None)) {
				if (GameState.Current.IsSinglePlayer) {
					GamePageManager.Instance.LoadPage(IngamePageType.PausedOffline);
				} else if (!GameState.Current.IsMatchRunning || !GameState.Current.PlayerData.IsAlive) {
					GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
				} else {
					GamePageManager.Instance.LoadPage(IngamePageType.Paused);
				}
			} else {
				GamePageManager.Instance.UnloadCurrentPage();
			}
		};
	}

	private void OnEnable() {
		continueButton.gameObject.SetActive(false);
		respawnButton.gameObject.SetActive(false);
		changeTeamButton.gameObject.SetActive(false);
		loadoutButton.gameObject.SetActive(false);
		GameData.Instance.PlayerState.Fire();
	}
}
