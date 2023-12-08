using System;
using System.Collections.Generic;
using Cmune.Core.Models.Views;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class AuthenticateApplicationView {
		public List<PhotonView> GameServers { get; set; }
		public PhotonView CommServer { get; set; }
		public bool WarnPlayer { get; set; }
		public bool IsEnabled { get; set; }
		public string EncryptionInitVector { get; set; }
		public string EncryptionPassPhrase { get; set; }
	}
}
