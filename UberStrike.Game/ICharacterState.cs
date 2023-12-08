using System;
using UberStrike.Core.Models;
using UnityEngine;

public interface ICharacterState {
	GameActorInfo Player { get; }
	Vector3 Velocity { get; }
	Vector3 Position { get; }
	Quaternion HorizontalRotation { get; }
	float VerticalRotation { get; }
	MoveStates MovementState { get; set; }
	KeyState KeyState { get; }
	event Action<GameActorInfoDelta> OnDeltaUpdate;
	event Action<PlayerMovement> OnPositionUpdate;
	bool Is(MoveStates state);
	void DeltaUpdate(GameActorInfoDelta update);
	void PositionUpdate(PlayerMovement update, ushort gameFrame);
}
