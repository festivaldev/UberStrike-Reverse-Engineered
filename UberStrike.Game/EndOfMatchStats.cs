using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UnityEngine;

public class EndOfMatchStats {
	public string PlayTimeXp { get; private set; }
	public string PlayTimePts { get; private set; }
	public string SkillBonusXp { get; private set; }
	public string SkillBonusPts { get; private set; }
	public string BoostXp { get; private set; }
	public string BoostPts { get; private set; }
	public string TotalXp { get; private set; }
	public string TotalPts { get; private set; }
	public string PlayTime { get; private set; }
	public string Kills { get; private set; }
	public string Nutshots { get; private set; }
	public string Headshots { get; private set; }
	public string Smackdowns { get; private set; }
	public string Deaths { get; private set; }
	public string KDR { get; private set; }
	public string Suicides { get; private set; }
	public int GainedXp { get; private set; }
	public int GainedPts { get; private set; }
	public EndOfMatchData Data { get; private set; }

	public EndOfMatchStats() {
		Data = new EndOfMatchData {
			MostValuablePlayers = new List<StatsSummary>(),
			PlayerStatsBestPerLife = new StatsCollection(),
			PlayerStatsTotal = new StatsCollection()
		};
	}

	public void Update(EndOfMatchData data) {
		Data = data;
		Data.TimeInGameMinutes++;

		if (Data.TimeInGameMinutes < 60) {
			PlayTime = "Less than 1 min";
		} else {
			PlayTime = string.Format("{0} min", Mathf.CeilToInt(Data.TimeInGameMinutes / 60));
		}

		Kills = string.Format("{0}", Mathf.Max(0, Data.PlayerStatsTotal.GetKills()));
		Headshots = string.Format("{0}", Mathf.Max(0, Data.PlayerStatsTotal.Headshots));
		Smackdowns = string.Format("{0}", Mathf.Max(0, Data.PlayerStatsTotal.MeleeKills));
		Nutshots = string.Format("{0}", Mathf.Max(0, Data.PlayerStatsTotal.Nutshots));
		Deaths = Data.PlayerStatsTotal.Deaths.ToString();
		Suicides = (-Data.PlayerStatsTotal.Suicides).ToString();
		KDR = GetKdr(Data.PlayerStatsTotal).ToString("N1");
		CalculateXp();
		CalculatePoints();
		Data.PlayerStatsTotal.Xp = GainedXp;
		Data.PlayerStatsTotal.Points = GainedPts;
		GameState.Current.UpdatePlayerStatistics(Data.PlayerStatsTotal, Data.PlayerStatsBestPerLife);
	}

	public float GetKdr(StatsCollection stats) {
		return Mathf.Max(stats.GetKills(), 0) / Mathf.Max(stats.Deaths, 1f);
	}

	private void CalculateXp() {
		if (Data.PlayerStatsTotal.GetDamageDealt() > 0) {
			var num = ((!Data.HasWonMatch) ? XpPointsUtil.Config.XpBaseLoser : XpPointsUtil.Config.XpBaseWinner);
			var num2 = ((!Data.HasWonMatch) ? XpPointsUtil.Config.XpPerMinuteLoser : XpPointsUtil.Config.XpPerMinuteWinner);
			var num3 = Mathf.Max(0, Data.PlayerStatsTotal.GetKills()) * XpPointsUtil.Config.XpKill + Mathf.Max(0, Data.PlayerStatsTotal.Nutshots) * XpPointsUtil.Config.XpNutshot + Mathf.Max(0, Data.PlayerStatsTotal.Headshots) * XpPointsUtil.Config.XpHeadshot + Mathf.Max(0, Data.PlayerStatsTotal.MeleeKills) * XpPointsUtil.Config.XpSmackdown;
			var num4 = Mathf.CeilToInt(Data.TimeInGameMinutes / 60 * num2);
			var num5 = Mathf.CeilToInt(Data.TimeInGameMinutes / 60 * num2 * CalculateBoost(ItemPropertyType.XpBoost));
			GainedXp = num + num3 + num4 + num5;
			PlayTimeXp = num4.ToString();
			SkillBonusXp = num3.ToString();
			BoostXp = num5.ToString();
			TotalXp = GainedXp.ToString();
		} else {
			GainedXp = 0;
			var text = "0";
			TotalXp = text;
			text = text;
			BoostXp = text;
			text = text;
			SkillBonusXp = text;
			PlayTimeXp = text;
		}
	}

	private void CalculatePoints() {
		var num = ((!Data.HasWonMatch) ? XpPointsUtil.Config.PointsBaseLoser : XpPointsUtil.Config.PointsBaseWinner);
		var num2 = ((!Data.HasWonMatch) ? XpPointsUtil.Config.PointsPerMinuteLoser : XpPointsUtil.Config.PointsPerMinuteWinner);
		var num3 = Mathf.Max(0, Data.PlayerStatsTotal.GetKills()) * XpPointsUtil.Config.PointsKill + Mathf.Max(0, Data.PlayerStatsTotal.Nutshots) * XpPointsUtil.Config.PointsNutshot + Mathf.Max(0, Data.PlayerStatsTotal.Headshots) * XpPointsUtil.Config.PointsHeadshot + Mathf.Max(0, Data.PlayerStatsTotal.MeleeKills) * XpPointsUtil.Config.PointsSmackdown;
		var num4 = Mathf.CeilToInt(Data.TimeInGameMinutes / 60 * num2);
		var num5 = Mathf.CeilToInt(Data.TimeInGameMinutes / 60 * num2 * CalculateBoost(ItemPropertyType.PointsBoost));
		GainedPts = num + num3 + num4 + num5;
		PlayTimePts = num4.ToString();
		SkillBonusPts = num3.ToString();
		BoostPts = num5.ToString();
		TotalPts = GainedPts.ToString();
	}

	private float CalculateBoost(ItemPropertyType propType) {
		var num = 0f;

		foreach (var inventoryItem in Singleton<InventoryManager>.Instance.InventoryItems) {
			if (inventoryItem.IsValid) {
				var itemProperties = inventoryItem.Item.View.ItemProperties;

				if (itemProperties != null && itemProperties.ContainsKey(propType)) {
					num = Mathf.Max(num, inventoryItem.Item.View.ItemProperties[propType] / 100f);
				}
			}
		}

		return num;
	}
}
