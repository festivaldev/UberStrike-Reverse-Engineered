using System;
using UnityEngine;

internal class EndOfMatchState : IState {
	public EndOfMatchState(StateMachine<GameStateId> stateMachine) { }

	public void OnEnter() {
		AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.EndOfRound);
		Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
		EventHandler.Global.AddListener(new Action<GameEvents.MatchCountdown>(OnMatchCountdown));
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
		GamePageManager.Instance.LoadPage(IngamePageType.EndOfMatch);
		EventHandler.Global.Fire(new GameEvents.MatchEnd());
		SpawnLocalAvatar();
	}

	public void OnExit() {
		EventHandler.Global.RemoveListener(new Action<GameEvents.MatchCountdown>(OnMatchCountdown));
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
	}

	public void OnResume() { }
	public void OnUpdate() { }

	private void SpawnLocalAvatar() {
		if (GameState.Current.Avatar.Decorator) {
			GameState.Current.Player.SpawnPlayerAt(GameState.Current.Map.DefaultSpawnPoint.position, GameState.Current.Map.DefaultSpawnPoint.rotation);

			if (GameState.Current.Player.Character) {
				GameState.Current.PlayerData.Reset();
				GameState.Current.Player.Character.Reset();
			}
		}

		GameState.Current.PlayerState.SetState(PlayerStateId.Overview);
	}

	private void OnPlayerRespawn(GameEvents.PlayerRespawn ev) {
		GameState.Current.RespawnLocalPlayerAt(ev.Position, Quaternion.Euler(0f, ev.Rotation, 0f));
		GameState.Current.PlayerState.SetState(PlayerStateId.PrepareForMatch);
	}

	private void OnMatchCountdown(GameEvents.MatchCountdown ev) {
		if (ev.Countdown <= 3 && ev.Countdown > 0) {
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.CountdownTonal1);
		}
	}
}
