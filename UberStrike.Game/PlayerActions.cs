using System;

public class PlayerActions {
	public Action<bool> AutomaticFire;
	public Action<bool> PausePlayer;
	public Action<bool> SetReadyForNextGame;
	public Action<bool> SniperMode;
	public Action<byte> SwitchWeapon;
	public Action<byte> UpdateKeyState;
	public Action<byte> UpdateMovementState;
	public Action<ushort> UpdatePing;

	public PlayerActions() {
		Clear();
	}

	public void Clear() {
		UpdateKeyState = delegate { };
		UpdateMovementState = delegate { };
		SwitchWeapon = delegate { };
		UpdatePing = delegate { };
		PausePlayer = delegate { };
		AutomaticFire = delegate { };
		SniperMode = delegate { };
		SetReadyForNextGame = delegate { };
	}
}
