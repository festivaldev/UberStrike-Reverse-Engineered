using System.IO;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization {
	public static class PointDepositViewProxy {
		public static void Serialize(Stream stream, PointDepositView instance) {
			using (var memoryStream = new MemoryStream()) {
				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				DateTimeProxy.Serialize(memoryStream, instance.DepositDate);
				EnumProxy<PointsDepositType>.Serialize(memoryStream, instance.DepositType);
				BooleanProxy.Serialize(memoryStream, instance.IsAdminAction);
				Int32Proxy.Serialize(memoryStream, instance.PointDepositId);
				Int32Proxy.Serialize(memoryStream, instance.Points);
				memoryStream.WriteTo(stream);
			}
		}

		public static PointDepositView Deserialize(Stream bytes) {
			return new PointDepositView {
				Cmid = Int32Proxy.Deserialize(bytes),
				DepositDate = DateTimeProxy.Deserialize(bytes),
				DepositType = EnumProxy<PointsDepositType>.Deserialize(bytes),
				IsAdminAction = BooleanProxy.Deserialize(bytes),
				PointDepositId = Int32Proxy.Deserialize(bytes),
				Points = Int32Proxy.Deserialize(bytes)
			};
		}
	}
}
