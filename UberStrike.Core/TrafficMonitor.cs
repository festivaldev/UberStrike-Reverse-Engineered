using System;
using System.Collections.Generic;
using UnityEngine;

public class TrafficMonitor {
	private Dictionary<int, string> eventNames = new Dictionary<int, string>();
	private string lastEvent;
	private Dictionary<int, string> peerOperationNames = new Dictionary<int, string>();
	private Dictionary<int, string> roomOperationNames = new Dictionary<int, string>();
	public LinkedList<string> AllEvents { get; private set; }
	public bool IsEnabled { get; internal set; }

	public TrafficMonitor(bool enable = true) {
		AllEvents = new LinkedList<string>();
		IsEnabled = enable;
	}

	public void AddEvent(string ev) {
		if (lastEvent == ev) {
			var last = AllEvents.Last;
			last.Value += ".";
		} else {
			AllEvents.AddLast(Time.frameCount + ": " + ev);
		}

		while (AllEvents.Count >= 200) {
			AllEvents.RemoveFirst();
		}

		lastEvent = ev;
	}

	public bool SendOperation(byte operationCode, Dictionary<byte, object> customOpParameters, bool sendReliable, byte channelId, bool encrypted) {
		if (customOpParameters.ContainsKey(0)) {
			AddEvent(string.Concat("Room Operation<", operationCode, ">: ", (!roomOperationNames.ContainsKey(operationCode)) ? operationCode.ToString() : roomOperationNames[operationCode]));
		} else if (customOpParameters.ContainsKey(1)) {
			AddEvent(string.Concat("Peer Operation<", operationCode, ">: ", (!peerOperationNames.ContainsKey(operationCode)) ? operationCode.ToString() : peerOperationNames[operationCode]));
		} else {
			AddEvent("Operation<" + operationCode + ">");
		}

		return true;
	}

	public void OnEvent(byte eventCode, byte[] data) {
		AddEvent(string.Concat("OnEvent<", eventCode, ">: ", (!eventNames.ContainsKey(eventCode)) ? eventCode.ToString() : eventNames[eventCode]));
	}

	public void AddNamesForPeerOperations(Type enumType) {
		if (!enumType.IsEnum) {
			throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
		}

		foreach (var obj in Enum.GetValues(enumType)) {
			peerOperationNames[(int)obj] = obj.ToString();
		}
	}

	public void AddNamesForRoomOperations(Type enumType) {
		if (!enumType.IsEnum) {
			throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
		}

		foreach (var obj in Enum.GetValues(enumType)) {
			roomOperationNames[(int)obj] = obj.ToString();
		}
	}

	public void AddNamesForEvents(Type enumType) {
		if (!enumType.IsEnum) {
			throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
		}

		foreach (var obj in Enum.GetValues(enumType)) {
			eventNames[(int)obj] = obj.ToString();
		}
	}
}
