using System;
using UnityEngine;

[Serializable]
public class SpringGrenadeConfiguration : QuickItemConfiguration {
	[CustomProperty("Force")]
	[SerializeField]
	private int _force = 1250;

	[CustomProperty("Sticky")]
	[SerializeField]
	private bool _isSticky = true;

	[SerializeField]
	private Vector3 _jumpDirection = Vector3.up;

	[CustomProperty("LifeTime")]
	[SerializeField]
	private int _lifeTime = 15;

	[SerializeField]
	private int _speed = 10;

	public Vector3 JumpDirection {
		get { return _jumpDirection; }
	}

	public int Force {
		get { return _force; }
	}

	public int LifeTime {
		get { return _lifeTime; }
	}

	public bool IsSticky {
		get { return _isSticky; }
	}

	public int Speed {
		get { return _speed; }
	}
}
