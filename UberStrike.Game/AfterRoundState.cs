internal class AfterRoundState : IState {
	public AfterRoundState(StateMachine<GameStateId> stateMachine) { }

	public void OnEnter() {
		GameState.Current.PlayerState.SetState(PlayerStateId.AfterRound);
	}

	public void OnResume() { }
	public void OnExit() { }
	public void OnUpdate() { }
}
