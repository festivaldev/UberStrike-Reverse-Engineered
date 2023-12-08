using System;
using UnityEngine;

[Serializable]
public class ExplosiveGrenadeConfiguration : QuickItemConfiguration {
	[CustomProperty("Bounciness")]
	[SerializeField]
	private int _bounciness = 3;

	[CustomProperty("Damage")]
	[SerializeField]
	private int _damage = 100;

	[CustomProperty("Sticky")]
	[SerializeField]
	private bool _isSticky = true;

	[CustomProperty("LifeTime")]
	[SerializeField]
	private int _lifeTime = 15;

	[SerializeField]
	private int _speed = 15;

	[CustomProperty("SplashRadius")]
	[SerializeField]
	private int _splash = 2;

	public int Damage {
		get { return _damage; }
	}

	public int SplashRadius {
		get { return _splash; }
	}

	public int LifeTime {
		get { return _lifeTime; }
	}

	public float Bounciness {
		get { return _bounciness * 0.1f; }
	}

	public bool IsSticky {
		get { return _isSticky; }
	}

	public int Speed {
		get { return _speed; }
	}
}
