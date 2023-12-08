using System;
using System.Collections.Generic;
using Cmune.Core.Models.Views;

namespace Cmune.DataCenter.Common.Entities {
	[Serializable]
	public class ApplicationView {
		public int ApplicationVersionId { get; set; }
		public string Version { get; set; }
		public BuildType Build { get; set; }
		public ChannelType Channel { get; set; }
		public string FileName { get; set; }
		public DateTime ReleaseDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public int RemainingTime { get; set; }
		public bool IsCurrent { get; set; }
		public List<PhotonView> Servers { get; set; }
		public string SupportUrl { get; set; }
		public int PhotonGroupId { get; set; }
		public string PhotonGroupName { get; set; }

		public ApplicationView() {
			Servers = new List<PhotonView>();
		}

		public ApplicationView(string version, BuildType build, ChannelType channel) {
			Version = version;
			Build = build;
			Channel = channel;
			Servers = new List<PhotonView>();
		}

		public ApplicationView(int applicationVersionId, string version, BuildType build, ChannelType channel, string fileName, DateTime releaseDate, DateTime? expirationDate, bool isCurrent, string supportUrl, int photonGroupId, List<PhotonView> servers) {
			var num = -1;

			if (expirationDate != null && expirationDate != null) {
				var value = expirationDate.Value;

				if (value.CompareTo(DateTime.UtcNow) <= 0) {
					num = 0;
				} else {
					num = (int)Math.Floor(DateTime.UtcNow.Subtract(value).TotalMinutes);
				}
			}

			SetApplication(applicationVersionId, version, build, channel, fileName, releaseDate, expirationDate, num, isCurrent, supportUrl, photonGroupId, servers);
		}

		private void SetApplication(int applicationVersionID, string version, BuildType build, ChannelType channel, string fileName, DateTime releaseDate, DateTime? expirationDate, int remainingTime, bool isCurrent, string supportUrl, int photonGroupId, List<PhotonView> servers) {
			ApplicationVersionId = applicationVersionID;
			Version = version;
			Build = build;
			Channel = channel;
			FileName = fileName;
			ReleaseDate = releaseDate;
			ExpirationDate = expirationDate;
			RemainingTime = remainingTime;
			IsCurrent = isCurrent;
			SupportUrl = supportUrl;
			PhotonGroupId = photonGroupId;

			if (servers != null) {
				Servers = servers;
			} else {
				Servers = new List<PhotonView>();
			}
		}

		public override string ToString() {
			var text = "[Application: ";
			var text2 = text;
			text = string.Concat(text2, "[ID: ", ApplicationVersionId, "][version: ", Version, "][Build: ", Build, "][Channel: ", Channel, "][File name: ", FileName, "][Release date: ", ReleaseDate, "][Expiration date: ", ExpirationDate, "][Remaining time: ", RemainingTime, "][Is current: ", IsCurrent, "][Support URL: ", SupportUrl, "]");
			text += "[Servers]";

			foreach (var photonView in Servers) {
				text += photonView.ToString();
			}

			text += "[/Servers]]";
			text += "]";

			return text;
		}
	}
}
