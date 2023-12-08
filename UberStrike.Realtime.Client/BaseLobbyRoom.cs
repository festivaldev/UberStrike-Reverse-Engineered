using System.Collections.Generic;
using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client {
	public abstract class BaseLobbyRoom : IEventDispatcher, IRoomLogic {
		public LobbyRoomOperations Operations { get; private set; }

		protected BaseLobbyRoom() {
			Operations = new LobbyRoomOperations();
		}

		public void OnEvent(byte id, byte[] data) {
			switch (id) {
				case 5:
					PlayerHide(data);

					break;
				case 6:
					PlayerLeft(data);

					break;
				case 7:
					PlayerUpdate(data);

					break;
				case 8:
					UpdateContacts(data);

					break;
				case 9:
					FullPlayerListUpdate(data);

					break;
				case 10:
					PlayerJoined(data);

					break;
				case 11:
					ClanChatMessage(data);

					break;
				case 12:
					InGameChatMessage(data);

					break;
				case 13:
					LobbyChatMessage(data);

					break;
				case 14:
					PrivateChatMessage(data);

					break;
				case 15:
					UpdateInboxRequests(data);

					break;
				case 16:
					UpdateFriendsList(data);

					break;
				case 17:
					UpdateInboxMessages(data);

					break;
				case 18:
					UpdateClanMembers(data);

					break;
				case 19:
					UpdateClanData(data);

					break;
				case 20:
					UpdateActorsForModeration(data);

					break;
				case 21:
					ModerationCustomMessage(data);

					break;
				case 22:
					ModerationMutePlayer(data);

					break;
				case 23:
					ModerationKickGame(data);

					break;
			}
		}

		IOperationSender IRoomLogic.Operations {
			get { return Operations; }
		}

		protected abstract void OnPlayerHide(int cmid);
		protected abstract void OnPlayerLeft(int cmid, bool refreshComm);
		protected abstract void OnPlayerUpdate(CommActorInfo data);
		protected abstract void OnUpdateContacts(List<CommActorInfo> updated, List<int> removed);
		protected abstract void OnFullPlayerListUpdate(List<CommActorInfo> players);
		protected abstract void OnPlayerJoined(CommActorInfo data);
		protected abstract void OnClanChatMessage(int cmid, string name, string message);
		protected abstract void OnInGameChatMessage(int cmid, string name, string message, MemberAccessLevel accessLevel, byte context);
		protected abstract void OnLobbyChatMessage(int cmid, string name, string message);
		protected abstract void OnPrivateChatMessage(int cmid, string name, string message);
		protected abstract void OnUpdateInboxRequests();
		protected abstract void OnUpdateFriendsList();
		protected abstract void OnUpdateInboxMessages(int messageId);
		protected abstract void OnUpdateClanMembers();
		protected abstract void OnUpdateClanData();
		protected abstract void OnUpdateActorsForModeration(List<CommActorInfo> allHackers);
		protected abstract void OnModerationCustomMessage(string message);
		protected abstract void OnModerationMutePlayer(bool isPlayerMuted);
		protected abstract void OnModerationKickGame();

		private void PlayerHide(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnPlayerHide(num);
			}
		}

		private void PlayerLeft(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnPlayerLeft(num, flag);
			}
		}

		private void PlayerUpdate(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var commActorInfo = CommActorInfoProxy.Deserialize(memoryStream);
				OnPlayerUpdate(commActorInfo);
			}
		}

		private void UpdateContacts(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<CommActorInfo>.Deserialize(memoryStream, CommActorInfoProxy.Deserialize);
				var list2 = ListProxy<int>.Deserialize(memoryStream, Int32Proxy.Deserialize);
				OnUpdateContacts(list, list2);
			}
		}

		private void FullPlayerListUpdate(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<CommActorInfo>.Deserialize(memoryStream, CommActorInfoProxy.Deserialize);
				OnFullPlayerListUpdate(list);
			}
		}

		private void PlayerJoined(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var commActorInfo = CommActorInfoProxy.Deserialize(memoryStream);
				OnPlayerJoined(commActorInfo);
			}
		}

		private void ClanChatMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var text = StringProxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				OnClanChatMessage(num, text, text2);
			}
		}

		private void InGameChatMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var text = StringProxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				var memberAccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(memoryStream);
				var b = ByteProxy.Deserialize(memoryStream);
				OnInGameChatMessage(num, text, text2, memberAccessLevel, b);
			}
		}

		private void LobbyChatMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var text = StringProxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				OnLobbyChatMessage(num, text, text2);
			}
		}

		private void PrivateChatMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				var text = StringProxy.Deserialize(memoryStream);
				var text2 = StringProxy.Deserialize(memoryStream);
				OnPrivateChatMessage(num, text, text2);
			}
		}

		private void UpdateInboxRequests(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnUpdateInboxRequests();
			}
		}

		private void UpdateFriendsList(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnUpdateFriendsList();
			}
		}

		private void UpdateInboxMessages(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var num = Int32Proxy.Deserialize(memoryStream);
				OnUpdateInboxMessages(num);
			}
		}

		private void UpdateClanMembers(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnUpdateClanMembers();
			}
		}

		private void UpdateClanData(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnUpdateClanData();
			}
		}

		private void UpdateActorsForModeration(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var list = ListProxy<CommActorInfo>.Deserialize(memoryStream, CommActorInfoProxy.Deserialize);
				OnUpdateActorsForModeration(list);
			}
		}

		private void ModerationCustomMessage(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var text = StringProxy.Deserialize(memoryStream);
				OnModerationCustomMessage(text);
			}
		}

		private void ModerationMutePlayer(byte[] _bytes) {
			using (var memoryStream = new MemoryStream(_bytes)) {
				var flag = BooleanProxy.Deserialize(memoryStream);
				OnModerationMutePlayer(flag);
			}
		}

		private void ModerationKickGame(byte[] _bytes) {
			using (new MemoryStream(_bytes)) {
				OnModerationKickGame();
			}
		}
	}
}
