using System;

[Serializable]
public class AmmoBuffConfiguration : QuickItemConfiguration {
	private const int MaxAmmo = 200;
	private const int StartAmmo = 100;

	[CustomProperty("AmmoIncrease")]
	public IncreaseStyle AmmoIncrease;

	public int IncreaseFrequency;
	public int IncreaseTimes;

	[CustomProperty("AmmoPoints")]
	public int PointsGain;

	[CustomProperty("RobotDestruction")]
	public int RobotLifeTimeMilliSeconds;

	[CustomProperty("ScrapsDestruction")]
	public int ScrapsLifeTimeMilliSeconds;

	public bool IsNeedCharge {
		get { return WarmUpTime > 0; }
	}

	public bool IsOverTime {
		get { return IncreaseTimes > 0; }
	}

	public bool IsInstant {
		get { return !IsNeedCharge && !IsOverTime; }
	}

	public string GetAmmoBonusDescription() {
		var num = ((IncreaseTimes != 0) ? IncreaseTimes : 1);

		switch (AmmoIncrease) {
			case IncreaseStyle.Absolute:
				return (num * PointsGain).ToString();
			case IncreaseStyle.PercentFromStart:
				return string.Format("{0}% of the start ammo", PointsGain);
			case IncreaseStyle.PercentFromMax:
				return string.Format("{0}% of the max ammo", PointsGain);
			default:
				return "n/a";
		}
	}
}
