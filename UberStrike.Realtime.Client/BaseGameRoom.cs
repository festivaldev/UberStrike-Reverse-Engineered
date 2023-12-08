using System.Collections.Generic;
using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Realtime.Client {
	public abstract class BaseGameRoom : IEventDispatcher, IRoomLogic {
		public GameRoomOperations Operations { get; private set; }

		protected BaseGameRoom() {
			Operations = new GameRoomOperations();
		}

		public void OnEvent(byte id, byte[] data) {
			switch (id) {
				case 12:
					PowerUpPicked(data);

					break;
				case 13:
					SetPowerupState(data);

					break;
				case 14:
					ResetAllPowerups(data);

					break;
				case 15:
					DoorOpen(data);

					break;
				case 16:
					DisconnectCountdown(data);

					break;
				case 17:
					MatchStartCountdown(data);

					break;
				case 18:
					MatchStart(data);

					break;
				case 19:
					MatchEnd(data);

					break;
				case 20:
					TeamWins(data);

					break;
				case 21:
					WaitingForPlayers(data);

					break;
				case 22:
					PrepareNextRound(data);

					break;
				case 23:
					AllPlayers(data);

					break;
				case 24:
					AllPlayerDeltas(data);

					break;
				case 25:
					AllPlayerPositions(data);

					break;
				case 26:
					PlayerDelta(data);

					break;
				case 27:
					PlayerJumped(data);

					break;
				case 28:
					PlayerRespawnCountdown(data);

					break;
				case 29:
					PlayerRespawned(data);

					break;
				case 30:
					PlayerJoinedGame(data);

					break;
				case 31:
					JoinGameFailed(data);

					break;
				case 32:
					PlayerLeftGame(data);

					break;
				case 33:
					PlayerChangedTeam(data);

					break;
				case 34:
					JoinedAsSpectator(data);

					break;
				case 35:
					PlayersReadyUpdated(data);

					break;
				case 36:
					DamageEvent(data);

					break;
				case 37:
					PlayerKilled(data);

					break;
				case 38:
					UpdateRoundScore(data);

					break;
				case 39:
					KillsRemaining(data);

					break;
				case 40:
					LevelUp(data);

					break;
				case 41:
					KickPlayer(data);

					break;
				case 42:
					QuickItemEvent(data);

					break;
				case 43:
					SingleBulletFire(data);

					break;
				case 44:
					PlayerHit(data);

					break;
				case 45:
					RemoveProjectile(data);

					break;
				case 46:
					EmitProjectile(data);

					break;
				case 47:
					EmitQuickItem(data);

					break;
				case 48:
					ActivateQuickItem(data);

					break;
				case 49:
					ChatMessage(data);

					break;
			}
		}

		IOperationSender IRoomLogic.Operations {
			get { return Operations; }
		}

		protected abstract void OnPowerUpPicked(int id, byte flag);
		protected abstract void OnSetPowerupState(List<int> states);
		protected abstract void OnResetAllPowerups();
		protected abstract void OnDoorOpen(int id);
		protected abstract void OnDisconnectCountdown(byte countdown);
		protected abstract void OnMatchStartCountdown(byte countdown);
		protected abstract void OnMatchStart(int roundNumber, int endTime);
		protected abstract void OnMatchEnd(EndOfMatchData data);
		protected abstract void OnTeamWins(TeamID team);
		protected abstract void OnWaitingForPlayers();
		protected abstract void OnPrepareNextRound();
		protected abstract void OnAllPlayers(List<GameActorInfo> allPlayers, List<PlayerMovement> allPositions, ushort gameframe);
		protected abstract void OnAllPlayerDeltas(List<GameActorInfoDelta> allDeltas);
		protected abstract void OnAllPlayerPositions(List<PlayerMovement> allPositions, ushort gameframe);
		protected abstract void OnPlayerDelta(GameActorInfoDelta delta);
		protected abstract void OnPlayerJumped(int cmid, Vector3 position);
		protected abstract void OnPlayerRespawnCountdown(byte countdown);
		protected abstract void OnPlayerRespawned(int cmid, Vector3 position, byte rotation);
		protected abstract void OnPlayerJoinedGame(GameActorInfo player, PlayerMovement position);
		protected abstract void OnJoinGameFailed(string message);
		protected abstract void OnPlayerLeftGame(int cmid);
		protected abstract void OnPlayerChangedTeam(int cmid, TeamID team);
		protected abstract void OnJoinedAsSpectator();
		protected abstract void OnPlayersReadyUpdated();
		protected abstract void OnDamageEvent(DamageEvent damageEvent);
		protected abstract void OnPlayerKilled(int shooter, int target, byte weaponClass, ushort damage, byte bodyPart, Vector3 direction);
		protected abstract void OnUpdateRoundScore(int round, short blue, short red);
		protected abstract void OnKillsRemaining(int killsRemaining, int leaderCmid);
		protected abstract void OnLevelUp(int newLevel);
		protected abstract void OnKickPlayer(string message);
		protected abstract void OnQuickItemEvent(int cmid, byte eventType, int robotLifeTime, int scrapsLifeTime, bool isInstant);
		protected abstract void OnSingleBulletFire(int cmid);
		protected abstract void OnPlayerHit(Vector3 force);
		protected abstract void OnRemoveProjectile(int projectileId, bool explode);
		protected abstract void OnEmitProjectile(int cmid, Vector3 origin, Vector3 direction, byte slot, int projectileID, bool explode);
		protected abstract void OnEmitQuickItem(Vector3 origin, Vector3 direction, int itemId, byte playerNumber, int projectileID);
		protected abstract void OnActivateQuickItem(int cmid, QuickItemLogic logic, int robotLifeTime, int scrapsLifeTime, bool isInstant);
		protected abstract void OnChatMessage(int cmid, string name, string message, MemberAccessLevel accessLevel, byte context);

		private void PowerUpPicked(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				OnPowerUpPicked(num, b);
			}
		}

		private void SetPowerupState(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<int>.Deserialize(memoryStream, Int32Proxy.Deserialize);
				OnSetPowerupState(list);
			}
		}

		private void ResetAllPowerups(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnResetAllPowerups();
			}
		}

		private void DoorOpen(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnDoorOpen(num);
			}
		}

		private void DisconnectCountdown(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var b = ByteProxy.Deserialize(memoryStream);
				OnDisconnectCountdown(b);
			}
		}

		private void MatchStartCountdown(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var b = ByteProxy.Deserialize(memoryStream);
				OnMatchStartCountdown(b);
			}
		}

		private void MatchStart(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				OnMatchStart(num, num2);
			}
		}

		private void MatchEnd(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var endOfMatchData = EndOfMatchDataProxy.Deserialize(memoryStream);
				OnMatchEnd(endOfMatchData);
			}
		}

		private void TeamWins(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var teamID = EnumProxy<TeamID>.Deserialize(memoryStream);
				OnTeamWins(teamID);
			}
		}

		private void WaitingForPlayers(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnWaitingForPlayers();
			}
		}

		private void PrepareNextRound(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnPrepareNextRound();
			}
		}

		private void AllPlayers(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<GameActorInfo>.Deserialize(memoryStream, GameActorInfoProxy.Deserialize);
				var list2 = ListProxy<PlayerMovement>.Deserialize(memoryStream, PlayerMovementProxy.Deserialize);
				var num = UInt16Proxy.Deserialize(memoryStream);
				OnAllPlayers(list, list2, num);
			}
		}

		private void AllPlayerDeltas(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<GameActorInfoDelta>.Deserialize(memoryStream, GameActorInfoDeltaProxy.Deserialize);
				OnAllPlayerDeltas(list);
			}
		}

		private void AllPlayerPositions(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<PlayerMovement>.Deserialize(memoryStream, PlayerMovementProxy.Deserialize);
				var num = UInt16Proxy.Deserialize(memoryStream);
				OnAllPlayerPositions(list, num);
			}
		}

		private void PlayerDelta(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var gameActorInfoDelta = GameActorInfoDeltaProxy.Deserialize(memoryStream);
				OnPlayerDelta(gameActorInfoDelta);
			}
		}

		private void PlayerJumped(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var vector = Vector3Proxy.Deserialize(memoryStream);
				OnPlayerJumped(num, vector);
			}
		}

		private void PlayerRespawnCountdown(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var b = ByteProxy.Deserialize(memoryStream);
				OnPlayerRespawnCountdown(b);
			}
		}

		private void PlayerRespawned(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var vector = Vector3Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				OnPlayerRespawned(num, vector, b);
			}
		}

		private void PlayerJoinedGame(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var gameActorInfo = GameActorInfoProxy.Deserialize(memoryStream);
				var playerMovement = PlayerMovementProxy.Deserialize(memoryStream);
				OnPlayerJoinedGame(gameActorInfo, playerMovement);
			}
		}

		private void JoinGameFailed(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				OnJoinGameFailed(text);
			}
		}

		private void PlayerLeftGame(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnPlayerLeftGame(num);
			}
		}

		private void PlayerChangedTeam(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var teamID = EnumProxy<TeamID>.Deserialize(memoryStream);
				OnPlayerChangedTeam(num, teamID);
			}
		}

		private void JoinedAsSpectator(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnJoinedAsSpectator();
			}
		}

		private void PlayersReadyUpdated(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnPlayersReadyUpdated();
			}
		}

		private void DamageEvent(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var damageEvent = DamageEventProxy.Deserialize(memoryStream);
				OnDamageEvent(damageEvent);
			}
		}

		private void PlayerKilled(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				var num3 = UInt16Proxy.Deserialize(memoryStream);
				var b2 = ByteProxy.Deserialize(memoryStream);
				var vector = Vector3Proxy.Deserialize(memoryStream);
				OnPlayerKilled(num, num2, b, num3, b2, vector);
			}
		}

		private void UpdateRoundScore(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var num2 = Int16Proxy.Deserialize(memoryStream);
				var num3 = Int16Proxy.Deserialize(memoryStream);
				OnUpdateRoundScore(num, num2, num3);
			}
		}

		private void KillsRemaining(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				OnKillsRemaining(num, num2);
			}
		}

		private void LevelUp(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnLevelUp(num);
			}
		}

		private void KickPlayer(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				OnKickPlayer(text);
			}
		}

		private void QuickItemEvent(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				var num3 = Int32Proxy.Deserialize(memoryStream);
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnQuickItemEvent(num, b, num2, num3, flag);
			}
		}

		private void SingleBulletFire(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnSingleBulletFire(num);
			}
		}

		private void PlayerHit(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var vector = Vector3Proxy.Deserialize(memoryStream);
				OnPlayerHit(vector);
			}
		}

		private void RemoveProjectile(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnRemoveProjectile(num, flag);
			}
		}

		private void EmitProjectile(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var vector = Vector3Proxy.Deserialize(memoryStream);
				var vector2 = Vector3Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnEmitProjectile(num, vector, vector2, b, num2, flag);
			}
		}

		private void EmitQuickItem(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var vector = Vector3Proxy.Deserialize(memoryStream);
				var vector2 = Vector3Proxy.Deserialize(memoryStream);
				var num = Int32Proxy.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				OnEmitQuickItem(vector, vector2, num, b, num2);
			}
		}

		private void ActivateQuickItem(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var quickItemLogic = EnumProxy<QuickItemLogic>.Deserialize(memoryStream);
				var num2 = Int32Proxy.Deserialize(memoryStream);
				var num3 = Int32Proxy.Deserialize(memoryStream);
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnActivateQuickItem(num, quickItemLogic, num2, num3, flag);
			}
		}

		private void ChatMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var text = StringProxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				var memberAccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				OnChatMessage(num, text, text2, memberAccessLevel, b);
			}
		}
	}
}
