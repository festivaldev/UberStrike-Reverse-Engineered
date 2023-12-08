using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager> {
	private static GameObject container;
	private List<int> _limitedProjectiles;
	private Dictionary<int, IProjectile> _projectiles;

	public IEnumerable<KeyValuePair<int, IProjectile>> AllProjectiles {
		get { return _projectiles; }
	}

	public IEnumerable<int> LimitedProjectiles {
		get { return _limitedProjectiles; }
	}

	public static GameObject Container {
		get {
			if (container == null) {
				container = new GameObject("Projectiles");
			}

			return container;
		}
	}

	private ProjectileManager() {
		_projectiles = new Dictionary<int, IProjectile>();
		_limitedProjectiles = new List<int>();
	}

	public void AddProjectile(IProjectile p, int id) {
		if (p != null) {
			p.ID = id;
			_projectiles[p.ID] = p;
		}
	}

	public void AddLimitedProjectile(IProjectile p, int id, int count) {
		if (p != null) {
			p.ID = id;
			_projectiles[p.ID] = p;
			_limitedProjectiles.Add(p.ID);
			CheckLimitedProjectiles(count);
		}
	}

	private void CheckLimitedProjectiles(int count) {
		var array = _limitedProjectiles.ToArray();

		for (var i = 0; i < _limitedProjectiles.Count - count; i++) {
			RemoveProjectile(array[i]);
			GameState.Current.Actions.RemoveProjectile(array[i], true);
		}
	}

	public void RemoveAllLimitedProjectiles(bool explode = true) {
		var array = _limitedProjectiles.ToArray();

		for (var i = 0; i < array.Length; i++) {
			RemoveProjectile(array[i], explode);
			GameState.Current.Actions.RemoveProjectile(array[i], explode);
		}
	}

	public void RemoveProjectile(int id, bool explode = true) {
		try {
			IProjectile projectile;

			if (_projectiles.TryGetValue(id, out projectile)) {
				if (explode) {
					projectile.Explode();
				} else {
					projectile.Destroy();
				}
			}
		} catch (Exception ex) {
			Debug.LogException(ex);
		} finally {
			_limitedProjectiles.RemoveAll(i => i == id);
			_projectiles.Remove(id);
		}
	}

	public void RemoveAllProjectilesFromPlayer(byte playerNumber) {
		foreach (var num in _projectiles.KeyArray()) {
			if ((num & 255) == playerNumber) {
				RemoveProjectile(num, false);
			}
		}
	}

	public void Clear() {
		try {
			foreach (var keyValuePair in _projectiles) {
				if (keyValuePair.Value != null) {
					keyValuePair.Value.Destroy();
				}
			}
		} catch (Exception ex) {
			Debug.LogException(ex);
		} finally {
			_projectiles.Clear();
			_limitedProjectiles.Clear();
			UnityEngine.Object.Destroy(container);
			container = null;
		}
	}

	public static int CreateGlobalProjectileID(byte playerNumber, int localProjectileId) {
		return (localProjectileId << 8) + playerNumber;
	}

	public static string PrintID(int id) {
		return GetPlayerId(id) + "/" + (id >> 8);
	}

	private static int GetPlayerId(int projectileId) {
		return projectileId & 255;
	}
}
