using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiDropDown {
	private List<Button> _data = new List<Button>();
	private bool _isDown;
	private Rect _rect;
	public float ButtonHeight = 20f;
	public float ButtonWidth = 100f;
	public bool ShowRightAligned = true;
	public GUIContent Caption { get; set; }

	public void Add(GUIContent content, Action onClick) {
		_data.Add(new Button(content) {
			Action = onClick
		});
	}

	public void Add(GUIContent onContent, GUIContent offContent, Func<bool> isOn, Action onClick) {
		_data.Add(new Button(onContent, offContent, isOn) {
			Action = onClick
		});
	}

	public void SetRect(Rect rect) {
		_rect = GUITools.ToGlobal(rect);
	}

	public void Draw() {
		var isDown = _isDown;
		_isDown = GUI.Toggle(_rect, _isDown, Caption, BlueStonez.buttondark_medium);

		if (isDown != _isDown) {
			MouseOrbit.Disable = false;
		}

		if (_isDown) {
			MouseOrbit.Disable = true;
			Rect rect;

			if (ShowRightAligned) {
				rect = new Rect(_rect.xMax - ButtonWidth, _rect.yMax, ButtonWidth, ButtonHeight * _data.Count);
			} else {
				rect = new Rect(_rect.x, _rect.yMax, ButtonWidth, ButtonHeight * _data.Count);
			}

			if (!rect.Contains(Event.current.mousePosition) && !_rect.Contains(Event.current.mousePosition) && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Used)) {
				_isDown = false;
				MouseOrbit.Disable = false;
			}

			GUI.BeginGroup(rect);

			for (var i = 0; i < _data.Count; i++) {
				if (_data[i].IsEnabled()) {
					var guistyle = BlueStonez.dropdown;

					if (ApplicationDataManager.IsMobile) {
						guistyle = BlueStonez.dropdown_large;
					}

					if (GUI.Button(new Rect(0f, i * ButtonHeight, ButtonWidth, ButtonHeight), _data[i].Content, guistyle)) {
						_isDown = false;
						MouseOrbit.Disable = false;
						_data[i].Action();
					}
				}
			}

			GUI.EndGroup();
		}
	}

	private class Button {
		public Action Action;
		public Func<bool> IsEnabled;
		private Func<bool> isOn;
		private GUIContent offContent;
		private GUIContent onContent;

		public GUIContent Content {
			get { return (isOn != null && !isOn()) ? offContent : onContent; }
		}

		public Button(GUIContent onContent) : this(onContent, onContent, () => true) { }

		public Button(GUIContent onContent, GUIContent offContent, Func<bool> isOn) {
			this.onContent = onContent;
			this.offContent = offContent;
			this.isOn = isOn;
			IsEnabled = () => true;
			Action = delegate { };
		}
	}
}
