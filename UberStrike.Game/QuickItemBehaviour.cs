using System;
using System.Text;
using UnityEngine;

public class QuickItemBehaviour {
	private float _chargeTimeOut;
	private CoolingDownState _coolDownState;
	private QuickItem _item;
	private StateMachine _machine;
	public int CurrentAmount;
	public GameInputKey FocusKey;
	public Action OnActivated;

	public float CoolDownTimeRemaining {
		get { return Mathf.Max(_coolDownState.TimeOut - Time.time, 0f); }
	}

	public float CoolDownTimeTotal {
		get { return _item.Configuration.CoolDownTime / 1000f; }
	}

	public float FocusTimeRemaining {
		get { return 0f; }
	}

	public float FocusTimeTotal {
		get { return _item.Configuration.WarmUpTime / 1000f; }
	}

	public float ChargingTimeRemaining {
		get { return Mathf.Max(_chargeTimeOut - Time.time, 0f); }
	}

	public float ChargingTimeTotal {
		get { return _item.Configuration.RechargeTime / 1000f; }
	}

	public bool IsCoolingDown {
		get { return _machine.CurrentStateId > 0; }
	}

	public float CooldownProgress {
		get { return (!IsCoolingDown) ? 1f : (1f - CoolDownTimeRemaining / CoolDownTimeTotal); }
	}

	public QuickItemBehaviour(QuickItem item, Action onActivated) {
		_item = item;
		OnActivated = onActivated;
		_machine = new StateMachine();
		_coolDownState = new CoolingDownState(this);
		_machine.RegisterState(1, _coolDownState);
	}

	private void Activate() {
		if (CurrentAmount == _item.Configuration.AmountRemaining) {
			_chargeTimeOut = Time.time + _item.Configuration.RechargeTime / 1000f;
		}

		if (_item.Configuration.CoolDownTime > 0) {
			_machine.PushState(1);
		}

		CurrentAmount--;
		GameData.Instance.OnQuickItemsChanged.Fire();

		if (OnActivated != null) {
			OnActivated();
		}
	}

	public bool Run() {
		if (CurrentAmount > 0 && _machine.CurrentStateId == 0) {
			AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += Update;
			Activate();

			return true;
		}

		return false;
	}

	private void Update() {
		_machine.Update();

		if (_item.Configuration.RechargeTime > 0 && _chargeTimeOut < Time.time && CurrentAmount < _item.Configuration.AmountRemaining) {
			CurrentAmount = Mathf.Min(CurrentAmount + 1, _item.Configuration.AmountRemaining);
			GameData.Instance.OnQuickItemsChanged.Fire();

			if (CurrentAmount < _item.Configuration.AmountRemaining) {
				_chargeTimeOut = Time.time + _item.Configuration.RechargeTime / 1000f;
			}
		}

		if (_machine.CurrentStateId == 0 && CurrentAmount == _item.Configuration.AmountRemaining) {
			AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate -= Update;
		}
	}

	public override string ToString() {
		var stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Name: " + _item.Configuration.Name);
		stringBuilder.AppendLine("IsBusy: " + IsCoolingDown);
		stringBuilder.AppendLine("State: " + _machine.CurrentStateId);
		stringBuilder.AppendLine("Amount Current: " + CurrentAmount);
		stringBuilder.AppendLine("Amount Total: " + _item.Configuration.AmountRemaining);
		stringBuilder.AppendLine("Time: " + CoolDownTimeRemaining.ToString("F2") + " || " + ChargingTimeRemaining.ToString("F2"));

		return stringBuilder.ToString();
	}

	private enum States {
		CoolingDown = 1
	}

	private class CoolingDownState : IState {
		private QuickItemBehaviour behaviour;
		public float TimeOut { get; private set; }

		public CoolingDownState(QuickItemBehaviour behaviour) {
			this.behaviour = behaviour;
		}

		public void OnEnter() {
			TimeOut = Time.time + behaviour._item.Configuration.CoolDownTime / 1000f;
		}

		public void OnResume() { }
		public void OnExit() { }

		public void OnUpdate() {
			GameData.Instance.OnQuickItemsCooldown.Fire(Array.IndexOf(Singleton<QuickItemController>.Instance.QuickItems, behaviour._item), behaviour.CooldownProgress);
			var num = Mathf.Clamp01(behaviour.CoolDownTimeRemaining / behaviour.CoolDownTimeTotal);

			if (num == 0f) {
				behaviour._machine.PopState();
			}
		}
	}
}
