using System;
using UnityEngine;

internal class PlayerPlayingState : IState {
	private StateMachine<PlayerStateId> stateMachine;

	public PlayerPlayingState(StateMachine<PlayerStateId> stateMachine) {
		this.stateMachine = stateMachine;
	}

	public void OnEnter() {
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerDamage>(OnPlayerDamage));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerDied>(OnPlayerKilled));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerUnpause>(OnPlayerUnpaused));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerPause>(OnPlayerPaused));
		EventHandler.Global.AddListener(new Action<GameEvents.ChatWindow>(OnPlayerChatting));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate += GameState.Current.Player.MoveController.UpdatePlayerMovement;
		OnPlayerRespawn(null);
		OnPlayerUnpaused(null);
	}

	public void OnResume() { }

	public void OnExit() {
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerDied>(OnPlayerKilled));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerDamage>(OnPlayerDamage));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerPause>(OnPlayerPaused));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerUnpause>(OnPlayerUnpaused));
		EventHandler.Global.RemoveListener(new Action<GameEvents.ChatWindow>(OnPlayerChatting));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
		AutoMonoBehaviour<UnityRuntime>.Instance.OnFixedUpdate -= GameState.Current.Player.MoveController.UpdatePlayerMovement;
	}

	public void OnUpdate() {
		if (!Screen.lockCursor && !ApplicationDataManager.IsMobile) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		var flag = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetKey(KeyCode.Tab);

		if (Input.GetKeyDown(KeyCode.Escape) || flag) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		if (!GameData.Instance.HUDChatIsTyping) {
			if (Input.GetKeyDown(KeyCode.L)) {
				if (GamePageManager.IsCurrentPage(IngamePageType.None)) {
					EventHandler.Global.Fire(new GameEvents.PlayerPause());

					if (GameState.Current.IsSinglePlayer) {
						GamePageManager.Instance.LoadPage(IngamePageType.PausedOffline);
					} else if (!GameState.Current.IsMatchRunning) {
						GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
					} else {
						GamePageManager.Instance.LoadPage(IngamePageType.Paused);
					}
				} else {
					GamePageManager.Instance.UnloadCurrentPage();
				}
			} else if (Input.GetKeyDown(KeyCode.Backspace)) {
				EventHandler.Global.Fire(new GameEvents.PlayerPause());
			}
		}
	}

	private void OnPlayerRespawn(GameEvents.PlayerRespawn ev) {
		GameState.Current.Player.InitializePlayer();
	}

	private void OnPlayerChatting(GameEvents.ChatWindow ev) {
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = !ev.IsEnabled && stateMachine.CurrentStateId == PlayerStateId.Playing;
		GameState.Current.PlayerData.ResetKeys();
	}

	private void OnPlayerPaused(GameEvents.PlayerPause ev) {
		stateMachine.PushState(PlayerStateId.Paused);
	}

	private void OnPlayerUnpaused(GameEvents.PlayerUnpause ev) {
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
		Singleton<QuickItemController>.Instance.IsEnabled = true;
		GameState.Current.Player.EnableWeaponControl = true;
		Screen.lockCursor = true;
		LevelCamera.SetMode(LevelCamera.CameraMode.FirstPerson);

		if (!Singleton<WeaponController>.Instance.CheckWeapons(Singleton<LoadoutManager>.Instance.GetWeapons())) {
			GameState.Current.Player.InitializeWeapons();
		}

		EventHandler.Global.Fire(new GameEvents.PlayerIngame());
	}

	private void OnPlayerDamage(GameEvents.PlayerDamage ev) {
		Singleton<DamageFeedbackHud>.Instance.AddDamageMark(Mathf.Clamp01(ev.DamageValue / 50f), ev.Angle);

		if (!GameState.Current.Player.MoveController.IsGrounded) {
			GameState.Current.Player.MoveController.ApplyForce(Quaternion.AngleAxis(-ev.Angle, Vector3.up) * Vector3.forward * ev.DamageValue * 10f, CharacterMoveController.ForceType.Additive);
		}

		if (GameState.Current.PlayerData.ArmorPoints > 0) {
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.LocalPlayerHitArmorRemaining);
		} else {
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.LocalPlayerHitNoArmor);
		}
	}

	private void OnPlayerKilled(GameEvents.PlayerDied ev) {
		stateMachine.SetState(PlayerStateId.Killed);
	}
}
