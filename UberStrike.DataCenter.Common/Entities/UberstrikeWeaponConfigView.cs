using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeWeaponConfigView {
		public int DamageKnockback { get; set; }
		public int DamagePerProjectile { get; set; }
		public int AccuracySpread { get; set; }
		public int RecoilKickback { get; set; }
		public int StartAmmo { get; set; }
		public int MaxAmmo { get; set; }
		public int MissileTimeToDetonate { get; set; }
		public int MissileForceImpulse { get; set; }
		public int MissileBounciness { get; set; }
		public int SplashRadius { get; set; }
		public int ProjectilesPerShot { get; set; }
		public int ProjectileSpeed { get; set; }
		public int RateOfFire { get; set; }
		public int RecoilMovement { get; set; }
		public int LevelRequired { get; set; }
		public UberstrikeWeaponConfigView() { }

		public UberstrikeWeaponConfigView(int damageKnockback, int damagePerProjectile, int accuracySpread, int recoilKickback, int startAmmo, int maxAmmo, int missileTimeToDetonate, int missileForceImpulse, int missileBounciness, int rateOfire, int splashRadius, int projectilesPerShot, int projectileSpeed, int recoilMovement) {
			DamageKnockback = damageKnockback;
			DamagePerProjectile = damagePerProjectile;
			AccuracySpread = accuracySpread;
			RecoilKickback = recoilKickback;
			StartAmmo = startAmmo;
			MaxAmmo = maxAmmo;
			MissileTimeToDetonate = missileTimeToDetonate;
			MissileForceImpulse = missileForceImpulse;
			MissileBounciness = missileBounciness;
			SplashRadius = splashRadius;
			ProjectilesPerShot = projectilesPerShot;
			ProjectileSpeed = projectileSpeed;
			RateOfFire = rateOfire;
			RecoilMovement = recoilMovement;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeWeaponConfigView: [DamageKnockback: ");
			stringBuilder.Append(DamageKnockback);
			stringBuilder.Append("][DamagePerProjectile: ");
			stringBuilder.Append(DamagePerProjectile);
			stringBuilder.Append("][AccuracySpread: ");
			stringBuilder.Append(AccuracySpread);
			stringBuilder.Append("][RecoilKickback: ");
			stringBuilder.Append(RecoilKickback);
			stringBuilder.Append("][StartAmmo: ");
			stringBuilder.Append(StartAmmo);
			stringBuilder.Append("][MaxAmmo: ");
			stringBuilder.Append(MaxAmmo);
			stringBuilder.Append("][MissileTimeToDetonate: ");
			stringBuilder.Append(MissileTimeToDetonate);
			stringBuilder.Append("][MissileForceImpulse: ");
			stringBuilder.Append(MissileForceImpulse);
			stringBuilder.Append("][MissileBounciness: ");
			stringBuilder.Append(MissileBounciness);
			stringBuilder.Append("][RateOfFire: ");
			stringBuilder.Append(RateOfFire);
			stringBuilder.Append("][SplashRadius: ");
			stringBuilder.Append(SplashRadius);
			stringBuilder.Append("][ProjectilesPerShot: ");
			stringBuilder.Append(ProjectilesPerShot);
			stringBuilder.Append("][ProjectileSpeed: ");
			stringBuilder.Append(ProjectileSpeed);
			stringBuilder.Append("][RecoilMovement: ");
			stringBuilder.Append(RecoilMovement);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
