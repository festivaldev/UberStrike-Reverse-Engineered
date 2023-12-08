using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class GameRoomProxy {
		public static void Serialize(Stream stream, GameRoom instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.MapId);
				Int32Proxy.Serialize(memoryStream, instance.Number);

				if (instance.Server != null) {
					ConnectionAddressProxy.Serialize(memoryStream, instance.Server);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static GameRoom Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var gameRoom = new GameRoom();
			gameRoom.MapId = Int32Proxy.Deserialize(bytes);
			gameRoom.Number = Int32Proxy.Deserialize(bytes);

			if ((num & 1) != 0) {
				gameRoom.Server = ConnectionAddressProxy.Deserialize(bytes);
			}

			return gameRoom;
		}
	}
}
