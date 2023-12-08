using System;
using Cmune.DataCenter.Common.Entities;

namespace Cmune.Core.Models.Views {
	[Serializable]
	public class PhotonView {
		public int PhotonId { get; set; }
		public string IP { get; set; }
		public string Name { get; set; }
		public RegionType Region { get; set; }
		public int Port { get; set; }
		public PhotonUsageType UsageType { get; set; }
		public int MinLatency { get; set; }
	}
}
