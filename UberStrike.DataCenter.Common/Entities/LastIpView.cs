using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.DataCenter.Common.Entities {
	public class LastIpView {
		public long Ip { get; private set; }
		public DateTime LastConnectionDate { get; private set; }
		public List<LinkedMemberView> LinkedMembers { get; private set; }
		public BannedIpView BannedIpView { get; private set; }

		public LastIpView(long ip, DateTime lastConnectionDate, List<LinkedMemberView> linkedMembers, BannedIpView bannedIpView) {
			Ip = ip;
			LastConnectionDate = lastConnectionDate;
			LinkedMembers = linkedMembers;
			BannedIpView = bannedIpView;
		}
	}
}
