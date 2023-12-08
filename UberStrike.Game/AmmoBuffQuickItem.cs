using System;
using UberStrike.Core.Types;
using UnityEngine;

public class AmmoBuffQuickItem : QuickItem {
	[SerializeField]
	private AmmoBuffConfiguration _config;

	private StateMachine machine = new StateMachine();

	public override QuickItemConfiguration Configuration {
		get { return _config; }
		set { _config = (AmmoBuffConfiguration)value; }
	}

	protected override void OnActivated() {
		if (!machine.ContainsState(1)) {
			machine.RegisterState(1, new ActivatedState(this));
		}

		Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(GameState.Current.Player.Character, QuickItemLogic.AmmoPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsInstant);
		GameState.Current.Actions.ActivateQuickItem(QuickItemLogic.AmmoPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsInstant);
		machine.SetState(1);
	}

	private void Update() {
		machine.Update();
	}

	private void OnGUI() {
		if (Behaviour.IsCoolingDown && Behaviour.FocusTimeRemaining > 0f) {
			var num = Mathf.Clamp(Screen.height * 0.03f, 10f, 40f);
			var num2 = num * 10f;
			GUI.Label(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 20, num2, num), "Charging Ammo", BlueStonez.label_interparkbold_16pt);
			GUITools.DrawWarmupBar(new Rect((Screen.width - num2) * 0.5f, Screen.height / 2 + 50, num2, num), Behaviour.FocusTimeTotal - Behaviour.FocusTimeRemaining, Behaviour.FocusTimeTotal);
		}
	}

	private class ActivatedState : IState {
		private float _increaseCounter;
		private AmmoBuffQuickItem _item;
		private float _nextHealthIncrease;

		public ActivatedState(AmmoBuffQuickItem configuration) {
			_item = configuration;
		}

		public void OnEnter() {
			if (_item._config.IncreaseTimes > 0) {
				_increaseCounter = _item._config.IncreaseTimes;
				_nextHealthIncrease = 0f;
			} else {
				SendAmmoIncrease();
				_item.machine.PopState();
			}
		}

		public void OnExit() { }
		public void OnResume() { }

		public void OnUpdate() {
			if (_nextHealthIncrease < Time.time) {
				_increaseCounter -= 1f;
				_nextHealthIncrease = Time.time + _item._config.IncreaseFrequency / 1000f;
				SendAmmoIncrease();

				if (_increaseCounter <= 0f) {
					_item.machine.PopState();
				}
			}
		}

		private void SendAmmoIncrease() {
			switch (_item._config.AmmoIncrease) {
				case IncreaseStyle.Absolute:
					foreach (var obj in Enum.GetValues(typeof(AmmoType))) {
						var ammoType = (AmmoType)((int)obj);
						AmmoDepot.AddAmmoOfType(ammoType, _item._config.PointsGain);
					}

					break;
				case IncreaseStyle.PercentFromStart:
					foreach (var obj2 in Enum.GetValues(typeof(AmmoType))) {
						var ammoType2 = (AmmoType)((int)obj2);
						AmmoDepot.AddStartAmmoOfType(ammoType2, _item._config.PointsGain / 100f);
					}

					break;
				case IncreaseStyle.PercentFromMax:
					foreach (var obj3 in Enum.GetValues(typeof(AmmoType))) {
						var ammoType3 = (AmmoType)((int)obj3);
						AmmoDepot.AddMaxAmmoOfType(ammoType3, _item._config.PointsGain / 100f);
					}

					break;
				default:
					throw new NotImplementedException("SendAmmoIncrease for type: " + _item._config.AmmoIncrease);
			}
		}
	}
}
