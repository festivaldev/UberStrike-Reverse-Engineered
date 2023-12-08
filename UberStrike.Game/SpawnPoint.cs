using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
	[SerializeField]
	private bool DrawGizmos = true;

	[SerializeField]
	public GameMode GameMode;

	[SerializeField]
	private float Radius = 1f;

	[SerializeField]
	public TeamID TeamPoint;

	public GameModeType GameModeType {
		get {
			var gameMode = GameMode;

			if (gameMode == GameMode.TeamDeathMatch) {
				return GameModeType.TeamDeathMatch;
			}

			if (gameMode == GameMode.DeathMatch) {
				return GameModeType.DeathMatch;
			}

			if (gameMode != GameMode.TeamElimination) {
				return GameModeType.None;
			}

			return GameModeType.EliminationMode;
		}
	}

	public Vector3 Position {
		get { return transform.position; }
	}

	public Vector2 Rotation {
		get { return new Vector2(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x); }
	}

	public TeamID TeamId {
		get { return TeamPoint; }
	}

	public float SpawnRadius {
		get { return Radius; }
	}

	private void OnDrawGizmos() {
		if (!DrawGizmos) {
			return;
		}

		switch (TeamPoint) {
			case TeamID.NONE:
				Gizmos.color = Color.green;

				break;
			case TeamID.BLUE:
				Gizmos.color = Color.blue;

				break;
			case TeamID.RED:
				Gizmos.color = Color.red;

				break;
		}

		Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1f, 0.1f, 1f));
		Gizmos.DrawSphere(Vector3.zero, Radius);
		var gameModeType = GameModeType;

		if (gameModeType != GameModeType.DeathMatch) {
			if (gameModeType == GameModeType.TeamDeathMatch) {
				Gizmos.color = Color.white;
			}
		} else {
			Gizmos.color = Color.yellow;
		}

		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.DrawLine(transform.position + transform.forward * Radius, transform.position + transform.forward * 2f * Radius);
	}
}
