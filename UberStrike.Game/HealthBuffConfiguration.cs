using System;
using UnityEngine;

[Serializable]
public class HealthBuffConfiguration : QuickItemConfiguration {
	private const int MaxHealth = 200;
	private const int StartHealth = 100;

	[CustomProperty("IncreaseStyle")]
	public IncreaseStyle HealthIncrease;

	[CustomProperty("Frequency")]
	public int IncreaseFrequency;

	[CustomProperty("Times")]
	public int IncreaseTimes;

	[CustomProperty("HealthPoints")]
	public int PointsGain;

	[CustomProperty("RobotDestruction")]
	public int RobotLifeTimeMilliSeconds;

	[CustomProperty("ScrapsDestruction")]
	public int ScrapsLifeTimeMilliSeconds;

	public bool IsHealNeedCharge {
		get { return WarmUpTime > 0; }
	}

	public bool IsHealOverTime {
		get { return IncreaseTimes > 0; }
	}

	public bool IsHealInstant {
		get { return !IsHealNeedCharge && !IsHealOverTime; }
	}

	public string GetHealthBonusDescription() {
		var num = ((IncreaseTimes != 0) ? IncreaseTimes : 1);

		switch (HealthIncrease) {
			case IncreaseStyle.Absolute:
				return (num * PointsGain) + "HP";
			case IncreaseStyle.PercentFromStart:
				return Mathf.RoundToInt(100 * num * PointsGain / 100f) + "HP";
			case IncreaseStyle.PercentFromMax:
				return Mathf.RoundToInt(200 * num * PointsGain / 100f) + "HP";
			default:
				return "n/a";
		}
	}
}
