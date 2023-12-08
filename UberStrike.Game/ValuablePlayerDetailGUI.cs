using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

internal class ValuablePlayerDetailGUI {
	private List<AchievementType> _achievementList;
	private int _curAchievementIndex = -1;
	private Texture2D _curBadge;
	private string _curBadgeText;
	private string _curBadgeTitle;
	private StatsSummary _curPlayerStats;

	public ValuablePlayerDetailGUI() {
		_achievementList = new List<AchievementType>();
	}

	public void SetValuablePlayer(StatsSummary playerStats) {
		_curPlayerStats = playerStats;
		_curBadgeTitle = string.Empty;
		_curBadgeText = string.Empty;
		_achievementList.Clear();

		if (playerStats != null) {
			foreach (var keyValuePair in _curPlayerStats.Achievements) {
				_achievementList.Add((AchievementType)keyValuePair.Key);
			}
		}

		UnityRuntime.StartRoutine(StartBadgeShow());
	}

	public void StopBadgeShow() {
		Singleton<PreemptiveCoroutineManager>.Instance.IncrementId(StartBadgeShow);
	}

	public void Draw(Rect rect) {
		GUI.BeginGroup(rect, GUIContent.none, StormFront.GrayPanelBox);

		if (_curBadge != null) {
			GUI.DrawTexture(new Rect((rect.width - 180f) / 2f, 10f, 180f, 125f), _curBadge);
		}

		if (_curPlayerStats != null) {
			GUI.BeginGroup(new Rect(0f, 140f, rect.width, rect.height - 140f));
			GUI.contentColor = ColorScheme.UberStrikeYellow;
			GUI.Label(new Rect(0f, 5f, rect.width, 20f), _curBadgeTitle, BlueStonez.label_interparkbold_16pt);
			GUI.contentColor = Color.white;
			GUI.Label(new Rect(0f, 30f, rect.width, 20f), _curBadgeText, BlueStonez.label_interparkbold_16pt);
			GUI.Label(new Rect(0f, 60f, rect.width, 20f), _curPlayerStats.Name, BlueStonez.label_interparkbold_18pt);
			GUI.EndGroup();
		}

		GUI.EndGroup();
	}

	private IEnumerator StartBadgeShow() {
		var coroutineId = Singleton<PreemptiveCoroutineManager>.Instance.IncrementId(StartBadgeShow);

		if (_achievementList.Count > 0 && _curPlayerStats != null && _curPlayerStats.Achievements.Count == _achievementList.Count) {
			_curAchievementIndex = 0;

			while (Singleton<PreemptiveCoroutineManager>.Instance.IsCurrent(StartBadgeShow, coroutineId)) {
				var type = _achievementList[_curAchievementIndex];
				SetCurrentAchievementBadge(type, _curPlayerStats.Achievements[(byte)type], string.Empty);

				yield return new WaitForSeconds(2f);

				if (_achievementList.Count > 0) {
					_curAchievementIndex = ++_curAchievementIndex % _achievementList.Count;
				}
			}

			yield break;
		}

		if (_curPlayerStats != null) {
			SetCurrentAchievementBadge(AchievementType.None, Mathf.RoundToInt(Math.Max(_curPlayerStats.Kills, 0) / Math.Max(_curPlayerStats.Deaths, 1f) * 10f), _curPlayerStats.Name);
		}
	}

	private string GetAchievementTitle(AchievementType type) {
		switch (type) {
			case AchievementType.MostValuable:
				return "MOST VALUABLE";
			case AchievementType.MostAggressive:
				return "MOST AGGRESSIVE";
			case AchievementType.SharpestShooter:
				return "SHARPEST SHOOTER";
			case AchievementType.TriggerHappy:
				return "TRIGGER HAPPY";
			case AchievementType.HardestHitter:
				return "HARDEST HITTER";
			case AchievementType.CostEffective:
				return "COST EFFECTIVE";
			default:
				return string.Empty;
		}
	}

	private void SetCurrentAchievementBadge(AchievementType type, int value, string title = "") {
		_curBadge = UberstrikeIconsHelper.GetAchievementBadgeTexture(type);
		_curBadgeTitle = GetAchievementTitle(type);

		if (string.IsNullOrEmpty(_curBadgeTitle)) {
			_curBadgeTitle = title;
		}

		switch (type) {
			case AchievementType.MostValuable:
				_curBadgeText = string.Format("Best KDR: {0:N1}", value / 10f);

				break;
			case AchievementType.MostAggressive:
				_curBadgeText = string.Format("Total Kills: {0:N0}", value);

				break;
			case AchievementType.SharpestShooter:
				_curBadgeText = string.Format("Critial Strikes: {0:N0}", value);

				break;
			case AchievementType.TriggerHappy:
				_curBadgeText = string.Format("Kills in a row: {0:N0}", value);

				break;
			case AchievementType.HardestHitter:
				_curBadgeText = string.Format("Damage Dealt: {0:N0}", value);

				break;
			case AchievementType.CostEffective:
				_curBadgeText = string.Format("Accuracy: {0:N1}%", value / 10f);

				break;
			default:
				_curBadgeText = string.Format("KDR: {0:N1}", value / 10f);

				break;
		}
	}
}
