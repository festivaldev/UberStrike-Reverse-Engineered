using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class GameData {
	public static GameData Instance = new GameData();
	public Property<GameStateId> GameState = new Property<GameStateId>(GameStateId.None);
	public bool HUDChatIsTyping;
	public Property<bool> IsShopLoaded = new Property<bool>(false);
	public Property<MainMenuState> MainMenu = new Property<MainMenuState>(MainMenuState.Home);
	public Property<Tuple> OnEndOfMatchTimer = new Property<Tuple>();
	public Property<Tuple> OnHUDChatClear = new Property<Tuple>();
	public Property<TupleThree<string, string, MemberAccessLevel>> OnHUDChatMessage = new Property<TupleThree<string, string, MemberAccessLevel>>();
	public Property<Tuple> OnHUDChatStartTyping = new Property<Tuple>();
	public Property<Tuple> OnHUDStreamClear = new Property<Tuple>();
	public Property<TupleThree<GameActorInfo, string, GameActorInfo>> OnHUDStreamMessage = new Property<TupleThree<GameActorInfo, string, GameActorInfo>>();
	public Property<TupleTwo<string, PickUpMessageType>> OnItemPickup = new Property<TupleTwo<string, PickUpMessageType>>();
	public Property<TupleOne<string>> OnNotification = new Property<TupleOne<string>>();
	public Property<TupleThree<string, string, float>> OnNotificationFull = new Property<TupleThree<string, string, float>>();
	public Property<TupleFour<GameActorInfo, GameActorInfo, UberstrikeItemClass, BodyPart>> OnPlayerKilled = new Property<TupleFour<GameActorInfo, GameActorInfo, UberstrikeItemClass, BodyPart>>();
	public Property<Tuple> OnQuickItemsChanged = new Property<Tuple>();
	public Property<TupleTwo<int, float>> OnQuickItemsCooldown = new Property<TupleTwo<int, float>>(new TupleTwo<int, float>(0, 0f));
	public Property<TupleTwo<int, float>> OnQuickItemsRecharge = new Property<TupleTwo<int, float>>(new TupleTwo<int, float>(0, 0f));
	public Property<TupleOne<int>> OnRespawnCountdown = new Property<TupleOne<int>>();
	public Property<TupleOne<string>> OnWarningNotification = new Property<TupleOne<string>>();
	public Property<List<CommActorInfo>> Players = new Property<List<CommActorInfo>>();
	public Property<PlayerStateId> PlayerState = new Property<PlayerStateId>(PlayerStateId.None);
	public Property<Tuple> VideoShowFps = new Property<Tuple>();

	public static bool CanShowFacebookView {
		get { return ApplicationDataManager.Channel == ChannelType.WebFacebook || ApplicationDataManager.Channel == ChannelType.IPad || Application.isEditor; }
	}
}
