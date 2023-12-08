using System;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Core.Models.Views {
	[Serializable]
	public class UberStrikeItemWeaponView : BaseUberStrikeItemView {
		[SerializeField]
		private int _accuracySpread;

		[SerializeField]
		private int _combatRange;

		private int _criticalStrikeBonus;

		[SerializeField]
		private int _damageKnockback;

		[SerializeField]
		private int _damagePerProjectile;

		[SerializeField]
		private int _defaultZoomMultiplier;

		[SerializeField]
		private bool _hasAutoFire;

		[SerializeField]
		private int _maxAmmo;

		[SerializeField]
		private int _maxZoomMultiplier;

		[SerializeField]
		private int _minZoomMultiplier;

		[SerializeField]
		private int _missileBounciness;

		[SerializeField]
		private int _missileForceImpulse;

		[SerializeField]
		private int _missileTimeToDetonate;

		[SerializeField]
		private int _projectileSpeed;

		[SerializeField]
		private int _projectilesPerShot;

		[SerializeField]
		private int _rateOfFire;

		[SerializeField]
		private int _recoilKickback;

		[SerializeField]
		private int _recoilMovement;

		[SerializeField]
		private int _secondaryActionReticle;

		[SerializeField]
		private int _splashRadius;

		[SerializeField]
		private int _startAmmo;

		[SerializeField]
		private int _tier;

		[SerializeField]
		private int _weaponSecondaryAction;

		public override UberstrikeItemType ItemType {
			get { return UberstrikeItemType.Weapon; }
		}

		public int DamageKnockback {
			get { return _damageKnockback; }
			set { _damageKnockback = value; }
		}

		public int DamagePerProjectile {
			get { return _damagePerProjectile; }
			set { _damagePerProjectile = value; }
		}

		public int AccuracySpread {
			get { return _accuracySpread; }
			set { _accuracySpread = value; }
		}

		public int RecoilKickback {
			get { return _recoilKickback; }
			set { _recoilKickback = value; }
		}

		public int StartAmmo {
			get { return _startAmmo; }
			set { _startAmmo = value; }
		}

		public int MaxAmmo {
			get { return _maxAmmo; }
			set { _maxAmmo = value; }
		}

		public int MissileTimeToDetonate {
			get { return _missileTimeToDetonate; }
			set { _missileTimeToDetonate = value; }
		}

		public int MissileForceImpulse {
			get { return _missileForceImpulse; }
			set { _missileForceImpulse = value; }
		}

		public int MissileBounciness {
			get { return _missileBounciness; }
			set { _missileBounciness = value; }
		}

		public int SplashRadius {
			get { return _splashRadius; }
			set { _splashRadius = value; }
		}

		public int ProjectilesPerShot {
			get { return _projectilesPerShot; }
			set { _projectilesPerShot = value; }
		}

		public int ProjectileSpeed {
			get { return _projectileSpeed; }
			set { _projectileSpeed = value; }
		}

		public int RateOfFire {
			get { return _rateOfFire; }
			set { _rateOfFire = value; }
		}

		public int RecoilMovement {
			get { return _recoilMovement; }
			set { _recoilMovement = value; }
		}

		public int CombatRange {
			get { return _combatRange; }
			set { _combatRange = value; }
		}

		public int Tier {
			get { return _tier; }
			set { _tier = value; }
		}

		public int SecondaryActionReticle {
			get { return _secondaryActionReticle; }
			set { _secondaryActionReticle = value; }
		}

		public int WeaponSecondaryAction {
			get { return _weaponSecondaryAction; }
			set { _weaponSecondaryAction = value; }
		}

		public int CriticalStrikeBonus {
			get { return _criticalStrikeBonus; }
			set { _criticalStrikeBonus = value; }
		}

		public float DamagePerSecond {
			get { return (RateOfFire == 0) ? 0 : (DamagePerProjectile * ProjectilesPerShot / RateOfFire); }
		}

		public bool HasAutomaticFire {
			get { return _hasAutoFire; }
			set { _hasAutoFire = value; }
		}

		public int DefaultZoomMultiplier {
			get { return _defaultZoomMultiplier; }
			set { _defaultZoomMultiplier = value; }
		}

		public int MinZoomMultiplier {
			get { return _minZoomMultiplier; }
			set { _minZoomMultiplier = value; }
		}

		public int MaxZoomMultiplier {
			get { return _maxZoomMultiplier; }
			set { _maxZoomMultiplier = value; }
		}
	}
}
