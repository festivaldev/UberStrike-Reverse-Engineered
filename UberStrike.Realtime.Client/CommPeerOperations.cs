using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client {
	public sealed class CommPeerOperations : IOperationSender {
		private byte __id;
		private RemoteProcedureCall sendOperation;

		public CommPeerOperations(byte id = 0) {
			__id = id;
		}

		public event RemoteProcedureCall SendOperation {
			add { sendOperation = (RemoteProcedureCall)Delegate.Combine(sendOperation, value); }
			remove { sendOperation = (RemoteProcedureCall)Delegate.Remove(sendOperation, value); }
		}

		public void SendAuthenticationRequest(string authToken, string magicHash) {
			using (var memoryStream = new MemoryStream()) {
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);

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
					sendOperation(2, dictionary);
				}
			}
		}
	}
}
