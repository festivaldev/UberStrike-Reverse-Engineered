using System.IO;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization {
	public static class GameActorInfoProxy {
		public static void Serialize(Stream stream, GameActorInfo instance) {
			var num = 0;

			using (var memoryStream = new MemoryStream()) {
				EnumProxy<MemberAccessLevel>.Serialize(memoryStream, instance.AccessLevel);
				ByteProxy.Serialize(memoryStream, instance.ArmorPointCapacity);
				ByteProxy.Serialize(memoryStream, instance.ArmorPoints);
				EnumProxy<ChannelType>.Serialize(memoryStream, instance.Channel);

				if (instance.ClanTag != null) {
					StringProxy.Serialize(memoryStream, instance.ClanTag);
				} else {
					num |= 1;
				}

				Int32Proxy.Serialize(memoryStream, instance.Cmid);
				EnumProxy<FireMode>.Serialize(memoryStream, instance.CurrentFiringMode);
				ByteProxy.Serialize(memoryStream, instance.CurrentWeaponSlot);
				Int16Proxy.Serialize(memoryStream, instance.Deaths);

				if (instance.FunctionalItems != null) {
					ListProxy<int>.Serialize(memoryStream, instance.FunctionalItems, Int32Proxy.Serialize);
				} else {
					num |= 2;
				}

				if (instance.Gear != null) {
					ListProxy<int>.Serialize(memoryStream, instance.Gear, Int32Proxy.Serialize);
				} else {
					num |= 4;
				}

				Int16Proxy.Serialize(memoryStream, instance.Health);
				Int16Proxy.Serialize(memoryStream, instance.Kills);
				Int32Proxy.Serialize(memoryStream, instance.Level);
				UInt16Proxy.Serialize(memoryStream, instance.Ping);
				ByteProxy.Serialize(memoryStream, instance.PlayerId);

				if (instance.PlayerName != null) {
					StringProxy.Serialize(memoryStream, instance.PlayerName);
				} else {
					num |= 8;
				}

				EnumProxy<PlayerStates>.Serialize(memoryStream, instance.PlayerState);

				if (instance.QuickItems != null) {
					ListProxy<int>.Serialize(memoryStream, instance.QuickItems, Int32Proxy.Serialize);
				} else {
					num |= 16;
				}

				ByteProxy.Serialize(memoryStream, instance.Rank);
				// Not trying to be racist here, that's what UberStrike wants ¯\_(ツ)_/¯
				ColorProxy.Serialize(memoryStream, UnityEngine.Color.white);
				EnumProxy<SurfaceType>.Serialize(memoryStream, instance.StepSound);
				EnumProxy<TeamID>.Serialize(memoryStream, instance.TeamID);

				if (instance.Weapons != null) {
					ListProxy<int>.Serialize(memoryStream, instance.Weapons, Int32Proxy.Serialize);
				} else {
					num |= 32;
				}

				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static GameActorInfo Deserialize(Stream bytes) {
			var num = Int32Proxy.Deserialize(bytes);
			var gameActorInfo = new GameActorInfo();
			gameActorInfo.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
			gameActorInfo.ArmorPointCapacity = ByteProxy.Deserialize(bytes);
			gameActorInfo.ArmorPoints = ByteProxy.Deserialize(bytes);
			gameActorInfo.Channel = EnumProxy<ChannelType>.Deserialize(bytes);

			if ((num & 1) != 0) {
				gameActorInfo.ClanTag = StringProxy.Deserialize(bytes);
			}

			gameActorInfo.Cmid = Int32Proxy.Deserialize(bytes);
			gameActorInfo.CurrentFiringMode = EnumProxy<FireMode>.Deserialize(bytes);
			gameActorInfo.CurrentWeaponSlot = ByteProxy.Deserialize(bytes);
			gameActorInfo.Deaths = Int16Proxy.Deserialize(bytes);

			if ((num & 2) != 0) {
				gameActorInfo.FunctionalItems = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}

			if ((num & 4) != 0) {
				gameActorInfo.Gear = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}

			gameActorInfo.Health = Int16Proxy.Deserialize(bytes);
			gameActorInfo.Kills = Int16Proxy.Deserialize(bytes);
			gameActorInfo.Level = Int32Proxy.Deserialize(bytes);
			gameActorInfo.Ping = UInt16Proxy.Deserialize(bytes);
			gameActorInfo.PlayerId = ByteProxy.Deserialize(bytes);

			if ((num & 8) != 0) {
				gameActorInfo.PlayerName = StringProxy.Deserialize(bytes);
			}

			gameActorInfo.PlayerState = EnumProxy<PlayerStates>.Deserialize(bytes);

			if ((num & 16) != 0) {
				gameActorInfo.QuickItems = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}

			gameActorInfo.Rank = ByteProxy.Deserialize(bytes);
			gameActorInfo.SkinColor = ColorProxy.Deserialize(bytes);
			gameActorInfo.StepSound = EnumProxy<SurfaceType>.Deserialize(bytes);
			gameActorInfo.TeamID = EnumProxy<TeamID>.Deserialize(bytes);

			if ((num & 32) != 0) {
				gameActorInfo.Weapons = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}

			return gameActorInfo;
		}
	}
}
