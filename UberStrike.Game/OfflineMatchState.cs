using System;

internal class OfflineMatchState : IState {
	public OfflineMatchState(StateMachine<GameStateId> stateMachine) { }

	public void OnEnter() {
		GamePageManager.Instance.UnloadCurrentPage();
		GameStateHelper.RespawnLocalPlayerAtRandom();
		GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
		EventHandler.Global.Fire(new GameEvents.PlayerIngame());
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerDied>(OnPlayerKilled));
	}

	public void OnExit() {
		GamePageManager.Instance.UnloadCurrentPage();
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerDied>(OnPlayerKilled));
	}

	public void OnResume() { }
	public void OnUpdate() { }

	private void OnPlayerKilled(GameEvents.PlayerDied ev) {
		GameState.Current.PlayerState.SetState(PlayerStateId.Killed);
	}
}
