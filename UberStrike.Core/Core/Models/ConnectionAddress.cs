using System;

namespace UberStrike.Core.Models {
	[Serializable]
	public class ConnectionAddress {
		public int Ipv4 { get; set; }
		public ushort Port { get; set; }

		public string ConnectionString {
			get { return string.Format("{0}:{1}", ToString(Ipv4), Port); }
		}

		public string IpAddress {
			get { return ToString(Ipv4); }
		}

		public ConnectionAddress() { }

		public ConnectionAddress(string connection) {
			try {
				var array = connection.Split(':');
				Ipv4 = ToInteger(array[0]);
				Port = ushort.Parse(array[1]);
			} catch { }
		}

		public ConnectionAddress(string ipAddress, ushort port) {
			Ipv4 = ToInteger(ipAddress);
			Port = port;
		}

		public static string ToString(int ipv4) {
			return string.Format("{0}.{1}.{2}.{3}", (ipv4 >> 24) & 255, (ipv4 >> 16) & 255, (ipv4 >> 8) & 255, ipv4 & 255);
		}

		public static int ToInteger(string ipAddress) {
			var num = 0;
			var array = ipAddress.Split('.');

			if (array.Length == 4) {
				for (var i = 0; i < array.Length; i++) {
					num |= int.Parse(array[i]) << (3 - i) * 8;
				}
			}

			return num;
		}
	}
}
