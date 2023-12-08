using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerData : ICharacterState {
	public static readonly Vector3 StandingOffset = new Vector3(0f, 0.65f, 0f);
	public static readonly Vector3 CrouchingOffset = new Vector3(0f, 0.1f, 0f);
	public readonly PlayerActions Actions = new PlayerActions();
	public Property<WeaponSlot> ActiveWeapon = new Property<WeaponSlot>();
	public IntegerProperty Ammo = new IntegerProperty(0, 0);
	public Property<DamageInfo> AppliedDamage = new Property<DamageInfo>();
	public IntegerProperty ArmorCarried = new IntegerProperty(0, 0, 200);
	public IntegerProperty ArmorPoints = new IntegerProperty(0, 0, 200);
	public IntegerProperty BlueTeamScore = new IntegerProperty(0, 0);
	private MovementUpdateCache cache = new MovementUpdateCache();
	public Property<TeamID> FocusedPlayerTeam = new Property<TeamID>(TeamID.NONE);
	public IntegerProperty Health = new IntegerProperty(0, 0, 200);
	public Property<bool> IsIronSighted = new Property<bool>(false);
	public Property<bool> IsZoomedIn = new Property<bool>(false);
	public Property<Dictionary<LoadoutSlotType, IUnityItem>> LoadoutWeapons = new Property<Dictionary<LoadoutSlotType, IUnityItem>>();
	public Property<WeaponSlot> NextActiveWeapon = new Property<WeaponSlot>();
	private float posSyncFrame;
	public IntegerProperty RedTeamScore = new IntegerProperty(0, 0);
	public IntegerProperty RemainingKills = new IntegerProperty(0, 0);
	public IntegerProperty RemainingTime = new IntegerProperty(0, 0);
	private float shotSyncFrame;
	public Property<TeamID> Team = new Property<TeamID>(TeamID.NONE);
	public Property<WeaponSlot> WeaponFired = new Property<WeaponSlot>();

	public int ArmorPointCapacity {
		get { return Player.ArmorPointCapacity; }
	}

	public PlayerStates PlayerState {
		get { return Player.PlayerState; }
	}

	public bool IsOnline {
		get { return !Is(PlayerStates.Offline); }
	}

	public bool IsAlive {
		get { return (byte)(PlayerState & PlayerStates.Dead) == 0; }
	}

	public bool IsSpectator {
		get { return (byte)(PlayerState & PlayerStates.Spectator) != 0; }
	}

	public bool IsUnderWater {
		get { return (byte)(MovementState & (MoveStates.Swimming | MoveStates.Diving)) != 0; }
	}

	public Vector3 CurrentOffset {
		get { return ((byte)(MovementState & MoveStates.Ducked) != 0) ? CrouchingOffset : StandingOffset; }
	}

	public Vector3 ShootingPoint {
		get { return GameState.Current.Player.transform.position + CurrentOffset; }
	}

	public Vector3 ShootingDirection {
		get { return UserInput.Rotation * Vector3.forward; }
	}

	public PlayerData() {
		Player = new GameActorInfo();
		Health.AddEvent(delegate(int el) { Player.Health = (short)el; }, null);
		ArmorPoints.AddEvent(delegate(int el) { Player.ArmorPoints = (byte)el; }, null);
	}

	public MoveStates MovementState { get; set; }
	public KeyState KeyState { get; private set; }
	public Vector3 Velocity { get; set; }

	public Vector3 Position {
		get { return (!(GameState.Current.Player != null)) ? Vector3.zero : GameState.Current.Player.transform.position; }
	}

	public Quaternion HorizontalRotation {
		get { return UserInput.HorizontalRotation; }
	}

	public float VerticalRotation {
		get { return UserInput.VerticalRotation.eulerAngles.x; }
	}

	public GameActorInfo Player { get; set; }
	public event Action<GameActorInfoDelta> OnDeltaUpdate = delegate { };
	public event Action<PlayerMovement> OnPositionUpdate = delegate { };

	public bool Is(MoveStates state) {
		return (byte)(MovementState & state) != 0;
	}

	public void DeltaUpdate(GameActorInfoDelta delta) {
		foreach (var keyValuePair in delta.Changes) {
			var key = keyValuePair.Key;

			if (key != GameActorInfoDelta.Keys.ArmorPoints) {
				if (key != GameActorInfoDelta.Keys.Health) {
					if (key == GameActorInfoDelta.Keys.PlayerState) {
						Player.PlayerState = (PlayerStates)((byte)keyValuePair.Value);
					}
				} else {
					Health.Value = (short)keyValuePair.Value;
				}
			} else {
				ArmorPoints.Value = (byte)keyValuePair.Value;
			}
		}

		delta.Apply(Player);
		OnDeltaUpdate(delta);
	}

	public void PositionUpdate(PlayerMovement update, ushort gameFrame) { }
	public event Action<Vector3> SendJumpUpdate = delegate { };
	public event Action<ShortVector3, ShortVector3, byte, byte, byte> SendMovementUpdate = delegate { };

	public void Reset() {
		var num = 0;
		Singleton<LoadoutManager>.Instance.GetArmorValues(out num);
		Player.ArmorPointCapacity = (byte)num;
		Player.PlayerState = PlayerStates.None;
		KeyState = KeyState.Still;
		MovementState = MoveStates.None;
		Velocity = Vector3.zero;
		Health.Value = 100;
		ArmorPoints.Value = num;
	}

	public void InitializePlayer() {
		Reset();
	}

	public void SwitchWeaponSlot(int slot) {
		Actions.SwitchWeapon((byte)slot);
		Player.CurrentWeaponSlot = (byte)slot;
	}

	public void SetPing(int ping) {
		if (Player.Ping != ping) {
			Player.Ping = (ushort)ping;
			Actions.UpdatePing(Player.Ping);
		}
	}

	public float GetAbsorptionRate() {
		return 0.66f;
	}

	public void GetArmorDamage(short damage, BodyPart part, out short healthDamage, out byte armorDamage) {
		if (ArmorPoints > 0) {
			var num = Mathf.CeilToInt(GetAbsorptionRate() * damage);
			armorDamage = (byte)Mathf.Clamp(num, 0, ArmorPoints);
			healthDamage = (short)(damage - armorDamage);
		} else {
			armorDamage = 0;
			healthDamage = damage;
		}
	}

	public void Set(MoveStates state, bool on) {
		if (on) {
			MovementState |= state;
		} else {
			MovementState &= ~state;
		}
	}

	public void Set(PlayerStates state, bool on) {
		if (on) {
			var player = Player;
			player.PlayerState |= state;

			if (state != PlayerStates.Paused) {
				if (state != PlayerStates.Sniping) {
					if (state == PlayerStates.Shooting) {
						Actions.AutomaticFire(true);
					}
				} else {
					Actions.SniperMode(true);
				}
			} else {
				Actions.PausePlayer(true);
			}
		} else {
			var player2 = Player;
			player2.PlayerState &= ~state;

			if (state != PlayerStates.Paused) {
				if (state != PlayerStates.Sniping) {
					if (state == PlayerStates.Shooting) {
						Actions.AutomaticFire(false);
					}
				} else {
					Actions.SniperMode(false);
				}
			} else {
				Actions.PausePlayer(false);
			}
		}
	}

	public void Set(KeyState state, bool on) {
		if (on && !Is(state)) {
			KeyState |= state;
			Actions.UpdateKeyState((byte)KeyState);
		} else if (!on && Is(state)) {
			KeyState &= ~state;
			Actions.UpdateKeyState((byte)KeyState);
		}
	}

	public void ResetKeys() {
		if (KeyState != KeyState.Still) {
			KeyState = KeyState.Still;
			Actions.UpdateKeyState((byte)KeyState);
		}
	}

	public bool Is(PlayerStates state) {
		return (byte)(PlayerState & state) != 0;
	}

	public bool Is(KeyState state) {
		return (byte)(KeyState & state) != 0;
	}

	public void LandingUpdate() {
		posSyncFrame = Time.realtimeSinceStartup + 0.05f;
		var b = (byte)(MovementState | MoveStates.Landed | MoveStates.Grounded);
		SendMovementUpdate(GameState.Current.Player.transform.position, GameState.Current.Player.MoveController.Velocity, Conversion.Angle2Byte(HorizontalRotation.eulerAngles.y), Conversion.Angle2Byte(VerticalRotation), b);
	}

	public void JumpingUpdate() {
		Set(MoveStates.Jumping, true);

		if (GameState.Current.Player != null) {
			SendJumpUpdate(GameState.Current.Player.transform.position);
		}
	}

	public void SendUpdates() {
		if (GameState.Current.IsInGame && IsAlive && posSyncFrame <= Time.realtimeSinceStartup && SendMovementUpdate != null) {
			posSyncFrame = Time.realtimeSinceStartup + 0.05f;

			if (cache.Update(GameState.Current.Player.transform.position, Conversion.Angle2Byte(HorizontalRotation.eulerAngles.y), Conversion.Angle2Byte(VerticalRotation), (byte)MovementState)) {
				SendMovementUpdate(cache.Position, GameState.Current.Player.MoveController.Velocity, cache.HRotation, cache.VRotation, cache.MovementState);
				Singleton<GameStateController>.Instance.Client.Peer.SendOutgoingCommands();
			}
		}
	}
}
