using System;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class GameActions {
	public Action<QuickItemLogic, int, int, bool> ActivateQuickItem;
	public Action ChangeTeam;
	public Action<string, byte> ChatMessage;
	public Action<int, ushort, BodyPart, Vector3, byte, byte> DirectHitDamage;
	public Action<Vector3, Vector3, LoadoutSlotType, int, bool> EmitProjectile;
	public Action<Vector3, Vector3, int, byte, int> EmitQuickItem;
	public Action<int, ushort, Vector3, byte, byte> ExplosionHitDamage;
	public Action<int, int> IncreaseHealthAndArmor;
	public Action JoinAsSpectator;
	public Action<TeamID> JoinTeam;
	public Action<int> KickPlayer;
	public Action KillPlayer;
	public Action<int> OpenDoor;
	public Action<int, PickupItemType, int> PickupPowerup;
	public Action<int, Vector3> PlayerHitFeeback;
	public Action<int, bool> RemoveProjectile;
	public Action RequestRespawn;
	public Action SingleBulletFire;

	public void Clear() {
		JoinTeam = delegate { };
		ChangeTeam = delegate { };
		RequestRespawn = delegate { };
		PickupPowerup = delegate { };
		OpenDoor = delegate { };
		SingleBulletFire = delegate { };
		EmitProjectile = delegate { };
		RemoveProjectile = delegate { };
		DirectHitDamage = delegate { };
		ExplosionHitDamage = delegate { };
		PlayerHitFeeback = delegate { };
		EmitQuickItem = delegate { };
		ActivateQuickItem = delegate { };
		IncreaseHealthAndArmor = delegate { };
		JoinAsSpectator = delegate { };
		KickPlayer = delegate { };
		ChatMessage = delegate { };
		KillPlayer = delegate { };
	}
}
