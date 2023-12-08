using System;
using UnityEngine;

[Serializable]
public class ArmorBuffConfiguration : QuickItemConfiguration {
	private const int MaxArmor = 200;
	private const int StartArmor = 100;
	public IncreaseStyle ArmorIncrease;
	public int IncreaseFrequency;
	public int IncreaseTimes;

	[CustomProperty("ArmorPoints")]
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

	public string GetArmorBonusDescription() {
		var num = ((IncreaseTimes != 0) ? IncreaseTimes : 1);

		switch (ArmorIncrease) {
			case IncreaseStyle.Absolute:
				return (num * PointsGain).ToString();
			case IncreaseStyle.PercentFromStart:
				return Mathf.RoundToInt(100 * num * PointsGain / 100f) + "AP";
			case IncreaseStyle.PercentFromMax:
				return Mathf.RoundToInt(200 * num * PointsGain / 100f) + "AP";
			default:
				return "n/a";
		}
	}
}
