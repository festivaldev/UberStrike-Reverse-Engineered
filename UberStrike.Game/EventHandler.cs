using System;
using System.Collections.Generic;

public class EventHandler {
	public static readonly EventHandler Global = new EventHandler();
	private Dictionary<Type, IEventContainer> eventContainer = new Dictionary<Type, IEventContainer>();

	public void Clear() {
		eventContainer.Clear();
	}

	public void AddListener<T>(Action<T> callback) {
		IEventContainer eventContainer;

		if (!this.eventContainer.TryGetValue(typeof(T), out eventContainer)) {
			eventContainer = new EventContainer<T>();
			this.eventContainer.Add(typeof(T), eventContainer);
		}

		var eventContainer2 = eventContainer as EventContainer<T>;

		if (eventContainer2 != null) {
			eventContainer2.AddCallbackMethod(callback);
		}
	}

	public void RemoveListener<T>(Action<T> callback) {
		IEventContainer eventContainer;

		if (this.eventContainer.TryGetValue(typeof(T), out eventContainer)) {
			var eventContainer2 = eventContainer as EventContainer<T>;

			if (eventContainer2 != null) {
				eventContainer2.RemoveCallbackMethod(callback);
			}
		}
	}

	public void Fire(object message) {
		IEventContainer eventContainer;

		if (this.eventContainer.TryGetValue(message.GetType(), out eventContainer)) {
			eventContainer.CastEvent(message);
		}
	}

	private interface IEventContainer {
		void CastEvent(object m);
	}

	private class EventContainer<T> : IEventContainer {
		private Dictionary<string, Action<T>> _dictionary = new Dictionary<string, Action<T>>();

		public void CastEvent(object m) {
			if (Sender != null) {
				Sender((T)m);
			}
		}

		public event Action<T> Sender;

		public void AddCallbackMethod(Action<T> callback) {
			var callbackMethodId = GetCallbackMethodId(callback);

			if (!_dictionary.ContainsKey(callbackMethodId)) {
				_dictionary.Add(callbackMethodId, callback);
				Sender = (Action<T>)Delegate.Combine(Sender, callback);
			}
		}

		public void RemoveCallbackMethod(Action<T> callback) {
			if (_dictionary.Remove(GetCallbackMethodId(callback))) {
				Sender = (Action<T>)Delegate.Remove(Sender, callback);
			}
		}

		private string GetCallbackMethodId(Action<T> callback) {
			var text = callback.Method.DeclaringType.FullName + callback.Method.Name;

			if (callback.Target != null) {
				text += callback.Target.GetHashCode().ToString();
			}

			return text;
		}
	}
}
