using System;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class RemoteCharacterState : ICharacterState {
	private PositionSyncDebugger debugger;
	private float hRotationTarget;
	private PositionInterpolator sampler = new PositionInterpolator();
	private float vRotationTarget;

	public RemoteCharacterState(GameActorInfo info, PlayerMovement update) {
		debugger = new PositionSyncDebugger();
		Player = info;
		sampler.Reset(update.Position);
		PositionUpdate(update, 0);
		HorizontalRotation = Quaternion.Euler(0f, hRotationTarget, 0f);
		VerticalRotation = vRotationTarget;
	}

	public GameActorInfo Player { get; private set; }
	public Vector3 Velocity { get; set; }
	public Vector3 Position { get; set; }
	public Quaternion HorizontalRotation { get; set; }
	public float VerticalRotation { get; set; }
	public MoveStates MovementState { get; set; }
	public KeyState KeyState { get; set; }
	public event Action<GameActorInfoDelta> OnDeltaUpdate = delegate { };
	public event Action<PlayerMovement> OnPositionUpdate = delegate { };

	public bool Is(MoveStates state) {
		return (byte)(MovementState & state) != 0;
	}

	public void PositionUpdate(PlayerMovement update, ushort gameFrame) {
		MovementState = (MoveStates)update.MovementState;
		KeyState = (KeyState)update.KeyState;
		Velocity = update.Velocity;
		hRotationTarget = Conversion.Byte2Angle(update.HorizontalRotation);
		vRotationTarget = Conversion.Byte2Angle(update.VerticalRotation);
		sampler.Add(update, gameFrame);
		debugger.AddSample(update.Position, MovementState);
		InterpolateMovement();
		OnPositionUpdate(update);
	}

	public void DeltaUpdate(GameActorInfoDelta update) {
		update.Apply(Player);
		OnDeltaUpdate(update);
	}

	public void SetPosition(Vector3 pos) {
		sampler.Reset(pos);
		InterpolateMovement();
	}

	public void InterpolateMovement() {
		if (Time.time > sampler.LastTime && KeyState != KeyState.Still) {
			sampler.Extrapolate();
		}

		Position = sampler.Lerp();
		HorizontalRotation = Quaternion.Lerp(HorizontalRotation, Quaternion.Euler(0f, hRotationTarget, 0f), Time.deltaTime * 5f);
		VerticalRotation = Mathf.LerpAngle(VerticalRotation, vRotationTarget, Time.deltaTime * 20f);

		if (VerticalRotation > 180f) {
			VerticalRotation -= 360f;
		}
	}

	private class PositionInterpolator {
		private float avgFrameTime;
		private int baseGameFrame;
		private float baseTime;
		private Packet sampleA;
		private Packet sampleB;
		private Packet sampleC;
		private float timeWindow = 1.5f;

		public float LastTime {
			get { return sampleA.Time; }
		}

		public PositionInterpolator() {
			Reset(Vector3.zero);
		}

		public void Add(PlayerMovement m, ushort gameFrame) {
			if (Mathf.Abs(gameFrame - sampleA.GameFrame) > 5) {
				baseGameFrame = gameFrame;
				baseTime = Time.time;
				avgFrameTime = 0.1f;
			} else {
				avgFrameTime = (Time.time - baseTime) / (gameFrame - baseGameFrame);

				if (float.IsNaN(avgFrameTime) || float.IsInfinity(avgFrameTime) || avgFrameTime < 0.01f) {
					avgFrameTime = 0.1f;
				}
			}

			var vector = Lerp();

			if (gameFrame == sampleA.GameFrame) {
				sampleA = new Packet {
					Position = m.Position,
					Time = baseTime + (gameFrame - baseGameFrame) * avgFrameTime + timeWindow * avgFrameTime,
					GameFrame = gameFrame
				};
			} else {
				sampleC = sampleB;
				sampleB = sampleA;

				sampleA = new Packet {
					Position = m.Position,
					Time = baseTime + (gameFrame - baseGameFrame) * avgFrameTime + timeWindow * avgFrameTime,
					GameFrame = gameFrame
				};

				var vector2 = Lerp();
			}
		}

		public Vector3 Lerp() {
			if (Time.time > sampleB.Time) {
				var num = 1f - Mathf.Clamp01(sampleA.Time - Time.time) / Mathf.Max(sampleA.Time - sampleB.Time, 0.05f);

				return Vector3.Lerp(sampleB.Position, sampleA.Position, num);
			}

			var num2 = 1f - Mathf.Clamp01(sampleB.Time - Time.time) / Mathf.Max(sampleB.Time - sampleC.Time, 0.05f);

			return Vector3.Lerp(sampleC.Position, sampleB.Position, num2);
		}

		public void Reset(Vector3 pos) {
			sampleA = new Packet {
				Position = pos,
				Time = Time.time + timeWindow * avgFrameTime
			};

			sampleB = new Packet {
				Position = pos,
				Time = Time.time + timeWindow * avgFrameTime - 1f * avgFrameTime
			};

			sampleC = new Packet {
				Position = pos,
				Time = Time.time + timeWindow * avgFrameTime - 2f * avgFrameTime
			};

			baseGameFrame = 0;
		}

		internal void Extrapolate() {
			Add(new PlayerMovement {
				Position = sampleA.Position + (sampleA.Position - sampleB.Position)
			}, (ushort)(sampleA.GameFrame + 1));
		}

		private class Packet {
			public float ArrivalTime;
			public int GameFrame;
			public Vector3 Position;
			public float Time;
		}
	}
}
