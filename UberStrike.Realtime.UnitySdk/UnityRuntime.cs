using System;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk {
	public class UnityRuntime : MonoBehaviour {
		private static UnityRuntime instance;
		private Action onFixedUpdate;
		private Action onShutdown;
		private Action onUpdate;

		[SerializeField]
		private bool showInvocationList;

		public static UnityRuntime Instance {
			get {
				if (instance == null) {
					var gameObject = GameObject.Find("AutoMonoBehaviours");

					if (gameObject == null) {
						gameObject = new GameObject("AutoMonoBehaviours");
					}

					instance = gameObject.AddComponent<UnityRuntime>();
				}

				return instance;
			}
		}

		public event Action OnFixedUpdate {
			add { onFixedUpdate = (Action)Delegate.Combine(onFixedUpdate, value); }
			remove { onFixedUpdate = (Action)Delegate.Remove(onFixedUpdate, value); }
		}

		public event Action OnUpdate {
			add { onUpdate = (Action)Delegate.Combine(onUpdate, value); }
			remove { onUpdate = (Action)Delegate.Remove(onUpdate, value); }
		}

		public event Action OnShutdown {
			add { onShutdown = (Action)Delegate.Combine(onShutdown, value); }
			remove { onShutdown = (Action)Delegate.Remove(onShutdown, value); }
		}

		private void OnGUI() {
			if (showInvocationList) {
				GUILayout.BeginArea(new Rect(10f, 100f, 400f, Screen.height - 200));

				if (onUpdate != null) {
					foreach (var @delegate in onUpdate.GetInvocationList()) {
						GUILayout.Label("Update: " + @delegate.Method.DeclaringType.Name + "." + @delegate.Method.Name);
					}
				}

				if (onFixedUpdate != null) {
					foreach (var delegate2 in onFixedUpdate.GetInvocationList()) {
						GUILayout.Label("FixedUpdate: " + delegate2.Method.DeclaringType.Name + "." + delegate2.Method.Name);
					}
				}

				if (onShutdown != null) {
					foreach (var delegate3 in onShutdown.GetInvocationList()) {
						GUILayout.Label("OnApplicationQuit: " + delegate3.Method.DeclaringType.Name + "." + delegate3.Method.Name);
					}
				}

				GUILayout.EndArea();
			}
		}

		private void Update() {
			if (onUpdate != null) {
				onUpdate();
			}
		}

		private void FixedUpdate() {
			if (onFixedUpdate != null) {
				onFixedUpdate();
			}
		}

		private void OnApplicationQuit() {
			if (onShutdown != null) {
				onShutdown();
			}
		}
	}
}
