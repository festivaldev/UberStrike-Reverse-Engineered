using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class SpawnPointManager : Singleton<SpawnPointManager> {
	private IDictionary<GameModeType, IDictionary<TeamID, IList<SpawnPoint>>> _spawnPointsDictionary;

	private SpawnPointManager() {
		_spawnPointsDictionary = new Dictionary<GameModeType, IDictionary<TeamID, IList<SpawnPoint>>>();

		foreach (var obj in Enum.GetValues(typeof(GameModeType))) {
			var gameModeType = (GameModeType)((int)obj);

			_spawnPointsDictionary[gameModeType] = new Dictionary<TeamID, IList<SpawnPoint>> {
				{
					TeamID.BLUE, new List<SpawnPoint>()
				}, {
					TeamID.RED, new List<SpawnPoint>()
				}, {
					TeamID.NONE, new List<SpawnPoint>()
				}
			};
		}
	}

	private void Clear() {
		foreach (var obj in Enum.GetValues(typeof(GameModeType))) {
			var gameModeType = (GameModeType)((int)obj);
			_spawnPointsDictionary[gameModeType][TeamID.NONE].Clear();
			_spawnPointsDictionary[gameModeType][TeamID.BLUE].Clear();
			_spawnPointsDictionary[gameModeType][TeamID.RED].Clear();
		}
	}

	private bool TryGetSpawnPointAt(int index, GameModeType gameMode, TeamID teamID, out SpawnPoint point) {
		point = ((index >= GetSpawnPointList(gameMode, teamID).Count) ? null : GetSpawnPointList(gameMode, teamID)[index]);

		return point != null;
	}

	private bool TryGetRandomSpawnPoint(GameModeType gameMode, TeamID teamID, out SpawnPoint point) {
		var spawnPointList = GetSpawnPointList(gameMode, teamID);
		point = ((spawnPointList.Count <= 0) ? null : spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)]);

		return point != null;
	}

	private IList<SpawnPoint> GetSpawnPointList(GameModeType gameMode, TeamID team) {
		if (gameMode == GameModeType.None) {
			return _spawnPointsDictionary[GameModeType.DeathMatch][TeamID.NONE];
		}

		return _spawnPointsDictionary[gameMode][team];
	}

	public void ConfigureSpawnPoints(SpawnPoint[] points) {
		Clear();

		foreach (var spawnPoint in points) {
			if (_spawnPointsDictionary.ContainsKey(spawnPoint.GameModeType)) {
				_spawnPointsDictionary[spawnPoint.GameModeType][spawnPoint.TeamId].Add(spawnPoint);
			}
		}
	}

	public int GetSpawnPointCount(GameModeType gameMode, TeamID team) {
		return GetSpawnPointList(gameMode, team).Count;
	}

	public void GetAllSpawnPoints(GameModeType gameMode, TeamID team, out List<Vector3> positions, out List<byte> angles) {
		var spawnPointList = GetSpawnPointList(gameMode, team);
		positions = new List<Vector3>(spawnPointList.Count);
		angles = new List<byte>(spawnPointList.Count);

		foreach (var spawnPoint in spawnPointList) {
			positions.Add(spawnPoint.Position);
			angles.Add(Conversion.Angle2Byte(spawnPoint.transform.rotation.eulerAngles.y));
		}
	}

	public void GetSpawnPointAt(int index, GameModeType gameMode, TeamID team, out Vector3 position, out Quaternion rotation) {
		if (gameMode == GameModeType.None) {
			gameMode = GameModeType.DeathMatch;
		}

		SpawnPoint spawnPoint;

		if (TryGetSpawnPointAt(index, gameMode, team, out spawnPoint)) {
			position = spawnPoint.transform.position;
			rotation = spawnPoint.transform.rotation;
		} else {
			Debug.LogException(new Exception(string.Concat("No spawnpoints found at ", index, " int list of length ", GetSpawnPointCount(gameMode, team))));

			if (GameState.Current.Map != null && GameState.Current.Map.DefaultSpawnPoint != null) {
				position = GameState.Current.Map.DefaultSpawnPoint.position;
			} else {
				position = new Vector3(0f, 10f, 0f);
			}

			rotation = Quaternion.identity;
		}
	}

	public void GetRandomSpawnPoint(GameModeType gameMode, TeamID team, out Vector3 position, out Quaternion rotation) {
		if (gameMode == GameModeType.None) {
			gameMode = GameModeType.DeathMatch;
		}

		var list = _spawnPointsDictionary[gameMode][team];

		if (list.Count > 0) {
			var spawnPoint = list[UnityEngine.Random.Range(0, list.Count)];
			position = spawnPoint.transform.position;
			rotation = spawnPoint.transform.rotation;
		} else {
			Debug.LogWarning(string.Concat("GetRandomSpawnPoint failed for ", team, "/", gameMode));
			position = Vector3.zero;
			rotation = Quaternion.identity;
		}
	}
}
