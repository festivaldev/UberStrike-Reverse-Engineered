using System.Collections.Generic;

namespace Cmune.Core.Models.Views {
	public class PhotonsClusterView {
		public int PhotonsClusterId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<PhotonView> Photons { get; set; }

		public PhotonsClusterView(int photonsClusterId, string name, string description, List<PhotonView> photons) {
			PhotonsClusterId = photonsClusterId;
			Name = name;
			Description = description;
			Photons = photons;
		}

		public PhotonsClusterView(int photonsClusterId, string name, List<PhotonView> photons) : this(photonsClusterId, name, string.Empty, photons) { }
	}
}
