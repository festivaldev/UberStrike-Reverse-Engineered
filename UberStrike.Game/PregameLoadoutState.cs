using System;

internal class PregameLoadoutState : IState {
	private StateMachine<GameStateId> stateMachine;

	public PregameLoadoutState(StateMachine<GameStateId> stateMachine) {
		this.stateMachine = stateMachine;
	}

	public void OnEnter() {
		GamePageManager.Instance.LoadPage(IngamePageType.PreGame);
		Singleton<QuickItemController>.Instance.Restriction.RenewRoundUses();
		EventHandler.Global.AddListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
		SpawnLocalAvatar();

		if (GameState.Current.IsMultiplayer) {
			Singleton<ChatManager>.Instance.SetGameSection(GameState.Current.RoomData.Server.ConnectionString, GameState.Current.RoomData.Number, GameState.Current.RoomData.MapID, GameState.Current.Players.Values);
		}
	}

	public void OnExit() {
		EventHandler.Global.RemoveListener(new Action<GameEvents.PlayerRespawn>(OnPlayerRespawn));
		GamePageManager.Instance.UnloadCurrentPage();
	}

	public void OnResume() { }
	public void OnUpdate() { }

	private void SpawnLocalAvatar() {
		if (GameState.Current.Avatar.Decorator) {
			GameState.Current.Player.SpawnPlayerAt(GameState.Current.Map.DefaultSpawnPoint.position, GameState.Current.Map.DefaultSpawnPoint.rotation);
			GameState.Current.Avatar.Decorator.SetPosition(GameState.Current.Map.DefaultSpawnPoint.position, GameState.Current.Map.DefaultSpawnPoint.rotation);
			GameState.Current.Avatar.HideWeapons();
		}

		GameState.Current.PlayerState.SetState(PlayerStateId.Overview);
	}

	private void OnPlayerRespawn(GameEvents.PlayerRespawn ev) {
		stateMachine.SetState(GameStateId.MatchRunning);
		stateMachine.Events.Fire(ev);
	}
}
