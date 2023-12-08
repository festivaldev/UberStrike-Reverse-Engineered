using System;
using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel {
	[Serializable]
	public class RegisterClientApplicationViewModel {
		public ApplicationRegistrationResult Result { get; set; }
		public ICollection<int> ItemsAttributed { get; set; }
	}
}
