using System;
using System.Collections.Generic;
using Cmune.Core.Models.Views;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class GameApplicationView {
		public string Version { get; set; }
		public List<PhotonView> GameServers { get; set; }
		public PhotonView CommServer { get; set; }
		public string SupportUrl { get; set; }
		public string EncryptionInitVector { get; set; }
		public string EncryptionPassPhrase { get; set; }
	}
}
