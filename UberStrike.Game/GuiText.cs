using UnityEngine;

public class GuiText : MonoBehaviour {
	[SerializeField]
	private Color _color;

	[SerializeField]
	private float _distanceCap = -1f;

	[SerializeField]
	private Font _font;

	private GUIText _guiText;

	[SerializeField]
	private bool _hasTimeLimit;

	private bool _isVisible = true;
	private Material _material;

	[SerializeField]
	private Vector3 _offset;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private string _text;

	private Transform _transform;
	private float _visibleTime;

	public bool IsTextVisible {
		get { return _isVisible; }
		set {
			if (_isVisible != value) {
				_isVisible = value;
				_guiText.enabled = value;
			}
		}
	}

	private void Awake() {
		_transform = transform;
	}

	private void Start() {
		_guiText = gameObject.AddComponent(typeof(GUIText)) as GUIText;
		_guiText.alignment = TextAlignment.Center;
		_guiText.anchor = TextAnchor.MiddleCenter;
		_guiText.font = _font;
		_guiText.text = _text;
		_guiText.material = _font.material;
		_material = _guiText.material;
	}

	private void LateUpdate() {
		if (Camera.main != null && _isVisible) {
			var vector = Camera.main.WorldToViewportPoint(_target.localPosition + _offset);
			_transform.position = vector;

			if (_hasTimeLimit) {
				_visibleTime -= Time.deltaTime;

				if (_visibleTime > 0f) {
					_color.a = _visibleTime;
					_material.color = _color;
				} else {
					_guiText.enabled = false;
				}
			} else {
				if (_distanceCap > 0f) {
					var num = 1f - Mathf.Clamp01(vector.z / _distanceCap);
					_color.a = num;
				}

				_material.color = _color;
			}
		}
	}

	public void ShowText(int seconds) {
		_visibleTime = seconds;
	}

	public void ShowText() {
		ShowText(5);
	}
}
