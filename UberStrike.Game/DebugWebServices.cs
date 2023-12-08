using System;
using System.Text;
using UberStrike.WebService.Unity;
using UnityEngine;

public class DebugWebServices : IDebugPage {
	private string _currentLog = string.Empty;
	private StringBuilder _requestLog;
	private Vector2 scroller;

	public DebugWebServices() {
		_requestLog = new StringBuilder();
		Configuration.RequestLogger = (Action<string>)Delegate.Combine(Configuration.RequestLogger, new Action<string>(AddRequestLog));
	}

	public string Title {
		get { return "WS"; }
	}

	public void Draw() {
		scroller = GUILayout.BeginScrollView(scroller);
		GUILayout.Label(string.Concat("IN (", WebServiceStatistics.TotalBytesIn, ") -  OUT (", WebServiceStatistics.TotalBytesOut, ")"));

		foreach (var keyValuePair in WebServiceStatistics.Data) {
			GUILayout.Label(keyValuePair.Key + ": " + keyValuePair.Value);
		}

		GUILayout.TextArea(_currentLog);
		GUILayout.EndScrollView();
	}

	private void AddRequestLog(string log) {
		_requestLog.AppendLine(log);
		_currentLog = _requestLog.ToString();
	}
}
