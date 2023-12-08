using System;
using System.Collections;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class DragAndDrop : Singleton<DragAndDrop> {
	private static int _itemSlotButtonHash = "Button".GetHashCode();
	private float _alphaValue = 1f;
	private bool _dragBegin;
	private Rect _draggedControlRect;
	private Vector2 _dragScalePivot;
	private bool _isZooming;
	private bool _releaseDragItem;
	private float _zoomMultiplier = 1f;
	public int CurrentId { get; private set; }

	public bool IsDragging {
		get { return CurrentId > 0 && DraggedItem != null && DraggedItem.Item != null; }
	}

	public IDragSlot DraggedItem { get; private set; }

	private DragAndDrop() {
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGui;
	}

	public event Action<IDragSlot> OnDragBegin;

	private void OnGui() {
		if (_releaseDragItem) {
			_releaseDragItem = false;
			CurrentId = 0;
			DraggedItem = null;
		}

		if (Event.current.type == EventType.MouseUp) {
			_releaseDragItem = true;
		}

		if (IsDragging) {
			if (_dragBegin) {
				_dragBegin = false;

				if (OnDragBegin != null) {
					OnDragBegin(DraggedItem);
				}

				UnityRuntime.StartRoutine(StartDragZoom(0f, 1f, 1.25f, 0.1f, 0.8f));
			} else {
				if (!_isZooming) {
					_dragScalePivot = GUIUtility.ScreenToGUIPoint(Event.current.mousePosition);
				}

				GUIUtility.ScaleAroundPivot(new Vector2(_zoomMultiplier, _zoomMultiplier), _dragScalePivot);
				GUI.backgroundColor = new Color(1f, 1f, 1f, _alphaValue);
				GUI.matrix = Matrix4x4.identity;
				DraggedItem.Item.DrawIcon(new Rect(_dragScalePivot.x - 24f, _dragScalePivot.y - 24f, 48f, 48f));
			}
		}
	}

	private IEnumerator StartDragZoom(float time, float startZoom, float endZoom, float startAlpha, float endAlpha) {
		_isZooming = true;
		var startPivot = new Vector2(_draggedControlRect.xMin + 32f, _draggedControlRect.yMin + 32f);
		var timer = 0f;

		while (timer < time) {
			_alphaValue = Mathf.Lerp(startAlpha, endAlpha, timer / time);
			_zoomMultiplier = Mathfx.Berp(startZoom, endZoom, timer / time);
			_dragScalePivot = Vector2.Lerp(startPivot, Event.current.mousePosition, timer / time);
			timer += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		_dragScalePivot = Event.current.mousePosition;
		_alphaValue = endAlpha;
		_zoomMultiplier = endZoom;
		_isZooming = false;
	}

	public void DrawSlot<T>(Rect rect, T item, Action<int, T> onDropAction = null, Color? color = null, bool isItemList = false) where T : IDragSlot {
		var controlID = GUIUtility.GetControlID(_itemSlotButtonHash, FocusType.Native);

		if ((ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone) && Event.current.GetTypeForControl(controlID) == EventType.MouseDown && isItemList) {
			rect.width = 50f;
		}

		switch (Event.current.GetTypeForControl(controlID)) {
			case EventType.MouseDown:
				if (Event.current.type != EventType.Used && rect.Contains(Event.current.mousePosition)) {
					GUIUtility.hotControl = controlID;
					Event.current.Use();
				}

				break;
			case EventType.MouseUp:
				MouseUp(rect, controlID, item.Id, onDropAction);

				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID) {
					var vector = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
					_draggedControlRect = new Rect(vector.x, vector.y, rect.width, rect.height);
					_dragBegin = true;
					DraggedItem = item;
					CurrentId = GUIUtility.hotControl;
					GUIUtility.hotControl = 0;
					Event.current.Use();
				}

				break;
			case EventType.Repaint:
				if (color != null) {
					GUI.color = color.Value;
					BlueStonez.loadoutdropslot_highlight.Draw(rect, GUIContent.none, controlID);
					GUI.color = Color.white;
				}

				break;
		}
	}

	public void DrawSlot<T>(Rect rect, Action<int, T> onDropAction) where T : IDragSlot {
		var controlID = GUIUtility.GetControlID(_itemSlotButtonHash, FocusType.Native);

		if (Event.current.GetTypeForControl(controlID) == EventType.MouseUp) {
			MouseUp(rect, controlID, 0, onDropAction);
		}
	}

	private void MouseUp<T>(Rect rect, int id, int slotId, Action<int, T> onDropAction) where T : IDragSlot {
		if (GUIUtility.hotControl == id) {
			GUIUtility.hotControl = 0;
			Event.current.Use();
		} else if (onDropAction != null && DraggedItem != null && rect.Contains(Event.current.mousePosition)) {
			onDropAction(slotId, (T)DraggedItem);
		}
	}
}
