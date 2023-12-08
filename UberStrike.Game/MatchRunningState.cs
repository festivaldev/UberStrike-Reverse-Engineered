using System;
using UnityEngine;

internal class MatchRunningState : IState {
	public MatchRunningState(StateMachine<GameStateId> stateMachine) { }

	public void OnEnter() {
		Singleton<ProjectileManager>.Instance.Clear();
		AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.Fight);

		if (GameState.Current.Players.ContainsKey(PlayerDataManager.Cmid) && !GameState.Current.PlayerData.IsSpectator) {
			GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
		} else {
			GameState.Current.PlayerState.SetState(PlayerStateId.Spectating);
		}

		EventHandler.Global.AddListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
	}

	public void OnResume() { }

	public void OnExit() {
		GameState.Current.PlayerState.PopAllStates();
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
	}

	public void OnUpdate() {
		GameStateHelper.UpdateMatchTime();
	}

	private void OnPlayerRespawn(GameEvents.PlayerRespawn ev) {
		GameState.Current.RespawnLocalPlayerAt(ev.Position, Quaternion.Euler(0f, ev.Rotation, 0f));
		GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
	}
}
