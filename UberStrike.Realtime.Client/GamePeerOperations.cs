using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client {
	public sealed class GamePeerOperations : IOperationSender {
		private byte __id;
		private RemoteProcedureCall sendOperation;

		public GamePeerOperations(byte id = 0) {
			__id = id;
		}

		public event RemoteProcedureCall SendOperation {
			add { sendOperation = (RemoteProcedureCall)Delegate.Combine(sendOperation, value); }
			remove { sendOperation = (RemoteProcedureCall)Delegate.Remove(sendOperation, value); }
		}

		public void SendSendHeartbeatResponse(string authToken, string responseHash) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, responseHash);

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

		public void SendGetServerLoad() {
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

		public void SendGetGameInformation(int number) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, number);

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

		public void SendGetGameListUpdates() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendEnterRoom(int roomId, string password, string clientVersion, string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, password);
				StringProxy.Serialize(memoryStream, clientVersion);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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

		public void SendCreateRoom(GameRoomData metaData, string password, string clientVersion, string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				GameRoomDataProxy.Serialize(memoryStream, metaData);
				StringProxy.Serialize(memoryStream, password);
				StringProxy.Serialize(memoryStream, clientVersion);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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

		public void SendLeaveRoom() {
			using (var memoryStream = new MemoryStream()) {
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

		public void SendCloseRoom(int roomId, string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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

		public void SendInspectRoom(int roomId, string authToken) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, authToken);

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

		public void SendReportPlayer(int cmid, string authToken) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, authToken);

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

		public void SendKickPlayer(int cmid, string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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

		public void SendUpdateLoadout() {
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

		public void SendUpdatePing(ushort ping) {
			using (var memoryStream = new MemoryStream()) {
				UInt16Proxy.Serialize(memoryStream, ping);

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

		public void SendUpdateKeyState(byte state) {
			using (var memoryStream = new MemoryStream()) {
				ByteProxy.Serialize(memoryStream, state);

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

		public void SendRefreshBackendData(string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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
	}
}
