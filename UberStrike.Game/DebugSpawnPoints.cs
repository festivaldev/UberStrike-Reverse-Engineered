using System;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class DebugSpawnPoints : IDebugPage {
	private GameModeType gameMode;
	private Vector2 scroll1;
	private Vector2 scroll2;
	private Vector2 scroll3;

	public string Title {
		get { return "Spawn"; }
	}

	public void Draw() {
		GUILayout.BeginHorizontal();

		foreach (var obj in Enum.GetValues(typeof(GameModeType))) {
			var gameModeType = (GameModeType)((int)obj);

			if (GUILayout.Button(gameModeType.ToString())) {
				gameMode = gameModeType;
			}
		}

		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		scroll1 = GUILayout.BeginScrollView(scroll1);
		GUILayout.Label(TeamID.NONE.ToString());

		for (var i = 0; i < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.NONE); i++) {
			Vector3 vector;
			Quaternion quaternion;
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(i, gameMode, TeamID.NONE, out vector, out quaternion);
			GUILayout.Label(i + ": " + vector);
		}

		GUILayout.EndScrollView();
		scroll2 = GUILayout.BeginScrollView(scroll2);
		GUILayout.Label(TeamID.BLUE.ToString());

		for (var j = 0; j < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.BLUE); j++) {
			Vector3 vector;
			Quaternion quaternion;
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(j, gameMode, TeamID.BLUE, out vector, out quaternion);
			GUILayout.Label(j + ": " + vector);
		}

		GUILayout.EndScrollView();
		scroll3 = GUILayout.BeginScrollView(scroll3);
		GUILayout.Label(TeamID.RED.ToString());

		for (var k = 0; k < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.RED); k++) {
			Vector3 vector;
			Quaternion quaternion;
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(k, gameMode, TeamID.RED, out vector, out quaternion);
			GUILayout.Label(k + ": " + vector);
		}

		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
	}
}
