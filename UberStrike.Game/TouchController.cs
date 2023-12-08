using System.Collections.Generic;
using UnityEngine;

public class TouchController : Singleton<TouchController> {
	private List<TouchBaseControl> _controls;
	public float GUIAlpha = 1f;

	private TouchController() {
		_controls = new List<TouchBaseControl>();
		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += OnUpdate;
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGui;
	}

	private void OnUpdate() {
		foreach (var touchBaseControl in _controls) {
			if (touchBaseControl.Enabled) {
				touchBaseControl.FirstUpdate();

				foreach (var touch in Input.touches) {
					touchBaseControl.UpdateTouches(touch);
				}

				touchBaseControl.FinalUpdate();
			}
		}
	}

	private void OnGui() {
		foreach (var touchBaseControl in _controls) {
			if (touchBaseControl.Enabled) {
				touchBaseControl.Draw();
			}
		}
	}

	public void AddControl(TouchBaseControl control) {
		_controls.Add(control);
	}

	public void RemoveControl(TouchBaseControl control) {
		_controls.Remove(control);
	}
}
