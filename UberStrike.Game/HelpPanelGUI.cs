using System;
using UnityEngine;

public class HelpPanelGUI : PanelGuiBase {
	private const int WIDTH = 500;
	private GUIContent[] _helpTabs;
	private Rect _rect;
	private Vector2 _scrollBasics;
	private Vector2 _scrollGameplay;
	private Vector2 _scrollItems;
	private int _selectedHelpTab;
	private DateTime baseTime = new DateTime(2012, 12, 14);
	private int currentPlayers = 12821196;
	private float newPlayersPerSecond = 0.23f;

	private void Start() {
		_helpTabs = new[] {
			new GUIContent(LocalizedStrings.General),
			new GUIContent(LocalizedStrings.Gameplay),
			new GUIContent(LocalizedStrings.Items),
			new GUIContent("About")
		};
	}

	private void OnGUI() {
		var num = Mathf.RoundToInt((Screen.height - 56) * 0.75f);
		_rect = new Rect((Screen.width - 630) * 0.5f, GlobalUIRibbon.Instance.Height(), 630f, num);
		GUI.BeginGroup(_rect, GUIContent.none, BlueStonez.window_standard_grey38);
		DrawHelpPanel();
		GUI.EndGroup();
	}

	private void DrawHelpPanel() {
		GUI.depth = 3;
		GUI.Label(new Rect(0f, 0f, _rect.width, 56f), LocalizedStrings.HelpCaps, BlueStonez.tab_strip);
		_selectedHelpTab = UnityGUI.Toolbar(new Rect(2f, 31f, 360f, 22f), _selectedHelpTab, _helpTabs, _helpTabs.Length, BlueStonez.tab_medium);
		GUI.BeginGroup(new Rect(16f, 55f, _rect.width - 32f, _rect.height - 56f - 44f), string.Empty, BlueStonez.window_standard_grey38);

		switch (_selectedHelpTab) {
			case 0:
				DrawGeneralGroup();

				break;
			case 1:
				DrawGameplayGroup();

				break;
			case 2:
				DrawItemsGroup();

				break;
			case 3:
				DrawCreditsGroup();

				break;
		}

		GUI.EndGroup();

		if (GUI.Button(new Rect(_rect.width - 136f, _rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button)) {
			PanelManager.Instance.ClosePanel(PanelType.Help);
		}
	}

	private void DrawRuleGroup() {
		GUI.skin = BlueStonez.Skin;
		var num = 550;
		_scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, _rect.width - 33f, _rect.height - 54f - 50f), _scrollItems, new Rect(0f, 0f, 560f, num));
		var rect = new Rect(14f, 16f, 530f, num - 30);
		DrawGroupControl(rect, "In-game Rules", BlueStonez.label_group_interparkbold_18pt);
		GUI.BeginGroup(rect);
		var num2 = 10f;
		num2 = DrawGroupLabel(num2, "Introduction", "Before we let you loose into the wild world of UberStrike, we've written a few simple guidelines that are in place to make your gaming experience as fun and fair as possible. Having a good time in a multiplayer game is a team effort! So do your part to help our community enjoy themselves;)\n\nWe hope that you have a pleasant stay in Uberstrike!");
		num2 = DrawGroupLabel(num2, "Chatting", "1: No swearing or inappropriate content. Every time an inappropriate word is typed, three puppies and a kitten get caught in a revolving door.\n2: No \"Caps lock\" (using it for emphasis is okay). Please only emphazise with discretion and tact.\n3: No spamming. This includes baloney, rubbish, prattle, balderdash, hogwash, fatuity, drivel, mumbo jumbo, and canned precooked meat products. \n4: Do not personally attack any person(s). If you happen to be a hata, don't be hatin,' becasue the mods gonna be moderatin.' \n5: No backseat moderating. Believe it or not, we didn't add the convenient little 'Report Button' just because it looks pretty up there in the corner of the screen, although it does go nicely with that cute little gear symbol.\n6: Do not discuss topics that involve race, color, creed, religion, sex, or politics. It's not like we play games to get extra exposure to the many issues we face constantly in our daily lives.");
		num2 = DrawGroupLabel(num2, "General", "1: Alternate or \"Second\" Accounts in-game ARE allowed, although we all love you just the way you are.\n2: No account sharing! Your account is yours, and if another player is caught using it, all parties will get banned. Sharing definitely isn't caring round these parts.\n3: Exploiting of glitches will not be tolerated. Cheating of any kind will result in a permanent ban, which may or may not include eternal banishment to the land of angry ankle-biting woodchucks.\n4: Be respectful to the Administrators/Moderators/QAs. These people work hard for you, so please show them respect. If you do, you might even get a cookie!\n5: Advertising of any content unrelated to UberStrike is not permitted.\n6: Please do not try to cleverly circumvent the rules listed here. Although some of these rules are flexible, they are here for a reason, and will be enforced.\n7: Join a server in your area. You will not get banned for lagging, although you may get kicked from the current game.\n8: Above all, use common sense. Studies have shown it works 87% better than no sense at all!\n9: Have fun!");
		GUI.EndGroup();
		GUITools.EndScrollView();
	}

	private void DrawGeneralGroup() {
		GUI.skin = BlueStonez.Skin;
		var num = 490;
		_scrollBasics = GUITools.BeginScrollView(new Rect(1f, 2f, _rect.width - 33f, _rect.height - 54f - 50f), _scrollBasics, new Rect(0f, 0f, 560f, num));
		var rect = new Rect(14f, 16f, 530f, num - 30);
		DrawGroupControl(rect, LocalizedStrings.WelcomeToUS, BlueStonez.label_group_interparkbold_18pt);
		GUI.BeginGroup(rect);
		var num2 = 10f;
		num2 = DrawGroupLabel(num2, LocalizedStrings.Introduction, LocalizedStrings.IntroHelpDesc);
		num2 = DrawGroupLabel(num2, LocalizedStrings.Home, LocalizedStrings.HomeHelpDesc);
		num2 = DrawGroupLabel(num2, LocalizedStrings.Play, LocalizedStrings.PlayHelpDesc);
		num2 = DrawGroupLabel(num2, LocalizedStrings.Profile, LocalizedStrings.ProfileHelpDesc);
		num2 = DrawGroupLabel(num2, LocalizedStrings.Shop, LocalizedStrings.ShopHelpDesc);
		GUI.EndGroup();
		GUITools.EndScrollView();
	}

	private void DrawGameplayGroup() {
		GUI.skin = BlueStonez.Skin;
		var num = 950;
		_scrollGameplay = GUITools.BeginScrollView(new Rect(1f, 2f, _rect.width - 33f, _rect.height - 54f - 50f), _scrollGameplay, new Rect(0f, 0f, 560f, num));
		var rect = new Rect(14f, 16f, 530f, num - 30);
		DrawGroupControl(rect, LocalizedStrings.Gameplay, BlueStonez.label_group_interparkbold_18pt);
		GUI.BeginGroup(rect);
		var num2 = 10f;
		num2 = DrawGroupLabel(num2, "Character Level", "Your character level in UberStrike determines what items you have access to in the Shop. The higher your level, the more items you are able to get. Your character levels up by earning XP in the game.");
		num2 = DrawGroupLabel(num2, "Earning XP", "There are several ways to gain XP in UberStrike. You'll be given the XP at the end of each round depending on how well you did in terms of kills and time spent in game.");
		num2 = DrawGroupLabel(num2, "Splatting an Enemy", "When you deal the final blow to an enemy you get " + XpPointsUtil.Config.XpKill + " XP.");
		num2 = DrawGroupLabel(num2, "Headshot Splats", "When you take down an enemy with a headshot you get " + XpPointsUtil.Config.XpHeadshot + " XP.");
		num2 = DrawGroupLabel(num2, "Nutshot Splats", "When you take down an enemy with a nutshot you get " + XpPointsUtil.Config.XpNutshot + " XP.");
		num2 = DrawGroupLabel(num2, "Melee Splats", "When you splat an enemy with a melee weapon you get " + XpPointsUtil.Config.XpSmackdown + " XP.");
		num2 = DrawGroupLabel(num2, "Bonus", "You will also get bonus XP for time spent in game, and whether or not your team wins (in Team Deathmatch mode).");
		num2 = DrawGroupLabel(num2, "Health", "Health is what you need to survive. You start every life with 100 health, and if it reaches zero, you are splatted and have to respawn. If you take damage, you can replenish your health by picking up health packs in game.");
		num2 = DrawGroupLabel(num2, "Armor Points", "Armor Points are picked up in the game. They absorb a percentage of the damage you receive.");
		num2 = DrawGroupLabel(num2, "Looking Around", "UberStrike is a 3D environment, which means you need to be able to look around. To make your character do this you need to move the mouse.");
		num2 = DrawGroupLabel(num2, "Moving Around", "In UberStrike you use the WASD keys to control the movement of your character. This means that pressing the W key on your keyboard will cause your character to walk forwards. With just the W key and the mouse you can navigate your character to almost every location in the game environment. Pressing the S key will cause you to walk backwards, and pressing the A and D keys will cause you to move left and right (called 'strafing').The final key you'll need to know to get around in UberStrike is the spacebar. Pressing this key will cause your character to jump, which is essential for quickly getting around certain obstacles in the game. If you can get the hang of using the WASD keys to move, the spacebar to jump over obstacles, and the mouse to look around all at the same time, then you have mastered the basics of navigating a first person 3D environment. The use of these keys is common throughout many first person games, so practice them in UberStrike and you'll be a pro in no time.");
		num2 = DrawGroupLabel(num2, "Selecting Different Weapons", "By scrolling the mouse wheel you can cycle through all of your available weapons. You can also choose specific weapons by pressing the number keys 1 through 4.");
		num2 = DrawGroupLabel(num2, "Combat", "In UberStrike your character carries weapons that you can use to splat other players. You use your weapons by clicking the mouse buttons. Pressing the left mouse button will cause the weapon to shoot, called 'Primary Fire' and pressing the right mouse button will use the weapon's special functions, called 'Alternate Fire' Be aware that not all weapons have an Alternate Fire function, and for those that do, it is often a different function for each weapon. An example of an Alternate Fire function would be the zoom, which is the Alternate Fire for Sniper Rifle class weapons.");
		GUI.EndGroup();
		GUITools.EndScrollView();
	}

	private void DrawItemsGroup() {
		GUI.skin = BlueStonez.Skin;
		var num = 690;
		_scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, _rect.width - 33f, _rect.height - 54f - 50f), _scrollItems, new Rect(0f, 0f, 560f, num));
		var rect = new Rect(14f, 16f, 530f, num - 30);
		DrawGroupControl(rect, LocalizedStrings.Items, BlueStonez.label_group_interparkbold_18pt);
		GUI.BeginGroup(rect);
		var num2 = 10f;
		num2 = DrawGroupLabel(num2, "Weapons", "Your character gets access to weapons after you buy them. Weapons are divided into seven classes: Melee, Machine Guns, Shotguns, Sniper Rifles, Splatter Guns, Cannons, and Launchers. Each weapon class functions differently in game and is applicable in different combat contexts. For example, shotgun class weapons are generally better for close range battles, while the sniper rifle class weapons are better from a distance.");
		num2 = DrawGroupLabel(num2, "Gear", "Gear items are used to customize your character and increase your in-game protection. They have an effect on the amount of damage that can be absorbed.");
		num2 = DrawGroupLabel(num2, "Loadout", "The Loadout is a list of all items that you own that your character currently has equipped. Your loadout dictates your character's appearance in the game.");
		num2 = DrawGroupLabel(num2, "Inventory", "The Inventory is a list of all the items that you own that your character does NOT have equipped.");
		num2 = DrawGroupLabel(num2, "Shop", "The Shop is a place where you can buy items for standardized prices. It has the widest variety of items in UberStrike. Purchasing items in the shop is restricted according to your character level. If an item has a level that is above your character level, you cannot purchase it. You can increase your character level by playing the game and earning XP (see gameplay).");
		num2 = DrawGroupLabel(num2, "Points", "Points are used to purchase items from the Shop. You gain them each time you play a round.");
		num2 = DrawGroupLabel(num2, "Credits", "Credits are used to purchase powerful items from the Shop. You can obtain credits by clicking on the 'Get Credits' button in the top right hand corner of the screen.");
		GUI.EndGroup();
		GUITools.EndScrollView();
	}

	private void DrawCreditsGroup() {
		GUI.skin = BlueStonez.Skin;
		var num = 445;
		_scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, _rect.width - 33f, _rect.height - 54f - 50f), _scrollItems, new Rect(0f, 0f, 560f, num));
		var rect = new Rect(14f, 16f, 530f, num - 30);
		DrawGroupControl(rect, "About Uberstrike", BlueStonez.label_group_interparkbold_18pt);
		GUI.BeginGroup(rect);
		var num2 = 5f;
		num2 = DrawGroupLabel(num2, "The Team", "Roman Anastasini, Alexander Bembel, Ludovic Bodin, Nad Chishtie, Jonny Farrell, Tommy Franken, Benjamin Joffe, Peter Jones, Lanmay Jung, Jamin Lee, Kate Li, Monika Michalak, Mark Parrish, Carlos Revelo Puentes, Shaun Lelacheur Sales, Dagmara Sitek, Tycho Terryn, Lee Turner, Graham Vanderplank, Alex Wang, Alice Zhao, Christina Zhao");
		num2 = DrawGroupLabel(num2, "The Mods", "Akalron, Ejh16, Gray Mouser, GUY82, karanraj, ~Karolina~, Luna Lovegood, New York City 1863, P_U_M_B_A, Remi<3, Sam22, Simon1700, The Monster Mike, timewarp01, <3 woot");
		num2 = DrawGroupLabel(num2, "The QA Testers", "Carlos Spicy Weine, -Cobalt-, Dark Drone, Deep Purple, Divv, Dracomine, Equi|ibrium, Final Snake, Freakin Emdjo, KXI System, Neofighter, Silence of Sound, -Skelemiere-, -Spiegel-, Syntix, tayw97, The Nesoi, The Silver Lining, TriggerSpazum");
		num2 = DrawGroupLabel(num2, "The Legends", "Army of One, avanos, Buford T Justice, Celestial Divinity, Chingachgook, Ehnonimus, Enzo., Equi|ibrium, ~H3ADSH0T~, hendronimus, karanraj, King Haids, king_john, Leeness, Lev175, neel4d, niashy, Shruikan-, Snake Doctor, Stylezxy, The Alpha Male, THE ENDER, Tweex, Ultimus Maximus");
		num2 = DrawGroupColourReferences(num2, "Chat Colour Reference");
		GUI.EndGroup();
		GUITools.EndScrollView();
	}

	private float DrawGroupLabel(float yOffset, string header, string text, bool center = false) {
		var rect = new Rect(16f, yOffset + 25f, 490f, 0f);

		if (!string.IsNullOrEmpty(header)) {
			GUI.color = new Color(0.87f, 0.64f, 0.035f, 1f);
			GUI.Label(new Rect(rect.x, rect.y, rect.width, 16f), header + ":", BlueStonez.label_interparkbold_13pt_left);
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
		}

		float num;

		if (center) {
			num = BlueStonez.label_interparkbold_11pt.CalcHeight(new GUIContent(text), rect.width);
			GUI.Label(new Rect(rect.x, rect.y + 16f, rect.width, num), text, BlueStonez.label_interparkbold_11pt);
		} else {
			num = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent(text), rect.width);
			GUI.Label(new Rect(rect.x, rect.y + 16f, rect.width, num), text, BlueStonez.label_interparkbold_11pt_left_wrap);
		}

		GUI.color = Color.white;

		return yOffset + 20f + num + 16f;
	}

	private float DrawGroupColourReferences(float yOffset, string header) {
		var rect = new Rect(16f, yOffset + 25f, 200f, 0f);

		if (!string.IsNullOrEmpty(header)) {
			GUI.color = new Color(0.87f, 0.64f, 0.035f, 1f);
			GUI.Label(new Rect(rect.x, rect.y, rect.width, 16f), header + ":", BlueStonez.label_interparkbold_13pt_left);
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
		}

		var num = rect.y + 16f;
		GUI.color = ColorScheme.ChatNameAdminUser;
		var num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent("Administrators"), rect.width);
		GUI.Label(new Rect(rect.x, num, rect.width, num2), "Administrators", BlueStonez.label_interparkbold_11pt_left_wrap);
		num += num2;
		GUI.color = ColorScheme.ChatNameSeniorModeratorUser;
		num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent("Senior Mods"), rect.width);
		GUI.Label(new Rect(rect.x, num, rect.width, num2), "Senior Mods", BlueStonez.label_interparkbold_11pt_left_wrap);
		num += num2;
		GUI.color = ColorScheme.ChatNameSeniorQAUser;
		num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent("Senior QA"), rect.width);
		GUI.Label(new Rect(rect.x, num, rect.width, num2), "Senior QA", BlueStonez.label_interparkbold_11pt_left_wrap);
		num += num2;
		GUI.color = ColorScheme.ChatNameModeratorUser;
		num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent("Mods"), rect.width);
		GUI.Label(new Rect(rect.x, num, rect.width, num2), "Mods", BlueStonez.label_interparkbold_11pt_left_wrap);
		num += num2;
		GUI.color = ColorScheme.ChatNameQAUser;
		num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent("QA"), rect.width);
		GUI.Label(new Rect(rect.x, num, rect.width, num2), "QA", BlueStonez.label_interparkbold_11pt_left_wrap);
		num += num2;
		GUI.color = Color.white;

		return yOffset + 20f + num + 16f;
	}

	private void DrawGroupControl(Rect rect, string title, GUIStyle style) {
		GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
		GUI.EndGroup();
		GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, style.CalcSize(new GUIContent(title)).x + 10f, 16f), title, style);
	}

	private int CurrentPlayers() {
		return currentPlayers + Mathf.RoundToInt((float)DateTime.Now.Subtract(baseTime).TotalSeconds * newPlayersPerSecond);
	}
}
