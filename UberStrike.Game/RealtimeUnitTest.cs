using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UberStrike.Realtime.Client;
using UnityEngine;

public class RealtimeUnitTest : AutoMonoBehaviour<RealtimeUnitTest> {
	private int index;
	private Dictionary<string, List<RemoteCall>> methodCalls;
	private Vector2 scroll;

	private RealtimeUnitTest() {
		methodCalls = new Dictionary<string, List<RemoteCall>>();
	}

	private void OnGUI() {
		var array = methodCalls.KeyArray();
		GUILayout.BeginArea(new Rect(0f, 100f, Screen.width, Screen.height - 100));
		index = GUILayout.SelectionGrid(index, array, array.Length);

		if (index < array.Length) {
			scroll = GUILayout.BeginScrollView(scroll);
			var list = methodCalls[array[index]];

			foreach (var remoteCall in list) {
				if (GUILayout.Button(remoteCall.debug)) {
					remoteCall.method.Invoke(remoteCall.target, remoteCall.arguments);
				}
			}

			GUILayout.EndScrollView();
		}

		GUILayout.EndArea();
	}

	public void Add(IOperationSender target) {
		List<RemoteCall> list;

		if (!methodCalls.TryGetValue(target.GetType().Name, out list)) {
			list = new List<RemoteCall>();
			methodCalls[target.GetType().Name] = list;
		}

		var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);

		foreach (var methodInfo in methods) {
			if (methodInfo.Name.StartsWith("Send")) {
				list.Add(new RemoteCall(target, methodInfo));
			}
		}
	}

	public void Add(IEventDispatcher target) {
		List<RemoteCall> list;

		if (!methodCalls.TryGetValue(target.GetType().Name, out list)) {
			list = new List<RemoteCall>();
			methodCalls[target.GetType().Name] = list;
		}

		var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

		foreach (var methodInfo in methods) {
			if (methodInfo.Name.StartsWith("On")) {
				list.Add(new RemoteCall(target, methodInfo));
			}
		}
	}

	private class RemoteCall {
		public object target { get; private set; }
		public MethodInfo method { get; private set; }
		public object[] arguments { get; private set; }
		public string debug { get; private set; }

		public RemoteCall(object target, MethodInfo method) {
			this.target = target;
			this.method = method;
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(method.Name).Append("(");
			var list = new List<object>();

			foreach (var parameterInfo in method.GetParameters()) {
				list.Add(CreateArgument(parameterInfo.ParameterType));
				stringBuilder.Append(parameterInfo.ParameterType.Name).Append(":").Append(CreateArgument(parameterInfo.ParameterType)).Append(" ");
			}

			stringBuilder.Append(")");
			arguments = list.ToArray();
			debug = stringBuilder.ToString();
		}

		private static object CreateArgument(Type t) {
			if (t.IsGenericType) {
				return Activator.CreateInstance(t);
			}

			if (t == typeof(string)) {
				return "asdf";
			}

			if (t.IsClass) {
				return null;
			}

			return Activator.CreateInstance(t);
		}
	}
}
