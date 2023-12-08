using System;
using System.Collections;
using System.Collections.Generic;
using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UnityEngine;

public class GameServerManager : Singleton<GameServerManager> {
	private const int ServerUpdateCycle = 30;
	private IComparer<PhotonServer> _comparer;
	private Dictionary<int, PhotonServer> _gameServers = new Dictionary<int, PhotonServer>();
	private Dictionary<int, ServerLoadRequest> _loadRequests = new Dictionary<int, ServerLoadRequest>();
	private bool _reverseSorting;
	private List<PhotonServer> _sortedServers = new List<PhotonServer>();
	public PhotonServer CommServer = PhotonServer.Empty;

	public int PhotonServerCount {
		get { return this._gameServers.Count; }
	}

	public int AllPlayersCount { get; private set; }
	public int AllGamesCount { get; private set; }

	public IEnumerable<PhotonServer> PhotonServerList {
		get { return this._sortedServers; }
	}

	public IEnumerable<ServerLoadRequest> ServerRequests {
		get { return this._loadRequests.Values; }
	}

	private GameServerManager() { }

	public void SortServers() {
		if (this._comparer != null) {
			this._sortedServers.Sort(this._comparer);

			if (this._reverseSorting) {
				this._sortedServers.Reverse();
			}
		}
	}

	public PhotonServer GetBestServer() {
		PhotonServer photonServer = this.GetBestServer(ApplicationDataManager.IsMobile);

		if (ApplicationDataManager.IsMobile && photonServer == null) {
			photonServer = this.GetBestServer(false);
		}

		return photonServer;
	}

	private PhotonServer GetBestServer(bool doMobileFilter) {
		List<PhotonServer> list = new List<PhotonServer>(this._gameServers.Values);
		list.Sort((PhotonServer s, PhotonServer t) => s.Latency - t.Latency);
		PhotonServer photonServer = null;

		for (int i = 0; i < list.Count; i++) {
			PhotonServer photonServer2 = list[i];

			if (photonServer2.Latency != 0) {
				if (!doMobileFilter || photonServer2.UsageType == PhotonUsageType.Mobile) {
					if (photonServer == null && photonServer2.CheckLatency()) {
						photonServer = photonServer2;
					} else if (photonServer2.CheckLatency() && photonServer2.Latency < 200 && photonServer.Data.PlayersConnected < photonServer2.Data.PlayersConnected) {
						photonServer = photonServer2;
					}
				}
			}
		}

		return photonServer;
	}

	internal string GetServerName(GameRoomData room) {
		string text = string.Empty;

		if (room != null && room.Server != null) {
			foreach (PhotonServer photonServer in this._gameServers.Values) {
				if (photonServer.ConnectionString == room.Server.ConnectionString) {
					text = photonServer.Name;

					break;
				}
			}
		}

		return text;
	}

	public void SortServers(IComparer<PhotonServer> comparer, bool reverse = false) {
		this._comparer = comparer;
		this._reverseSorting = reverse;
		List<PhotonServer> sortedServers = this._sortedServers;

		lock (sortedServers) {
			this._sortedServers.Clear();
			this._sortedServers.AddRange(this._gameServers.Values);
		}

		this.SortServers();
	}

	public void AddTestPhotonGameServer(int id, PhotonServer photonServer) {
		this._gameServers[id] = photonServer;
	}

	public void AddPhotonGameServer(PhotonView view) {
		this._gameServers[view.PhotonId] = new PhotonServer(view);

		if (view.MinLatency > 0) {
			view.Name = string.Concat(new object[] {
				view.Name,
				" - ",
				view.MinLatency,
				"ms"
			});
		}

		this.SortServers();
	}

	public void AddPhotonGameServers(List<PhotonView> servers) {
		foreach (PhotonView photonView in servers) {
			this.AddPhotonGameServer(photonView);
		}
	}

	public int GetServerLatency(string connection) {
		foreach (PhotonServer photonServer in this._gameServers.Values) {
			if (photonServer.ConnectionString == connection) {
				return photonServer.Latency;
			}
		}

		return 0;
	}

	public IEnumerator StartUpdatingServerLoads() {
		foreach (PhotonServer server in this._gameServers.Values) {
			ServerLoadRequest request;

			if (!this._loadRequests.TryGetValue(server.Id, out request)) {
				request = ServerLoadRequest.Run(server, delegate { this.UpdateGamesAndPlayerCount(); });
				this._loadRequests.Add(server.Id, request);
			}

			if (request.RequestState != ServerLoadRequest.RequestStateType.Waiting) {
				request.Run();
			}

			yield return new WaitForSeconds(0.1f);
		}

		yield break;
	}

	public IEnumerator StartUpdatingLatency(Action<float> progressCallback) {
		yield return UnityRuntime.StartRoutine(this.StartUpdatingServerLoads());

		float minTimeout = Time.time + 4f;
		float maxTimeout = Time.time + 10f;
		int count = 0;

		while (count != this._loadRequests.Count) {
			yield return new WaitForSeconds(1f);

			count = 0;

			foreach (ServerLoadRequest r in this._loadRequests.Values) {
				if (r.RequestState != ServerLoadRequest.RequestStateType.Waiting) {
					count++;
				}
			}

			progressCallback((float)count / (float)this._loadRequests.Count);

			if ((count > 0 && Time.time > minTimeout) || Time.time > maxTimeout) {
				yield break;
			}
		}

		yield break;
	}

	private void UpdateGamesAndPlayerCount() {
		this.AllPlayersCount = 0;
		this.AllGamesCount = 0;

		foreach (PhotonServer photonServer in this._gameServers.Values) {
			this.AllPlayersCount += photonServer.Data.PlayersConnected;
			this.AllGamesCount += photonServer.Data.RoomsCreated;
		}

		this.SortServers();
	}
}
