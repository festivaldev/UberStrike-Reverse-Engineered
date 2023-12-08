using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : struct, IConvertible {
	public readonly EventHandler Events = new EventHandler();
	private Dictionary<T, IState> registeredStates;
	private Stack<T> stateStack;

	public T CurrentStateId {
		get { return (stateStack.Count <= 0) ? default(T) : stateStack.Peek(); }
	}

	private IState CurrentState {
		get { return GetState(CurrentStateId); }
	}

	public StateMachine() {
		registeredStates = new Dictionary<T, IState>();
		stateStack = new Stack<T>();
	}

	public event Action<T> OnChanged;

	public void SetState(T stateId) {
		if (ContainsState(stateId)) {
			if (!stateId.Equals(CurrentStateId)) {
				PopAllStates();
				stateStack.Push(stateId);
				GetState(stateId).OnEnter();

				if (OnChanged != null) {
					OnChanged(stateId);
				}
			}

			return;
		}

		throw new Exception("Unsupported state of type: " + stateId);
	}

	public void PushState(T stateId) {
		if (ContainsState(stateId)) {
			if (!stateStack.Contains(stateId)) {
				stateStack.Push(stateId);
				GetState(stateId).OnEnter();

				if (OnChanged != null) {
					OnChanged(stateId);
				}
			}
		} else {
			Debug.LogWarning("Unsupported state of type: " + stateId);
		}
	}

	public void PopState(bool resume = true) {
		if (stateStack.Count != 0) {
			CurrentState.OnExit();
			stateStack.Pop();

			if (resume && stateStack.Count != 0) {
				CurrentState.OnResume();
			}

			if (OnChanged != null && stateStack.Count > 0) {
				OnChanged(stateStack.Peek());
			}
		}
	}

	public void Reset() {
		PopAllStates();
		stateStack.Clear();
		registeredStates.Clear();
		Events.Clear();

		if (OnChanged != null) {
			OnChanged(default(T));
		}
	}

	public void PopAllStates() {
		while (stateStack.Count > 0) {
			PopState(false);
		}

		if (OnChanged != null) {
			OnChanged(default(T));
		}
	}

	public void RegisterState(T stateId, IState state) {
		if (!registeredStates.ContainsKey(stateId)) {
			registeredStates.Add(stateId, state);

			return;
		}

		throw new Exception("StateMachine::RegisterState - state [" + stateId + "] already exists in the current registry");
	}

	public bool ContainsState(T stateId) {
		return registeredStates.ContainsKey(stateId);
	}

	public void Update() {
		if (stateStack.Count > 0) {
			CurrentState.OnUpdate();
		}
	}

	public IState GetState(T stateId) {
		IState state;
		registeredStates.TryGetValue(stateId, out state);

		return state;
	}
}
