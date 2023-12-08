using System;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class MemberAuthenticationViewModel {
		public MemberAuthenticationResult MemberAuthenticationResult { get; set; }
		public MemberView MemberView { get; set; }
	}
}
