using UnityEngine;

public abstract class PanelGuiBase : MonoBehaviour, IPanelGui {
	public bool IsEnabled {
		get { return enabled; }
	}

	public virtual void Show() {
		enabled = true;
	}

	public virtual void Hide() {
		enabled = false;
	}
}
