using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.Core.Models {
	public class GameActorInfoDelta {
		public enum Keys {
			AccessLevel,
			ArmorPointCapacity,
			ArmorPoints,
			Channel,
			ClanTag,
			Cmid,
			CurrentFiringMode,
			CurrentWeaponSlot,
			Deaths,
			FunctionalItems,
			Gear,
			Health,
			Kills,
			Level,
			Ping,
			PlayerId,
			PlayerName,
			PlayerState,
			QuickItems,
			Rank,
			SkinColor,
			StepSound,
			TeamID,
			Weapons
		}

		public readonly Dictionary<Keys, object> Changes = new Dictionary<Keys, object>();
		public int DeltaMask { get; set; }
		public byte Id { get; set; }

		public void Apply(GameActorInfo instance) {
			foreach (var keyValuePair in Changes) {
				switch (keyValuePair.Key) {
					case Keys.AccessLevel:
						instance.AccessLevel = (MemberAccessLevel)((int)keyValuePair.Value);

						break;
					case Keys.ArmorPointCapacity:
						instance.ArmorPointCapacity = (byte)keyValuePair.Value;

						break;
					case Keys.ArmorPoints:
						instance.ArmorPoints = (byte)keyValuePair.Value;

						break;
					case Keys.Channel:
						instance.Channel = (ChannelType)((int)keyValuePair.Value);

						break;
					case Keys.ClanTag:
						instance.ClanTag = (string)keyValuePair.Value;

						break;
					case Keys.Cmid:
						instance.Cmid = (int)keyValuePair.Value;

						break;
					case Keys.CurrentFiringMode:
						instance.CurrentFiringMode = (FireMode)((int)keyValuePair.Value);

						break;
					case Keys.CurrentWeaponSlot:
						instance.CurrentWeaponSlot = (byte)keyValuePair.Value;

						break;
					case Keys.Deaths:
						instance.Deaths = (short)keyValuePair.Value;

						break;
					case Keys.FunctionalItems:
						instance.FunctionalItems = (List<int>)keyValuePair.Value;

						break;
					case Keys.Gear:
						instance.Gear = (List<int>)keyValuePair.Value;

						break;
					case Keys.Health:
						instance.Health = (short)keyValuePair.Value;

						break;
					case Keys.Kills:
						instance.Kills = (short)keyValuePair.Value;

						break;
					case Keys.Level:
						instance.Level = (int)keyValuePair.Value;

						break;
					case Keys.Ping:
						instance.Ping = (ushort)keyValuePair.Value;

						break;
					case Keys.PlayerId:
						instance.PlayerId = (byte)keyValuePair.Value;

						break;
					case Keys.PlayerName:
						instance.PlayerName = (string)keyValuePair.Value;

						break;
					case Keys.PlayerState:
						instance.PlayerState = (PlayerStates)((byte)keyValuePair.Value);

						break;
					case Keys.QuickItems:
						instance.QuickItems = (List<int>)keyValuePair.Value;

						break;
					case Keys.Rank:
						instance.Rank = (byte)keyValuePair.Value;

						break;
					case Keys.SkinColor:
						instance.SkinColor = (Color)keyValuePair.Value;

						break;
					case Keys.StepSound:
						instance.StepSound = (SurfaceType)((int)keyValuePair.Value);

						break;
					case Keys.TeamID:
						instance.TeamID = (TeamID)((int)keyValuePair.Value);

						break;
					case Keys.Weapons:
						instance.Weapons = (List<int>)keyValuePair.Value;

						break;
				}
			}
		}

		public void UpdateDeltaMask() {
			lock (Changes) {
				var mask = 0;

				foreach (var key in Changes.Keys) {
					mask |= 1 << (int)key;
				}

				DeltaMask = mask;
			}
		}

		public void Reset() {
			Changes.Clear();
			UpdateDeltaMask();
		}
	}
}
