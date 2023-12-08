using System;
using Cmune.DataCenter.Common.Entities;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class UberstrikeUserViewModel {
		public MemberView CmuneMemberView { get; set; }
		public UberstrikeMemberView UberstrikeMemberView { get; set; }
	}
}
