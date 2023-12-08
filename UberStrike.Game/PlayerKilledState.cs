using System;
using UnityEngine;

internal class PlayerKilledState : IState {
	private const int DisconnectionTimeout = 120;
	private const int DisconnectionTimeoutAdmin = 1200;
	private StateMachine<PlayerStateId> stateMachine;

	public PlayerKilledState(StateMachine<PlayerStateId> stateMachine) {
		this.stateMachine = stateMachine;
	}

	public void OnEnter() {
		Screen.lockCursor = false;
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
		LevelCamera.SetMode(LevelCamera.CameraMode.Ragdoll);
		stateMachine.Events.AddListener(new Action<GameEvents.RespawnCountdown>(OnRespawnCountdown));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerUnpause>(OnUnpause));
	}

	public void OnResume() { }

	public void OnExit() {
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerUnpause>(OnUnpause));
		stateMachine.Events.RemoveListener(new Action<GameEvents.RespawnCountdown>(OnRespawnCountdown));
	}

	public void OnUpdate() {
		GameStateHelper.UpdateMatchTime();

		if (Input.GetKeyDown(KeyCode.L) && !GameData.Instance.HUDChatIsTyping) {
			if (GamePageManager.IsCurrentPage(IngamePageType.None)) {
				GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
			} else {
				GamePageManager.Instance.UnloadCurrentPage();
			}
		}
	}

	private void OnUnpause(GameEvents.PlayerUnpause ev) { }

	private void OnRespawnCountdown(GameEvents.RespawnCountdown ev) {
		GameData.Instance.OnRespawnCountdown.Fire(ev.Countdown);
	}
}
