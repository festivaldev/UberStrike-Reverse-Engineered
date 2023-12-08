using System;
using UberStrike.Core.Models;
using UnityEngine;

public class TryInShopRoom : IDisposable, IGameMode {
	private bool isDisposed;

	public TryInShopRoom() {
		GameState.Current.MatchState.RegisterState(GameStateId.MatchRunning, new OfflineMatchState(GameState.Current.MatchState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Playing, new PlayerPlayingState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Paused, new PlayerPausedState(GameState.Current.PlayerState));
		GameState.Current.Actions.DirectHitDamage = delegate(int targetCmid, ushort damage, BodyPart part, Vector3 force, byte slot, byte bullets) { GameState.Current.Player.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive); };
		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += OnUpdate;
		GameStateHelper.EnterGameMode();

		var gameActorInfo = new GameActorInfo {
			Cmid = PlayerDataManager.Cmid,
			SkinColor = PlayerDataManager.SkinColor
		};

		GameState.Current.PlayerData.Player = gameActorInfo;
		GameState.Current.Players[gameActorInfo.Cmid] = gameActorInfo;
		GameState.Current.InstantiateAvatar(gameActorInfo);
		MenuPageManager.Instance.UnloadCurrentPage();
		GameState.Current.MatchState.SetState(GameStateId.MatchRunning);
	}

	public void Dispose() {
		if (!isDisposed) {
			isDisposed = true;
			AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate -= OnUpdate;
			GameStateHelper.ExitGameMode();
		}
	}

	public GameMode Type {
		get { return GameMode.Training; }
	}

	private void OnUpdate() {
		Singleton<QuickItemController>.Instance.Update();

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Singleton<GameStateController>.Instance.UnloadGameMode();
			MenuPageManager.Instance.LoadPage(PageType.Shop);
		}
	}
}
