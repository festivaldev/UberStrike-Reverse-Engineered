using System.Text;
using UberStrike.Core.Models;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class MiscellaneousExtension {
	public static string ToCustomString(this GameActorInfo info) {
		var stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Name: {0} - {1}/{2}\n", info.PlayerName, info.Cmid, info.PlayerId);
		stringBuilder.AppendLine("Play: " + CmunePrint.Flag(info.PlayerState));
		stringBuilder.AppendLine(string.Concat("CurrentWeapon: ", info.CurrentWeaponSlot, "/", info.CurrentWeaponID));
		stringBuilder.AppendLine(string.Concat("Life: ", info.Health, "/", info.ArmorPoints));
		stringBuilder.AppendLine("Team: " + info.TeamID);
		stringBuilder.AppendLine("Color: " + info.SkinColor);
		stringBuilder.AppendLine("Weapons: " + CmunePrint.Values(info.Weapons));
		stringBuilder.AppendLine("Gear: " + CmunePrint.Values(info.Gear));

		return stringBuilder.ToString();
	}

	public static Transform FindChildWithName(this Transform tr, string name) {
		foreach (var transform in tr.GetComponentsInChildren<Transform>(true)) {
			if (transform.name == name) {
				return transform;
			}
		}

		return null;
	}

	public static void ShareParent(this Transform _this, Transform transform) {
		var localPosition = transform.localPosition;
		var localRotation = transform.localRotation;
		var localScale = transform.localScale;
		_this.parent = transform.parent;
		_this.localPosition = localPosition;
		_this.localRotation = localRotation;
		_this.localScale = localScale;
	}

	public static void Reparent(this Transform _this, Transform newParent) {
		var localPosition = _this.localPosition;
		var localRotation = _this.localRotation;
		var localScale = _this.localScale;
		_this.parent = newParent;
		_this.localPosition = localPosition;
		_this.localRotation = localRotation;
		_this.localScale = localScale;
	}

	public static string ToCustomString(this GameActorInfoDelta info) {
		var stringBuilder = new StringBuilder();
		stringBuilder.Append("Delta ").Append(info.Id).Append(":");

		foreach (var keyValuePair in info.Changes) {
			stringBuilder.Append(keyValuePair.Key.ToString()).Append("|");
		}

		return stringBuilder.ToString();
	}

	public static MapSettings Default(this MapSettings info) {
		return new MapSettings {
			KillsCurrent = 5,
			KillsMax = 100,
			KillsMin = 1,
			PlayersCurrent = 0,
			PlayersMax = 16,
			PlayersMin = 1,
			TimeCurrent = 60,
			TimeMax = 600,
			TimeMin = 1
		};
	}
}
