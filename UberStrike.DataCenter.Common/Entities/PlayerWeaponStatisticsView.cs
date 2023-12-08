using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class PlayerWeaponStatisticsView {
		public int MeleeTotalSplats { get; set; }
		public int HandgunTotalSplats { get; set; } // # LEGACY # //
		public int MachineGunTotalSplats { get; set; }
		public int ShotgunTotalSplats { get; set; }
		public int SniperTotalSplats { get; set; }
		public int SplattergunTotalSplats { get; set; }
		public int CannonTotalSplats { get; set; }
		public int LauncherTotalSplats { get; set; }
		public int MeleeTotalShotsFired { get; set; }
		public int MeleeTotalShotsHit { get; set; }
		public int MeleeTotalDamageDone { get; set; }
		public int HandgunTotalDamageDone { get; set; } // # LEGACY # //
		public int HandgunTotalShotsFired { get; set; } // # LEGACY # //
		public int HandgunTotalShotsHit { get; set; } // # LEGACY # //
		public int MachineGunTotalShotsFired { get; set; }
		public int MachineGunTotalShotsHit { get; set; }
		public int MachineGunTotalDamageDone { get; set; }
		public int ShotgunTotalShotsFired { get; set; }
		public int ShotgunTotalShotsHit { get; set; }
		public int ShotgunTotalDamageDone { get; set; }
		public int SniperTotalShotsFired { get; set; }
		public int SniperTotalShotsHit { get; set; }
		public int SniperTotalDamageDone { get; set; }
		public int SplattergunTotalShotsFired { get; set; }
		public int SplattergunTotalShotsHit { get; set; }
		public int SplattergunTotalDamageDone { get; set; }
		public int CannonTotalShotsFired { get; set; }
		public int CannonTotalShotsHit { get; set; }
		public int CannonTotalDamageDone { get; set; }
		public int LauncherTotalShotsFired { get; set; }
		public int LauncherTotalShotsHit { get; set; }
		public int LauncherTotalDamageDone { get; set; }
		public PlayerWeaponStatisticsView() { }

		public PlayerWeaponStatisticsView(int meleeTotalSplats, int machineGunTotalSplats, int shotgunTotalSplats, int sniperTotalSplats, int splattergunTotalSplats, int cannonTotalSplats, int launcherTotalSplats, int meleeTotalShotsFired, int meleeTotalShotsHit, int meleeTotalDamageDone, int machineGunTotalShotsFired, int machineGunTotalShotsHit, int machineGunTotalDamageDone, int shotgunTotalShotsFired, int shotgunTotalShotsHit, int shotgunTotalDamageDone, int sniperTotalShotsFired, int sniperTotalShotsHit, int sniperTotalDamageDone, int splattergunTotalShotsFired, int splattergunTotalShotsHit, int splattergunTotalDamageDone, int cannonTotalShotsFired, int cannonTotalShotsHit, int cannonTotalDamageDone, int launcherTotalShotsFired, int launcherTotalShotsHit, int launcherTotalDamageDone) {
			CannonTotalDamageDone = cannonTotalDamageDone;
			CannonTotalShotsFired = cannonTotalShotsFired;
			CannonTotalShotsHit = cannonTotalShotsHit;
			CannonTotalSplats = cannonTotalSplats;
			LauncherTotalDamageDone = launcherTotalDamageDone;
			LauncherTotalShotsFired = launcherTotalShotsFired;
			LauncherTotalShotsHit = launcherTotalShotsHit;
			LauncherTotalSplats = launcherTotalSplats;
			MachineGunTotalDamageDone = machineGunTotalDamageDone;
			MachineGunTotalShotsFired = machineGunTotalShotsFired;
			MachineGunTotalShotsHit = machineGunTotalShotsHit;
			MachineGunTotalSplats = machineGunTotalSplats;
			MeleeTotalDamageDone = meleeTotalDamageDone;
			MeleeTotalShotsFired = meleeTotalShotsFired;
			MeleeTotalShotsHit = meleeTotalShotsHit;
			MeleeTotalSplats = meleeTotalSplats;
			ShotgunTotalDamageDone = shotgunTotalDamageDone;
			ShotgunTotalShotsFired = shotgunTotalShotsFired;
			ShotgunTotalShotsHit = shotgunTotalShotsHit;
			ShotgunTotalSplats = shotgunTotalSplats;
			SniperTotalDamageDone = sniperTotalDamageDone;
			SniperTotalShotsFired = sniperTotalShotsFired;
			SniperTotalShotsHit = sniperTotalShotsHit;
			SniperTotalSplats = sniperTotalSplats;
			SplattergunTotalDamageDone = splattergunTotalDamageDone;
			SplattergunTotalShotsFired = splattergunTotalShotsFired;
			SplattergunTotalShotsHit = splattergunTotalShotsHit;
			SplattergunTotalSplats = splattergunTotalSplats;
		}

		public PlayerWeaponStatisticsView(int meleeTotalSplats, int handgunTotalSplats, int machineGunTotalSplats, int shotgunTotalSplats, int sniperTotalSplats, int splattergunTotalSplats, int cannonTotalSplats, int launcherTotalSplats, int meleeTotalShotsFired, int meleeTotalShotsHit, int meleeTotalDamageDone, int handgunTotalShotsFired, int handgunTotalShotsHit, int handgunTotalDamageDone, int machineGunTotalShotsFired, int machineGunTotalShotsHit, int machineGunTotalDamageDone, int shotgunTotalShotsFired, int shotgunTotalShotsHit, int shotgunTotalDamageDone, int sniperTotalShotsFired, int sniperTotalShotsHit, int sniperTotalDamageDone, int splattergunTotalShotsFired, int splattergunTotalShotsHit, int splattergunTotalDamageDone, int cannonTotalShotsFired, int cannonTotalShotsHit, int cannonTotalDamageDone, int launcherTotalShotsFired, int launcherTotalShotsHit, int launcherTotalDamageDone) {
			CannonTotalDamageDone = cannonTotalDamageDone;
			CannonTotalShotsFired = cannonTotalShotsFired;
			CannonTotalShotsHit = cannonTotalShotsHit;
			CannonTotalSplats = cannonTotalSplats;
			LauncherTotalDamageDone = launcherTotalDamageDone;
			LauncherTotalShotsFired = launcherTotalShotsFired;
			LauncherTotalShotsHit = launcherTotalShotsHit;
			LauncherTotalSplats = launcherTotalSplats;
			HandgunTotalDamageDone = handgunTotalDamageDone;
			HandgunTotalShotsFired = handgunTotalShotsFired;
			HandgunTotalShotsHit = handgunTotalShotsHit;
			HandgunTotalSplats = handgunTotalSplats;
			MachineGunTotalDamageDone = machineGunTotalDamageDone;
			MachineGunTotalShotsFired = machineGunTotalShotsFired;
			MachineGunTotalShotsHit = machineGunTotalShotsHit;
			MachineGunTotalSplats = machineGunTotalSplats;
			MeleeTotalDamageDone = meleeTotalDamageDone;
			MeleeTotalShotsFired = meleeTotalShotsFired;
			MeleeTotalShotsHit = meleeTotalShotsHit;
			MeleeTotalSplats = meleeTotalSplats;
			ShotgunTotalDamageDone = shotgunTotalDamageDone;
			ShotgunTotalShotsFired = shotgunTotalShotsFired;
			ShotgunTotalShotsHit = shotgunTotalShotsHit;
			ShotgunTotalSplats = shotgunTotalSplats;
			SniperTotalDamageDone = sniperTotalDamageDone;
			SniperTotalShotsFired = sniperTotalShotsFired;
			SniperTotalShotsHit = sniperTotalShotsHit;
			SniperTotalSplats = sniperTotalSplats;
			SplattergunTotalDamageDone = splattergunTotalDamageDone;
			SplattergunTotalShotsFired = splattergunTotalShotsFired;
			SplattergunTotalShotsHit = splattergunTotalShotsHit;
			SplattergunTotalSplats = splattergunTotalSplats;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[PlayerWeaponStatisticsView: ");
			stringBuilder.Append("[CannonTotalDamageDone: ");
			stringBuilder.Append(CannonTotalDamageDone);
			stringBuilder.Append("][CannonTotalShotsFired: ");
			stringBuilder.Append(CannonTotalShotsFired);
			stringBuilder.Append("][CannonTotalShotsHit: ");
			stringBuilder.Append(CannonTotalShotsHit);
			stringBuilder.Append("][CannonTotalSplats: ");
			stringBuilder.Append(CannonTotalSplats);
			stringBuilder.Append("][HandgunTotalDamageDone: ");
			stringBuilder.Append(HandgunTotalDamageDone);
			stringBuilder.Append("][HandgunTotalShotsFired: ");
			stringBuilder.Append(HandgunTotalShotsFired);
			stringBuilder.Append("][HandgunTotalShotsHit: ");
			stringBuilder.Append(HandgunTotalShotsHit);
			stringBuilder.Append("][HandgunTotalSplats: ");
			stringBuilder.Append(HandgunTotalSplats);
			stringBuilder.Append("][LauncherTotalDamageDone: ");
			stringBuilder.Append(LauncherTotalDamageDone);
			stringBuilder.Append("][LauncherTotalShotsFired: ");
			stringBuilder.Append(LauncherTotalShotsFired);
			stringBuilder.Append("][LauncherTotalShotsHit: ");
			stringBuilder.Append(LauncherTotalShotsHit);
			stringBuilder.Append("][LauncherTotalSplats: ");
			stringBuilder.Append(LauncherTotalSplats);
			stringBuilder.Append("][MachineGunTotalDamageDone: ");
			stringBuilder.Append(MachineGunTotalDamageDone);
			stringBuilder.Append("][MachineGunTotalShotsFired: ");
			stringBuilder.Append(MachineGunTotalShotsFired);
			stringBuilder.Append("][MachineGunTotalShotsHit: ");
			stringBuilder.Append(MachineGunTotalShotsHit);
			stringBuilder.Append("][MachineGunTotalSplats: ");
			stringBuilder.Append(MachineGunTotalSplats);
			stringBuilder.Append("][MeleeTotalDamageDone: ");
			stringBuilder.Append(MeleeTotalDamageDone);
			stringBuilder.Append("][MeleeTotalShotsFired: ");
			stringBuilder.Append(MeleeTotalShotsFired);
			stringBuilder.Append("][MeleeTotalShotsHit: ");
			stringBuilder.Append(MeleeTotalShotsHit);
			stringBuilder.Append("][MeleeTotalSplats: ");
			stringBuilder.Append(MeleeTotalSplats);
			stringBuilder.Append("][ShotgunTotalDamageDone: ");
			stringBuilder.Append(ShotgunTotalDamageDone);
			stringBuilder.Append("][ShotgunTotalShotsFired: ");
			stringBuilder.Append(ShotgunTotalShotsFired);
			stringBuilder.Append("][ShotgunTotalShotsHit: ");
			stringBuilder.Append(ShotgunTotalShotsHit);
			stringBuilder.Append("][ShotgunTotalSplats: ");
			stringBuilder.Append(ShotgunTotalSplats);
			stringBuilder.Append("][SniperTotalDamageDone: ");
			stringBuilder.Append(SniperTotalDamageDone);
			stringBuilder.Append("][SniperTotalShotsFired: ");
			stringBuilder.Append(SniperTotalShotsFired);
			stringBuilder.Append("][SniperTotalShotsHit: ");
			stringBuilder.Append(SniperTotalShotsHit);
			stringBuilder.Append("][SniperTotalSplats: ");
			stringBuilder.Append(SniperTotalSplats);
			stringBuilder.Append("][SplattergunTotalDamageDone: ");
			stringBuilder.Append(SplattergunTotalDamageDone);
			stringBuilder.Append("][SplattergunTotalShotsFired: ");
			stringBuilder.Append(SplattergunTotalShotsFired);
			stringBuilder.Append("][SplattergunTotalShotsHit: ");
			stringBuilder.Append(SplattergunTotalShotsHit);
			stringBuilder.Append("][SplattergunTotalSplats: ");
			stringBuilder.Append(SplattergunTotalSplats);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
