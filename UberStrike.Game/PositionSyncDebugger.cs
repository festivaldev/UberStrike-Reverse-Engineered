using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class PositionSyncDebugger {
	private Queue<PositionInfo> positions = new Queue<PositionInfo>();
	public void AddSample(Vector3 position, MoveStates state) { }

	private class PositionInfo {
		public Color Color;
		public Vector3 Position;
		public MoveStates State;
	}
}
