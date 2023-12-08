using System.Collections.Generic;
using System.IO;
using Cmune.Core.Models;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client {
	public abstract class BaseGamePeer : BasePeer, IEventDispatcher {
		public GamePeerOperations Operations { get; private set; }

		protected BaseGamePeer(int syncFrequency, bool monitorTraffic = false) : base(syncFrequency, monitorTraffic) {
			Operations = new GamePeerOperations(1);
			AddRoomLogic(this, Operations);
		}

		public void OnEvent(byte id, byte[] data) {
			switch (id) {
				case 1:
					HeartbeatChallenge(data);

					break;
				case 2:
					RoomEntered(data);

					break;
				case 3:
					RoomEnterFailed(data);

					break;
				case 4:
					RequestPasswordForRoom(data);

					break;
				case 5:
					RoomLeft(data);

					break;
				case 6:
					FullGameList(data);

					break;
				case 7:
					GameListUpdate(data);

					break;
				case 8:
					GameListUpdateEnd(data);

					break;
				case 9:
					GetGameInformation(data);

					break;
				case 10:
					ServerLoadData(data);

					break;
				case 11:
					DisconnectAndDisablePhoton(data);

					break;
			}
		}

		protected abstract void OnHeartbeatChallenge(string challengeHash);
		protected abstract void OnRoomEntered(GameRoomData game);
		protected abstract void OnRoomEnterFailed(string server, int roomId, string message);
		protected abstract void OnRequestPasswordForRoom(string server, int roomId);
		protected abstract void OnRoomLeft();
		protected abstract void OnFullGameList(List<GameRoomData> gameList);
		protected abstract void OnGameListUpdate(List<GameRoomData> updatedGames, List<int> removedGames);
		protected abstract void OnGameListUpdateEnd();
		protected abstract void OnGetGameInformation(GameRoomData room, List<GameActorInfo> players, int endTime);
		protected abstract void OnServerLoadData(PhotonServerLoad data);
		protected abstract void OnDisconnectAndDisablePhoton(string message);

		private void HeartbeatChallenge(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				OnHeartbeatChallenge(text);
			}
		}

		private void RoomEntered(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var gameRoomData = GameRoomDataProxy.Deserialize(memoryStream);
				OnRoomEntered(gameRoomData);
			}
		}

		private void RoomEnterFailed(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				var num = Int32Proxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				OnRoomEnterFailed(text, num, text2);
			}
		}

		private void RequestPasswordForRoom(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				var num = Int32Proxy.Deserialize(memoryStream);
				OnRequestPasswordForRoom(text, num);
			}
		}

		private void RoomLeft(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnRoomLeft();
			}
		}

		private void FullGameList(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<GameRoomData>.Deserialize(memoryStream, GameRoomDataProxy.Deserialize);
				OnFullGameList(list);
			}
		}

		private void GameListUpdate(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<GameRoomData>.Deserialize(memoryStream, GameRoomDataProxy.Deserialize);
				var list2 = ListProxy<int>.Deserialize(memoryStream, Int32Proxy.Deserialize);
				OnGameListUpdate(list, list2);
			}
		}

		private void GameListUpdateEnd(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnGameListUpdateEnd();
			}
		}

		private void GetGameInformation(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var gameRoomData = GameRoomDataProxy.Deserialize(memoryStream);
				var list = ListProxy<GameActorInfo>.Deserialize(memoryStream, GameActorInfoProxy.Deserialize);
				var num = Int32Proxy.Deserialize(memoryStream);
				OnGetGameInformation(gameRoomData, list, num);
			}
		}

		private void ServerLoadData(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var photonServerLoad = PhotonServerLoadProxy.Deserialize(memoryStream);
				OnServerLoadData(photonServerLoad);
			}
		}

		private void DisconnectAndDisablePhoton(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				OnDisconnectAndDisablePhoton(text);
			}
		}
	}
}
