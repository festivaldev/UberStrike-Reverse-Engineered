using System;
using UberStrike.Core.Types;
using UnityEngine;

public class ArmorBuffQuickItem : QuickItem {
	[SerializeField]
	private ArmorBuffConfiguration _config;

	private StateMachine machine = new StateMachine();

	public override QuickItemConfiguration Configuration {
		get { return _config; }
		set { _config = (ArmorBuffConfiguration)value; }
	}

	protected override void OnActivated() {
		if (!machine.ContainsState(1)) {
			machine.RegisterState(1, new ActivatedState(this));
		}

		Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(GameState.Current.Player.Character, QuickItemLogic.ArmorPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsInstant);
		GameState.Current.Actions.ActivateQuickItem(QuickItemLogic.ArmorPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsInstant);
		machine.SetState(1);
	}

	private void Update() {
		machine.Update();
	}

	private void OnGUI() {
		if (Behaviour.IsCoolingDown && Behaviour.FocusTimeRemaining > 0f) {
			var num = Mathf.Clamp(Screen.height * 0.03f, 10f, 40f);
			var num2 = num * 10f;
			GUI.Label(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 20, num2, num), "Charging Armor", BlueStonez.label_interparkbold_16pt);
			GUITools.DrawWarmupBar(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 50, num2, num), Behaviour.FocusTimeTotal - Behaviour.FocusTimeRemaining, Behaviour.FocusTimeTotal);
		}
	}

	private class ActivatedState : IState {
		private float _increaseCounter;
		private ArmorBuffQuickItem _item;
		private float _nextHealthIncrease;

		public ActivatedState(ArmorBuffQuickItem configuration) {
			_item = configuration;
		}

		public void OnEnter() {
			if (_item._config.IncreaseTimes > 0) {
				_increaseCounter = _item._config.IncreaseTimes;
				_nextHealthIncrease = 0f;
			} else {
				SendHealthIncrease();
				_item.machine.PopState();
			}
		}

		public void OnResume() { }
		public void OnExit() { }

		public void OnUpdate() {
			if (_nextHealthIncrease < Time.time) {
				_increaseCounter -= 1f;
				_nextHealthIncrease = Time.time + _item._config.IncreaseFrequency / 1000f;
				SendHealthIncrease();

				if (_increaseCounter <= 0f) {
					_item.machine.PopState();
				}
			}
		}

		private void SendHealthIncrease() {
			int num;

			switch (_item._config.ArmorIncrease) {
				case IncreaseStyle.Absolute:
					num = _item._config.PointsGain;

					break;
				case IncreaseStyle.PercentFromStart:
					num = Mathf.RoundToInt(100f * Mathf.Clamp01(_item._config.PointsGain / 100f));

					break;
				case IncreaseStyle.PercentFromMax:
					num = Mathf.RoundToInt(200f * Mathf.Clamp01(_item._config.PointsGain / 100f));

					break;
				default:
					throw new NotImplementedException("SendArmorIncrease for type: " + _item._config.ArmorIncrease);
			}

			GameState.Current.Actions.IncreaseHealthAndArmor(0, num);
			GameState.Current.PlayerData.ArmorPoints.Value += num;
		}
	}
}
