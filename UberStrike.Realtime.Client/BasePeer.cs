using System;
using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

namespace UberStrike.Realtime.Client {
	public abstract class BasePeer : IDisposable {
		private PhotonPeerListener listener;
		private float nextUpdateTime;
		private bool sendThreadShouldRun;
		public TrafficMonitor Monitor { get; private set; }
		public PhotonPeer Peer { get; private set; }
		public bool IsEnabled { get; private set; }
		public float SyncFrequency { get; private set; }

		public int ServerTimeTicks {
			get { return Peer.ServerTimeInMilliSeconds & int.MaxValue; }
		}

		public bool IsConnected {
			get { return Peer.PeerState == PeerStateValue.Connected; }
		}

		protected BasePeer(int syncFrequency, bool monitorTraffic) {
			listener = new PhotonPeerListener();
			Peer = new PhotonPeer(listener, ConnectionProtocol.Udp);
			SyncFrequency = syncFrequency / 1000f;
			Monitor = new TrafficMonitor(monitorTraffic);
			IsEnabled = true;

			if (monitorTraffic) {
				listener.OnError += delegate(string error) { Monitor.AddEvent(error); };
				listener.OnConnect += delegate { Monitor.AddEvent("Connected"); };
				listener.OnDisconnect += delegate { Monitor.AddEvent("Disconnected"); };
				listener.EventDispatcher += Monitor.OnEvent;
			}

			listener.OnConnect += OnConnected;
			listener.OnDisconnect += OnDisconnected;
			listener.OnError += OnError;
			UnityRuntime.Instance.OnUpdate += SendDispatch;
			StartFallbackSendAckThread();
			UnityRuntime.Instance.OnShutdown += StopFallbackSendAckThread;
		}

		public void Dispose() {
			if (IsEnabled) {
				Disconnect();
				IsEnabled = false;
				listener.ClearEvents();
				UnityRuntime.Instance.OnUpdate -= SendDispatch;
			}
		}

		public void Connect(string endpointAddress) {
			if (Monitor.IsEnabled) {
				Monitor.AddEvent("Connect " + endpointAddress);
			}

			var ipAddress = new ConnectionAddress(endpointAddress).IpAddress;

			if (CrossdomainPolicy.HasValidPolicy(ipAddress)) {
				ConnectToServer(endpointAddress);
			} else {
				UnityRuntime.Instance.StartCoroutine(CrossdomainPolicy.CheckPolicyRoutine(ipAddress, delegate {
					if (CrossdomainPolicy.HasValidPolicy(ipAddress)) {
						ConnectToServer(endpointAddress);
					} else {
						OnConnectionFail(endpointAddress);
					}
				}));
			}
		}

		private void ConnectToServer(string endpointAddress) {
			if (!IsEnabled || !Peer.Connect(endpointAddress, ApiVersion.Current)) {
				Debug.LogWarning("connection failed to " + endpointAddress);
				OnConnectionFail(endpointAddress);
			}
		}

		public void Disconnect() {
			if (Monitor.IsEnabled) {
				Monitor.AddEvent("Disconnect");
			}

			Peer.SendOutgoingCommands();
			Peer.Disconnect();
		}

		private void SendDispatch() {
			if (Peer.PeerState != PeerStateValue.Disconnected) {
				Peer.Service();
			}
		}

		public void StartFallbackSendAckThread() {
			if (sendThreadShouldRun) {
				return;
			}

			sendThreadShouldRun = true;
			SupportClass.CallInBackground(FallbackSendAckThread);
		}

		public void StopFallbackSendAckThread() {
			sendThreadShouldRun = false;
		}

		public bool FallbackSendAckThread() {
			if (sendThreadShouldRun && Peer != null) {
				Peer.SendAcksOnly();
			}

			return sendThreadShouldRun;
		}

		protected void AddRoomLogic(IEventDispatcher evDispatcher, IOperationSender opSender) {
			if (Monitor.IsEnabled) {
				opSender.SendOperation += Monitor.SendOperation;
			}

			opSender.SendOperation += Peer.OpCustom;
			listener.EventDispatcher += evDispatcher.OnEvent;
		}

		protected void RemoveRoomLogic(IEventDispatcher evDispatcher, IOperationSender opSender) {
			if (Monitor.IsEnabled) {
				opSender.SendOperation -= Monitor.SendOperation;
			}

			opSender.SendOperation -= Peer.OpCustom;
			listener.EventDispatcher -= evDispatcher.OnEvent;
		}

		public override string ToString() {
			return Peer.PeerState.ToString();
		}

		protected abstract void OnConnected();
		protected abstract void OnDisconnected(StatusCode status);
		protected abstract void OnError(string message);
		protected virtual void OnConnectionFail(string endpointAddress) { }
	}
}
