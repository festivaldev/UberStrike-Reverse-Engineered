using System.Collections.Generic;
using UberStrike.Core.Models;

public class GameListManager : Singleton<GameListManager> {
	private Dictionary<int, GameRoomData> _gameList = new Dictionary<int, GameRoomData>();

	public IEnumerable<GameRoomData> GameList {
		get { return _gameList.Values; }
	}

	public int GamesCount {
		get { return _gameList.Count; }
	}

	public int PlayersCount { get; private set; }
	private GameListManager() { }

	public void SetGameList(List<GameRoomData> data) {
		_gameList.Clear();
		data.ForEach(delegate(GameRoomData g) { _gameList[g.Number] = g; });
		UpdatePlayerCount();
	}

	public void AddGame(GameRoomData game) {
		_gameList[game.Number] = game;
		UpdatePlayerCount();
	}

	public void RemoveGame(int id) {
		_gameList.Remove(id);
		UpdatePlayerCount();
	}

	public void Clear() {
		_gameList.Clear();
		PlayersCount = 0;
	}

	private void UpdatePlayerCount() {
		PlayersCount = 0;

		foreach (var keyValuePair in _gameList) {
			PlayersCount += keyValuePair.Value.ConnectedPlayers;
		}
	}
}
