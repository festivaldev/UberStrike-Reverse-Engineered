using System;
using System.Collections;
using UnityEngine;

public class UnityRuntime : AutoMonoBehaviour<UnityRuntime> {
	[SerializeField]
	private bool showInvocationList;

	public event Action OnGui;
	public event Action OnUpdate;
	public event Action OnLateUpdate;
	public event Action OnFixedUpdate;
	public event Action OnDrawGizmo;
	public event Action<bool> OnAppFocus;

	private void FixedUpdate() {
		if (OnFixedUpdate != null) {
			OnFixedUpdate();
		}
	}

	private void Update() {
		if (OnUpdate != null) {
			OnUpdate();
		}
	}

	private void LateUpdate() {
		if (OnLateUpdate != null) {
			OnLateUpdate();
		}
	}

	private void OnGUI() {
		if (OnGui != null) {
			OnGui();
		}

		if (showInvocationList) {
			GUILayout.BeginArea(new Rect(10f, 100f, 400f, Screen.height - 200));

			if (OnUpdate != null) {
				foreach (var @delegate in OnUpdate.GetInvocationList()) {
					GUILayout.Label("Update: " + @delegate.Method.DeclaringType.Name + "." + @delegate.Method.Name);
				}
			}

			if (OnFixedUpdate != null) {
				foreach (var delegate2 in OnFixedUpdate.GetInvocationList()) {
					GUILayout.Label("FixedUpdate: " + delegate2.Method.DeclaringType.Name + "." + delegate2.Method.Name);
				}
			}

			if (OnAppFocus != null) {
				foreach (var delegate3 in OnAppFocus.GetInvocationList()) {
					GUILayout.Label("OnApplicationFocus: " + delegate3.Method.DeclaringType.Name + "." + delegate3.Method.Name);
				}
			}

			GUILayout.EndArea();
		}
	}

	private void OnApplicationFocus(bool focus) {
		if (OnAppFocus != null) {
			OnAppFocus(focus);
		}
	}

	public static Coroutine StartRoutine(IEnumerator routine) {
		if (IsRunning) {
			return Instance.StartCoroutine(routine);
		}

		return null;
	}
}
