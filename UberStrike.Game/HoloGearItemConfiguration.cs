using System;
using UberStrike.Core.Models.Views;
using UnityEngine;

[Serializable]
public class HoloGearItemConfiguration : UberStrikeItemGearView {
	[SerializeField]
	private AvatarDecorator _avatar;

	[SerializeField]
	private AvatarDecoratorConfig _ragdoll;

	public AvatarDecorator Avatar {
		get { return _avatar; }
	}

	public AvatarDecoratorConfig Ragdoll {
		get { return _ragdoll; }
	}
}
