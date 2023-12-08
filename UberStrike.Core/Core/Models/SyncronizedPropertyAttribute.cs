using System;

namespace UberStrike.Core.Models {
	[AttributeUsage(AttributeTargets.Property)]
	public class SyncronizedPropertyAttribute : Attribute {
		public int ID { get; private set; }

		public SyncronizedPropertyAttribute(int id) {
			ID = id;
		}
	}
}
