using System;

namespace UberStrike.Core.Models {
	[AttributeUsage(AttributeTargets.Property)]
	public class NonSynchronizableAttribute : Attribute { }
}
