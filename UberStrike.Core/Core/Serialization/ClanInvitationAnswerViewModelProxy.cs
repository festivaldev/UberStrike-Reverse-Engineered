using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization {
	public static class ClanInvitationAnswerViewModelProxy {
		public static void Serialize(Stream stream, ClanInvitationAnswerViewModel instance) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.GroupInvitationId);
				BooleanProxy.Serialize(memoryStream, instance.IsInvitationAccepted);
				Int32Proxy.Serialize(memoryStream, instance.ReturnValue);
				memoryStream.WriteTo(stream);
			}
		}

		public static ClanInvitationAnswerViewModel Deserialize(Stream bytes) {
			return new ClanInvitationAnswerViewModel {
				GroupInvitationId = Int32Proxy.Deserialize(bytes),
				IsInvitationAccepted = BooleanProxy.Deserialize(bytes),
				ReturnValue = Int32Proxy.Deserialize(bytes)
			};
		}
	}
}
