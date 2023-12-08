using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

internal class PlayerSpectatingState : IState {
	private int _currentFollowIndex;
	private int currentPlayerId;
	private StateMachine<PlayerStateId> stateMachine;

	public PlayerSpectatingState(StateMachine<PlayerStateId> stateMachine) {
		this.stateMachine = stateMachine;
	}

	public void OnEnter() {
		GamePageManager.Instance.UnloadCurrentPage();
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerPause>(OnPlayerPaused));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerUnpause>(OnPlayerUnpaused));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerLeft>(OnPlayerLeft));
		EventHandler.Global.AddListener(new Action<GameEvents.FollowPlayer>(FollowNextPlayer));
		EventHandler.Global.AddListener(new Action<GlobalEvents.InputChanged>(OnInputChanged));
		LevelCamera.SetMode(LevelCamera.CameraMode.FreeSpectator);
		EnterFreeMoveMode();
		GameState.Current.PlayerData.ResetKeys();
		OnPlayerUnpaused(null);
		EventHandler.Global.Fire(new GameEvents.PlayerSpectator());
	}

	public void OnResume() { }

	public void OnExit() {
		currentPlayerId = 0;
		LevelCamera.SetMode(LevelCamera.CameraMode.Disabled);
		GamePageManager.Instance.UnloadCurrentPage();
		EventHandler.Global.RemoveListener(new Action<GlobalEvents.InputChanged>(OnInputChanged));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerPause>(OnPlayerPaused));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerUnpause>(OnPlayerUnpaused));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerLeft>(OnPlayerLeft));
		EventHandler.Global.RemoveListener(new Action<GameEvents.FollowPlayer>(FollowNextPlayer));
	}

	public void OnUpdate() {
		if (!Screen.lockCursor && !ApplicationDataManager.IsMobile) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		var flag = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetKey(KeyCode.Tab);

		if (Input.GetKeyDown(KeyCode.Escape) || flag) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}

		if (!GameData.Instance.HUDChatIsTyping && Input.GetKeyDown(KeyCode.Backspace)) {
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}
	}

	private void OnPlayerPaused(GameEvents.PlayerPause ev) {
		stateMachine.PushState(PlayerStateId.Paused);
	}

	private void OnPlayerUnpaused(GameEvents.PlayerUnpause ev) {
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		GameState.Current.Player.EnableWeaponControl = false;
		Screen.lockCursor = true;
		EventHandler.Global.Fire(new GameEvents.PlayerIngame());
	}

	private void OnPlayerLeft(GameEvents.PlayerLeft ev) {
		if (currentPlayerId == ev.Cmid) {
			EnterFreeMoveMode();
		}
	}

	private void OnInputChanged(GlobalEvents.InputChanged ev) {
		if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled && !GameData.Instance.HUDChatIsTyping && Screen.lockCursor) {
			if (ev.Key == GameInputKey.PrimaryFire && ev.IsDown) {
				FollowPrevPlayer();
			} else if (ev.Key == GameInputKey.SecondaryFire && ev.IsDown) {
				FollowNextPlayer();
			} else if (ev.Key == GameInputKey.Jump && ev.IsDown) {
				EnterFreeMoveMode();
			}
		}
	}

	private void FollowNextPlayer(GameEvents.FollowPlayer ev) {
		FollowNextPlayer();
	}

	private void FollowNextPlayer() {
		try {
			if (GameState.Current.HasJoinedGame && GameState.Current.Players.Count > 0) {
				var array = GameState.Current.Players.ValueArray();
				_currentFollowIndex = (_currentFollowIndex + 1) % array.Length;
				var currentFollowIndex = _currentFollowIndex;

				while (array[_currentFollowIndex].Cmid == PlayerDataManager.Cmid || !array[_currentFollowIndex].IsAlive || !GameState.Current.HasAvatarLoaded(array[_currentFollowIndex].Cmid)) {
					_currentFollowIndex = (_currentFollowIndex + 1) % array.Length;

					if (_currentFollowIndex == currentFollowIndex) {
						EnterFreeMoveMode();

						return;
					}
				}

				if (array[_currentFollowIndex] != null) {
					ChangeTarget(array[_currentFollowIndex].Cmid);
				} else {
					EnterFreeMoveMode();
				}
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to follow next player: " + ex.Message);
		}
	}

	private void FollowPrevPlayer() {
		try {
			if (GameState.Current.HasJoinedGame && GameState.Current.Players.Count > 0) {
				var list = new List<GameActorInfo>(GameState.Current.Players.Values);
				_currentFollowIndex = (_currentFollowIndex + list.Count - 1) % list.Count;
				var currentFollowIndex = _currentFollowIndex;

				while (list[_currentFollowIndex].Cmid == PlayerDataManager.Cmid || !list[_currentFollowIndex].IsAlive || !GameState.Current.HasAvatarLoaded(list[_currentFollowIndex].Cmid)) {
					_currentFollowIndex = (_currentFollowIndex + list.Count - 1) % list.Count;

					if (_currentFollowIndex == currentFollowIndex) {
						EnterFreeMoveMode();

						return;
					}
				}

				if (list[_currentFollowIndex] != null) {
					ChangeTarget(list[_currentFollowIndex].Cmid);
				} else {
					EnterFreeMoveMode();
				}
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to follow prev player: " + ex.Message);
		}
	}

	private void ChangeTarget(int cmid) {
		if (currentPlayerId != cmid) {
			CharacterConfig characterConfig;

			if (GameState.Current.TryGetPlayerAvatar(cmid, out characterConfig) && characterConfig.Avatar.Decorator) {
				currentPlayerId = cmid;
				LevelCamera.SetMode(LevelCamera.CameraMode.SmoothFollow, characterConfig.Avatar.Decorator.transform);

				if (!characterConfig.State.Player.IsAlive) {
					LevelCamera.SetPosition(characterConfig.transform.position);
				}
			} else {
				EnterFreeMoveMode();
			}
		}
	}

	private void EnterFreeMoveMode() {
		if (LevelCamera.CurrentMode != LevelCamera.CameraMode.FreeSpectator) {
			currentPlayerId = 0;
			LevelCamera.SetMode(LevelCamera.CameraMode.FreeSpectator);
			Screen.lockCursor = true;
		}
	}
}
