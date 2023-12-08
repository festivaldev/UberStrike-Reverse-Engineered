using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client {
	public sealed class LobbyRoomOperations : IOperationSender {
		private byte __id;
		private RemoteProcedureCall sendOperation;

		public LobbyRoomOperations(byte id = 0) {
			__id = id;
		}

		public event RemoteProcedureCall SendOperation {
			add { sendOperation = (RemoteProcedureCall)Delegate.Combine(sendOperation, value); }
			remove { sendOperation = (RemoteProcedureCall)Delegate.Remove(sendOperation, value); }
		}

		public void SendFullPlayerListUpdate() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendUpdatePlayerRoom(GameRoom room) {
			using (var memoryStream = new MemoryStream()) {
				GameRoomProxy.Serialize(memoryStream, room);

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

		public void SendResetPlayerRoom() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendUpdateFriendsList(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendUpdateClanData(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendUpdateInboxMessages(int cmid, int messageId) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);
				Int32Proxy.Serialize(memoryStream, messageId);

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

		public void SendUpdateInboxRequests(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendUpdateClanMembers(List<int> clanMembers) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<int>.Serialize(memoryStream, clanMembers, Int32Proxy.Serialize);

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

		public void SendGetPlayersWithMatchingName(string search) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, search);

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

		public void SendChatMessageToAll(string message) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, message);

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

		public void SendChatMessageToPlayer(int cmid, string message) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, message);

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

		public void SendChatMessageToClan(List<int> clanMembers, string message) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<int>.Serialize(memoryStream, clanMembers, Int32Proxy.Serialize);
				StringProxy.Serialize(memoryStream, message);

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

		public void SendModerationMutePlayer(int durationInMinutes, int mutedCmid, bool disableChat) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, durationInMinutes);
				Int32Proxy.Serialize(memoryStream, mutedCmid);
				BooleanProxy.Serialize(memoryStream, disableChat);

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

		public void SendModerationPermanentBan(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

				var dictionary = new Dictionary<byte, object> {
					{
						__id, memoryStream.ToArray()
					}
				};

				if (sendOperation != null) {
					sendOperation(14, dictionary);
				}
			}
		}

		public void SendModerationBanPlayer(int cmid) {
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

		public void SendModerationKickGame(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendModerationUnbanPlayer(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendModerationCustomMessage(int cmid, string message) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, message);

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

		public void SendSpeedhackDetection() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendSpeedhackDetectionNew(List<float> timeDifferences) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<float>.Serialize(memoryStream, timeDifferences, SingleProxy.Serialize);

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

		public void SendPlayersReported(List<int> cmids, int type, string details, string logs) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<int>.Serialize(memoryStream, cmids, Int32Proxy.Serialize);
				Int32Proxy.Serialize(memoryStream, type);
				StringProxy.Serialize(memoryStream, details);
				StringProxy.Serialize(memoryStream, logs);

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

		public void SendUpdateNaughtyList() {
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

		public void SendClearModeratorFlags(int cmid) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);

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

		public void SendSetContactList(List<int> cmids) {
			using (var memoryStream = new MemoryStream()) {
				ListProxy<int>.Serialize(memoryStream, cmids, Int32Proxy.Serialize);

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

		public void SendUpdateAllActors() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendUpdateContacts() {
			using (var memoryStream = new MemoryStream()) {
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
	}
}
