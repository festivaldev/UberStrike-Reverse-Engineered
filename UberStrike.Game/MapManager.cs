using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class MapManager : Singleton<MapManager> {
	private Dictionary<string, UberstrikeMap> _mapsByName = new Dictionary<string, UberstrikeMap>();

	public IEnumerable<UberstrikeMap> AllMaps {
		get { return _mapsByName.Values; }
	}

	public int Count {
		get { return _mapsByName.Count; }
	}

	private MapManager() {
		Clear();
	}

	public string GetMapDescription(int mapId) {
		var mapWithId = GetMapWithId(mapId);

		if (mapWithId != null) {
			return mapWithId.Description;
		}

		return LocalizedStrings.None;
	}

	public string GetMapName(string name) {
		UberstrikeMap uberstrikeMap;

		if (_mapsByName.TryGetValue(name, out uberstrikeMap)) {
			return uberstrikeMap.Name;
		}

		return LocalizedStrings.None;
	}

	public string GetMapName(int mapId) {
		var mapWithId = GetMapWithId(mapId);

		if (mapWithId != null) {
			return mapWithId.Name;
		}

		return LocalizedStrings.None;
	}

	public string GetMapSceneName(int mapId) {
		var mapWithId = GetMapWithId(mapId);

		if (mapWithId != null) {
			return mapWithId.SceneName;
		}

		return LocalizedStrings.None;
	}

	public UberstrikeMap GetMapWithId(int mapId) {
		foreach (var uberstrikeMap in _mapsByName.Values) {
			if (uberstrikeMap.Id == mapId) {
				return uberstrikeMap;
			}
		}

		return null;
	}

	public bool MapExistsWithId(int mapId) {
		return GetMapWithId(mapId) != null;
	}

	public bool HasMapWithId(int mapId) {
		return GetMapWithId(mapId) != null;
	}

	private UberstrikeMap AddMapView(MapView mapView, bool isVisible = true, bool isBuiltIn = false) {
		var uberstrikeMap = new UberstrikeMap(mapView) {
			IsVisible = isVisible,
			IsBuiltIn = isBuiltIn
		};

		UberstrikeMap uberstrikeMap2;

		if (_mapsByName.TryGetValue(mapView.SceneName, out uberstrikeMap2)) {
			uberstrikeMap.View.MapId = uberstrikeMap2.View.MapId;
			uberstrikeMap.View.Settings = uberstrikeMap2.View.Settings;
			uberstrikeMap.View.SupportedGameModes = uberstrikeMap2.View.SupportedGameModes;
		}

		_mapsByName[mapView.SceneName] = uberstrikeMap;

		return uberstrikeMap;
	}

	private void Clear() {
		_mapsByName.Clear();

		AddMapView(new MapView {
			Description = "Menu",
			DisplayName = "Menu",
			SceneName = "Menu"
		}, false, true);
	}

	public bool InitializeMapsToLoad(List<MapView> mapViews) {
		Clear();

		foreach (var mapView in mapViews) {
			AddMapView(mapView);
		}

		return _mapsByName.Count > 0;
	}

	public void LoadMap(UberstrikeMap map, Action onSuccess) {
		PickupItem.Reset();
		Debug.LogWarning("Loading map: " + map.SceneName);

		Singleton<SceneLoader>.Instance.LoadLevel(map.SceneName, delegate {
			if (onSuccess != null) {
				onSuccess();
			}

			Debug.LogWarning("Finished Loading map");
		});
	}

	public bool TryGetMapId(string mapName, out int mapId) {
		foreach (var uberstrikeMap in _mapsByName.Values) {
			if (uberstrikeMap.SceneName == mapName) {
				mapId = uberstrikeMap.Id;

				return true;
			}
		}

		mapId = 0;

		return false;
	}
}
