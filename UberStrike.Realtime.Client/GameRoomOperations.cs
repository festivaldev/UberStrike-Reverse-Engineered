using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Realtime.Client {
	public sealed class GameRoomOperations : IOperationSender {
		private byte __id;
		private RemoteProcedureCall sendOperation;

		public GameRoomOperations(byte id = 0) {
			__id = id;
		}

		public event RemoteProcedureCall SendOperation {
			add { sendOperation = (RemoteProcedureCall)Delegate.Combine(sendOperation, value); }
			remove { sendOperation = (RemoteProcedureCall)Delegate.Remove(sendOperation, value); }
		}

		public void SendJoinGame(TeamID team) {
			using (var memoryStream = new MemoryStream()) {
				EnumProxy<TeamID>.Serialize(memoryStream, team);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(1, dictionary);
				}
			}
		}

		public void SendJoinAsSpectator() {
			using (var memoryStream = new MemoryStream()) {
				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(2, dictionary);
				}
			}
		}

		public void SendPowerUpRespawnTimes(List<ushort> respawnTimes) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<ushort>.Serialize(memoryStream, respawnTimes, UInt16Proxy.Serialize);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(3, dictionary);
				}
			}
		}

		public void SendPowerUpPicked(int powerupId, byte type, byte value) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, powerupId);
				ByteProxy.Serialize(memoryStream, type);
				ByteProxy.Serialize(memoryStream, value);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(4, dictionary);
				}
			}
		}

		public void SendIncreaseHealthAndArmor(byte health, byte armor) {
			using (var memoryStream = new MemoryStream()) {
				ByteProxy.Serialize(memoryStream, health);
				ByteProxy.Serialize(memoryStream, armor);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(5, dictionary);
				}
			}
		}

		public void SendOpenDoor(int doorId) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, doorId);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(6, dictionary);
				}
			}
		}

		public void SendSpawnPositions(TeamID team, List<Vector3> positions, List<byte> rotations) {
			using (var memoryStream = new MemoryStream()) {
				EnumProxy<TeamID>.Serialize(memoryStream, team);
				ListProxy<Vector3>.Serialize(memoryStream, positions, Vector3Proxy.Serialize);
				ListProxy<byte>.Serialize(memoryStream, rotations, ByteProxy.Serialize);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(7, dictionary);
				}
			}
		}

		public void SendRespawnRequest() {
			using (var memoryStream = new MemoryStream()) {
				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(8, dictionary);
				}
			}
		}

		public void SendDirectHitDamage(int target, byte bodyPart, byte bullets) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, target);
				ByteProxy.Serialize(memoryStream, bodyPart);
				ByteProxy.Serialize(memoryStream, bullets);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(9, dictionary);
				}
			}
		}

		public void SendExplosionDamage(int target, byte slot, byte distance, Vector3 force) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, target);
				ByteProxy.Serialize(memoryStream, slot);
				ByteProxy.Serialize(memoryStream, distance);
				Vector3Proxy.Serialize(memoryStream, force);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(10, dictionary);
				}
			}
		}

		public void SendDirectDamage(ushort damage) {
			using (var memoryStream = new MemoryStream()) {
				UInt16Proxy.Serialize(memoryStream, damage);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(11, dictionary);
				}
			}
		}

		public void SendDirectDeath() {
			using (var memoryStream = new MemoryStream()) {
				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(12, dictionary);
				}
			}
		}

		public void SendJump(Vector3 position) {
			using (var memoryStream = new MemoryStream()) {
				Vector3Proxy.Serialize(memoryStream, position);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(13, dictionary);
				}
			}
		}

		public void SendUpdatePositionAndRotation(ShortVector3 position, ShortVector3 velocity, byte hrot, byte vrot, byte moveState) {
			using (var memoryStream = new MemoryStream()) {
				ShortVector3Proxy.Serialize(memoryStream, position);
				ShortVector3Proxy.Serialize(memoryStream, velocity);
				ByteProxy.Serialize(memoryStream, hrot);
				ByteProxy.Serialize(memoryStream, vrot);
				ByteProxy.Serialize(memoryStream, moveState);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(14, dictionary, false);
				}
			}
		}

		public void SendKickPlayer(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(15, dictionary);
				}
			}
		}

		public void SendIsFiring(bool on) {
			using (var memoryStream = new MemoryStream()) {
				BooleanProxy.Serialize(memoryStream, on);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(16, dictionary);
				}
			}
		}

		public void SendIsReadyForNextMatch(bool on) {
			using (var memoryStream = new MemoryStream()) {
				BooleanProxy.Serialize(memoryStream, on);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(17, dictionary);
				}
			}
		}

		public void SendIsPaused(bool on) {
			using (var memoryStream = new MemoryStream()) {
				BooleanProxy.Serialize(memoryStream, on);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(18, dictionary);
				}
			}
		}

		public void SendIsInSniperMode(bool on) {
			using (var memoryStream = new MemoryStream()) {
				BooleanProxy.Serialize(memoryStream, on);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(19, dictionary);
				}
			}
		}

		public void SendSingleBulletFire() {
			using (var memoryStream = new MemoryStream()) {
				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(20, dictionary);
				}
			}
		}

		public void SendSwitchWeapon(byte weaponSlot) {
			using (var memoryStream = new MemoryStream()) {
				ByteProxy.Serialize(memoryStream, weaponSlot);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(21, dictionary);
				}
			}
		}

		public void SendSwitchTeam() {
			using (var memoryStream = new MemoryStream()) {
				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(22, dictionary);
				}
			}
		}

		public void SendChangeGear(int head, int face, int upperBody, int lowerBody, int gloves, int boots, int holo) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, head);
				Int32Proxy.Serialize(memoryStream, face);
				Int32Proxy.Serialize(memoryStream, upperBody);
				Int32Proxy.Serialize(memoryStream, lowerBody);
				Int32Proxy.Serialize(memoryStream, gloves);
				Int32Proxy.Serialize(memoryStream, boots);
				Int32Proxy.Serialize(memoryStream, holo);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(23, dictionary);
				}
			}
		}

		public void SendEmitProjectile(Vector3 origin, Vector3 direction, byte slot, int projectileID, bool explode) {
			using (var memoryStream = new MemoryStream()) {
				Vector3Proxy.Serialize(memoryStream, origin);
				Vector3Proxy.Serialize(memoryStream, direction);
				ByteProxy.Serialize(memoryStream, slot);
				Int32Proxy.Serialize(memoryStream, projectileID);
				BooleanProxy.Serialize(memoryStream, explode);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(24, dictionary);
				}
			}
		}

		public void SendEmitQuickItem(Vector3 origin, Vector3 direction, int itemId, byte playerNumber, int projectileID) {
			using (var memoryStream = new MemoryStream()) {
				Vector3Proxy.Serialize(memoryStream, origin);
				Vector3Proxy.Serialize(memoryStream, direction);
				Int32Proxy.Serialize(memoryStream, itemId);
				ByteProxy.Serialize(memoryStream, playerNumber);
				Int32Proxy.Serialize(memoryStream, projectileID);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(25, dictionary);
				}
			}
		}

		public void SendRemoveProjectile(int projectileId, bool explode) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, projectileId);
				BooleanProxy.Serialize(memoryStream, explode);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(26, dictionary);
				}
			}
		}

		public void SendHitFeedback(int targetCmid, Vector3 force) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, targetCmid);
				Vector3Proxy.Serialize(memoryStream, force);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(27, dictionary);
				}
			}
		}

		public void SendActivateQuickItem(QuickItemLogic logic, int robotLifeTime, int scrapsLifeTime, bool isInstant) {
			using (var memoryStream = new MemoryStream()) {
				EnumProxy<QuickItemLogic>.Serialize(memoryStream, logic);
				Int32Proxy.Serialize(memoryStream, robotLifeTime);
				Int32Proxy.Serialize(memoryStream, scrapsLifeTime);
				BooleanProxy.Serialize(memoryStream, isInstant);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(28, dictionary);
				}
			}
		}

		public void SendChatMessage(string message, byte context) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, message);
				ByteProxy.Serialize(memoryStream, context);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(29, dictionary);
				}
			}
		}
	}
}
