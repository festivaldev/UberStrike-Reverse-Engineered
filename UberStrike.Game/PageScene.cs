using UnityEngine;

public abstract class PageScene : MonoBehaviour {
	[SerializeField]
	protected Transform _avatarAnchor;

	[SerializeField]
	protected int _guiWidth = -1;

	[SerializeField]
	private bool _haveMouseOrbitCamera;

	[SerializeField]
	protected Vector3 _mouseOrbitConfig;

	[SerializeField]
	private Vector3 _mouseOrbitPivot;

	public bool HaveMouseOrbitCamera {
		get { return _haveMouseOrbitCamera; }
	}

	public int GuiWidth {
		get { return _guiWidth; }
	}

	public Transform AvatarAnchor {
		get { return _avatarAnchor; }
	}

	public Vector3 MouseOrbitConfig {
		get { return _mouseOrbitConfig; }
	}

	public Vector3 MouseOrbitPivot {
		get { return _mouseOrbitPivot; }
	}

	public abstract PageType PageType { get; }

	public bool IsEnabled {
		get { return gameObject.activeSelf; }
	}

	private void Awake() {
		_mouseOrbitConfig.x = (_mouseOrbitConfig.x + 360f) % 360f;
		_mouseOrbitConfig.y = (_mouseOrbitConfig.y + 360f) % 360f;
	}

	public void Load() {
		gameObject.SetActive(true);
		OnLoad();
	}

	public void Unload() {
		gameObject.SetActive(false);
		OnUnload();
	}

	protected virtual void OnLoad() { }
	protected virtual void OnUnload() { }
}
