using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class RemotePlayerInterpolator {
	private Dictionary<byte, RemoteCharacterState> remoteStates;

	public RemotePlayerInterpolator() {
		remoteStates = new Dictionary<byte, RemoteCharacterState>(20);
	}

	public void Update() {
		foreach (var remoteCharacterState in remoteStates.Values) {
			remoteCharacterState.InterpolateMovement();
		}
	}

	public void PositionUpdate(PlayerMovement update, ushort gameFrame) {
		RemoteCharacterState remoteCharacterState;

		if (remoteStates.TryGetValue(update.Number, out remoteCharacterState)) {
			remoteCharacterState.PositionUpdate(update, gameFrame);
		}
	}

	public void DeltaUpdate(GameActorInfoDelta delta) {
		RemoteCharacterState remoteCharacterState;

		if (remoteStates.TryGetValue(delta.Id, out remoteCharacterState)) {
			remoteCharacterState.DeltaUpdate(delta);
		}
	}

	public void UpdatePositionHard(byte playerNumber, Vector3 pos) {
		RemoteCharacterState remoteCharacterState;

		if (remoteStates.TryGetValue(playerNumber, out remoteCharacterState)) {
			remoteCharacterState.SetPosition(pos);
		}
	}

	public void AddCharacterInfo(GameActorInfo player, PlayerMovement position) {
		remoteStates[player.PlayerId] = new RemoteCharacterState(player, position);
	}

	public void Reset() {
		remoteStates.Clear();
	}

	public void RemoveCharacterInfo(byte playerID) {
		RemoteCharacterState remoteCharacterState;

		if (remoteStates.TryGetValue(playerID, out remoteCharacterState)) {
			remoteStates.Remove(remoteCharacterState.Player.PlayerId);
		}
	}

	public RemoteCharacterState GetState(byte playerID) {
		RemoteCharacterState remoteCharacterState = null;
		remoteStates.TryGetValue(playerID, out remoteCharacterState);

		return remoteCharacterState;
	}
}
